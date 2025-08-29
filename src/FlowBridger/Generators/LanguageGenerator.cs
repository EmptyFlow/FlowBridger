using FlowBridger.Models;
using Jint;

namespace FlowBridger.Generators {

    internal static class LanguageGenerator {

        public static IEnumerable<GeneratedFile> GenerateScheme ( SchemaModel schema, IEnumerable<string> languages ) {
            var resultFiles = new List<GeneratedFile> ();

            foreach ( var language in languages ) {
                var script = "";
                if ( language.StartsWith ( "Embedded." ) ) {
                    var assembly = typeof ( LanguageGenerator ).Assembly;
                    if ( assembly.GetManifestResourceNames ().Contains ( language ) ) {
                        using var stream = assembly.GetManifestResourceStream ( language );
                        if ( stream == null ) {
                            script = "";
                        } else {
                            script = new StreamReader ( stream ).ReadToEnd ();
                        }
                    }
                } else {
                    //TODO: implement read from file
                }

                var engine = new Engine ();
                engine.Modules.Add ( "host", builder => builder
                    .ExportObject ( "schema", schema )
                );
                engine.Modules.Add ( "scriptModule", script );
                var module = engine.Modules.Import ( "scriptModule" );

                var function = module.Get ( "scriptModule" ).AsFunctionInstance ();
                var result = function.Call ();
                if ( result.IsArray () ) {
                    var array = result.AsArray ();
                    if ( array.Length == 2 ) {
                        var fileName = array[0].AsString ();
                        var content = array[1].AsString ();
                        resultFiles.Add ( new GeneratedFile ( fileName, content, language ) );
                    }
                }
            }

            return resultFiles;
        }

        //Embedded.Cpp.RuntimeLoading
        //Embedded.Cpp.DynamicLinking

    }

}
