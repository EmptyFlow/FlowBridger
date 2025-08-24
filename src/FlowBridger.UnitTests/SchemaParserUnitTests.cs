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
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Unknown, ParsedContainerDataType.NotContainer ), result );
        }

        [Fact]
        public void GetDataType_Completed_Int32 () {
            //arrange
            var line = "int32";

            //act
            var result = SchemaParser.GetDataType ( line );

            //assert
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, ParsedContainerDataType.NotContainer ), result );
        }

        [Fact]
        public void GetDataType_Completed_Int32Array () {
            //arrange
            var line = "int32-array";

            //act
            var result = SchemaParser.GetDataType ( line );

            //assert
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, ParsedContainerDataType.Array ), result );
        }

        [Fact]
        public void ParseMethod_Completed_Case1 () {
            //arrange
            var lines = new DefaultSchemaLines (
                new List<string> {
                    "globalmethod testMethod",
                    "parameter1 int32",
                    "parameter2 int64"
                }
            );

            //act
            var result = SchemaParser.ParseMethod ( lines );

            //assert
            Assert.Equal ( "testMethod", result.Name );
            var firstParameter = result.Parameters.First ();
            Assert.Equal ( "parameter1", firstParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int32, ParsedContainerDataType.NotContainer ), firstParameter.DataType );
            var secondParameter = result.Parameters.Last ();
            Assert.Equal ( "parameter2", secondParameter.Name );
            Assert.Equal ( new DataTypeModel ( ParsedDataType.Int64, ParsedContainerDataType.NotContainer ), secondParameter.DataType );
        }

    }

}
