#ifndef __WZAV1_ENC_H__
#define __WZAV1_ENC_H__
#include "wzav1def.h"

#if defined(__cplusplus)
extern "C" {
#endif  //__cplusplus

static const char *const wz_preset_names[] = { "ultrafast", "superfast", "veryfast",
                                               "fast",      "medium",    0 };
static const char *const wz_tunes_names[] = { "default", "screen", "camera", 0 };

static const char *const wz_rctype_names[] = { "cbr", "crf", "q", 0 };

#define WZ_FORCE_KEYFRAME_FLAG (1 << 0) /**< Force this frame to be a key frame */
#define WZ_FORCE_SFRAME_FLAG (1 << 1)   /**< Force this frame to be a key frame */

typedef struct WZ_ENCODER_CONFIG {
  const char *preset;         // codec preset for speed level
  const char *tune;           // codec tune for scene
  const char *rc_type;        // rate control method type.
  int32_t width;              // image origin width
  int32_t width_max;          // image maximum width
  int32_t height;             // image origin height
  int32_t height_max;         // image maximum height
  int32_t threads;            // encoder threads number
  int32_t frame_rate_num;     // frame rate numerator
  int32_t frame_rate_den;     // frame rate denominator,default should be 1
  int32_t time_base_num;      // time base numerator, default should be 1
  int32_t time_base_den;      // time base denominator, time scale of pts
  int32_t bitratekbps;        // bit rate kbps per second, valid when rc type is "vbr", "cbr"
  int32_t vbv_buffer_size;    // buffer size of vbv, 0 means vbv not enabled,default 0
  int32_t vbv_max_rate;       // max rate of vbv, 0 means vbv not enabled,default 0
  int32_t frame_kbits_limit;  // frame max kbits limit for rc, if qpmax is also set,the actual frame
                              // bit may exceed this
  int32_t max_intra_kbits;    // maximum allowed bitrate(kbps) for any intra frame, default 0
  int32_t crf;                // crf for video quality, only valid when rc_type is "crf"
  int32_t qp;                 // qp , only valid when rc_type is "q"
  int32_t qpmin;              // minimal qp, valid when rc_type is not "q", 1~qpmax
  int32_t qpmax;              // maximal qp, valid when rc_type is not "q", 1~255
  int32_t max_keyframe_dist;  // max dist between key frames
  int32_t min_keyframe_dist;  // min dist between key frames
  int32_t s_frame_dist;       // min dist between S frames
  int32_t gop_size;           // number of b frames [0, 16]
  int32_t b_pyramid;
  int32_t lookahead;  // lookahead depth [0, 65]
  int32_t log_level;
  int32_t enable_scalability;              // enable svc (will overwrite gop_size and lookahead)
  wz_scalability_mode_t scalability_mode;  // svc mode
  int32_t enable_frame_skip;  // max continuous skipped frame for rate control,default 0
  int32_t tile_cols;          // default 1
  int32_t tile_rows;          // default 1
  int32_t enable_frame_id;    // enable frame_id_numbers_present_flag
  int32_t scenecut;           // scenecut threshold
  int32_t fpp;                // frame parallel threads, default 0, 0 means disable

} wz_encoder_config_t;

/**
 * fill config with default values
 * @param cfg : base config to be filled
 */
_h_dll_export void wz_encoder_default_cfg(wz_encoder_config_t *cfg);

/**
 * create encoder
 * @param cfg : base config of encoder
 * @param extra_cfg string type extra config
 * @param errorCode: error code
 * @return encoder handle
 */
_h_dll_export void *wz_encoder_open(wz_encoder_config_t *cfg, const char *extra_cfg,
                                    int32_t *errorCode);

/**
 * destroy encoder
 * @param encoder   handle of encoder
 * @return error code
 */
_h_dll_export int32_t wz_encoder_close(void *encoder);

/**
 * parse extra string config
 *
 * @param encoder   handle of encoder
 * @param cfg : base config of encoder
 * @param extraCfg string type extra config
 * @return if succeed, return 0; if failed, return the error code
 */
_h_dll_export int32_t wz_encoder_update_config(void *encoder, wz_encoder_config_t *cfg,
                                               const char *extra_cfg);

/**
 * get global headers that will be used for the whole stream.
 *
 * @param encoder   handle of encoder
 * @param obus      pointer array of output OBU units
 * @param obu_cnt   output OBU unit count
 * @return if succeed, return 0; if failed, return the error code
 */
_h_dll_export int32_t wz_encoder_get_global_headers(void *encoder, wz_obu_t **obus,
                                                    int32_t *obu_cnt);

/**
 * Encode one frame
 *
 * @param encoder   handle of encoder
 * @param obus      pointer array of output OBU units
 * @param obu_cnt   output OBU unit count
 * @param inpic    input frame, could be NULL for flush data
 * @param flags     is current frame a key frame
 * @return if succeed, return 0; if failed, return the error code
 */
_h_dll_export int32_t wz_encoder_encode_frame(void *encoder, wz_obu_t **obus, int32_t *obu_cnt,
                                              wz_video_sample_t *inpic, int32_t flags);

/*
 * Library Version Number Interface
 *
 * For example, see the following sample return values:
 *     wz_codec_version()           (1<<16 | 2<<8 | 3)
 *     wz_codec_version_str()       "v1.2.3-2-f11ebec"
 *     wz_codec_version_extra_str() "2-f11ebec"
 */

/*!\brief Return the version information (as an integer)
 *
 * Returns a packed encoding of the library version number. This will only
 * include the major.minor.patch component of the version number.
 */
_h_dll_export int wz_codec_version(void);

/*!\brief Return the version information (as a string)
 *
 * Returns a printable string containing the full library version number.
 */
_h_dll_export const char *wz_codec_version_str(void);

/*!\brief Return the version information (as a string)
 *
 * Returns a printable "extra string". This is the component of the string
 * returned
 * by wz_codec_version_str() following the three digit version number.
 *
 */
_h_dll_export const char *wz_codec_version_extra_str(void);

#if defined(__cplusplus)
}
#endif  //__cplusplus
#endif  //__WZAV1_ENC_H__
