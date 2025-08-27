using FlowBridger.Enums;
using FlowBridger.Generators;
using FlowBridger.Models;

namespace FlowBridger.UnitTests {

    public class LanguageGeneratorUnitTests {

        [Fact]
        public void GenerateScheme_Completed_GlobalMethod_Case1 () {
            // arrange
            var schema = new SchemaModel {
                Version = "1.0",
                GlobalMethods = [
                    new MethodModel {
                        Name = "Argus",
                        Options = new Dictionary<string, string> {
                            ["CppNamespace"] = "librarynamespace"
                        },
                        ReturnType = new DataTypeModel(ParsedDataType.Int64, ParsedContainerDataType.NotContainer),
                    }
                ]
            };

            // act
            var generatedFiles = LanguageGenerator.GenerateScheme ( schema, ["Embedded.Cpp.DynamicLinking"] );

            // assert
            Assert.Single ( generatedFiles );
            var file = generatedFiles.First ();
            Assert.Equal ( "", file.FileName );
        }

    }

}
