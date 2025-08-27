import { schema } from "host"

function redefineName(originalName) {
    let name = originalName.substring(1);
    return originalName[0].toLowerCase() + name;
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
    return `${methodName}method = (${methodName})getExport(lib, "${methodName}");`;
}

function defineClassMethod(method) {
    const methodName = redefineName(method.Name);
    return `${methodName} ${methodName}method = nullptr;`;
}

function defineClassGlobalMethods(methods) {
    const methods = methods.map(a => defineClassMethod(a)).join("\n");
    const constructorInitialization = methods.map(a => defineClassConstructorInitialization(a)).join("\n");

    return `class GlobalFunctions {
public:
    GlobalFunctions(std::string pathToLibrary) {
        void *lib = loadLibrary(pathToLibrary);

        ${constructorInitialization}
    }

    ${methods}
}
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

    return `typedef ${returnType} (FLOWBRIDGER_DELEGATE_CALLTYPE *${method.Name})(${parameters});\n`;
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