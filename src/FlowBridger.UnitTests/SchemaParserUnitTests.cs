using FlowBridger.Enums;
using FlowBridger.Models;
using FlowBridger.Parsers;

namespace FlowBridger.UnitTests {

    public class SchemaParserUnitTests {

        [Fact]
        public void ParseLine_Completed_Case1 () {
            //arrange
            var line = "name1 value1";

            //act
            var (name, value) = SchemaParser.ParseLine ( line );

            //assert
            Assert.Equal ( "name1", name );
            Assert.Equal ( "value1", value );
        }

        [Fact]
        public void ParseLine_Completed_EmptyValue () {
            //arrange
            var line = "name1";

            //act
            var (name, value) = SchemaParser.ParseLine ( line );

            //assert
            Assert.Equal ( "name1", name );
            Assert.Equal ( "", value );
        }

        [Fact]
        public void ParseLine_Completed_EmptyName () {
            //arrange
            var line = " value1";

            //act
            var (name, value) = SchemaParser.ParseLine ( line );

            //assert
            Assert.Equal ( "", name );
            Assert.Equal ( "value1", value );
        }

        [Fact]
        public void ParseLine_Completed_EmptyLine () {
            //arrange
            var line = "";

            //act
            var (name, value) = SchemaParser.ParseLine ( line );

            //assert
            Assert.Equal ( "", name );
            Assert.Equal ( "", value );
        }

        [Fact]
        public void ParseLine_Completed_MultipleSpaces () {
            //arrange
            var line = "name1    value1";

            //act
            var (name, value) = SchemaParser.ParseLine ( line );

            //assert
            Assert.Equal ( "name1", name );
            Assert.Equal ( "   value1", value );
        }

        [Fact]
        public void ParseDataType_Completed_UnknownType () {
            //arrange
            var line = "borozda";

            //act
            var result = SchemaParser.ParseDataType ( line );

            //assert
            Assert.Equal ( ParsedDataType.Unknown, result );
        }

        [Fact]
        public void ParseDataType_Completed_Int32 () {
            //arrange
            var line = "int32";

            //act
            var result = SchemaParser.ParseDataType ( line );

            //assert
            Assert.Equal ( ParsedDataType.Int32, result );
        }

        [Fact]
        public void ParseDataType_Completed_Int64 () {
            //arrange
            var line = "int64";

            //act
            var result = SchemaParser.ParseDataType ( line );

            //assert
            Assert.Equal ( ParsedDataType.Int64, result );
        }

        [Fact]
        public void ParseDataType_Completed_StringAnsi () {
            //arrange
            var line = "StringAnsi";

            //act
            var result = SchemaParser.ParseDataType ( line );

            //assert
            Assert.Equal ( ParsedDataType.StringAnsi, result );
        }

        [Fact]
        public void ParseContainerDataType_Completed_Unknown () {
            //arrange
            var line = "borozda";

            //act
            var result = SchemaParser.ParseContainerDataType ( line );

            //assert
            Assert.Equal ( ParsedContainerDataType.NotContainer, result );
        }

        [Fact]
        public void ParseContainerDataType_Completed_Array () {
            //arrange
            var line = "array";

            //act
            var result = SchemaParser.ParseContainerDataType ( line );

            //assert
            Assert.Equal ( ParsedContainerDataType.Array, result );
        }

        [Fact]
        public void GetDataType_Completed_Empty () {
            //arrange
            var line = "";

            //act
            var result = SchemaParser.GetDataType ( line );

            //assert
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Unknown, "" ), result );
        }

        [Fact]
        public void GetDataType_Completed_Int32 () {
            //arrange
            var line = "int32";

            //act
            var result = SchemaParser.GetDataType ( line );

            //assert
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), result );
        }

        [Fact]
        public void GetDataType_Completed_Int32Array () {
            //arrange
            var line = "int32-array";

            //act
            var result = SchemaParser.GetDataType ( line );

            //assert
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "array" ), result );
        }

        [Fact]
        public void ParseMethod_Completed_Case1 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globalmethod testMethod",
                    "int32 parameter1",
                    "int64 parameter2"
                }
            );

            //act
            var result = SchemaParser.ParseMethod ( lines, "1.0" );

            //assert
            Assert.Equal ( "testMethod", result.Name );
            var firstParameter = result.Parameters.First ();
            Assert.Equal ( "parameter1", firstParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstParameter.ParameterType );
            var secondParameter = result.Parameters.Last ();
            Assert.Equal ( "parameter2", secondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondParameter.ParameterType );
        }

        [Fact]
        public void ParseMethod_Completed_Case2 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globalmethod testMethod",
                    "#csnamespace LALALA",
                    "int32 parameter1",
                    "int64 parameter2"
                }
            );

            //act
            var result = SchemaParser.ParseMethod ( lines, "1.0" );

            //assert
            Assert.Equal ( "testMethod", result.Name );
            var firstParameter = result.Parameters.First ();
            Assert.Equal ( "parameter1", firstParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstParameter.ParameterType );
            var secondParameter = result.Parameters.Last ();
            Assert.Equal ( "parameter2", secondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondParameter.ParameterType );

            var option = result.Options.First ();
            Assert.Equal ( "csnamespace", option.Key );
            Assert.Equal ( "LALALA", option.Value );
        }

        [Fact]
        public void ParseMethod_Completed_Case3 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globalmethod testMethod",
                    "int32 parameter1",
                    "#csnamespace LALALA",
                    "int64 parameter2",
                    "",
                    "globalmethod lalala"
                }
            );

            //act
            var result = SchemaParser.ParseMethod ( lines, "1.0" );

            //assert
            Assert.Equal ( "testMethod", result.Name );
            var firstParameter = result.Parameters.First ();
            Assert.Equal ( "parameter1", firstParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstParameter.ParameterType );
            var secondParameter = result.Parameters.Last ();
            Assert.Equal ( "parameter2", secondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondParameter.ParameterType );

            var option = result.Options.First ();
            Assert.Equal ( "csnamespace", option.Key );
            Assert.Equal ( "LALALA", option.Value );
        }

        [Fact]
        public void ParseGlobalOptions_Completed_Case1 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globaloption option value"
                }
            );

            //act
            var result = SchemaParser.ParseGlobalOption ( lines, "1.0" );

            //assert
            Assert.Equal ( "option", result.name );
            Assert.Equal ( "value", result.value );
        }

        [Fact]
        public void ParseGlobalOptions_Completed_Case2 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globaloption"
                }
            );

            //act
            var result = SchemaParser.ParseGlobalOption ( lines, "1.0" );

            //assert
            Assert.Empty ( result.name );
            Assert.Empty ( result.value );
        }

        [Fact]
        public void ParseGlobalOptions_Completed_Case3 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globaloption "
                }
            );

            //act
            var result = SchemaParser.ParseGlobalOption ( lines, "1.0" );

            //assert
            Assert.Empty ( result.name );
            Assert.Empty ( result.value );
        }

        [Fact]
        public void ParseSchema_Completed_Case1 () {
            //arrange
            var content =
"""
version 1.0

globalmethod testMethod
int32 parameter1
int64 parameter2

globalmethod test2Method
int32 parameter3
int64 parameter4
""";

            //act
            var result = SchemaParser.ParseSchema ( content );

            //assert
            Assert.Equal ( "1.0", result.Version );
            Assert.Equal ( 2, result.GlobalMethods.Count () );

            var firstGlobalMethod = result.GlobalMethods.First ();
            var secondGlobalMethod = result.GlobalMethods.Last ();

            Assert.Equal ( "testMethod", firstGlobalMethod.Name );
            var firstParameter = firstGlobalMethod.Parameters.First ();
            Assert.Equal ( "parameter1", firstParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstParameter.ParameterType );
            var secondParameter = firstGlobalMethod.Parameters.Last ();
            Assert.Equal ( "parameter2", secondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondParameter.ParameterType );

            Assert.Equal ( "test2Method", secondGlobalMethod.Name );
            var firstSecondParameter = secondGlobalMethod.Parameters.First ();
            Assert.Equal ( "parameter3", firstSecondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstSecondParameter.ParameterType );
            var secondSecondParameter = secondGlobalMethod.Parameters.Last ();
            Assert.Equal ( "parameter4", secondSecondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondSecondParameter.ParameterType );
        }


        [Fact]
        public void ParseSchema_Completed_ManySpace () {
            //arrange
            var content =
"""


version 1.0




globalmethod testMethod
int32 parameter1
int64 parameter2






globalmethod test2Method
int32 parameter3
int64 parameter4
""";

            //act
            var result = SchemaParser.ParseSchema ( content );

            //assert
            Assert.Equal ( "1.0", result.Version );
            Assert.Equal ( 2, result.GlobalMethods.Count () );

            var firstGlobalMethod = result.GlobalMethods.First ();
            var secondGlobalMethod = result.GlobalMethods.Last ();

            Assert.Equal ( "testMethod", firstGlobalMethod.Name );
            var firstParameter = firstGlobalMethod.Parameters.First ();
            Assert.Equal ( "parameter1", firstParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstParameter.ParameterType );
            var secondParameter = firstGlobalMethod.Parameters.Last ();
            Assert.Equal ( "parameter2", secondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondParameter.ParameterType );

            Assert.Equal ( "test2Method", secondGlobalMethod.Name );
            var firstSecondParameter = secondGlobalMethod.Parameters.First ();
            Assert.Equal ( "parameter3", firstSecondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, "" ), firstSecondParameter.ParameterType );
            var secondSecondParameter = secondGlobalMethod.Parameters.Last ();
            Assert.Equal ( "parameter4", secondSecondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, "" ), secondSecondParameter.ParameterType );
        }

    }

}
