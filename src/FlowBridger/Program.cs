using FlowBridger.Models.ConsoleCommands;
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
            foreach ( var item in parameters.Languages ) {
                var span = item.AsSpan ();
                if ( span.IndexOf ( ":" ) == -1 ) continue;

                var index = span.IndexOf ( ":" );
                var language = span.Slice ( 0, index ).ToString ();
                var path = span.Slice ( index + 1 ).ToString ();
            }
        },
        "Generating source code files. The parameter is multiple and has the format - language:path \"language:pathinquotes\"",
        [
            FlowCommandParameter.CreateRequired("l", "languages", "List of languages which need to generate")
        ]
    )
    .RunCommand ();