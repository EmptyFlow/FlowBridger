using FlowBridger.Generators;
using FlowBridger.Models;
using FlowBridger.Models.ConsoleCommands;
using FlowBridger.Parsers;
using FlowCommandLine;

namespace FlowBridger.Commands {

    internal static class GenerateCommand {

        public static void Run ( GenerateFilesCommand parameters ) {
            Console.WriteLine ( "Start to generate files..." );

            SchemaModel? schema = null;
            try {
                Console.WriteLine ( $"Parse schema: {parameters.Schema}" );
                var schemaContent = File.ReadAllText ( Path.GetFullPath ( parameters.Schema ) );
                schema = SchemaParser.ParseSchema ( schemaContent );
            } catch ( Exception ex ) {
                Console.WriteLine ( ex.Message );
                Environment.Exit ( 1 );
            }
            if ( schema == null ) return;

            var items = new Dictionary<string, string> ();
            foreach ( var item in parameters.Languages ) {
                var span = item.AsSpan ();
                if ( span.IndexOf ( ":" ) == -1 ) continue;

                var index = span.IndexOf ( ":" );
                if ( index == -1 ) continue;

                var language = span.Slice ( 0, index ).ToString ();
                var path = span.Slice ( index + 1 ).ToString ();
                items.Add ( language, path );
            }

            if ( !items.Any () ) {
                Console.WriteLine ( $"No languages ​​defined, please specify at least one language!" );
                Environment.Exit ( 1 );
            }

            Console.WriteLine ( $"Passed {items.Count ()} languages" );

            var generatedFiles = Enumerable.Empty<GeneratedFile> ();
            try {
                Console.WriteLine ( $"Generate content for languages {string.Join ( ", ", items.Keys )}" );
                generatedFiles = LanguageGenerator.GenerateScheme ( schema, items.Keys );
            } catch ( Exception ex ) {
                Console.WriteLine ( ex.Message );
                Environment.Exit ( 1 );
            }

            foreach ( var generatedFile in generatedFiles ) {
                if ( !items.ContainsKey ( generatedFile.Language ) ) continue;

                var languagePath = items[generatedFile.Language];
                try {
                    Console.WriteLine ( $"Save file for language {generatedFile.Language}" );
                    File.WriteAllText ( Path.Combine ( Path.GetFullPath ( languagePath ), generatedFile.FileName ), generatedFile.Content );
                } catch ( Exception ex ) {
                    Console.WriteLine ( ex.Message );
                    Environment.Exit ( 1 );
                }
            }

            Console.WriteLine ( $"Generation is completed" );
        }

        public static IEnumerable<FlowCommandParameter> GetParameters () {
            return [
                FlowCommandParameter.CreateDefault(name: "schema", help: "Path and filename "),
                FlowCommandParameter.CreateRequired("l", "languages", "List of languages which need to generate. The parameter is multiple and has the format - language:path \"language:pathinquotes\"")
            ];
        }

    }

}
