// CppTest.cpp : Defines the entry point for the application.
//

#include "CppTest.h"
#include <flowbridger.h>

using namespace std;

int main()
{
	cout << "Pizda." << endl;
	auto value = digital_method(1, 1000, 0.5f, 10.89, 256, 5559);
	if (value == 138.890) cout << "Value is correct." << endl;

	cout << "Hello CMake." << endl;
	return 0;
}
