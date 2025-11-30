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
typedef void (*event_click1_callback)(int32_t eventId);
typedef void (*event_click3_callback)(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE double_t digital_method(int32_t int32Value, int64_t int64Value, float_t floatValue, double_t doubleValue, uint32_t uint32Value, uint64_t uint64Value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE char* string_method(const char* stringAnsi, const wchar_t* stringUni);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE bool callback_method(void_delegate callbackWithVoid, int32_delegate callbackWithoutVoid);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE bool pointer_method(void* arrayPointer, int32_t length);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE simple_delegate callback_return_method();

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click1_set_order(int32_t eventId, int32_t value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click1_set_name(int32_t eventId, const char* value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE int32_t event_click1_create();

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click1_complete_set(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE int32_t event_click1_get_order(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE char* event_click1_get_name(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE event_click1_callback event_click1_callback_get(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click1_complete_get(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click2_set_order(int32_t eventId, int32_t value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click2_set_items(int32_t eventId, int32_t value);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE int32_t event_click2_create();

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click2_complete_set(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE int32_t event_click3_get_order(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE int32_t event_click3_get_items(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE event_click3_callback event_click3_callback_get(int32_t eventId);

extern "C" FLOWBRIDGER_DELEGATE_CALLTYPE void event_click3_complete_get(int32_t eventId);

#endif // FLOW_BRIDGER_H_