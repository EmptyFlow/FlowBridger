import { schema } from "host"

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

function defineLocalHelpers() {
    return `\n        private static List<byte> m_zeroBytes = [0, 0, 0, 0];

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

function getDataType(dataType) {
    switch (dataType) {
        case 1:
            return 'int';
        case 2:
            return 'long';
        case 3:
            return 'nint';
        case 4:
            return 'nint';
        case 5:
            return 'float';
        case 6:
            return 'double';
        case 7:
            return 'uint32';
        case 8:
            return 'uint64';
        case 9:
            return 'nint'; // remake on concrete method
    }

    return "";
}

function getInternalDataType(dataType) {
    switch (dataType) {
        case 1:
            return 'int';
        case 2:
            return 'long';
        case 3:
            return 'string';
        case 4:
            return 'string';
        case 5:
            return 'float';
        case 6:
            return 'double';
        case 7:
            return 'uint32';
        case 8:
            return 'uint64';
        case 9:
            return 'nint'; // remake on concrete method
    }

    return "";
}

function defineParameter(parameter) {
    return `${getDataType(parameter.ParameterType.DataType)} ${convertNameToCamelCase(parameter.Name)}`;
}

function defineInternalParameter(parameter) {
    return `${getInternalDataType(parameter.ParameterType.DataType)} ${convertNameToCamelCase(parameter.Name)}`;
}

function definePassedParameter(parameter) {
    switch (parameter.ParameterType.DataType) {
        case 1:
        case 2:
        case 5:
        case 6:
        case 7:
        case 8:
            return convertNameToCamelCase(parameter.Name);
        case 3:
            return `GetUniStringFromPointer(${convertNameToCamelCase(parameter.Name)})`;
        case 4:
            return `GetAnsiStringFromPointer(${convertNameToCamelCase(parameter.Name)})`;
        case 9:
            return 'nint'; // remake on concrete method
    }

    return "";
}

function defineMethod(method) {
    const nameInSnakeCase = convertNameToSnakeCase(method.Name);
    const originalMethodName = method.Options["Namespace"] ? method.Options["Namespace"] + "_" + nameInSnakeCase : nameInSnakeCase;
    const parameters = method.Parameters.map(a => defineParameter(a)).join(', ');
    const passedParameters = method.Parameters.map(a => definePassedParameter(a)).join(', ');;
    const internalParameters = method.Parameters.map(a => defineInternalParameter(a)).join(', ');

    return `
        [UnmanagedCallersOnly ( EntryPoint = "${originalMethodName}" )]
        public static int ${method.Name} ( ${parameters} ) {
            return ${method.Name}Internal(${passedParameters});
        }

        public static partial int ${method.Name}Internal ( ${internalParameters} );\n`;

}

function defineFile(schema) {
    let result = "";
    result += defineImports();
    result += defineNamespace(schema);
    result += defineClass(schema);
    result += defineLocalHelpers();

    for (var globalMethod of schema.GlobalMethods) {
        result += defineMethod(globalMethod);
    }

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