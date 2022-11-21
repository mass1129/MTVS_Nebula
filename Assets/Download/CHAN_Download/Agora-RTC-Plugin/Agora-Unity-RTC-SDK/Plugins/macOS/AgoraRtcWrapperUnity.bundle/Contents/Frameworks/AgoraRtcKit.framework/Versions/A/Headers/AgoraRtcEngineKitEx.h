//
//  AgoraRtcEngineKitEx.h
//  AgoraRtcEngineKit
//
//  Copyright (c) 2020 Agora. All rights reserved.
//  Created by LLF on 2020/3/9.
//

#import "AgoraRtcEngineKit.h"
#import "AgoraObjects.h"

#if TARGET_OS_IPHONE
#import <UIKit/UIKit.h>
#elif TARGET_OS_MAC
#import <AppKit/AppKit.h>
#endif

NS_ASSUME_NONNULL_BEGIN
@interface AgoraRtcEngineKit(Ex)

/**
 * Joins a channel.
 *
 * You can call this method multiple times to join multiple channels.
 *
 * @param token The token for authentication.
 * - In situations not requiring high security: You can use the temporary token
 * generated at Console. For details, see [Get a temporary token](https://docs.agora.io/en/Agora%20Platform/token?platform=All%20Platforms#get-a-temporary-token).
 * - In situations requiring high security: Set it as the token generated at
 * you server. For details, see [Generate a token](https://docs.agora.io/en/Agora%20Platform/token?platform=All%20Platforms#get-a-token).
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 * @param delegate AgoraRtcEngineDelegate protocol.(Need a new object when called)
 * @param mediaOptions AgoraRtcChannelMediaOptions Object.
 * @param joinSuccessBlock Same as \ref AgoraRtcEngineDelegate.rtcEngine:didJoinChannel:withUid:elapsed: didJoinChannel. We recommend you set this parameter as `nil` to use `didJoinChannel`.
 * - If `joinSuccessBlock` is nil, the SDK triggers the `didJoinChannel` callback.
 * - If you implement both `joinSuccessBlock` and `didJoinChannel`, `joinSuccessBlock` takes higher priority than `didJoinChannel`.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)joinChannelExByToken:(NSString* _Nullable)token
                 connection:(AgoraRtcConnection * _Nonnull)connection
                   delegate:(id<AgoraRtcEngineDelegate> _Nullable)delegate
               mediaOptions:(AgoraRtcChannelMediaOptions* _Nonnull)mediaOptions
                joinSuccess:(void(^ _Nullable)(NSString* _Nonnull channel, NSUInteger uid, NSInteger elapsed))joinSuccessBlock;

/** Joins the channel with a user account.
 *
 * After the user successfully joins the channel, the SDK triggers the following callbacks:
 *
 * - The local client: \ref agora::rtc::IRtcEngineEventHandler::onLocalUserRegistered "onLocalUserRegistered" and \ref agora::rtc::IRtcEngineEventHandler::onJoinChannelSuccess "onJoinChannelSuccess" .
 * The remote client: \ref agora::rtc::IRtcEngineEventHandler::onUserJoined "onUserJoined" and \ref agora::rtc::IRtcEngineEventHandler::onUserInfoUpdated "onUserInfoUpdated" , if the user joining the channel is in the `COMMUNICATION` profile, or is a host in the `LIVE_BROADCASTING` profile.
 *
 * @note To ensure smooth communication, use the same parameter type to identify the user. For example, if a user joins the channel with a user ID, then ensure all the other users use the user ID too. The same applies to the user account.
 * If a user joins the channel with the Agora Web SDK, ensure that the uid of the user is set to the same parameter type.
 *
 * @param token The token generated at your server:
 * - For low-security requirements: You can use the temporary token generated at Console. For details, see [Get a temporary toke](https://docs.agora.io/en/Voice/token?platform=All%20Platforms#get-a-temporary-token).
 * - For high-security requirements: Set it as the token generated at your server. For details, see [Get a token](https://docs.agora.io/en/Voice/token?platform=All%20Platforms#get-a-token).
 * @param channelId The channel name. The maximum length of this parameter is 64 bytes. Supported character scopes are:
 * - All lowercase English letters: a to z.
 * - All uppercase English letters: A to Z.
 * - All numeric characters: 0 to 9.
 * - The space character.
 * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
 * @param userAccount The user account. The maximum length of this parameter is 255 bytes. Ensure that you set this parameter and do not set it as null. Supported character scopes are:
 * - All lowercase English letters: a to z.
 * - All uppercase English letters: A to Z.
 * - All numeric characters: 0 to 9.
 * - The space character.
 * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
 * @param delegate AgoraRtcEngineDelegate protocol.
 * @param mediaOptions  The channel media options: \ref agora::rtc::ChannelMediaOptions::ChannelMediaOptions "ChannelMediaOptions"
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 *  - #ERR_INVALID_ARGUMENT (-2)
 *  - #ERR_NOT_READY (-3)
 *  - #ERR_REFUSED (-5)
 */
- (int)joinChannelExByToken:(NSString* _Nullable)token
                  channelId:(NSString* _Nonnull)channelId
                userAccount:(NSString* _Nonnull)userAccount
                   delegate:(id<AgoraRtcEngineDelegate> _Nullable)delegate
               mediaOptions:(AgoraRtcChannelMediaOptions* _Nonnull)mediaOptions
                joinSuccess:(void(^ _Nullable)(NSString* _Nonnull channel, NSUInteger uid, NSInteger elapsed))joinSuccessBlock;

/**
 *  Updates the channel media options after joining the channel.
 *
 * @param mediaOptions The channel media options: ChannelMediaOptions.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)updateChannelExWithMediaOptions:(AgoraRtcChannelMediaOptions* _Nonnull)mediaOptions
                            connection:(AgoraRtcConnection * _Nonnull)connection;

/**
 * Leaves the channel by connection.
 *
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 * @param leaveChannelBlock This callback indicates that a user leaves a channel, and provides the statistics of the call in #AgoraChannelStats.
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)leaveChannelEx:(AgoraRtcConnection * _Nonnull)connection
    leaveChannelBlock:(void(^ _Nullable)(AgoraChannelStats* _Nonnull stat))leaveChannelBlock;

/** Mutes a specified remote user's audio stream.

 @note  When setting to YES, this method stops playing audio streams without affecting the audio stream receiving process.

 @param uid  User ID whose audio streams the user intends to mute.
 @param mute * YES: Stops playing a specified user’s audio streams.
 * NO: Resumes playing a specified user’s audio streams.
 @param connection  \ref AgoraRtcConnection by channelId and uid combine

 @return * 0: Success.
* <0: Failure.
 */
- (int)muteRemoteAudioStreamEx:(NSUInteger)uid
                          mute:(BOOL)mute
                    connection:(AgoraRtcConnection * _Nonnull)connection;

/**
 * Sets the video encoder configuration.
 *
 * Each configuration profile corresponds to a set of video parameters,
 * including the resolution, frame rate, and bitrate.
 *
 * The parameters specified in this method are the maximum values under ideal network conditions.
 * If the video engine cannot render the video using the specified parameters
 * due to poor network conditions, the parameters further down the list are
 * considered until a successful configuration is found.
 *
 * @param config The local video encoder configuration, see #AgoraVideoEncoderConfiguration.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)setVideoEncoderConfigurationEx:(AgoraVideoEncoderConfiguration* _Nonnull)config
                           connection:(AgoraRtcConnection * _Nonnull)connection;

/** Binds the remote user to the video display window, that is, sets the view for the user of the specified uid.
*
* Usually, the application should specify the uid of the remote video in the method call before the user enters a channel. If the remote uid is unknown to the application, you can set the uid after receiving the \ref AgoraRtcEngineDelegate.rtcEngine:didJoinedOfUid:elapsed: didJoinedOfUid event.
*
* @param remote \ref AgoraRtcVideoCanvas
* @param connection  \ref AgoraRtcConnection by channelId and uid combine
* @return
* - 0: Success.
* - <0: Failure.
 */
- (int)setupRemoteVideoEx:(AgoraRtcVideoCanvas* _Nonnull)remote
               connection:(AgoraRtcConnection * _Nonnull)connection;

/** Configures the remote video display mode. The application can call this method multiple times to change the display mode.
*
* @param uid  User id of the user whose video streams are received.
* @param mode AgoraVideoRenderMode
* @param mirror AgoraVideoMirrorMode
* @param connection  \ref AgoraRtcConnection by channelId and uid combine
*
* @return
* - 0: Success.
* - <0: Failure.
*/
- (int)setRemoteRenderModeEx:(NSUInteger)uid
                        mode:(AgoraVideoRenderMode)mode
                      mirror:(AgoraVideoMirrorMode)mirror
                  connection:(AgoraRtcConnection * _Nonnull)connection;

/**
 * Stops or resumes receiving the video stream of a specified user.
 *
 * @note
 * Once you leave the channel, the settings in this method becomes invalid.
 *
 * @param uid ID of the specified remote user.
 * @param mute Determines whether to receive or stop receiving a specified video stream:
 * - `YES`: Stop receiving the specified video stream.
 * - `NO`: (Default) Receive the specified video stream.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)muteRemoteVideoStreamEx:(NSUInteger)uid
                          mute:(BOOL)mute
                    connection:(AgoraRtcConnection * _Nonnull)connection;

 /**
   * Enables or disables the dual video stream mode.
   *
   * If dual-stream mode is enabled, the subscriber can choose to receive the high-stream
   * (high-resolution high-bitrate video stream) or low-stream (low-resolution low-bitrate video
   * stream) video using \ref setRemoteVideoStreamType setRemoteVideoStreamType.
   *
   * @param sourceType The video source type.
   * @param enabled
   * - true: Enable the dual-stream mode.
   * - false: (default) Disable the dual-stream mode.
   * @param streamConfig The minor stream config
   * @param connection An output parameter which is used to control different connection instances.
   */
- (int)enableDualStreamModeEx:(AgoraVideoSourceType)sourceType
                      enabled:(BOOL)enabled
                 streamConfig:(AgoraSimulcastStreamConfig*)streamConfig
                   connection:(AgoraRtcConnection* _Nonnull)connection;

 /**
   * Enables or disables the dual video stream mode.
   *
   * If dual-stream mode is enabled, the subscriber can choose to receive the high-stream
   * (high-resolution high-bitrate video stream) or low-stream (low-resolution low-bitrate video
   * stream) video using \ref setRemoteVideoStreamType setRemoteVideoStreamType.
   *
   * @param sourceType The video source type.
   * @param mode The dual-stream mode.
   * @param streamConfig The minor stream config
   * @param connection An output parameter which is used to control different connection instances.
   */
- (int)setDualStreamModeEx:(AgoraVideoSourceType)sourceType
                      mode:(AgoraSimulcastStreamMode)mode
              streamConfig:(AgoraSimulcastStreamConfig*)streamConfig
                connection:(AgoraRtcConnection* _Nonnull)connection;


/**
 * Sets the remote video stream type.
 *
 * If the remote user has enabled the dual-stream mode, by default the SDK
 * receives the high-stream video. Call this method to switch to the low-stream
 * video.
 *
 * @note
 * This method applies to scenarios where the remote user has enabled the
 * dual-stream mode by \ref enableDualStreamMode: enableDualStreamMode
 * before joining the channel.
 *
 * @param uid ID of the remote user sending the video stream.
 * @param streamType The video stream type: #AgoraVideoStreamType.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)setRemoteVideoStreamEx:(NSUInteger)uid
                         type:(AgoraVideoStreamType)streamType
                   connection:(AgoraRtcConnection * _Nonnull)connection;
/**
   * Sets the remote video subscription options
   *
   *
   * @param uid ID of the remote user sending the video stream.
   * @param options Sets the video subscription options.
   * @param connection  \ref AgoraRtcConnection by channelId and uid combine
   * @return
   * - 0: Success.
   * - < 0: Failure.
   */
- (int)setRemoteVideo:(NSUInteger)uid SubscriptionOptionsEx:(AgoraVideoSubscriptionOptions* _Nonnull)options
           connection:(AgoraRtcConnection* _Nonnull)connection;

/**
 * Pushes the encoded external video frame to specified connection in Agora SDK.
 *
 * @note
 * Ensure that you have configured encoded video source before calling this method.
 *
 * @param data The encoded external video data, which must be the direct buffer.
 * @param frameInfo The encoded external video frame info: AgoraEncodedVideoFrameInfo.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 *
 * @return
 * - 0: Success, which means that the encoded external video frame is pushed successfully.
 * - < 0: Failure, which means that the encoded external video frame fails to be pushed.
 */
- (int)pushExternalEncodedVideoFrameEx:(NSData* _Nonnull)frame
                                  info:(AgoraEncodedVideoFrameInfo * _Nonnull)info
                            videoTrackId:(NSUInteger)videoTrackId;

/**
 * Pushes the external video frame.
 *
 * This method pushes the video frame using the AgoraVideoFrame class and
 * passes it to the Agora SDK with the `format` parameter in AgoraVideoFormat.
 *
 * Call \ref setExternalVideoSource:useTexture:pushMode: setExternalVideoSource
 * and set the `pushMode` parameter as `YES` before calling this method.
 * @note
 * In the Communication profile, this method does not support pushing textured
 * video frames.
 * @param frame Video frame containing the SDK's encoded video data to be
 * pushed: #AgoraVideoFrame.
 * @param videoTrackId The id of the video track.
 * @return
 * - `YES`: Success.
 * - `NO`: Failure.
 */
- (BOOL)pushExternalVideoFrame:(AgoraVideoFrame * _Nonnull)frame videoTrackId:(NSUInteger)videoTrackId;

/** Gets the user information by passing in the user account.

 * After a remote user joins the channel, the SDK gets the user ID and user account of the remote user, caches them in a mapping table object (`AgoraUserInfo`), and triggers the [didUpdatedUserInfo]([AgoraRtcEngineDelegate rtcEngine:didUpdatedUserInfo:withUid:]) callback on the local client.

 * After receiving the [didUpdatedUserInfo]([AgoraRtcEngineDelegate rtcEngine:didUpdatedUserInfo:withUid:]) callback, you can call this method to get the user ID of the remote user from the `userInfo` object by passing in the user account.

 * @param userAccount The user account of the user. Ensure that you set this parameter.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 * @return An [AgoraUserInfo](AgoraUserInfo) object that contains the user account and user ID of the user.
 */
- (AgoraUserInfo* _Nullable)getUserInfoByUserAccountEx:(NSString* _Nonnull)userAccount
                                            connection:(AgoraRtcConnection * _Nonnull)connection
                                             withError:(AgoraErrorCode* _Nullable)error;

/** Gets the user information by passing in the user ID.
 *
 * After a remote user joins the channel, the SDK gets the user ID and user account of the remote user,
 * caches them in a mapping table object (`userInfo`), and triggers the \ref agora::rtc::IRtcEngineEventHandler::onUserInfoUpdated "onUserInfoUpdated" callback on the local client.
 *
 * After receiving the \ref agora::rtc::IRtcEngineEventHandler::onUserInfoUpdated "onUserInfoUpdated" callback, you can call this method to get the user account of the remote user
 * from the `userInfo` object by passing in the user ID.
 *
 * @param uid The user ID of the remote user. Ensure that you set this parameter.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 * @return An [AgoraUserInfo](AgoraUserInfo) object that contains the user account and user ID of the user.
 */
- (AgoraUserInfo* _Nullable)getUserInfoByUidEx:(NSUInteger)uid
                                    connection:(AgoraRtcConnection * _Nonnull)connection
                                     withError:(AgoraErrorCode* _Nullable)error;

/**
 * Gets the connection state of the SDK.
 *
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine
 *
 * @return The connection state. See \ref AgoraConnectionState.
 */
- (AgoraConnectionState)getConnectionStateEx:(AgoraRtcConnection * _Nonnull)connection;

#if (!(TARGET_OS_IPHONE) && (TARGET_OS_MAC))
/** Enables loopback sampling. (macOS only)
  * If you enable loopback sampling, the output of the sound card is mixed into the audio stream sent to the other end.
  * You can call this method either before or after joining a channel.
  
  * *Note:**
  * macOS does not support loopback sampling of the default sound card. If you need to use this method,
  * please use a virtual sound card and pass its name to the `deviceName` parameter. Agora has tested and recommends using soundflower.

  * @param enabled Sets whether to enable/disable loopback sampling.
  * YES: Enable loopback sampling.
  * NO: (Default) Disable loopback sampling.

  * @param deviceName Pointer to the device name of the sound card. The default value is nil (default sound card).
  * If you use a virtual sound card like "Soundflower", set this parameter as the name of the sound card, "Soundflower",
  * and the SDK will find the corresponding sound card and start capturing.
  * @param connection  \ref AgoraRtcConnection by channelId and uid combine
  * @return
  * 0: Success.
  * < 0: Failure.
  */
- (int)enableLoopbackRecordingEx:(BOOL)enabled
                      deviceName:(NSString* _Nullable)deviceName
                      connection:(AgoraRtcConnection * _Nonnull)connection;;
#endif

- (int)sendCustomReportMessageEx:(NSString * _Nullable)messageId
                        category:(NSString * _Nullable)category
                           event:(NSString * _Nullable)event
                           label:(NSString * _Nullable)label
                           value:(NSInteger)value
                      connection:(AgoraRtcConnection * _Nonnull)connection;

- (int)enableAudioVolumeIndicationEx:(NSInteger)interval
                              smooth:(NSInteger)smooth
                           reportVad:(BOOL)reportVad
                          connection:(AgoraRtcConnection* _Nonnull)connection;

/** Sets the sound position and gain of a remote user.

 When the local user calls this method to set the sound position of a remote user, the sound difference between the left and right channels allows the local user to track the real-time position of the remote user, creating a real sense of space. This method applies to massively multiplayer online games, such as Battle Royale games.

**Note:**

- Ensure that you call this method after joining a channel. For this method to work, enable stereo panning for remote users by calling [enableSoundPositionIndication]([AgoraRtcEngineKit enableSoundPositionIndication:]) before joining a channel.
This method requires hardware support.
- For the best effect, we recommend using the following audio output devices:
  - (iOS) A stereo headset.
  - (macOS) A stereo loudspeaker.
 @param uid The ID of the remote user.
 @param pan The sound position of the remote user. The value ranges from -1.0 to 1.0:

 * 0.0: (default) the remote sound comes from the front.
 * -1.0: the remote sound comes from the left.
 * 1.0: the remote sound comes from the right.

 @param gain Gain of the remote user. The value ranges from 0.0 to 100.0. The default value is 100.0 (the original gain of the remote user). The smaller the value, the less the gain.
 @param connection  \ref AgoraRtcConnection by channelId and uid combine

 @return * 0: Success.
* < 0: Failure.
 */
- (int)setRemoteVoicePositionEx:(NSUInteger)uid
                          pan:(double)pan
                         gain:(double)gain
                   connection:(AgoraRtcConnection * _Nonnull)connection;

/** Sets spatial audio parameters of a remote user.

 When the local user calls this method to set the spatial audio parameters  of a remote user, the sound difference between the left and right channels allows the local user to track the real-time position of the remote user, creating a real sense of spatial.

**Note:**

- For the best effect, we recommend using the following audio output devices:
  - (iOS) A stereo headset.
  - (macOS) A stereo loudspeaker.
 @param uid The ID of the remote user.
 @param params The spatial audio parameters of the remote user. 
 @param connection  \ref AgoraRtcConnection by channelId and uid combine
 
 @return * 0: Success.
* < 0: Failure.
 */
- (int)setRemoteUserSpatialAudioParamsEx:(NSUInteger)uid
                                  params:(AgoraSpatialAudioParams* _Nonnull)params
                              connection:(AgoraRtcConnection* _Nonnull)connection;

/** Adds a watermark image to the local video.

 This method adds a PNG watermark image to the local video in the interactive live streaming. Once the watermark image is added, all the audience in the channel (CDN audience included), and the capturing device can see and capture it. Agora supports adding only one watermark image onto the local video, and the newly watermark image replaces the previous one.

 The watermark position depends on the settings in the [setVideoEncoderConfiguration]([AgoraRtcEngineKit setVideoEncoderConfiguration:]) method:

 - If the orientation mode of the encoding video is AgoraVideoOutputOrientationModeFixedLandscape, or the landscape mode in AgoraVideoOutputOrientationModeAdaptative, the watermark uses the landscape orientation.
 - If the orientation mode of the encoding video is AgoraVideoOutputOrientationModeFixedPortrait, or the portrait mode in AgoraVideoOutputOrientationModeAdaptative, the watermark uses the portrait orientation.
 - When setting the watermark position, the region must be less than the dimensions set in the [setVideoEncoderConfiguration]([AgoraRtcEngineKit setVideoEncoderConfiguration:]) method. Otherwise, the watermark image will be cropped.

 **Note**

 - Ensure that you have called the [enableVideo]([AgoraRtcEngineKit enableVideo]) method to enable the video module before calling this method.
 - If you only want to add a watermark image to the local video for the audience in the CDN live streaming channel to see and capture, you can call this method or the [setLiveTranscoding]([AgoraRtcEngineKit setLiveTranscoding:]) method.
 - This method supports adding a watermark image in the PNG file format only. Supported pixel formats of the PNG image are RGBA, RGB, Palette, Gray, and Alpha_gray.
 - If the dimensions of the PNG image differ from your settings in this method, the image will be cropped or zoomed to conform to your settings.
 - If you have enabled the local video preview by calling the [startPreview]([AgoraRtcEngineKit startPreview]) method, you can use the `visibleInPreview` member in the WatermarkOptions class to set whether or not the watermark is visible in preview.
 - If you have enabled the mirror mode for the local video, the watermark on the local video is also mirrored. To avoid mirroring the watermark, Agora recommends that you do not use the mirror and watermark functions for the local video at the same time. You can implement the watermark function in your application layer.

 @param url The local file path of the watermark image to be added. This method supports adding a watermark image from the local file path. If the watermark image to be added is in the project file, you need to change the image's Type from PNG image to Data in the Xcode property, otherwise, the Agora Native SDK cannot recognize the image.
 @param options The options of the watermark image to be added. See WatermarkOptions.
 @param connection  \ref AgoraRtcConnection by channelId and uid combine

 @return * 0: Success.
 * < 0: Failure.
 */
- (int)addVideoWatermarkEx:(NSURL* _Nonnull)url options:(WatermarkOptions* _Nonnull)options connection:(AgoraRtcConnection * _Nonnull)connection;

/** Clears the watermark image on the video stream.
 @param connection  \ref AgoraRtcConnection by channelId and uid combine

 @return * 0: Success.
 * < 0: Failure.
 */
- (int)clearVideoWatermarkEx:(AgoraRtcConnection * _Nonnull)connection;

/**-----------------------------------------------------------------------------
 * @name Data Steam
 * -----------------------------------------------------------------------------
 */
/** Creates a data stream.
*
* Each user can create up to five data streams during the lifecycle of the `AgoraRtcEngineKit`.
*
* @note Set both the `reliable` and `ordered` parameters to `YES` or `NO`. Do not set one as `YES` and the other as `NO`.
*
* @param streamId ID of the created data stream.
* @param reliable Sets whether or not the recipients are guaranteed to receive the data stream from the sender within five seconds:
* - YES: The recipients receive the data stream from the sender within five seconds. If the recipient does not receive the data stream within five seconds, an error is reported to the app.
* - NO: There is no guarantee that the recipients receive the data stream within five seconds and no error message is reported for any delay or missing data stream.
*
* @param ordered  Sets whether or not the recipients receive the data stream in the sent order:
* - YES: The recipients receive the data stream in the sent order.
* - NO: The recipients do not receive the data stream in the sent order.
* @param connection  \ref AgoraRtcConnection by channelId and uid combine.
*
* @return
* - 0: Success.
* - < 0: Failure.
*/
- (int)createDataStreamEx:(NSInteger * _Nonnull)streamId
                 reliable:(BOOL)reliable
                  ordered:(BOOL)ordered
               connection:(AgoraRtcConnection * _Nonnull)connection;
/** Creates a data stream.

 Each user can create up to five data streams during the lifecycle of the [AgoraRtcChannel](AgoraRtcChannel).

 @param streamId ID of the created data stream.
 @param config the config of data stream.
 @param connection  \ref AgoraRtcConnection by channelId and uid combine.
 @return * 0: Success.
* < 0: Failure.
 */
- (int)createDataStreamEx:(NSInteger * _Nonnull)streamId
                   config:(AgoraDataStreamConfig * _Nonnull)config
               connection:(AgoraRtcConnection * _Nonnull)connection;

/** Sends data stream messages to all users in a channel.

The SDK has the following restrictions on this method:

- Up to 60 packets can be sent per second in a channel with each packet having a maximum size of 1 KB.
- Each client can send up to 30 KB of data per second.
- Each user can have up to five data streams simultaneously.

If the remote user receives the data stream within five seconds, the SDK triggers the \ref AgoraRtcEngineDelegate.rtcEngine:receiveStreamMessageFromUid:streamId:data: receiveStreamMessageFromUid callback on the remote client, from which the remote user gets the stream message.

If the remote user does not receive the data stream within five seconds, the SDK triggers the \ref AgoraRtcEngineDelegate.rtcEngine:didOccurStreamMessageErrorFromUid:streamId:error:missed:cached: didOccurStreamMessageErrorFromUid callback on the remote client.

 @note
 - This method applies only to the Communication profile or to the hosts in the live interactive streaming profile. If an audience in the live interactive streaming profile calls this method, the audience role may be changed to a host.
 - Ensure that you have created the data stream using \ref createDataStream:reliable:ordered: createDataStream before calling this method.

 @param streamId ID of the sent data stream returned in the `createDataStream` method.
 @param data   Sent data.
 @param connection  \ref AgoraRtcConnection by channelId and uid combine.

 @return
 - 0: Success.
 - < 0: Failure.
*/
- (int)sendStreamMessageEx:(NSInteger)streamId
                      data:(NSData * _Nonnull)data
                connection:(AgoraRtcConnection * _Nonnull)connection;

/**-----------------------------------------------------------------------------
 * @name Stream Publish
 * -----------------------------------------------------------------------------
 */

 /**
 * Sets the blacklist of subscribe remote stream audio.
 *
 * @param blacklist The uid list of users who do not subscribe to audio.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine.
 * @note
 * If uid is in blacklist, the remote user's audio will not be subscribed,
 * even if muteRemoteAudioStream(uid, false) and muteAllRemoteAudioStreams(false) are operated.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)setSubscribeAudioBlacklistEx:(NSArray <NSNumber *> *_Nonnull)blacklist connection:(AgoraRtcConnection * _Nonnull)connection;

/**
 * Sets the whitelist of subscribe remote stream audio.
 *
 * @param whitelist The uid list of users who do subscribe to audio.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine.
 * @note
 * If uid is in whitelist, the remote user's audio will be subscribed,
 * even if muteRemoteAudioStream(uid, true) and muteAllRemoteAudioStreams(true) are operated.
 *
 * If a user is in the blacklist and whitelist at the same time, the user will not subscribe to audio.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)setSubscribeAudioWhitelistEx:(NSArray <NSNumber *> *_Nonnull)whitelist connection:(AgoraRtcConnection * _Nonnull)connection;

/**
 * Sets the blacklist of subscribe remote stream video.
 *
 * @param blacklist The uid list of users who do not subscribe to video.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine.
 * @note
 * If uid is in blacklist, the remote user's video will not be subscribed,
 * even if muteRemoteVideoStream(uid, false) and muteAllRemoteVideoStreams(false) are operated.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)setSubscribeVideoBlacklistEx:(NSArray <NSNumber *> *_Nonnull)blacklist connection:(AgoraRtcConnection * _Nonnull)connection;

/**
 * Sets the whitelist of subscribe remote stream video.
 *
 * @param whitelist The uid list of users who do subscribe to video.
 * @param connection  \ref AgoraRtcConnection by channelId and uid combine.
 * @note
 * If uid is in whitelist, the remote user's video will be subscribed,
 * even if muteRemoteVideoStream(uid, true) and muteAllRemoteVideoStreams(true) are operated.
 *
 * If a user is in the blacklist and whitelist at the same time, the user will not subscribe to video.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)setSubscribeVideoWhitelistEx:(NSArray <NSNumber *> *_Nonnull)whitelist connection:(AgoraRtcConnection * _Nonnull)connection;

- (NSInteger)takeSnapshotEx:(AgoraRtcConnection * _Nonnull)connection uid:(NSInteger)uid filePath:(NSString* _Nonnull)filePath;
@end

NS_ASSUME_NONNULL_END
