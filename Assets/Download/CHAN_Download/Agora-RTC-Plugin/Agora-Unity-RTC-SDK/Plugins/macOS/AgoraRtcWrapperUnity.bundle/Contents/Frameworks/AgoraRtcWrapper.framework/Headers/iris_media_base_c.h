//
// Created by LXH on 2021/7/20.
//

#ifndef IRIS_MEDIA_BASE_C_H_
#define IRIS_MEDIA_BASE_C_H_

#include "iris_base.h"
#include <stdint.h>

EXTERN_C_ENTER

typedef enum AudioFrameType {
  kAudioFrameTypePCM16,
} AudioFrameType;

typedef struct IrisAudioFrame {
  AudioFrameType type;
  int samples;
  int bytes_per_sample;
  int channels;
  int samples_per_sec;
  void *buffer;
  unsigned int buffer_length;
  int64_t render_time_ms;
  int av_sync_type;
} IrisAudioFrame;

typedef enum IRIS_AUDIO_CODEC_TYPE {
  AUDIO_CODEC_OPUS = 1,
  AUDIO_CODEC_PCMA = 3,
  AUDIO_CODEC_PCMU = 4,
  AUDIO_CODEC_G722 = 5,
  AUDIO_CODEC_AACLC = 8,
  AUDIO_CODEC_HEAAC = 9,
  AUDIO_CODEC_JC1 = 10,
  AUDIO_CODEC_HEAAC2 = 11,
  AUDIO_CODEC_LPCNET = 12,
} IRIS_AUDIO_CODEC_TYPE;

typedef struct IrisEncodedAudioFrameAdvancedSettings {
  bool speech;
  bool sendEvenIfEmpty;
} IrisEncodedAudioFrameAdvancedSettings;

typedef struct IrisEncodedAudioFrameInfo {
  //  AudioFrameType type;
  IRIS_AUDIO_CODEC_TYPE codec;
  int sampleRateHz;
  int samplesPerChannel;
  int numberOfChannels;
  IrisEncodedAudioFrameAdvancedSettings advancedSettings;
  int64_t captureTimeMs;
} IrisEncodedAudioFrameInfo;

typedef enum IRIS_AUDIO_FRAME_POSITION {
  IRIS_AUDIO_FRAME_POSITION_NONE = 0x0000,
  IRIS_AUDIO_FRAME_POSITION_PLAYBACK = 0x0001,
  IRIS_AUDIO_FRAME_POSITION_RECORD = 0x0002,
  IRIS_AUDIO_FRAME_POSITION_MIXED = 0x0004,
  IRIS_AUDIO_FRAME_POSITION_BEFORE_MIXING = 0x0008,
} IRIS_AUDIO_FRAME_POSITION;

typedef enum IRIS_RAW_AUDIO_FRAME_OP_MODE_TYPE {

  IRIS_RAW_AUDIO_FRAME_OP_MODE_READ_ONLY = 0,
  IRIS_RAW_AUDIO_FRAME_OP_MODE_READ_WRITE = 2,
} IRIS_RAW_AUDIO_FRAME_OP_MODE_TYPE;

typedef struct IrisAudioParams {
  int sample_rate;
  int channels;
  IRIS_RAW_AUDIO_FRAME_OP_MODE_TYPE mode;
  int samples_per_call;
} IrisAudioParams;

typedef enum IRIS_AUDIO_ENCODED_FRAME_OBSERVER_POSITION {
  /**
  * 1: mic
  */
  IRIS_AUDIO_ENCODED_FRAME_OBSERVER_POSITION_RECORD = 1,
  /**
  * 2: playback audio file recording.
  */
  IRIS_AUDIO_ENCODED_FRAME_OBSERVER_POSITION_PLAYBACK = 2,
  /**
  * 3: mixed audio file recording, include mic and playback.
  */
  IRIS_AUDIO_ENCODED_FRAME_OBSERVER_POSITION_MIXED = 3,
} IRIS_AUDIO_ENCODED_FRAME_OBSERVER_POSITION;

typedef enum IRIS_AUDIO_ENCODING_TYPE {
  /**
   * 1: codecType AAC; sampleRate 16000; quality low which around 1.2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_16000_LOW = 0x010101,
  /**
   * 1: codecType AAC; sampleRate 16000; quality medium which around 2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_16000_MEDIUM = 0x010102,
  /**
   * 1: codecType AAC; sampleRate 32000; quality low which around 1.2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_32000_LOW = 0x010201,
  /**
   * 1: codecType AAC; sampleRate 32000; quality medium which around 2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_32000_MEDIUM = 0x010202,
  /**
   * 1: codecType AAC; sampleRate 32000; quality high which around 3.5 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_32000_HIGH = 0x010203,
  /**
   * 1: codecType AAC; sampleRate 48000; quality medium which around 2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_48000_MEDIUM = 0x010302,
  /**
   * 1: codecType AAC; sampleRate 48000; quality high which around 3.5 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_AAC_48000_HIGH = 0x010303,

  /**
   * 1: codecType OPUS; sampleRate 16000; quality low which around 1.2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_OPUS_16000_LOW = 0x020101,
  /**
   * 1: codecType OPUS; sampleRate 16000; quality medium which around 2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_OPUS_16000_MEDIUM = 0x020102,
  /**
   * 1: codecType OPUS; sampleRate 48000; quality medium which around 2 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_OPUS_48000_MEDIUM = 0x020302,
  /**
   * 1: codecType OPUS; sampleRate 48000; quality high which around 3.5 MB after 10 minutes
   */
  IRIS_AUDIO_ENCODING_TYPE_OPUS_48000_HIGH = 0x020303,
} IRIS_AUDIO_ENCODING_TYPE;

typedef struct IrisAudioEncodedFrameObserverConfig {
  /**
     * The position where SDK record the audio, and callback to encoded audio frame receiver.
     */
  IRIS_AUDIO_ENCODED_FRAME_OBSERVER_POSITION postionType;
  /**
     * The audio encoding type of encoded frame.
     */
  IRIS_AUDIO_ENCODING_TYPE encodingType;
} IrisAudioEncodedFrameObserverConfig;

IRIS_API const struct IrisAudioFrame IrisAudioFrame_default;

IRIS_API void ResizeAudioFrame(IrisAudioFrame *audio_frame);

IRIS_API void ClearAudioFrame(IrisAudioFrame *audio_frame);

IRIS_API void CopyAudioFrame(IrisAudioFrame *dst, const IrisAudioFrame *src);

typedef enum IrisVideoFrameType {
  kVideoFrameTypeYUV420,
  kVideoFrameTypeYUV422,
  kVideoFrameTypeRGBA,
  kVideoFrameTypeBGRA,
} IrisVideoFrameType;

typedef enum IrisVideoSourceType {
  kVideoSourceTypeCameraPrimary,
  kVideoSourceTypeCameraSecondary,
  kVideoSourceTypeScreenPrimary,
  kVideoSourceTypeScreenSecondary,
  kVideoSourceTypeCustom,
  kVideoSourceTypeMediaPlayer,
  kVideoSourceTypeRtcImagePng,
  kVideoSourceTypeRtcImageJpeg,
  kVideoSourceTypeRtcImageGif,
  kVideoSourceTypeRemote,
  kVideoSourceTypeTranscoded,
  kVideoSourceTypePreEncode,
  kVideoSourceTypePreEncodeSecondaryCamera,
  kVideoSourceTypePreEncodeScreen,
  kVideoSourceTypePreEncodeSecondaryScreen,
  kVideoSourceTypeUnknown,
} IrisVideoSourceType;

typedef struct IrisVideoFrame {
  IrisVideoFrameType type;
  int width;
  int height;
  int y_stride;
  int u_stride;
  int v_stride;
  void *y_buffer;
  void *u_buffer;
  void *v_buffer;
  unsigned int y_buffer_length;
  unsigned int u_buffer_length;
  unsigned int v_buffer_length;
  int rotation;
  int64_t render_time_ms;
  int av_sync_type;
  void *metadata_buffer;
  int metadata_size;
  void *sharedContext;
  int textureId;
  float matrix[16];
} IrisVideoFrame;

typedef enum IRIS_VIDEO_SOURCE_TYPE {
  /** Video captured by the camera.
   */
  IRIS_VIDEO_SOURCE_CAMERA_PRIMARY,
  IRIS_VIDEO_SOURCE_CAMERA = IRIS_VIDEO_SOURCE_CAMERA_PRIMARY,
  /** Video captured by the secondary camera.
   */
  IRIS_VIDEO_SOURCE_CAMERA_SECONDARY,
  /** Video for screen sharing.
   */
  IRIS_VIDEO_SOURCE_SCREEN_PRIMARY,
  IRIS_VIDEO_SOURCE_SCREEN = IRIS_VIDEO_SOURCE_SCREEN_PRIMARY,
  /** Video for secondary screen sharing.
   */
  IRIS_VIDEO_SOURCE_SCREEN_SECONDARY,
  /** Not define.
   */
  IRIS_VIDEO_SOURCE_CUSTOM,
  /** Video for media player sharing.
   */
  IRIS_VIDEO_SOURCE_MEDIA_PLAYER,
  /** Video for png image.
   */
  IRIS_VIDEO_SOURCE_RTC_IMAGE_PNG,
  /** Video for png image.
   */
  IRIS_VIDEO_SOURCE_RTC_IMAGE_JPEG,
  /** Video for png image.
   */
  IRIS_VIDEO_SOURCE_RTC_IMAGE_GIF,
  /** Remote video received from network.
   */
  IRIS_VIDEO_SOURCE_REMOTE,
  /** Video for transcoded.
   */
  IRIS_VIDEO_SOURCE_TRANSCODED,

  IRIS_VIDEO_SOURCE_UNKNOWN = 100
} IRIS_VIDEO_SOURCE_TYPE;

typedef struct IrisMetadata {
  unsigned int uid;

  unsigned int size;

  unsigned char *buffer;

  long long timeStampMs;
} IrisMetadata;

IRIS_API const struct IrisVideoFrame IrisVideoFrame_default;

IRIS_API void ResizeVideoFrame(IrisVideoFrame *video_frame);

IRIS_API void ClearVideoFrame(IrisVideoFrame *video_frame);

IRIS_API bool CopyVideoFrame(IrisVideoFrame *dst, const IrisVideoFrame *src,
                             bool deepCopy);

IRIS_API bool ConvertVideoFrame(IrisVideoFrame *dst, const IrisVideoFrame *src);

typedef struct IrisPacket {
  const unsigned char *buffer;
  unsigned int size;
} IrisPacket;
typedef enum IRIS_VIDEO_PIXEL_FORMAT {
  /**
   * 0: Default format.
   */
  IRIS_VIDEO_PIXEL_DEFAULT = 0,
  /**
   * 1: I420.
   */
  IRIS_VIDEO_PIXEL_I420 = 1,
  /**
   * 2: BGRA.
   */
  IRIS_VIDEO_PIXEL_BGRA = 2,
  /**
   * 3: NV21.
   */
  IRIS_VIDEO_PIXEL_NV21 = 3,
  /**
   * 4: RGBA.
   */
  IRIS_VIDEO_PIXEL_RGBA = 4,
  /**
   * 8: NV12.
   */
  IRIS_VIDEO_PIXEL_NV12 = 8,
  /**
   * 10: GL_TEXTURE_2D
   */
  VIDEO_TEXTURE_2D = 10,
  /**
   * 11: GL_TEXTURE_OES
   */
  VIDEO_TEXTURE_OES = 11,
  /*
  12: pixel format for iOS CVPixelBuffer NV12
  */
  IRIS_VIDEO_CVPIXEL_NV12 = 12,
  /*
  13: pixel format for iOS CVPixelBuffer I420
  */
  IRIS_VIDEO_CVPIXEL_I420 = 13,
  /*
  14: pixel format for iOS CVPixelBuffer BGRA
  */
  IRIS_VIDEO_CVPIXEL_BGRA = 14,
  /**
   * 16: I422.
   */
  IRIS_IRIS_VIDEO_PIXEL_I422 = 16,
} IRIS_VIDEO_PIXEL_FORMAT;
EXTERN_C_LEAVE

#endif//IRIS_MEDIA_BASE_C_H_
