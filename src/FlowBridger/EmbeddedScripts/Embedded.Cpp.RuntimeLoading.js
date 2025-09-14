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

function convertToUpperName(value) {
    return value.toUpperCase().replace(".", "_");
}

function defineSection() {
    return `#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __stdcall
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif\n\n`;
}

function defineInclude() {
    const upperFileName = schema.GlobalOptions["CppFileName"] ? convertToUpperName(schema.GlobalOptions["CppFileName"]) : "FLOW_BRIDGER_H_"

    return `#ifndef ${upperFileName}
#define ${upperFileName}

#include <string>
#include <cmath>
#include <cassert>

#if defined(_WIN32)
#include <windows.h>
#else
#include <dlfcn.h>
#endif\n\n`;
}

function defineEndFile() {
    const upperFileName = schema.GlobalOptions["CppFileName"] ? convertToUpperName(schema.GlobalOptions["CppFileName"]) : "FLOW_BRIDGER_H_"

    return `#endif // ${upperFileName}`;
}

function defineClassConstructorInitialization(method) {
    const methodName = redefineName(method.Name);
    const nameInSnakeCase = convertNameToSnakeCase(method.Name);
    const originalMethodName = method.Options["Namespace"] ? method.Options["Namespace"] + "_" + nameInSnakeCase : nameInSnakeCase;

    return `        ${methodName} = (${nameInSnakeCase})getExport(lib, "${originalMethodName}");`;
}

function defineClassMethod(method) {
    const methodName = redefineName(method.Name);
    const nameInSnakeCase = convertNameToSnakeCase(method.Name);
    return `    ${nameInSnakeCase} ${methodName} = nullptr;`;
}

function defineClassGlobalMethods(methods) {
    const filteredMethods = methods.map(a => defineClassMethod(a)).join("\n");
    const constructorInitialization = methods.map(a => defineClassConstructorInitialization(a)).join("\n");

    return `\nclass ImportFunctions {
private:
    void* loadLibrary(const std::wstring& path)
    {
#if defined(_WIN32)
        HMODULE h = ::LoadLibraryW(path.c_str());
        assert(h != nullptr);
        return (void*)h;
#else
        void *h = dlopen(path.c_str(), RTLD_LAZY | RTLD_LOCAL);
        assert(h != nullptr);
        return h;
#endif
    }

    void *getExport(void *h, const char *name)
    {
#if defined(_WIN32)
        void *f = ::GetProcAddress((HMODULE)h, name);
        assert(f != nullptr);
        return f;
#else
        void *f = dlsym(h, name);
        assert(f != nullptr);
        return f;
#endif
    }

public:
    ImportFunctions(const std::wstring& pathToLibrary) {
        void *lib = loadLibrary(pathToLibrary);

${constructorInitialization}
    }

${filteredMethods}
};\n
`;
}

function getDataType(parameter) {
    switch (parameter.DataType) {
        case 1:
            return 'int32_t';
        case 2:
            return 'int64_t';
        case 3:
            return 'const wchar_t*';
        case 4:
            return 'const char*';
        case 5:
            return 'float_t';
        case 6:
            return 'double_t';
        case 7:
            return 'uint32_t';
        case 8:
            return 'uint64_t';
        case 9:
            return convertNameToSnakeCase(parameter.CustomType);
        case 10:
            return 'bool';
    }

    return "";
}

function getDataTypeForReturn(parameter) {
    switch (parameter.DataType) {
        case 3:
            return 'wchar_t*';
        case 4:
            return 'char*';
    }
    return getDataType(parameter);
}

function defineParameter(parameter) {
    let name = parameter.Name.substring(1);
    name = parameter.Name[0].toLowerCase() + name;

    const dataType = getDataType(parameter.ParameterType);

    return `${dataType} ${name}`;
}

function defineMethod(method) {
    var parameters = method.Parameters.map(a => defineParameter(a)).join(", ");
    var returnType = getDataTypeForReturn(method.ReturnType);
    if (!returnType) returnType = "void";

    const nameInSnakeCase = convertNameToSnakeCase(method.Name);
    const methodName = method.Options["Namespace"] ? method.Options["Namespace"] + "_" + nameInSnakeCase : nameInSnakeCase;

    return `typedef ${returnType} (FLOWBRIDGER_DELEGATE_CALLTYPE *${methodName})(${parameters});\n`;
}

function defineDelegate(delegate) {
    var parameters = delegate.Parameters.map(a => defineParameter(a)).join(", ");
    var returnType = getDataType(delegate.ReturnType);
    if (!returnType) returnType = "void";

    const nameInSnakeCase = convertNameToSnakeCase(delegate.Name);
    let methodName = delegate.Options["Namespace"] ? delegate.Options["Namespace"] + "_" + nameInSnakeCase : nameInSnakeCase;

    return `typedef ${returnType} (*${methodName})(${parameters});\n`;
}

function defineFile(schema) {
    let result = "";
    result += defineInclude();
    result += defineSection();

    for (var globalDelegate of schema.GlobalDelegates) result += defineDelegate(globalDelegate);
    if (schema.GlobalDelegates.length) result += '\n';

    for (var globalMethod of schema.GlobalMethods) {
        result += defineMethod(globalMethod);
    }

    result += defineClassGlobalMethods(schema.GlobalMethods);

    result += defineEndFile();

    return result;
}

function generateLanguage() {
    if (!schema) return;

    const fileName = schema.GlobalOptions["CppFileName"] ? schema.GlobalOptions["CppFileName"] : "flowbridger.h";

    return [
        fileName,
        defineFile(schema)
    ];
}

export const scriptModule = generateLanguage;