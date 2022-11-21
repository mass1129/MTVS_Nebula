//
// Created by LXH on 2021/5/10.
//

#ifndef IRIS_BASE_H_
#define IRIS_BASE_H_

#include <stdbool.h>
#include <stddef.h>
#if defined(__APPLE__)
#include <TargetConditionals.h>
#endif

#ifdef __cplusplus
#define EXTERN_C extern "C"
#define EXTERN_C_ENTER extern "C" {
#define EXTERN_C_LEAVE }
#else
#define EXTERN_C
#define EXTERN_C_ENTER
#define EXTERN_C_LEAVE
#endif

#ifdef __GNUC__
#define AGORA_GCC_VERSION_AT_LEAST(x, y)                                       \
  (__GNUC__ > (x) || __GNUC__ == (x) && __GNUC_MINOR__ >= (y))
#else
#define AGORA_GCC_VERSION_AT_LEAST(x, y) 0
#endif

#if defined(_WIN32)
#define IRIS_CALL __cdecl
#if defined(IRIS_EXPORT)
#define IRIS_API EXTERN_C __declspec(dllexport)
#define IRIS_CPP_API __declspec(dllexport)
#else
#define IRIS_API EXTERN_C __declspec(dllimport)
#define IRIS_CPP_API __declspec(dllimport)
#endif
#elif defined(__APPLE__)
#if AGORA_GCC_VERSION_AT_LEAST(3, 3)
#define IRIS_API __attribute__((visibility("default"))) EXTERN_C
#define IRIS_CPP_API __attribute__((visibility("default")))
#else
#define IRIS_API EXTERN_C
#define IRIS_CPP_API
#endif
#define IRIS_CALL
#elif defined(__ANDROID__) || defined(__linux__)
#if AGORA_GCC_VERSION_AT_LEAST(3, 3)
#define IRIS_API EXTERN_C __attribute__((visibility("default")))
#define IRIS_CPP_API __attribute__((visibility("default")))
#else
#define IRIS_API EXTERN_C
#define IRIS_CPP_API
#endif
#define IRIS_CALL
#else
#define IRIS_API EXTERN_C
#define IRIS_CPP_API
#define IRIS_CALL
#endif

#if AGORA_GCC_VERSION_AT_LEAST(3, 1)
#define IRIS_DEPRECATED __attribute__((deprecated))
#elif defined(_MSC_VER)
#define IRIS_DEPRECATED
#else
#define IRIS_DEPRECATED
#endif

#if defined(IRIS_DEBUG)
#define IRIS_DEBUG_API IRIS_API
#define IRIS_DEBUG_CPP_API IRIS_CPP_API
#else
#define IRIS_DEBUG_API
#define IRIS_DEBUG_CPP_API
#endif

EXTERN_C_ENTER

const int kBasicResultLength = 64 * 1024;
const int kBasicStringLength = 512;

typedef enum IrisAppType {
  kAppTypeNative = 0,
  kAppTypeCocos = 1,
  kAppTypeUnity = 2,
  kAppTypeElectron = 3,
  kAppTypeFlutter = 4,
  kAppTypeUnreal = 5,
  kAppTypeXamarin = 6,
  kAppTypeApiCloud = 7,
  kAppTypeReactNative = 8,
  kAppTypePython = 9,
  kAppTypeCocosCreator = 10,
  kAppTypeRust = 11,
  kAppTypeCSharp = 12,
  kAppTypeCef = 13,
  kAppTypeUniApp = 14,
} IrisAppType;

typedef enum IrisLogLevel {
  levelTrace,
  levelDebug,
  levelInfo,
  levelWarn,
  levelErr,
} IrisLogLevel;

IRIS_API void enableUseJsonArray(bool enable);

IRIS_API void InitIrisLogger(const char *path, int maxSize, IrisLogLevel level);

typedef void(IRIS_CALL *Func_Event)(const char *event, const char *data,
                                    const void **buffer, unsigned int *length,
                                    unsigned int buffer_count);

typedef void(IRIS_CALL *Func_EventEx)(const char *event, const char *data,
                                      char result[kBasicResultLength],
                                      const void **buffer, unsigned int *length,
                                      unsigned int buffer_count);

typedef struct IrisCEventHandler {
  Func_Event OnEvent;
  Func_EventEx OnEventEx;
} IrisCEventHandler;

typedef void *IrisEventHandlerHandle;
typedef void *IrisVideoFrameBufferDelegateHandle;

typedef void *IrisVideoFrameBufferManagerPtr;

EXTERN_C_LEAVE

#endif//IRIS_BASE_H_
