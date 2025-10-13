#include "CppTest.h"
#include <flowbridger.h>

using namespace std;

bool voidDelegateTestCalled = false;

void VoidDelegateTest(int32_t int32Value, int64_t int64Value) {
	cout << "VoidDelegateTest called!";
	voidDelegateTestCalled = true;
}

int32_t NonVoidDelegateTest(int32_t int32Value, int64_t int64Value) {
	cout << "NonVoidDelegateTest called!";
	voidDelegateTestCalled = true;
	return 10;
}

int main()
{
	cout << "Start test program for binding Embedded.Cpp.DynamicLinking." << endl;

	bool isHasErrors = false;

	// digital_method Test
	auto value = digital_method(1, 1000, 0.5f, 10.89, 256, 5559);
	cout << "digital_method" << (value == 138.890 ? "Value is correct." : "Value is incorrect.") << endl << endl;
	if (value != 138.890) isHasErrors = true;

	// string_method Test

	wstring uniStr(L"Lalalala1212");
	string ansiStr("lalalalal");
	auto stringOutput = string_method(ansiStr.c_str(), uniStr.c_str());
	string testValue = "return string123";
	string str(stringOutput);
	cout << "string_method" << (stringOutput == testValue ? "Value is correct." : "Value is incorrect.") << endl << endl;
	if (stringOutput != testValue) isHasErrors = true;

	// callback_method Test
	auto callBackResult = callback_method(VoidDelegateTest, NonVoidDelegateTest);
	if (!callBackResult || !voidDelegateTestCalled) {
		cout << "callback_method result " << (callBackResult ? "correct" : "incorrect") << endl;
		cout << "voidDelegateTestCalled result " << (voidDelegateTestCalled ? "correct" : "incorrect") << endl;
		isHasErrors = true;
	}
	else {
		cout << "callback_method result is correct." << endl;
	}

	// pointer_method Test
	int myArray[5] = { 500, 3489, 126890, 565767, 984545 };
	int* myArrayPointer = myArray;
	auto pointerResult = pointer_method(myArrayPointer, 5);
	cout << "pointer_method result " << (pointerResult ? "correct" : "incorrect") << endl;
	if (!pointerResult) isHasErrors = true;
	
	return isHasErrors ? 1 : 0;
}