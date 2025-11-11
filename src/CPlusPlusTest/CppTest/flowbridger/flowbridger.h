#ifndef FLOW_BRIDGER_H_
#define FLOW_BRIDGER_H_

#include <string>
#include <cmath>

#if defined(_WIN32)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __declspec(dllimport)
#elif defined(__GNUC__) || defined(__clang__)
#define FLOWBRIDGER_DELEGATE_CALLTYPE __attribute__((visibility("default")))
#else
#define FLOWBRIDGER_DELEGATE_CALLTYPE
#endif

typedef void (*void_delegate)(int32_t int32Value, int64_t int64Value);
typedef int32_t (*int32_delegate)(int32_t int32Value, int64_t int64Value);
typedef void (*simple_delegate)(int32_t int32Value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE double_t digital_method(int32_t int32Value, int64_t int64Value, float_t floatValue, double_t doubleValue, uint32_t uint32Value, uint64_t uint64Value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE char* string_method(const char* stringAnsi, const wchar_t* stringUni);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE bool callback_method(void_delegate callbackWithVoid, int32_delegate callbackWithoutVoid);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE bool pointer_method(void* arrayPointer, int32_t length);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE simple_delegate callback_return_method();

#endif // FLOW_BRIDGER_H_