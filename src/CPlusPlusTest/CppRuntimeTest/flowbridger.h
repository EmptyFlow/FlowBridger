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

typedef double_t (FLOWBRIDGER_DELEGATE_CALLTYPE *digital_method)(int32_t int32Value, int64_t int64Value, float_t floatValue, double_t doubleValue, uint32_t uint32Value, uint64_t uint64Value);
typedef char* (FLOWBRIDGER_DELEGATE_CALLTYPE *string_method)(const char* stringAnsi, const wchar_t* stringUni);
typedef bool (FLOWBRIDGER_DELEGATE_CALLTYPE *callback_method)(void_delegate callbackWithVoid, int32_delegate callbackWithoutVoid);

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
    }

    digital_method digitalMethod = nullptr;
    string_method stringMethod = nullptr;
    callback_method callbackMethod = nullptr;
};

#endif // FLOW_BRIDGER_H_