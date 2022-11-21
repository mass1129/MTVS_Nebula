//
// Created by LXH on 2021/1/14.
//

#ifndef IRIS_RTC_BASE_H_
#define IRIS_RTC_BASE_H_

#include "iris_api_type.h"
#include "iris_base.h"
#include "iris_media_base_c.h"
#include "iris_video_processor_c.h"

EXTERN_C_ENTER

typedef enum ApiTypeRawDataPluginManager {
  kRDPMRegisterPlugin = 0,
  kRDPMUnregisterPlugin = 1,
  kRDPMHasPlugin = 2,
  kRDPMEnablePlugin = 3,
  kRDPMGetPlugins = 4,
  kRDPMSetPluginParameter = 5,
  kRDPMGetPluginParameter = 6,
  kRDPMRelease = 7,
} ApiTypeRawDataPluginManager;

typedef enum VideoObserverPosition {
  kPositionPostCapturer = 1 << 0,
  kPositionPreRenderer = 1 << 1,
  kPositionPreEncoder = 1 << 2,
} VideoObserverPosition;

typedef struct IrisEncodedVideoFrameInfo {
  int codecType;
  int width;
  int height;
  int framesPerSecond;
  int frameType;
  int rotation;
  int trackId;
  //int64_t decodeTimeMs;
  int64_t captureTimeMs;
  unsigned int uid;
  int streamType;
} IrisEncodedVideoFrameInfo;

typedef enum IRIS_BYTES_PER_SAMPLE {
  IRIS_TWO_BYTES_PER_SAMPLE = 2,
} IRIS_BYTES_PER_SAMPLE;

typedef struct IrisAudioPcmFrame {
  enum {
    kMaxDataSizeSamples = 3840,
    kMaxDataSizeBytes = 3840 * 2,
  };

  uint32_t capture_timestamp;
  uint64_t samples_per_channel_;
  int sample_rate_hz_;
  IRIS_BYTES_PER_SAMPLE bytes_per_sample;
  uint64_t num_channels_;
  int16_t data_[kMaxDataSizeSamples];
} IrisAudioPcmFrame;

typedef struct IrisAudioSpectrumData {
  const float *audioSpectrumData;
  int dataLength;
} IrisAudioSpectrumData;

typedef struct IrisUserAudioSpectrumInfo {
  unsigned int uid;
  struct IrisAudioSpectrumData spectrumData;
} IrisUserAudioSpectrumInfo;

typedef struct FuncParams {
  const char *buffer;
  size_t buffer_length;
} FuncParams;

// callbck
typedef bool(IRIS_CALL *Func_Bool)();
typedef uint32_t(IRIS_CALL *Func_Uint32_t)();
typedef IRIS_VIDEO_PIXEL_FORMAT(IRIS_CALL *Func_VideoPixelFormat)();
typedef bool(IRIS_CALL *Func_AudioFrameLocal)(const char *channelId,
                                              IrisAudioFrame *audio_frame);
typedef bool(IRIS_CALL *Func_AudioFrameRemote)(const char *channel_id,
                                               unsigned int uid,
                                               IrisAudioFrame *audio_frame);
typedef bool(IRIS_CALL *Func_AudioFrameRemoteStringUid)(
    const char *channel_id, const char *uid, IrisAudioFrame *audio_frame);
typedef bool(IRIS_CALL *Func_AudioFrameEx)(const char *channel_id,
                                           unsigned int uid,
                                           IrisAudioFrame *audio_frame);
//typedef bool(IRIS_CALL *Func_AudioFrame)(IrisAudioFrame *audio_frame);

typedef IrisAudioParams(IRIS_CALL *Func_AudioParams)();
typedef int(IRIS_CALL *Func_AudioFramePosition)();

typedef struct IrisRtcCAudioFrameObserver {
  Func_AudioFrameLocal OnRecordAudioFrame;
  Func_AudioFrameLocal OnPlaybackAudioFrame;
  Func_AudioFrameLocal OnMixedAudioFrame;
  //Func_AudioFrame OnEarMonitoringAudioFrame;
  Func_AudioFrameRemote OnPlaybackAudioFrameBeforeMixing;
  Func_AudioFrameRemoteStringUid OnPlaybackAudioFrameBeforeMixing2;
  Func_AudioParams GetPlaybackAudioParams;
  Func_AudioParams GetRecordAudioParams;
  Func_AudioParams GetMixedAudioParams;
  Func_AudioFramePosition GetObservedAudioFramePosition;
} IrisRtcCAudioFrameObserver;
typedef void *IrisRtcAudioFrameObserverHandle;

typedef bool(IRIS_CALL *Func_VideoFrameLocal)(IrisVideoFrame *video_frame);
typedef bool(IRIS_CALL *Func_VideoCaptureLocal)(
    IrisVideoFrame *video_frame, const IrisVideoFrameBufferConfig *config);
typedef bool(IRIS_CALL *Func_VideoFrameRemote)(const char *channel_id,
                                               unsigned int uid,
                                               IrisVideoFrame *video_frame);

typedef bool(IRIS_CALL *Func_AudioOnFrame)(IrisAudioPcmFrame *audio_frame,
                                           int mediaPlayerId);

typedef int64_t(IRIS_CALL *Func_OnSeek)(int64_t offset, int whence,
                                        int playerId);
typedef int(IRIS_CALL *Func_onReadData)(unsigned char *buffer, int bufferSize,
                                        int playerId);

typedef bool(IRIS_CALL *Func_LocalAudioSpectrum)(
    int playerId, const IrisAudioSpectrumData *data);
typedef bool(IRIS_CALL *Func_RemoteAudioSpectrum)(
    int playerId, const IrisUserAudioSpectrumInfo *audio_frame,
    unsigned int spectrumNumber);

typedef struct IrisMediaPlayerCAudioFrameObserver {
  Func_AudioOnFrame onFrame;
} IrisMediaPlayerCAudioFrameObserver;
typedef void *IrisMediaPlayerAudioFrameObserverHandle;

typedef struct IrisMediaPlayerCCustomDataProvider {
  Func_OnSeek onSeek;
  Func_onReadData onReadData;
} IrisMediaPlayerCCustomDataProvider;
typedef void *IrisMediaPlayerCustomDataProviderHandle;

typedef struct IrisRtcCVideoFrameObserver {
  Func_VideoCaptureLocal OnCaptureVideoFrame;
  Func_VideoCaptureLocal OnPreEncodeVideoFrame;
  Func_VideoFrameRemote OnRenderVideoFrame;
  Func_Uint32_t GetObservedFramePosition;
  //Func_VideoPixelFormat GetVideoFormatPreference;
} IrisRtcCVideoFrameObserver;
typedef void *IrisRtcVideoFrameObserverHandle;

typedef bool(IRIS_CALL *Func_EncodedVideoFrame)(
    unsigned int uid, const uint8_t *imageBuffer, unsigned long long length,
    const IrisEncodedVideoFrameInfo *videoEncodedFrameInfo);
typedef struct IrisRtcCVideoEncodedVideoFrameObserver {
  Func_EncodedVideoFrame OnEncodedVideoFrameReceived;
} IrisRtcCVideoEncodedVideoFrameObserver;

typedef void *IrisRtcVideoEncodedVideoFrameObserverHandle;
typedef struct IrisMediaPlayerCAudioSpectrumObserver {
  Func_LocalAudioSpectrum onLocalAudioSpectrum;
  Func_RemoteAudioSpectrum onRemoteAudioSpectrum;
} IrisMediaPlayerCAudioSpectrumObserver;
typedef struct IrisMediaPlayerCAudioSpectrumObserver
    IrisRtcCAudioSpectrumObserver;
typedef void *IrisMediaPlayerAudioSpectrumObserverHandle;
typedef void *IrisRtcAudioSpectrumObserverHandle;

typedef void *IrisAudioEncodedFrameObserverHandle;
typedef void(IRIS_CALL *Func_RecordAudioEncodedFrame)(
    const uint8_t *frameBuffer, int length,
    const IrisEncodedAudioFrameInfo *audioEncodedFrameInfo);

typedef void(IRIS_CALL *Func_PlaybackAudioEncodedFrame)(
    const uint8_t *frameBuffer, int length,
    const IrisEncodedAudioFrameInfo *audioEncodedFrameInfo);

typedef void(IRIS_CALL *Func_MixedAudioEncodedFrame)(
    const uint8_t *frameBuffer, int length,
    const IrisEncodedAudioFrameInfo *audioEncodedFrameInfo);
typedef struct IrisCAudioEncodedFrameObserver {
  Func_RecordAudioEncodedFrame onRecordAudioEncodedFrame;
  Func_PlaybackAudioEncodedFrame OnPlaybackAudioEncodedFrame;
  Func_MixedAudioEncodedFrame OnMixedAudioEncodedFrame;

} IrisCAudioEncodedFrameObserver;

typedef void *IrisMediaMetadataObserverHandle;

typedef int(IRIS_CALL *Func_MaxMetadataSize)();
typedef bool(IRIS_CALL *Func_ReadyToSendMetadata)(
    IrisMetadata *metadata, IRIS_VIDEO_SOURCE_TYPE source_type);
typedef void(IRIS_CALL *Func_MetadataReceived)(const IrisMetadata *metadata);

typedef struct IrisCMediaMetadataObserver {
  Func_MaxMetadataSize getMaxMetadataSize;
  Func_ReadyToSendMetadata onReadyToSendMetadata;
  Func_MetadataReceived onMetadataReceived;
} IrisCMediaMetadataObserver;

EXTERN_C_LEAVE

#endif//IRIS_RTC_BASE_H_
