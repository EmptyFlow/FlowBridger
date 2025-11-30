import { schemaJson } from "host"

const schema = JSON.parse(schemaJson);

function convertNameToSnakeCase(value) {
    return value
        .replace(/([A-Z])/g, '_$1')
        .toLowerCase()
        .replace(/^_/, '');
}

function convertNameToCamelCase(value) {
    if (!value) return value;
    if (value[0] === value[0].toLowerCase()) return value;

    return value[0].toLowerCase() + value.substring(1);
}

function defineImports() {
    return `using System.Runtime.InteropServices;
using System.Text;\n\n`;
}

function defineStringUniLocalHelper() {
    return `
        private static List<byte> m_zeroBytes = [0, 0, 0, 0];

        private static string GetUniStringFromPointer ( nint pointer ) {
            if ( RuntimeInformation.IsOSPlatform ( OSPlatform.Linux ) || RuntimeInformation.IsOSPlatform ( OSPlatform.OSX ) ) {
                if ( pointer == nint.Zero ) return "";

                var buffer = new List<byte> ();
                var offset = 0;
                while ( true ) {
                    var readedByte = Marshal.ReadByte ( pointer, offset );
                    offset++;

                    buffer.Add ( readedByte );

                    if ( buffer.Count () % 4 == 0 && buffer[^4..].SequenceEqual ( m_zeroBytes ) ) break;
                }

                if ( buffer.Count () == 4 && buffer.SequenceEqual ( m_zeroBytes ) ) return "";

                return Encoding.UTF8.GetString ( buffer.ToArray () ).Replace ( "\\u0000", "" );
            }

            return Marshal.PtrToStringUni ( pointer ) ?? "";
        }
`
}

function defineNamespace(schema) {
    const namespace = schema.GlobalOptions["CsNameSpace"] ? schema.GlobalOptions["CsNameSpace"] : "FlowBridger.Export";

    return `namespace ${namespace} {\n`;
}

function defineClass(schema) {
    const className = schema.GlobalOptions["CsClassName"] ? schema.GlobalOptions["CsClassName"] : "FlowBridgerExports";

    return `    public static partial class ${className} {`;
}

function defineEndFile() {
    return `
    }
}`;
}

function getDataType(parameterType) {
    const dataType = parameterType.DataType;

    switch (dataType) {
        case 1: // Int32
            return 'int';
        case 2: // Int64
            return 'long';
        case 3: // StringUni
            return 'nint';
        case 4: // StringAnsi
            return 'nint';
        case 5: // Float
            return 'float';
        case 6: // Double
            return 'double';
        case 7: // UInt32
            return 'uint';
        case 8: // UInt64
            return 'ulong';
        case 9: // Method
            return 'nint';
        case 10: // Boolean
            return 'bool';
        case 11: // Pointer
            return 'nint';
    }

    return "";
}

function getInternalDataType(parameterType) {
    const dataType = parameterType.DataType;

    if (dataType === 3) return 'string';
    if (dataType === 4) return 'string';
    if (dataType === 9) return parameterType.CustomType;

    return getDataType(parameterType);
}

function defineParameter(parameter) {
    return `${getDataType(parameter.ParameterType)} ${convertNameToCamelCase(parameter.Name)}`;
}

function defineInternalParameter(parameter) {
    return `${getInternalDataType(parameter.ParameterType)} ${convertNameToCamelCase(parameter.Name)}`;
}

function definePassedParameter(parameter) {
    if (parameter.ParameterType.DataType === 3) return `GetUniStringFromPointer(${convertNameToCamelCase(parameter.Name)})`;
    if (parameter.ParameterType.DataType === 4) return `Marshal.PtrToStringAnsi(${convertNameToCamelCase(parameter.Name)}) ?? ""`;
    if (parameter.ParameterType.DataType === 9) return `Marshal.GetDelegateForFunctionPointer<${parameter.ParameterType.CustomType}>(${convertNameToCamelCase(parameter.Name)})`;

    return convertNameToCamelCase(parameter.Name);
}

function defineReturnPassType(method) {
    if (method.ReturnType == null || method.ReturnType.DataType === 0) return `void`;
    if (method.ReturnType.DataType === 3) return `nint`;
    if (method.ReturnType.DataType === 4) return `nint`;
    if (method.ReturnType.DataType === 9) return 'nint'; // remake on concrete method

    return getDataType(method.ReturnType);
}

function defineReturnType(method) {
    if (method.ReturnType == null || method.ReturnType.DataType === 0) return `void`;
    if (method.ReturnType.DataType === 3 || method.ReturnType.DataType === 4) return `string`;
    if (method.ReturnType.DataType === 9) return `nint`; // remake on concrete method

    return getDataType(method.ReturnType);
}

function defineReturnTypeCall(method, internalCall) {
    let openBracket = ""
    let closeBracket = ""
    if (method.ReturnType.DataType === 3) {
        openBracket = "Marshal.StringToHGlobalUni("
        closeBracket = ")"
    }
    if (method.ReturnType.DataType === 4) {
        openBracket = "Marshal.StringToHGlobalAnsi("
        closeBracket = ")"
    }

    return `return ${openBracket}${internalCall}${closeBracket};`
}

function defineMethod(method) {
    const nameInSnakeCase = convertNameToSnakeCase(method.Name);
    const originalMethodName = method.Options["Namespace"] ? method.Options["Namespace"] + "_" + nameInSnakeCase : nameInSnakeCase;
    const parameters = method.Parameters.map(a => defineParameter(a)).join(', ');
    const passedParameters = method.Parameters.map(a => definePassedParameter(a)).join(', ');;
    const internalParameters = method.Parameters.map(a => defineInternalParameter(a)).join(', ');
    const returnPassType = defineReturnPassType(method);
    const returnCsharpType = defineReturnType(method);
    const internalCall = `${method.Name}Internal(${passedParameters})`;
    const noReturnType = method.ReturnType == null || method.ReturnType.DataType === 0;
    const internalLine = noReturnType ? `${internalCall};` : `${defineReturnTypeCall(method, internalCall)}`;

    return `
        [UnmanagedCallersOnly ( EntryPoint = "${originalMethodName}" )]
        public static ${returnPassType} ${method.Name} ( ${parameters} ) {
            ${internalLine}
        }

        public static partial ${returnCsharpType} ${method.Name}Internal ( ${internalParameters} );\n`;

}

function defineDelegate(method) {
    const parameters = method.Parameters.map(a => defineParameter(a)).join(', ');
    const returnPassType = defineReturnPassType(method);

    return `
        public delegate ${returnPassType} ${method.Name} ( ${parameters} );\n`;
}

function defineEventClassProperties(event) {

}

function defineEventClass(event) {
    return `
    public class Event${event.Name} {

        private static int m_iterator = 0;

        private static ConcurrentDictionary<int, EventClick1> m_notCompletedEvents = new ConcurrentDictionary<int, EventClick1> ();

        ${defineEventClassProperties(event)}

        public static Event${event.Name} CreateEvent () {
            var newEvent = new Event${event.Name} ();
            var index = Interlocked.Increment ( ref m_iterator );
            var completed = m_notCompletedEvents.TryAdd ( index, newEvent );
            if ( completed ) throw new Exception ( "Can't add new event to not completed list!" );

            return newEvent;
        }

        public static void CompleteEvent ( int index ) {
            if ( !m_notCompletedEvents.ContainsKey ( index ) ) return;

            m_notCompletedEvents.TryRemove ( index, out var _ );
        }

    }
    `;
}

function defineEventClasses(events) {
    let result = ``;
    for (const event of events) {
        result += defineEventClass(event);
    }

    return result;
}

function defineFile(schema) {
    let result = "";
    result += defineImports();
    result += defineNamespace(schema);
    result += defineClass(schema);

    // if we have stringUni type we need to generate special method helper
    const isMethodsHaveStringUni = schema.GlobalMethods.find(a => a.Parameters.find(a => a.ParameterType.DataType === 3));
    const isDelegatesHaveStringUni = schema.GlobalDelegates.find(a => a.Parameters.find(a => a.ParameterType.DataType === 3));
    if (isMethodsHaveStringUni || isDelegatesHaveStringUni) result += defineStringUniLocalHelper();

    for (var globalDelegate of schema.GlobalDelegates) {
        result += defineDelegate(globalDelegate);
    }

    for (var globalMethod of schema.GlobalMethods) {
        result += defineMethod(globalMethod);
    }

    var eventClasses = schema.GlobalOptions["CsEventClass"] === "enabled";
    if (eventClasses) defineEventClasses(schema.Events);

    result += defineEndFile();

    return result;
}

function generateLanguage() {
    if (!schema) return;

    const fileName = schema.GlobalOptions["CsFileName"] ? schema.GlobalOptions["CsFileName"] : "FlowBridger.cs";

    return [
        fileName,
        defineFile(schema)
    ];
}

export const scriptModule = generateLanguage;