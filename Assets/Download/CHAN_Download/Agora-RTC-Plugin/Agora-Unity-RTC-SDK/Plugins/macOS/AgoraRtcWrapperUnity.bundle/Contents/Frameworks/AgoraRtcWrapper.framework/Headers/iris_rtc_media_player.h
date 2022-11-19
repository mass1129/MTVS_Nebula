#ifndef IRIS_RTC_MEDIA_PLAYER_H_
#define IRIS_RTC_MEDIA_PLAYER_H_

#include "iris_delegate.h"
#include "iris_rtc_base.h"
#include "iris_rtc_internal_obj.h"
#include "iris_rtc_raw_data.h"

namespace agora {
namespace iris {
namespace rtc {

class IrisMediaPlayerImpl;

class IrisMediaPlayerAudioFrameObserver {
 public:
  virtual void onFrame(IrisAudioPcmFrame *frame, int playerId) = 0;
};

class IrisMediaPlayerVideoFrameObserver {
 public:
  virtual void onFrame(IrisVideoFrame *frame, int playerId) = 0;
};

class IrisMediaPlayerCustomDataProvider {
 public:
  virtual int64_t onSeek(int64_t offset, int whence, int playerId) = 0;
  virtual int onReadData(unsigned char *buffer, int bufferSize,
                         int playerId) = 0;
  ~IrisMediaPlayerCustomDataProvider() = default;
};

class IrisMediaPlayerAudioSpectrumObserver {
 public:
  virtual bool onLocalAudioSpectrum(int playerId,
                                    const IrisAudioSpectrumData &data) = 0;

  virtual bool onRemoteAudioSpectrum(int playerId,
                                     const IrisUserAudioSpectrumInfo *spectrums,
                                     unsigned int spectrumNumber) = 0;
};

typedef IrisMediaPlayerAudioSpectrumObserver IrisRtcAudioSpectrumObserver;

}// namespace rtc
}// namespace iris
}// namespace agora

#endif//IRIS_RTC_MEDIA_PLAYER_H_
