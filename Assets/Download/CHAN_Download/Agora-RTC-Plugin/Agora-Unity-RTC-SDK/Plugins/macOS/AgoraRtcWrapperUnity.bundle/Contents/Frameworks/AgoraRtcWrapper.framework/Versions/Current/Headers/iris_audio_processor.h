//
// Created by LXH on 2021/7/20.
//

#ifndef IRIS_AUDIO_PROCESSOR_H_
#define IRIS_AUDIO_PROCESSOR_H_

#include "iris_media_base_c.h"

EXTERN_C_ENTER

typedef void *IrisAudioFrameMixingPtr;

IRIS_API IrisAudioFrameMixingPtr CreateIrisAudioFrameMixing();

IRIS_API void FreeIrisAudioFrameMixing(IrisAudioFrameMixingPtr mixing_ptr);

IRIS_API void PushAudioFrame(IrisAudioFrameMixingPtr mixing_ptr,
                             IrisAudioFrame *audio_frame);

IRIS_API void Mixing(IrisAudioFrameMixingPtr mixing_ptr,
                     IrisAudioFrame *audio_frame);

EXTERN_C_LEAVE

#endif//IRIS_AUDIO_PROCESSOR_H_
