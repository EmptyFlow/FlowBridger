cmake_minimum_required (VERSION 3.8)

if (POLICY CMP0141)
  cmake_policy(SET CMP0141 NEW)
  set(CMAKE_MSVC_DEBUG_INFORMATION_FORMAT "$<IF:$<AND:$<C_COMPILER_ID:MSVC>,$<CXX_COMPILER_ID:MSVC>>,$<$<CONFIG:Debug,RelWithDebInfo>:EditAndContinue>,$<$<CONFIG:Debug,RelWithDebInfo>:ProgramDatabase>>")
endif()

project ("CppTest")

add_executable (CppTest "CppTest.cpp" "CppTest.h")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET CppTest PROPERTY CXX_STANDARD 20)
endif()

if (WIN32)
    target_link_libraries(AniLiberty PRIVATE kernel32)

    #target_include_directories(AniLiberty PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/windows-mpv)
    target_link_libraries(AniLiberty PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}/windows-mpv/libmpv.dll.lib")
endif ()
#if (APPLE)
#    target_link_libraries(AniLiberty PRIVATE dl)

#    target_include_directories(AniLiberty PRIVATE ${LIBMPV_INCLUDE_DIRS})
#    target_link_libraries(AniLiberty PUBLIC PkgConfig::LIBMPV)
#endif ()
#if (NOT (APPLE OR WIN32))
#    target_link_libraries(AniLiberty PRIVATE dl)

#    find_package(PkgConfig)
#    pkg_check_modules(LIBMPV REQUIRED IMPORTED_TARGET GLOBAL mpv)
#    target_include_directories(AniLiberty PRIVATE ${LIBMPV_INCLUDE_DIRS})
#    target_link_libraries(AniLiberty PUBLIC PkgConfig::LIBMPV)
#endif ()