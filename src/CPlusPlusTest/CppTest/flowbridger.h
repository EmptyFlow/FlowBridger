#ifndef FLOW_BRIDGER_H_
#define FLOW_BRIDGER_H_

#include <string>
#include <cmath>

#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __declspec(dllexport)
#elif defined(__GNUC__) || defined(__clang__)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __attribute__((visibility("default")))
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif

FLOWBRIDGER_DELEGATE_CALLTYPE double_t digital_method(int32_t int32Value, int64_t int64Value, float_t floatValue, double_t doubleValue, uint32_t uint32Value, uint64_t uint64Value);

FLOWBRIDGER_DELEGATE_CALLTYPE char* string_method(char* stringAnsi, wchar_t* stringUni);

#endif // FLOW_BRIDGER_H_