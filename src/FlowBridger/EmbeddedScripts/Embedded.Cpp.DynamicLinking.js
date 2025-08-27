import { schema } from "host"

function defineSection() {
    return `#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __stdcall
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif\n\n`;
}

function defineInclude() {
    return `#if defined(_WIN32)
#else
#include <dlfcn.h>
#endif\n\n`;
}

function getDataType(dataType) {
    switch (dataType) {
        case 1:
            return 'int32_t';
        case 2:
            return 'int64_t';
        case 3:
            return 'wchar_t*';
        case 4:
            return 'char*';
        case 5:
            return 'float';
        case 6:
            return 'double';
        case 7:
            return 'uint32_t';
        case 8:
            return 'uint64_t';
        case 9:
            return 'void*'; // remake on concrete method
    }

    return "";
}

function defineParameter(parameter) {
    let name = parameter.Name.substring(1);
    name = parameter.Name[0].toLower() + name;

    const dataType = getDataType(parameter.ParameterType.DataType);

    return `${dataType} ${name}`;
}

function defineMethod(method) {
    var parameters = method.Parameters.map(a => defineParameter(a)).join(", ");
    var returnType = getDataType(method.ReturnType.DataType);
    if (!returnType) returnType = "void";

    return `typedef ${returnType} (FLOWBRIDGER_DELEGATE_CALLTYPE *${method.Name})(${parameters});\n`;
}

function defineFile(schema) {
    let result = "";
    result += defineInclude();
    result += defineSection();

    for (var globalMethod of schema.GlobalMethods) {
        result += defineMethod(globalMethod);
    }

    return result;
}

function generateLanguage() {
    if (!schema) return;

    return [
        "flowbridger.h",
        defineFile(schema)
    ];
}

export const scriptModule = generateLanguage;