//
// Created by LXH on 2021/7/20.
//

#ifndef IRIS_MEDIA_BASE_H_
#define IRIS_MEDIA_BASE_H_

#include "iris_media_base_c.h"
#include <cstdint>

namespace agora {
namespace iris {

class IrisVideoFrameBufferManager;

class IRIS_CPP_API IrisAudioFrameObserver {
 public:
  virtual bool OnRecordAudioFrame(const char *channelId,
                                  IrisAudioFrame &audio_frame) = 0;
};

class IRIS_CPP_API IrisAudioEncodedFrameObserver {
 public:
  virtual void OnRecordAudioEncodedFrame(
      const uint8_t *frameBuffer, int length,
      const IrisEncodedAudioFrameInfo &audioEncodedFrameInfo) = 0;
  virtual void OnPlaybackAudioEncodedFrame(
      const uint8_t *frameBuffer, int length,
      const IrisEncodedAudioFrameInfo &audioEncodedFrameInfo) = 0;
  virtual void OnMixedAudioEncodedFrame(
      const uint8_t *frameBuffer, int length,
      const IrisEncodedAudioFrameInfo &audioEncodedFrameInfo) = 0;
};

class IRIS_CPP_API IrisVideoFrameObserver {
 public:
  virtual bool OnCaptureVideoFrame(IrisVideoFrame &video_frame) = 0;
};

class IRIS_CPP_API IrisPacketObserver {
 public:
  virtual bool OnSendAudioPacket(IrisPacket &packet) = 0;
  virtual bool OnSendVideoPacket(IrisPacket &packet) = 0;
  virtual bool OnReceiveAudioPacket(IrisPacket &packet) = 0;
  virtual bool OnReceiveVideoPacket(IrisPacket &packet) = 0;
};

class IRIS_CPP_API IrisAudioFrameObserverManager {
 public:
  IrisAudioFrameObserverManager();
  ~IrisAudioFrameObserverManager();

 public:
  void RegisterAudioFrameObserver(IrisAudioFrameObserver *observer, int order,
                                  const char *identifier);

  void UnRegisterAudioFrameObserver(const char *identifier = nullptr);

  unsigned int GetAudioFrameObserverCount();

  IrisAudioFrameObserver *GetAudioFrameObserver(unsigned int index);

 private:
  class Impl;
  Impl *impl_;
};

class IRIS_CPP_API IrisAudioEncodedFrameObserverManager {
 public:
  IrisAudioEncodedFrameObserverManager();
  ~IrisAudioEncodedFrameObserverManager();

 public:
  void
  RegisterAudioEncodedFrameObserver(IrisAudioEncodedFrameObserver *observer,
                                    int order, const char *identifier);

  void UnRegisterAudioEncodedFrameObserver(const char *identifier = nullptr);

  unsigned int GetAudioEncodedFrameObserverCount();

  IrisAudioEncodedFrameObserver *
  GetAudioEncodedFrameObserver(unsigned int index);

 private:
  class Impl;
  Impl *impl_;
};

class IRIS_CPP_API IrisVideoFrameObserverManager {
 public:
  IrisVideoFrameObserverManager();
  ~IrisVideoFrameObserverManager();

 public:
  void RegisterVideoFrameObserver(IrisVideoFrameObserver *observer, int order,
                                  const char *identifier);

  void UnRegisterVideoFrameObserver(const char *identifier = nullptr);

  unsigned int GetVideoFrameObserverCount();

  IrisVideoFrameObserver *GetVideoFrameObserver(unsigned int index);

  void Attach(IrisVideoFrameBufferManager *manager);

  IrisVideoFrameBufferManager *buffer_manager();

 private:
  class Impl;
  Impl *impl_;
};

class IRIS_CPP_API IrisPacketObserverManager {
 public:
  IrisPacketObserverManager();
  ~IrisPacketObserverManager();

 public:
  void RegisterPacketObserver(IrisPacketObserver *observer, int order,
                              const char *identifier);

  void UnRegisterPacketObserver(const char *identifier = nullptr);

  unsigned int GetPacketObserverCount();

  IrisPacketObserver *GetPacketObserver(unsigned int index);

 private:
  class Impl;
  Impl *impl_;
};

class IRIS_CPP_API IrisCommonObserverManager
    : public IrisAudioFrameObserverManager,
      public IrisVideoFrameObserverManager,
      public IrisPacketObserverManager {
 public:
  IrisCommonObserverManager();
  virtual ~IrisCommonObserverManager();
};

class IRIS_CPP_API IrisMetadataObserver {
 public:
  virtual int getMaxMetadataSize() = 0;
  virtual bool onReadyToSendMetadata(IrisMetadata &metadata,
                                     IRIS_VIDEO_SOURCE_TYPE source_type) = 0;
  virtual void onMetadataReceived(const IrisMetadata &metadata) = 0;
};

}// namespace iris
}// namespace agora

#endif//IRIS_MEDIA_BASE_H_
