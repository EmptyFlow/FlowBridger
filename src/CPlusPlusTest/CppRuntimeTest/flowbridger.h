#ifndef FLOW_BRIDGER_H_
#define FLOW_BRIDGER_H_

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

typedef void (*void_delegate)(int32_t int32Value, int64_t int64Value);
typedef int32_t (*int32_delegate)(int32_t int32Value, int64_t int64Value);
typedef void (*simple_delegate)(int32_t int32Value);
typedef void (*event_click1_callback)(int32_t eventId);
typedef void (*event_click3_callback)(int32_t eventId);

typedef double_t (FLOWBRIDGER_DELEGATE_CALLTYPE *digital_method)(int32_t int32Value, int64_t int64Value, float_t floatValue, double_t doubleValue, uint32_t uint32Value, uint64_t uint64Value);
typedef char* (FLOWBRIDGER_DELEGATE_CALLTYPE *string_method)(const char* stringAnsi, const wchar_t* stringUni);
typedef bool (FLOWBRIDGER_DELEGATE_CALLTYPE *callback_method)(void_delegate callbackWithVoid, int32_delegate callbackWithoutVoid);
typedef bool (FLOWBRIDGER_DELEGATE_CALLTYPE *pointer_method)(void* arrayPointer, int32_t length);
typedef simple_delegate (FLOWBRIDGER_DELEGATE_CALLTYPE *callback_return_method)();
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_set_order)(int32_t eventId, int32_t value);
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_set_name)(int32_t eventId, const char* value);
typedef int32_t (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_create)();
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_complete_set)(int32_t eventId);
typedef int32_t (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_get_order)(int32_t eventId);
typedef char* (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_get_name)(int32_t eventId);
typedef event_click1_callback (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_callback_get)(int32_t eventId);
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click1_complete_get)(int32_t eventId);
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click2_set_order)(int32_t eventId, int32_t value);
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click2_set_items)(int32_t eventId, int32_t value);
typedef int32_t (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click2_create)();
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click2_complete_set)(int32_t eventId);
typedef int32_t (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click3_get_order)(int32_t eventId);
typedef int32_t (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click3_get_items)(int32_t eventId);
typedef event_click3_callback (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click3_callback_get)(int32_t eventId);
typedef void (FLOWBRIDGER_DELEGATE_CALLTYPE *event_click3_complete_get)(int32_t eventId);

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

        digitalMethod = (digital_method)getExport(lib, "digital_method");
        stringMethod = (string_method)getExport(lib, "string_method");
        callbackMethod = (callback_method)getExport(lib, "callback_method");
        pointerMethod = (pointer_method)getExport(lib, "pointer_method");
        callbackReturnMethod = (callback_return_method)getExport(lib, "callback_return_method");
        eventClick1SetOrder = (event_click1_set_order)getExport(lib, "event_click1_set_order");
        eventClick1SetName = (event_click1_set_name)getExport(lib, "event_click1_set_name");
        eventClick1Create = (event_click1_create)getExport(lib, "event_click1_create");
        eventClick1CompleteSet = (event_click1_complete_set)getExport(lib, "event_click1_complete_set");
        eventClick1GetOrder = (event_click1_get_order)getExport(lib, "event_click1_get_order");
        eventClick1GetName = (event_click1_get_name)getExport(lib, "event_click1_get_name");
        eventClick1CallbackGet = (event_click1_callback_get)getExport(lib, "event_click1_callback_get");
        eventClick1CompleteGet = (event_click1_complete_get)getExport(lib, "event_click1_complete_get");
        eventClick2SetOrder = (event_click2_set_order)getExport(lib, "event_click2_set_order");
        eventClick2SetItems = (event_click2_set_items)getExport(lib, "event_click2_set_items");
        eventClick2Create = (event_click2_create)getExport(lib, "event_click2_create");
        eventClick2CompleteSet = (event_click2_complete_set)getExport(lib, "event_click2_complete_set");
        eventClick3GetOrder = (event_click3_get_order)getExport(lib, "event_click3_get_order");
        eventClick3GetItems = (event_click3_get_items)getExport(lib, "event_click3_get_items");
        eventClick3CallbackGet = (event_click3_callback_get)getExport(lib, "event_click3_callback_get");
        eventClick3CompleteGet = (event_click3_complete_get)getExport(lib, "event_click3_complete_get");
    }

    digital_method digitalMethod = nullptr;
    string_method stringMethod = nullptr;
    callback_method callbackMethod = nullptr;
    pointer_method pointerMethod = nullptr;
    callback_return_method callbackReturnMethod = nullptr;
    event_click1_set_order eventClick1SetOrder = nullptr;
    event_click1_set_name eventClick1SetName = nullptr;
    event_click1_create eventClick1Create = nullptr;
    event_click1_complete_set eventClick1CompleteSet = nullptr;
    event_click1_get_order eventClick1GetOrder = nullptr;
    event_click1_get_name eventClick1GetName = nullptr;
    event_click1_callback_get eventClick1CallbackGet = nullptr;
    event_click1_complete_get eventClick1CompleteGet = nullptr;
    event_click2_set_order eventClick2SetOrder = nullptr;
    event_click2_set_items eventClick2SetItems = nullptr;
    event_click2_create eventClick2Create = nullptr;
    event_click2_complete_set eventClick2CompleteSet = nullptr;
    event_click3_get_order eventClick3GetOrder = nullptr;
    event_click3_get_items eventClick3GetItems = nullptr;
    event_click3_callback_get eventClick3CallbackGet = nullptr;
    event_click3_complete_get eventClick3CompleteGet = nullptr;
};

#endif // FLOW_BRIDGER_H_