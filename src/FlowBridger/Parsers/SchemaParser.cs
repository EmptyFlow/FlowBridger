using FlowBridger.Enums;
using FlowBridger.Exceptions;
using FlowBridger.Models;

namespace FlowBridger.Parsers {

    internal static class SchemaParser {

        public static (string name, string value) ParseLine ( string line ) {
            var firstSpaceIndex = line.IndexOf ( ' ' );
            if ( firstSpaceIndex == -1 ) return (line, "");

            return (line.Substring ( 0, firstSpaceIndex ), line.Substring ( firstSpaceIndex + 1 ));
        }

        public static ParsedDataType ParseDataType ( string value ) {
            if ( Enum.TryParse ( typeof ( ParsedDataType ), value, ignoreCase: true, out var dataTypeResult ) ) {
                return (ParsedDataType) dataTypeResult;
            }

            return ParsedDataType.Unknown;
        }

        public static ParsedContainerDataType ParseContainerDataType ( string value ) {
            if ( Enum.TryParse ( typeof ( ParsedContainerDataType ), value, ignoreCase: true, out var dataTypeResult ) ) {
                return (ParsedContainerDataType) dataTypeResult;
            }

            return ParsedContainerDataType.NotContainer;
        }

        public static DataTypeModel GetDataType ( string value ) {
            if ( value.Any ( a => a == '-' ) ) { // value in format: <data-type>-<container-type>
                var spaceIndex = value.IndexOf ( "-" );
                var dataType = ParseDataType ( value.Substring ( 0, spaceIndex ) );
                return new DataTypeModel ( dataType, value.Substring ( spaceIndex + 1 ) );
            } else { // value in format: <data-type>
                var singleDataType = ParseDataType ( value );
                if ( singleDataType == ParsedDataType.Method ) throw new Exception ( "Method type must be defined in format: <name of parameter> method-<custom name delegate>. Delegate must be defined as: globaldelegate <custom name delegate>" );

                return new DataTypeModel ( singleDataType, "" );
            }
        }

        public static MethodModel ParseMethod ( ISchemaLines lines, string version ) {
            if ( version != "1.0" ) throw new BridgerParseException ( "Incorrect scheme version or version not defined. Version must be defined in line like `version 1.0`" );

            var line = lines.GetLastLine ();
            var (methodLineName, methodName) = ParseLine ( line );
            if ( string.IsNullOrEmpty ( methodName ) ) lines.ThrowError ( "Global Method", $"The method must have a name in the format: `method <name>`" );

            var methodReturnType = ParsedDataType.Unknown;
            if ( methodName.Contains ( "-" ) ) { // method have return type
                var returnIndex = methodName.IndexOf ( " " );
                var returnType = methodName.Substring ( returnIndex );
                methodName = methodName.Substring ( 0, returnIndex );
                methodReturnType = ParseDataType ( returnType );
            }

            lines.TakeNextLine ();

            var options = new Dictionary<string, string> ();
            var parameters = new List<MethodParameterModel> ();
            DataTypeModel returnMethodType = new DataTypeModel ( ParsedDataType.Unknown, "" );

            while ( !lines.IsEnd () ) {
                var currentLine = lines.GetLastLine ();
                var (optionName, optionLine) = ParseLine ( currentLine );
                if ( optionName.StartsWith ( "#" ) ) {
                    options.Add ( optionName.Substring ( 1 ), optionLine );
                    lines.TakeNextLine ();
                    continue;
                }

                if ( optionName.ToLowerInvariant () == "returntype" ) {
                    returnMethodType = GetDataType ( optionLine );
                    lines.TakeNextLine ();
                    continue;
                }

                parameters.Add (
                    new MethodParameterModel {
                        Name = optionName,
                        ParameterType = GetDataType ( optionLine ),
                    }
                );

                lines.TakeNextLine ();
            }

            return new MethodModel {
                Name = methodName,
                Options = options,
                Parameters = parameters,
                ReturnType = returnMethodType
            };
        }

        public static (string name, string value) ParseGlobalOption ( ISchemaLines lines, string version ) {
            if ( version != "1.0" ) throw new BridgerParseException ( "Incorrect scheme version or version not defined. Version must be defined in line like `version 1.0`" );

            var line = lines.GetLastLine ();
            var (_, optionLine) = ParseLine ( line );
            var (name, value) = ParseLine ( optionLine );

            lines.TakeNextLine ();

            return (name, value);
        }

        private const string GlobalMethod = "globalmethod";

        private const string GlobalDelegate = "globaldelegate";

        private const string GlobalOptions = "globaloption";

        private const string Version = "version";

        public static SchemaModel ParseSchema ( string content ) {
            var lines = new DefaultSchemaLines (
                content
                    .Replace ( "\r", "" )
                    .Split ( '\n' )
            );

            var globalMethods = new List<MethodModel> ();
            var globalDelegates = new List<MethodModel> ();
            var globalOptions = new Dictionary<string, string> ();
            var version = "";

            while ( !lines.IsEndScheme () ) {
                var currentLine = lines.GetLastLine ();
                if ( string.IsNullOrWhiteSpace ( currentLine ) || currentLine.Trim ().StartsWith ( '#' ) ) { // if line empty or it is comment
                    lines.TakeNextLine ();
                    continue;
                }
                var (lineName, lineValue) = ParseLine ( currentLine );
                var lowerName = lineName.ToLowerInvariant ();
                if ( lowerName is GlobalMethod ) globalMethods.Add ( ParseMethod ( lines, version ) );
                if ( lowerName is GlobalDelegate ) globalDelegates.Add ( ParseMethod ( lines, version ) );
                if ( lowerName == GlobalOptions ) {
                    var (optionName, optionValue) = ParseGlobalOption ( lines, version );
                    if ( !string.IsNullOrEmpty ( optionName ) ) globalOptions.Add ( optionName, optionValue );
                }
                if ( lowerName == Version && string.IsNullOrEmpty ( version ) ) {
                    version = lineValue;
                    lines.TakeNextLine ();
                }

                lines.TakeNextLine ();
            }


            return new SchemaModel {
                Version = version,
                GlobalMethods = globalMethods,
                GlobalOptions = globalOptions,
                GlobalDelegates = globalDelegates,
            };
        }

    }

}
