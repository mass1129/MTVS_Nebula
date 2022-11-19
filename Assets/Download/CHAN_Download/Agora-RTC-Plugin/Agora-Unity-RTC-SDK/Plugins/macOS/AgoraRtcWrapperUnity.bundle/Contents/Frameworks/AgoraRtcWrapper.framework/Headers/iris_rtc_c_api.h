//
// Created by LXH on 2021/1/14.
//

#ifndef IRIS_RTC_C_API_H_
#define IRIS_RTC_C_API_H_

#include "iris_rtc_base.h"

EXTERN_C_ENTER

typedef enum IRIS_API_ERROR_CODE_TYPE {
  IRIS_API_NOT_CREATE = 666666,
} IRIS_API_ERROR_CODE_TYPE;

typedef void *IrisApiEnginePtr;

IRIS_API void *IRIS_CALL GetObserver(IrisApiEnginePtr engine_ptr,
                                     const char *api_type);

IRIS_API void *IRIS_CALL CreateObserver(IrisApiEnginePtr engine_ptr,
                                        const char *api_type,
                                        IrisEventHandlerHandle handle,
                                        const char *params,
                                        uint32_t paramLength);

IRIS_API void IRIS_CALL DestroyObserver(IrisApiEnginePtr engine_ptr,
                                        const char *api_type, void *handle);

IRIS_API int IRIS_CALL CallIrisApi(IrisApiEnginePtr engine_ptr,
                                   const char *func_name, const char *params,
                                   uint32_t paramLength, void **buffer,
                                   uint32_t bufferLength,
                                   char result[kBasicResultLength]);

/// IrisRtcEngine

IRIS_API IrisApiEnginePtr IRIS_CALL CreateIrisApiEngine();

IRIS_API void IRIS_CALL DestroyIrisApiEngine(IrisApiEnginePtr engine_ptr);

IRIS_API IrisEventHandlerHandle IRIS_CALL
CreateIrisEventHandler(IrisCEventHandler *event_handler);

IRIS_API void IRIS_CALL DestroyIrisEventHandler(IrisEventHandlerHandle handler);

IRIS_API IrisEventHandlerHandle IRIS_CALL SetIrisRtcEngineEventHandler(
    IrisApiEnginePtr engine_ptr, IrisCEventHandler *event_handler);

IRIS_API int IRIS_CALL UnsetIrisRtcEngineEventHandler(
    IrisApiEnginePtr engine_ptr, IrisEventHandlerHandle handle);

/// IrisRtcRawData

IRIS_API IrisRtcAudioFrameObserverHandle IRIS_CALL RegisterAudioFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisRtcCAudioFrameObserver *observer,
    int order, const char *identifier);

IRIS_API int IRIS_CALL UnRegisterAudioFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisRtcAudioFrameObserverHandle handle,
    const char *identifier);

IRIS_API IrisRtcVideoFrameObserverHandle IRIS_CALL RegisterVideoFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisRtcCVideoFrameObserver *observer,
    int order, const char *identifier);

IRIS_API int IRIS_CALL UnRegisterVideoFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisRtcVideoFrameObserverHandle handle,
    const char *identifier);

IRIS_API IrisRtcVideoEncodedVideoFrameObserverHandle IRIS_CALL
RegisterVideoEncodedFrameObserver(
    IrisApiEnginePtr engine_ptr,
    IrisRtcCVideoEncodedVideoFrameObserver *observer, int order,
    const char *identifier);

IRIS_API int IRIS_CALL UnRegisterVideoEncodedFrameObserver(
    IrisApiEnginePtr engine_ptr,
    IrisRtcVideoEncodedVideoFrameObserverHandle handle, const char *identifier);

IRIS_API IrisAudioEncodedFrameObserverHandle IRIS_CALL
RegisterAudioEncodedFrameObserver(IrisApiEnginePtr engine_ptr,
                                  IrisCAudioEncodedFrameObserver *observer,
                                  const char *params);

IRIS_API int IRIS_CALL UnRegisterAudioEncodedFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisAudioEncodedFrameObserverHandle handle,
    const char *params);

IRIS_API IrisEventHandlerHandle IRIS_CALL RegisterAudioEncodedFrameObserverEx(
    IrisApiEnginePtr engine_ptr, IrisAudioEncodedFrameObserverHandle *handle,
    const char *params);

IRIS_API int IRIS_CALL UnRegisterAudioEncodedFrameObserverEx(
    IrisApiEnginePtr engine_ptr, IrisAudioEncodedFrameObserverHandle *handle);

IRIS_API IrisMediaMetadataObserverHandle IRIS_CALL
RegisterMediaMetadataObserver(IrisApiEnginePtr engine_ptr,
                              IrisCMediaMetadataObserver *observer,
                              const char *params);

IRIS_API int IRIS_CALL UnRegisterMediaMetadataObserver(
    IrisApiEnginePtr engine_ptr, IrisMediaMetadataObserverHandle handle,
    const char *params);

IRIS_API int IRIS_CALL Attach(IrisApiEnginePtr engine_ptr,
                              IrisVideoFrameBufferManagerPtr manager_ptr);

IRIS_API int IRIS_CALL Detach(IrisApiEnginePtr engine_ptr,
                              IrisVideoFrameBufferManagerPtr manager_ptr);

/// IrisRtcRawDataPluginManager

IRIS_API int IRIS_CALL CallIrisRtcRawDataPluginManagerApi(
    IrisApiEnginePtr engine_ptr, enum ApiTypeRawDataPluginManager api_type,
    const char *params, char result[kBasicStringLength]);

/// IrisMediaPlayerPtr
IRIS_API IrisEventHandlerHandle IRIS_CALL SetIrisMediaPlayerEventHandler(
    IrisApiEnginePtr engine_ptr, IrisCEventHandler *event_handler);

IRIS_API int IRIS_CALL UnsetIrisMediaPlayerEventHandler(
    IrisApiEnginePtr engine_ptr, IrisEventHandlerHandle handle);

IRIS_API IrisEventHandlerHandle IRIS_CALL SetIrisMediaRecorderEventHandler(
    IrisApiEnginePtr engine_ptr, IrisCEventHandler *event_handler);
IRIS_API int IRIS_CALL UnsetIrisMediaRecorderEventHandler(
    IrisApiEnginePtr engine_ptr, IrisEventHandlerHandle handle);
/**
 * media player audio frame observer 
 */
IRIS_API IrisMediaPlayerAudioFrameObserverHandle IRIS_CALL
RegisterMediaPlayerAudioFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisMediaPlayerCAudioFrameObserver *observer,
    const char *params);

IRIS_API int IRIS_CALL UnRegisterMediaPlayerAudioFrameObserver(
    IrisApiEnginePtr engine_ptr, IrisMediaPlayerAudioFrameObserverHandle handle,
    const char *params);

IRIS_API IrisRtcAudioSpectrumObserverHandle IRIS_CALL
RegisterRtcAudioSpectrumObserver(IrisApiEnginePtr engine_ptr,
                                 IrisRtcCAudioSpectrumObserver *observer,
                                 const char *params);

IRIS_API int IRIS_CALL UnRegisterRtcAudioSpectrumObserver(
    IrisApiEnginePtr engine_ptr, IrisRtcAudioSpectrumObserverHandle handle,
    const char *params);

/**
 * media player audio spectrum observer 
 */
IRIS_API IrisMediaPlayerAudioSpectrumObserverHandle IRIS_CALL
RegisterMediaPlayerAudioSpectrumObserver(
    IrisApiEnginePtr engine_ptr,
    IrisMediaPlayerCAudioSpectrumObserver *observer, const char *params);

IRIS_API int IRIS_CALL UnRegisterMediaPlayerAudioSpectrumObserver(
    IrisApiEnginePtr engine_ptr,
    IrisMediaPlayerAudioSpectrumObserverHandle handle, const char *params);

IRIS_API
IrisMediaPlayerCustomDataProviderHandle IRIS_CALL
MediaPlayerOpenWithCustomSource(IrisApiEnginePtr engine_ptr,
                                IrisMediaPlayerCCustomDataProvider *provider,
                                const char *params);

IRIS_API int IRIS_CALL MediaPlayerUnOpenWithCustomSource(
    IrisApiEnginePtr engine_ptr, IrisMediaPlayerCustomDataProviderHandle handle,
    const char *params);

IRIS_API
IrisMediaPlayerCustomDataProviderHandle IRIS_CALL
MediaPlayerOpenWithMediaSource(IrisApiEnginePtr engine_ptr,
                               IrisMediaPlayerCCustomDataProvider *provider,
                               const char *params);

IRIS_API
IrisMediaPlayerCustomDataProviderHandle IRIS_CALL
MediaPlayerOpenWithMediaSource(IrisApiEnginePtr engine_ptr,
                               IrisMediaPlayerCCustomDataProvider *provider,
                               const char *params);

IRIS_API int IRIS_CALL MediaPlayerUnOpenWithMediaSource(
    IrisApiEnginePtr engine_ptr, IrisMediaPlayerCustomDataProviderHandle handle,
    const char *params);
/// IrisCloudSpatialAudioEnginePtr

IRIS_API IrisEventHandlerHandle IRIS_CALL SetIrisCloudAudioEngineEventHandler(
    IrisApiEnginePtr engine_ptr, IrisCEventHandler *event_handler);

IRIS_API int IRIS_CALL UnsetIrisCloudAudioEngineEventHandler(
    IrisApiEnginePtr engine_ptr, IrisEventHandlerHandle *handle);

EXTERN_C_LEAVE

#endif//IRIS_RTC_C_API_H_
