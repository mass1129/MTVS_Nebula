//
// Created by LXH on 2021/1/14.
//

#ifndef IRIS_EVENT_HANDLER_H_
#define IRIS_EVENT_HANDLER_H_

#include "iris_base.h"

namespace agora {
namespace iris {

class IrisEventHandler {
 public:
  virtual ~IrisEventHandler() = default;

 public:
  virtual void OnEvent(const char *event, const char *data, const void **buffer,
                       unsigned int *length, unsigned int buffer_count) = 0;
  virtual void OnEvent(const char *event, const char *data,
                       char result[kBasicResultLength], const void **buffer,
                       unsigned int *length, unsigned int buffer_count){};
};

}// namespace iris
}// namespace agora

#endif//IRIS_EVENT_HANDLER_H_
