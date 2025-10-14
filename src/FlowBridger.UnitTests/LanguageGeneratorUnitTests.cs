using FlowBridger.Enums;
using FlowBridger.Generators;
using FlowBridger.Models;

namespace FlowBridger.UnitTests {

    public class LanguageGeneratorUnitTests {

        [Fact]
        public void GenerateScheme_Completed_GlobalMethod_RuntimeLoading () {
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
                                ParameterType = new DataTypeModel(ParsedDataType.Float,  "")
                            },
                            new MethodParameterModel {
                                Name = "SecondParameter",
                                ParameterType = new DataTypeModel(ParsedDataType.StringUni,  "")
                            }
                        },
                        ReturnType = new DataTypeModel(ParsedDataType.Int64,  ""),
                    }
                ],
                GlobalDelegates = new List<MethodModel> (),
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

typedef int64_t (FLOWBRIDGER_DELEGATE_CALLTYPE *arg_argus)(float_t index, const wchar_t* secondParameter);

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

        argus = (argus)getExport(lib, "arg_argus");
    }

    argus argus = nullptr;
};

#endif // MYCLASSNAME_H
""".Replace ( "\r", "" );
            Assert.Equal ( correctContent, file.Content );
            //File.WriteAllText ( "C:/work/Experiments/cpptest/CppTest/file.h", file.Content );
        }

        [Fact]
        public void GenerateScheme_Completed_GlobalMethod_DynamicLinking () {
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
                                ParameterType = new DataTypeModel(ParsedDataType.Float,  "")
                            },
                            new MethodParameterModel {
                                Name = "SecondParameter",
                                ParameterType = new DataTypeModel(ParsedDataType.StringUni,  "")
                            }
                        },
                        ReturnType = new DataTypeModel(ParsedDataType.Int64,  ""),
                    }
                ],
                GlobalDelegates = new List<MethodModel> (),
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
            var correctContent =
"""
#ifndef MYCLASSNAME_H
#define MYCLASSNAME_H

#include <string>
#include <cmath>

#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __declspec(dllimport)
#elif defined(__GNUC__) || defined(__clang__)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __attribute__((visibility("default")))
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE int64_t arg_argus(float_t index, const wchar_t* secondParameter);

#endif // MYCLASSNAME_H
""".Replace ( "\r", "" );
            Assert.Equal ( correctContent, file.Content );
            //File.WriteAllText ( "C:/work/Experiments/cpptest/CppTest/file2.h", file.Content );
        }

        [Fact]
        public void GenerateScheme_Completed_GlobalMethod_CSharpImport () {
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
                                ParameterType = new DataTypeModel(ParsedDataType.Float,  "")
                            },
                            new MethodParameterModel {
                                Name = "SecondParameter",
                                ParameterType = new DataTypeModel(ParsedDataType.StringUni,  "")
                            }
                        },
                        ReturnType = new DataTypeModel(ParsedDataType.Int64,  ""),
                    }
                ],
                GlobalDelegates = new List<MethodModel> (),
                GlobalOptions = new Dictionary<string, string> {
                    ["CsFileName"] = "MyClass.cs",
                    ["CsNameSpace"] = "MyCustomNamespace",
                }
            };

            // act
            var generatedFiles = LanguageGenerator.GenerateScheme ( schema, ["Embedded.CSharp.Import"] );

            // assert
            Assert.Single ( generatedFiles );
            var file = generatedFiles.First ();
            Assert.Equal ( "MyClass.cs", file.FileName );
            var correctContent =
"""
using System.Runtime.InteropServices;
using System.Text;

namespace MyCustomNamespace {
    public static partial class FlowBridgerExports {
        private static List<byte> m_zeroBytes = [0, 0, 0, 0];

        private static string GetUniStringFromPointer ( nint pointer ) {
            if ( RuntimeInformation.IsOSPlatform ( OSPlatform.Linux ) || RuntimeInformation.IsOSPlatform ( OSPlatform.OSX ) ) {
                if ( pointer == nint.Zero ) return "";

                var buffer = new List<byte> ();
                var offset = 0;
                while ( true ) {
                    var readedByte = Marshal.ReadByte ( pointer, offset );
                    offset++;

                    buffer.Add ( readedByte );

                    if ( buffer.Count () % 4 == 0 && buffer[^4..].SequenceEqual ( m_zeroBytes ) ) break;
                }

                if ( buffer.Count () == 4 && buffer.SequenceEqual ( m_zeroBytes ) ) return "";

                return Encoding.UTF8.GetString ( buffer.ToArray () ).Replace ( "\u0000", "" );
            }

            return Marshal.PtrToStringUni ( pointer ) ?? "";
        }

        [UnmanagedCallersOnly ( EntryPoint = "arg_argus" )]
        public static long Argus ( float index, nint secondParameter ) {
            return ArgusInternal(index, GetUniStringFromPointer(secondParameter));
        }

        public static partial long ArgusInternal ( float index, string secondParameter );

    }
}
""".Replace ( "\r", "" );
            Assert.Equal ( correctContent, file.Content );
            //File.WriteAllText ( "C:/work/Repositories/FlowBridger/Repository/FlowBridger/src/FlowBridger/test.cs", file.Content );
        }

    }

}
