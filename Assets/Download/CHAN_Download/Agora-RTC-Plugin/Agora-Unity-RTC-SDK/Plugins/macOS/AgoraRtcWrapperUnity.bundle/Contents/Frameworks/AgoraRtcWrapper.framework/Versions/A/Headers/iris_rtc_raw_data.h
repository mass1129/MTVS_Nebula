//
// Created by LXH on 2021/3/1.
//

#ifndef IRIS_RTC_RAW_DATA_H_
#define IRIS_RTC_RAW_DATA_H_

#include "iris_delegate.h"
#include "iris_event_handler.h"
#include "iris_media_base_cxx.h"
#include "iris_rtc_base.h"
#include "iris_video_processor_c.h"

namespace agora {
namespace rtc {
class IRtcEngine;
}

namespace iris {
namespace rtc {

class IRIS_DEBUG_CPP_API IrisRtcAudioFrameObserver
    : public IrisAudioFrameObserver {
 public:
  bool OnRecordAudioFrame(const char *channelId,
                          IrisAudioFrame &audio_frame) override = 0;

  virtual bool OnPlaybackAudioFrame(const char *channelId,
                                    IrisAudioFrame &audio_frame) = 0;

  virtual bool OnMixedAudioFrame(const char *channelId,
                                 IrisAudioFrame &audio_frame) = 0;
  virtual bool OnEarMonitoringAudioFrame(IrisAudioFrame &audio_frame) = 0;

  virtual bool
  OnPlaybackAudioFrameBeforeMixing(const char *channelId, unsigned int uid,
                                   IrisAudioFrame &audio_frame) = 0;
  virtual bool
  OnPlaybackAudioFrameBeforeMixing(const char *channelId, const char *userid,
                                   IrisAudioFrame &audio_frame) = 0;
  virtual int getObservedAudioFramePosition() = 0;
  virtual IrisAudioParams getPlaybackAudioParams() = 0;
  virtual IrisAudioParams getRecordAudioParams() = 0;
  virtual IrisAudioParams getMixedAudioParams() = 0;
};

class IRIS_DEBUG_CPP_API IrisRtcVideoFrameObserver
    : public IrisVideoFrameObserver {
 public:
  bool OnCaptureVideoFrame(IrisVideoFrame &video_frame) override {
    return false;
  }

  virtual bool
  OnCaptureVideoFrame(IrisVideoFrame &video_frame,
                      const IrisVideoFrameBufferConfig *config) = 0;

  virtual bool OnPreEncodeVideoFrame(IrisVideoFrame &video_frame,
                                     const IrisVideoFrameBufferConfig *config) {
    return true;
  }

  virtual uint32_t GetObservedFramePosition() {
    return static_cast<uint32_t>(kPositionPostCapturer | kPositionPreRenderer);
  }

  virtual bool OnRenderVideoFrame(const char *channel_id, unsigned int uid,
                                  IrisVideoFrame &video_frame) {
    return false;
  }

  virtual IRIS_VIDEO_PIXEL_FORMAT GetVideoFormatPreference() {
    return IRIS_VIDEO_PIXEL_I420;
  }
};

class IRIS_DEBUG_CPP_API IrisRtcPacketObserver : public IrisPacketObserver {};

class IRIS_DEBUG_CPP_API IrisRtcVideoEncodedVideoFrameObserver {
 public:
  virtual bool OnEncodedVideoFrameReceived(
      unsigned int uid, const uint8_t *imageBuffer, unsigned long long length,
      const IrisEncodedVideoFrameInfo &videoEncodedFrameInfo) = 0;
};

class IRIS_CPP_API IrisRtcVideoEncodedVideoFrameObserverManager {
 public:
  IrisRtcVideoEncodedVideoFrameObserverManager();
  ~IrisRtcVideoEncodedVideoFrameObserverManager();

 public:
  void RegisterVideoEncodedFrameObserver(
      IrisRtcVideoEncodedVideoFrameObserver *observer, int order,
      const char *identifier);

  void UnRegisterVideoEncodedFrameObserver(const char *identifier = nullptr);

  unsigned int GetVideoEncodedVideoFrameObserverCount();

  IrisRtcVideoEncodedVideoFrameObserver *
  GetVideoEncodedVideoFrameObserver(unsigned int index);

 private:
  class Impl;
  Impl *impl_;
};

class IRIS_CPP_API IrisRtcObserverManager
    : public IrisCommonObserverManager,
      public IrisRtcVideoEncodedVideoFrameObserverManager,
      public IrisAudioEncodedFrameObserverManager {};

class IRIS_CPP_API IrisRtcRawData : public IrisRtcObserverManager {
 public:
  explicit IrisRtcRawData(IModule *plugin_manager = nullptr);
  ~IrisRtcRawData() override;

  void Initialize(agora::rtc::IRtcEngine *rtc_engine);

  void Release();

  void enableVideoEncoded(bool bEncoded);

  IModule *plugin_manager();

 private:
  class Impl;
  Impl *impl_;
  IModule *plugin_manager_;
};

}// namespace rtc
}// namespace iris
}// namespace agora

#endif//IRIS_RTC_RAW_DATA_H_
