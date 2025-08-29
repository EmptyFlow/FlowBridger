import { schema } from "host"

function redefineName(originalName) {
    let name = originalName.substring(1);
    return originalName[0].toLowerCase() + name;
}

function convertNameToSnakeCase(value) {
    return value
        .replace(/([A-Z])/g, '_$1')
        .toLowerCase()
        .replace(/^_/, '');
}

function defineSection() {
    return `#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __stdcall
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif\n\n`;
}

function defineInclude() {
    return `#ifndef FLOW_BRIDGER_H_
#define FLOW_BRIDGER_H_

#if defined(_WIN32)
#else
#include <dlfcn.h>
#endif\n\n`;
}

function defineEndFile() {
    return `#endif // FLOW_BRIDGER_H_`;
}

function defineClassConstructorInitialization(method) {
    const methodName = redefineName(method.Name);
    const nameInSnakeCase = convertNameToSnakeCase(method.Name);
    const originalMethodName = method.Options["Namespace"] ? method.Options["Namespace"] + "_" + nameInSnakeCase : nameInSnakeCase;

    return `${methodName}method = (${methodName})getExport(lib, "${originalMethodName}");`;
}

function defineClassMethod(method) {
    const methodName = redefineName(method.Name);
    return `${methodName} ${methodName}method = nullptr;`;
}

function defineClassGlobalMethods(methods) {
    const filteredMethods = methods.map(a => defineClassMethod(a)).join("\n");
    const constructorInitialization = methods.map(a => defineClassConstructorInitialization(a)).join("\n");

    return `\nclass GlobalFunctions {
public:
    GlobalFunctions(std::string pathToLibrary) {
        void *lib = loadLibrary(pathToLibrary);

        ${constructorInitialization}
    }

    ${filteredMethods}
}\n
`;
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
            return 'float_t';
        case 6:
            return 'double_t';
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
    name = parameter.Name[0].toLowerCase() + name;

    const dataType = getDataType(parameter.ParameterType.DataType);

    return `${dataType} ${name}`;
}

function defineMethod(method) {
    var parameters = method.Parameters.map(a => defineParameter(a)).join(", ");
    var returnType = getDataType(method.ReturnType.DataType);
    if (!returnType) returnType = "void";

    let methodName = redefineName(method.Name);

    return `typedef ${returnType} (FLOWBRIDGER_DELEGATE_CALLTYPE *${methodName})(${parameters});\n`;
}

function defineFile(schema) {
    let result = "";
    result += defineInclude();
    result += defineSection();

    for (var globalMethod of schema.GlobalMethods) {
        result += defineMethod(globalMethod);
    }

    result += defineClassGlobalMethods(schema.GlobalMethods);

    result += defineEndFile();

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