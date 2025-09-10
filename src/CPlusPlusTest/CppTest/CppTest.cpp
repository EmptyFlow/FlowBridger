#include "CppTest.h"
#include <flowbridger.h>

using namespace std;

int main()
{
	cout << "Start test program for binding Embedded.Cpp.DynamicLinking." << endl;

	bool isHasErrors = false;

	auto value = digital_method(1, 1000, 0.5f, 10.89, 256, 5559);
	cout << "digital_method" << (value == 138.890 ? "Value is correct." : "Value is incorrect.") << endl;
	if (value != 138.890) isHasErrors = true;

	wstring uniStr(L"Lalalala1212");
	string ansiStr("lalalalal");
	auto stringOutput = string_method(ansiStr.c_str(), uniStr.c_str());
	string testValue = "return string123";
	string str(stringOutput);
	cout << "string_method" << (stringOutput == testValue ? "Value is correct." : "Value is incorrect.") << endl;
	if (stringOutput != testValue) isHasErrors = true;

	return isHasErrors ? 1 : 0;
}
