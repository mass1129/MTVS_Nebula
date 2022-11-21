#ifndef __WZAV1_DEF_H__
#define __WZAV1_DEF_H__
#include <stdint.h>

#ifdef WIN32
#define _h_dll_export __declspec(dllexport)
#else  // for GCC
#define _h_dll_export __attribute__((visibility("default")))
#endif

#define WZ_YUV_FMT_I420 0x30323449

/*!\brief input frame data and info. */
typedef struct WZ_VIDEO_SAMPLE {
  int32_t fmt;        // pixel format as FourCC, such as I420(0x30323449)
  int64_t pts;        // input frame width
  int32_t width;      // input frame width
  int32_t height;     // input frame height
  uint8_t *buf[3];    // input frame Y U V
  int32_t stride[3];  // stride for Y U V
  void *usrdata;
} wz_video_sample_t;

/*!\brief OBU types. */
typedef enum WZ_OBU_TYPE {
  WZ_OBU_SEQUENCE_HEADER = 1,
  WZ_OBU_TEMPORAL_DELIMITER = 2,
  WZ_OBU_FRAME_HEADER = 3,
  WZ_OBU_TILE_GROUP = 4,
  WZ_OBU_METADATA = 5,
  WZ_OBU_FRAME = 6,
  WZ_OBU_REDUNDANT_FRAME_HEADER = 7,
  WZ_OBU_TILE_LIST = 8,
  WZ_OBU_PADDING = 15,
} wz_obu_type_t;

/*!\brief OBU metadata types. */
typedef enum WZ_OBU_METADATA_TYPE {
  WZ_OBU_METADATA_TYPE_AOM_RESERVED_0 = 0,
  WZ_OBU_METADATA_TYPE_HDR_CLL = 1,
  WZ_OBU_METADATA_TYPE_HDR_MDCV = 2,
  WZ_OBU_METADATA_TYPE_SCALABILITY = 3,
  WZ_OBU_METADATA_TYPE_ITUT_T35 = 4,
  WZ_OBU_METADATA_TYPE_TIMECODE = 5,
} wz_obu_metadata_type_t;

// Picture prediction structures (0-13 are predefined) in scalability metadata.
typedef enum WZ_SCALABILITY_MODE {
  WZ_SCALABILITY_L1T2 = 0,
  WZ_SCALABILITY_L1T3 = 1,
  WZ_SCALABILITY_L2T1 = 2,    // Not support yet
  WZ_SCALABILITY_L2T2 = 3,    // Not support yet
  WZ_SCALABILITY_L2T3 = 4,    // Not support yet
  WZ_SCALABILITY_S2T1 = 5,    // Not support yet
  WZ_SCALABILITY_S2T2 = 6,    // Not support yet
  WZ_SCALABILITY_S2T3 = 7,    // Not support yet
  WZ_SCALABILITY_L2T1h = 8,   // Not support yet
  WZ_SCALABILITY_L2T2h = 9,   // Not support yet
  WZ_SCALABILITY_L2T3h = 10,  // Not support yet
  WZ_SCALABILITY_S2T1h = 11,  // Not support yet
  WZ_SCALABILITY_S2T2h = 12,  // Not support yet
  WZ_SCALABILITY_S2T3h = 13,  // Not support yet
  WZ_SCALABILITY_SS = 14,     // Not support yet
  WZ_SCALABILITY_NONE = 255
} wz_scalability_mode_t;

typedef struct OBU {
  int64_t pts;               // presentation time stamp of video data
  int64_t dts;               // decoding time stamp of video data
  int32_t tid;               // temporal layer id
  uint32_t size;             // OBU payload size
  uint8_t *buf;              // OBU payload data pointer
  wz_obu_type_t obutype;     // OBU type for specific codec
  uint8_t is_keyframe;       // this obu is keyframe
  uint8_t frame_qp;          // this frame's average qindex
  int32_t ref_frame_ids[7];  // frame_id of ref_frames used by this obu
  double psnr[3];            // frame psnr Y U V  for WZ_OBU_FRAME obu when psnr calculation enabled
} wz_obu_t;

enum {
  WZ_OK = (0x00000000),             // Success codes
  WZ_NO_MORE_FRAME = (0x00000001),  // No more cached frame

  WZ_FAILED = (0xFFFFFFFF),        //  Unspecified error
  WZ_FAIL = (0xFFFFFFFF),          //  Unspecified error
  WZ_OUTOFMEMORY = (0x80000002),   //  Ran out of memory
  WZ_POINTER = (0x80000003),       //  Invalid pointer
  WZ_NOTSUPPORTED = (0x80000004),  //  NOT support feature
  WZ_EXPIRED = (0x80000005),       //  SDK auth failed(eg. expired)
};

#endif  // __WZAV1_DEF_H__
