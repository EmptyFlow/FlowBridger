#include <iostream>
#include <string>
#include "flowbridger.h"

int main()
{
	auto pathToLibrary = L"C:/work/Repositories/FlowBridger/Repository/FlowBridger/src/CPlusPlusTest/CppTest/flowbridger/FlowBridgerTestLibrary.dll";
	std::wstring path(pathToLibrary);
	ImportFunctions functions(path);
	std::cout << "Start test program for binding Embedded.Cpp.DynamicLinking." << std::endl;

	return 0;
}