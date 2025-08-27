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

function defineParameter(parameter) {

}

function defineMethod(method) {
    var parameters = method.Parameters.map(a => defineParameter(a)).join(", ");

    return `typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *${method.Name})(${parameters});\n`;
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