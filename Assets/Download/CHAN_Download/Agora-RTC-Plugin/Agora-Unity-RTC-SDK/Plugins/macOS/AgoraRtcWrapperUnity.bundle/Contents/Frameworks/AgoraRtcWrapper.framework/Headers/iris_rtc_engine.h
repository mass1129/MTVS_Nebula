//
// Created by LXH on 2021/1/14.
//

#ifndef IRIS_RTC_ENGINE_H_
#define IRIS_RTC_ENGINE_H_

#include "iris_rtc_raw_data.h"

namespace agora {
namespace iris {
namespace rtc {

class IRIS_DEBUG_CPP_API IIrisRtcEngine : public IModule {
 public:
  virtual IModule *device_manager() = 0;

  virtual IModule *media_player() = 0;

  virtual IModule *local_audio_engine() = 0;

  virtual IModule *cloud_audio_engine() = 0;

  virtual IModule *media_recoder() = 0;

  virtual IrisRtcRawData *raw_data() = 0;

  virtual IModule *media_player_cache_manager() = 0;
};

class IRIS_DEBUG_CPP_API IrisRtcEngine : public IIrisRtcEngine {
 public:
  explicit IrisRtcEngine(IIrisRtcEngine *delegate = nullptr);
  ~IrisRtcEngine() override;

  void Initialize(void *rtc_engine) override;

  void Release() override;

  void SetEventHandler(IrisEventHandler *event_handler) override;

  IrisEventHandler *GetEventHandler() override;

  int CallApi(const char *func_name, const char *buff, uint32_t buffLength,
              std::string &out) override;

  IModule *device_manager() override;

  IModule *media_player() override;

  IModule *local_audio_engine() override;

  IModule *cloud_audio_engine() override;

  IModule *media_recoder() override;

  IrisRtcRawData *raw_data() override;
  IModule *media_player_cache_manager() override;

 public:
  IIrisRtcEngine *delegate_;
};

}// namespace rtc
}// namespace iris
}// namespace agora

#endif//IRIS_RTC_ENGINE_H_
