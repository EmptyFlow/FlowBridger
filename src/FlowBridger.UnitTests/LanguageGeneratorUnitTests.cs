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
            var correctContent =
"""
#ifndef MYCLASSNAME_H
#define MYCLASSNAME_H

#include <string>
#include <cmath>
#include <cassert>

#if defined(_WIN32)
#include <windows.h>
#else
#include <dlfcn.h>
#endif

#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __stdcall
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif

typedef int64_t (FLOWBRIDGER_DELEGATE_CALLTYPE *argus)(float_t index, wchar_t* secondParameter);

class ImportFunctions {
private:
    void* loadLibrary(const std::wstring& path)
    {
#if defined(_WIN32)
        HMODULE h = ::LoadLibraryW(path.c_str());
        assert(h != nullptr);
        return (void*)h;
#else
        void *h = dlopen(path.c_str(), RTLD_LAZY | RTLD_LOCAL);
        assert(h != nullptr);
        return h;
#endif
    }

    void *getExport(void *h, const char *name)
    {
#if defined(_WIN32)
        void *f = ::GetProcAddress((HMODULE)h, name);
        assert(f != nullptr);
        return f;
#else
        void *f = dlsym(h, name);
        assert(f != nullptr);
        return f;
#endif
    }

public:
    ImportFunctions(const std::wstring& pathToLibrary) {
        void *lib = loadLibrary(pathToLibrary);

        argusmethod = (argus)getExport(lib, "arg_argus");
    }

    argus argusmethod = nullptr;
};

#endif // MYCLASSNAME_H
""".Replace("\r", "");
            Assert.Equal ( correctContent, file.Content );
            //File.WriteAllText ( "C:/work/Experiments/cpptest/CppTest/file.h", file.Content );
        }

        [Fact]
        public void GenerateScheme_Completed_GlobalMethod_Case2 () {
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
            var generatedFiles = LanguageGenerator.GenerateScheme ( schema, ["Embedded.Cpp.DynamicLinking"] );

            // assert
            Assert.Single ( generatedFiles );
            var file = generatedFiles.First ();
            Assert.Equal ( "myclassname.h", file.FileName );
            //File.WriteAllText ( "C:/work/Experiments/cpptest/CppTest/file2.h", file.Content );
        }

    }

}
