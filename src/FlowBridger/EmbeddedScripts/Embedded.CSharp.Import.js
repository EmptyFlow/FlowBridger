import { schema } from "host"

function defineFile(schema) {
    let result = "";
    result += defineInclude();

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
        "FlowBridger.cs",
        defineFile(schema)
    ];
}

export const scriptModule = generateLanguage;