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
                            ["Namespace"] = "arg"
                        },
                        Parameters = new List<MethodParameterModel> {
                            new MethodParameterModel {
                                Name = "Index",
                                ParameterType = new DataTypeModel(ParsedDataType.Float, ParsedContainerDataType.NotContainer)
                            },
                            new MethodParameterModel {
                                Name = "SecondParameter",
                                ParameterType = new DataTypeModel(ParsedDataType.StringUni, ParsedContainerDataType.NotContainer)
                            }
                        },
                        ReturnType = new DataTypeModel(ParsedDataType.Int64, ParsedContainerDataType.NotContainer),
                    }
                ],
                GlobalOptions = new Dictionary<string, string> {
                    ["CppFileName"] = "myclassname.h"
                }
            };

            // act
            var generatedFiles = LanguageGenerator.GenerateScheme ( schema, ["Embedded.Cpp.RuntimeLoading"] );

            // assert
            Assert.Single ( generatedFiles );
            var file = generatedFiles.First ();
            Assert.Equal ( "myclassname.h", file.FileName );
            Assert.Equal ( "", file.Content );
        }

    }

}
