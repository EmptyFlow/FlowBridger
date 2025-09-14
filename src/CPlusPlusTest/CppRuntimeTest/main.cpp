#include <iostream>
#include <string>
#include "flowbridger.h"

bool voidDelegateTestCalled = false;

void VoidDelegateTest(int32_t int32Value, int64_t int64Value) {
	std::cout << "VoidDelegateTest called!";
	voidDelegateTestCalled = true;
}

int32_t NonVoidDelegateTest(int32_t int32Value, int64_t int64Value) {
	std::cout << "NonVoidDelegateTest called!";
	voidDelegateTestCalled = true;
	return 10;
}

int main()
{
	std::cout << "Start test program for binding Embedded.Cpp.DynamicLinking." << std::endl;

	auto pathToLibrary = L"C:/work/Repositories/FlowBridger/Repository/FlowBridger/src/CPlusPlusTest/CppTest/flowbridger/FlowBridgerTestLibrary.dll";
	std::wstring path(pathToLibrary);
	ImportFunctions functions(path);

	int isHasErrors = false;

	// digital_method Test

	auto value = functions.digitalMethod(1, 1000, 0.5f, 10.89, 256, 5559);
	std::cout << "digital_method" << (value == 138.890 ? "Value is correct." : "Value is incorrect.") << std::endl << std::endl;
	if (value != 138.890) isHasErrors = true;

	// string_method Test

	std::wstring uniStr(L"Lalalala1212");
	std::string ansiStr("lalalalal");
	auto stringOutput = functions.stringMethod(ansiStr.c_str(), uniStr.c_str());
	std::string testValue = "return string123";
	std::string str(stringOutput);
	std::cout << "string_method" << (stringOutput == testValue ? "Value is correct." : "Value is incorrect.") << std::endl << std::endl;
	if (stringOutput != testValue) isHasErrors = true;

	// callback_method Test

	auto callBackResult = functions.callbackMethod(VoidDelegateTest, NonVoidDelegateTest);
	if (!callBackResult || !voidDelegateTestCalled) {
		std::cout << "callback_method result " << (callBackResult ? "correct" : "incorrect") << std::endl;
		std::cout << "voidDelegateTestCalled result " << (voidDelegateTestCalled ? "correct" : "incorrect") << std::endl;
		isHasErrors = true;
	}
	else {
		std::cout << "callback_method result is correct." << std::endl;
	}

	return isHasErrors ? 1 : 0;
}