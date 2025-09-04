using FlowBridger.Generators;
using FlowBridger.Models;
using FlowBridger.Models.ConsoleCommands;
using FlowBridger.Parsers;
using FlowCommandLine;

var version = typeof ( Program ).Assembly.GetName ().Version;
var versionString = "";
if ( version != null ) versionString = $"{version.Major}.{version.Minor}.{version.Build}";

CommandLine.Console ()
    .Application (
        "FlowBridger",
        versionString,
        "The application generates source code for creating native libraries in C#.",
        "Copyright (c) Roman Vladimirov",
        "fbridger"
    )
    .AddCommand (
        "generate",
        ( GenerateFilesCommand parameters ) => {
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
        },
        "Generating source code files. The parameter is multiple and has the format - language:path \"language:pathinquotes\"",
        [
            FlowCommandParameter.CreateRequired("l", "languages", "List of languages which need to generate"),
            FlowCommandParameter.CreateRequired("s", "schema", "Path and filename "),
        ]
    )
    .RunCommand ();