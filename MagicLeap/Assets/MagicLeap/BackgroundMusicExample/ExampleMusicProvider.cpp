// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. (COMPANY) All Rights Reserved.
// Magic Leap, Inc. Confidential and Proprietary
//
// NOTICE: All information contained herein is, and remains the property
// of COMPANY. The intellectual and technical concepts contained herein
// are proprietary to COMPANY and may be covered by U.S. and Foreign
// Patents, patents in process, and are protected by trade secstatus or
// copyright law. Dissemination of this information or reproduction of
// this material is strictly forbidden unless prior written permission is
// obtained from COMPANY. Access to the source code contained herein is
// hereby forbidden to anyone except current COMPANY employees, managers
// or contractors who have executed Confidentiality and Non-disclosure
// agreements explicitly covering such access.
//
// The copyright notice above does not evidence any actual or intended
// publication or disclosure of this source code, which includes
// information that is confidential and/or proprietary, and is a trade
// secret, of COMPANY. ANY REPRODUCTION, MODIFICATION, DISTRIBUTION,
// PUBLIC PERFORMANCE, OR PUBLIC DISPLAY OF OR THROUGH USE OF THIS
// SOURCE CODE WITHOUT THE EXPRESS WRITTEN CONSENT OF COMPANY IS
// STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE LAWS AND
// INTERNATIONAL TREATIES. THE RECEIPT OR POSSESSION OF THIS SOURCE
// CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
// TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE,
// USE, OR SELL ANYTHING THAT IT MAY DESCRIBE, IN WHOLE OR IN PART.
//
// %COPYRIGHT_END%
// --------------------------------------------------------------------
// %BANNER_END%

#include <vector>
#include <thread>
#include <string>

#include <cstdlib>
#include <ctime>
#include <unistd.h>

#include <ml_logging.h>
#include <ml_media_error.h>
#include <ml_music_service_provider.h>

#include "Utility.h"
#include "DecoderContext.h"
#include "MusicProviderContext.h"

// BMSProvider* callbacks do the following:
//   tells event loop running on worker thread what to do next
//   receive user commands on a different thread
//   specify what arguments are to be made in context structure

MLResult BMSProviderSetAuthString(const char* auth_string, void* context)
{
    ML_LOG(Info, "%s() line: %d auth_string: \"%s\" context: %p", __FUNCTION__, __LINE__, auth_string ? auth_string : "", context);
    return MLResult_Ok;
}

MLResult BMSProviderSetUrl(const char* uri, void* context)
{
    ML_LOG(Info, "%s() line: %d uri: %s context: %p", __FUNCTION__, __LINE__, uri ? uri : "", context);
    MLResult result = MLResult_Ok;

    if (nullptr == uri)
    {
        ML_LOG(Error, "BMSProviderSetUrl failed. Reason: uri was null.");
        return MLResult_InvalidParam;
    }

    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderSetUrl failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    // only add a string uri to the playlist after confirming its existence
    std::string uriPath = SearchMedia(uri);
    if (uriPath.empty())
    {
        ML_LOG(Error, "BMSProviderSetUrl call to SearchMedia failed.");
        return MLResult_InvalidParam;
    }

    // append uri to the bottom of playlist
    contextPtr->playlist.position = contextPtr->playlist.uris.size();
    contextPtr->playlist.uris.emplace_back(uriPath);

    ML_LOG(Info, "%s() line: %d playlist.uris[playlist.position]: %s", __FUNCTION__, __LINE__,
        contextPtr->playlist.uris[contextPtr->playlist.position].c_str());

    contextPtr->desiredUri = contextPtr->playlist.uris[contextPtr->playlist.position];

    contextPtr->command = Command::OPEN;

    return result;
}

MLResult BMSProviderSetPlaylist(const char** list, size_t count, void* context)
{
    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderSetPlaylist failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    if (nullptr == list)
    {
        ML_LOG(Error, "BMSProviderSetPlaylist failed. Reason: list was null.");
        return MLResult_InvalidParam;
    }

    if (count == 0)
    {
        ML_LOG(Error, "BMSProviderSetPlaylist failed. Reason: count is not greather than 0.");
        return MLResult_InvalidParam;
    }

    contextPtr->playlist.position = contextPtr->playlist.uris.size();
    size_t successCount = 0;
    for (size_t i = 0; i < count; ++i)
    {
        ML_LOG(Info, "%s() line: %d uri: %s", __FUNCTION__, __LINE__, list[i] ? list[i] : "");
        if (list[i] == nullptr || strlen(list[i]) <= 0)
        {
            continue;
        }

        // only add a uri to the playlist after confirming its existence
        std::string uriPath = SearchMedia(std::string(list[i]));
        if (!uriPath.empty())
        {
            contextPtr->playlist.uris.emplace_back(uriPath);
            ++successCount;
        }
    }

    if (successCount < count)
    {
        ML_LOG(Error, "BMSProviderSetPlaylist failed. Reason: failed to find %lu files.", count - successCount);
        return MLResult_InvalidParam;
    }

    ML_LOG(Info, "%s() line: %d playlist.uris[playlist.position]: %s", __FUNCTION__, __LINE__,
        contextPtr->playlist.uris[contextPtr->playlist.position].c_str());

    contextPtr->desiredUri = contextPtr->playlist.uris[contextPtr->playlist.position];
    contextPtr->command = Command::OPEN;
    return MLResult_Ok;
}

MLResult SetContextCommand(void* context, Command command, const char* function_name)
{
    if (nullptr == context)
    {
        ML_LOG(Error, "%s failed. Reason: context was null.", function_name);
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;
    contextPtr->command = command;
    return MLResult_Ok;
}

MLResult BMSProviderStart(void* context)
{
    return SetContextCommand(context, Command::START, "BMSProviderStart");
}

MLResult BMSProviderStop(void* context)
{
    return SetContextCommand(context, Command::STOP, "BMSProviderStop");
}

MLResult BMSProviderPause(void* context)
{
    return SetContextCommand(context, Command::PAUSE, "BMSProviderPause");
}

MLResult BMSProviderResume(void* context)
{
    return SetContextCommand(context, Command::RESUME, "BMSProviderResume");
}

MLResult BMSProviderSeek(uint32_t seekPosition, void* context)
{
    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderSeek failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    if (contextPtr->playbackState == MLMusicServicePlaybackState_Stopped)
    {
        MLResult result = BMSProviderStart(context);
        if (result != MLResult_Ok)
        {
            ML_LOG(Error, "BMSProviderSeek failed, unable to restart codec after seeing previous end. Reason: %d", result);
            return result;
        }
    }

    contextPtr->desiredSeekPosition = seekPosition;
    contextPtr->command = Command::SEEK;

    return MLResult_Ok;
}

MLResult BMSProviderNext(void* context)
{
    return SetContextCommand(context, Command::NEXT, "BMSProviderNext");
}

MLResult BMSProviderPrevious(void* context)
{
    return SetContextCommand(context, Command::PREVIOUS, "BMSProviderPrevious");
}

MLResult BMSProviderSetShuffle(MLMusicServiceShuffleState shuffleState, void* context)
{
    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderSetShuffle failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    contextPtr->shuffleState = shuffleState;
    MLResult result = MLMusicServiceProviderNotifyShuffleStateChange(contextPtr->shuffleState);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderSetShuffle call to MLMusicServiceProviderNotifyShuffleStateChange failed. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    return MLResult_Ok;
}

MLResult BMSProviderSetRepeat(MLMusicServiceRepeatState repeatState, void* context)
{
    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderSetShuffle failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    contextPtr->repeatState = repeatState;
    MLResult result = MLMusicServiceProviderNotifyRepeatStateChange(contextPtr->repeatState);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderSetRepeat call to MLMusicServiceProviderNotifyRepeatStateChange failed. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    return MLResult_Ok;
}

MLResult BMSProviderSetVolume(float volume, void* context)
{
    MLResult result = MLResult_Ok;

    result = MLMusicServiceProviderSetVolume(volume);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderSetVolume call to MLMusicServiceProviderSetVolume failed. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    result = MLMusicServiceProviderNotifyVolumeChange(volume);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderSetVolume call to MLMusicServiceProviderNotifyVolumeChange failed. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    return MLResult_Ok;
}

MLResult BMSProviderGetTrackLength(uint32_t* lengthMS, void* context)
{
    if (nullptr == lengthMS)
    {
        ML_LOG(Error, "BMSProviderGetTrackLength failed. Reason: lengthMS was null.");
        return MLResult_InvalidParam;
    }

    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderGetTrackLength failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    if (contextPtr->command == Command::OPEN)
    {
        // we can't get the track length while a new track is being opened
        *lengthMS = 0;
        return MLResult_Ok;
    }

    if (nullptr == contextPtr->decoderContext.get())
    {
        ML_LOG(Error, "BMSProviderGetTrackLength failed. Reason: contextPtr->decoderContext was null.");
        return MLResult_InvalidParam;
    }

    int64_t durationUs = 0;
    MLResult result = contextPtr->decoderContext->GetDuration(durationUs);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderGetTrackLength failed to get duration. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    *lengthMS = durationUs / 1000; // milliseconds resolution

    return MLResult_Ok;
}

MLResult BMSProviderGetCurrentPosition(uint32_t* position, void* context)
{
    if (nullptr == position)
    {
        ML_LOG(Error, "BMSProviderGetCurrentPosition failed. Reason: position was null.");
        return MLResult_InvalidParam;
    }

    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderGetCurrentPosition failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    if (contextPtr->command == Command::OPEN)
    {
        // we can't get current position while a new track is being opened
        *position = 0;
        return MLResult_Ok;
    }

    if (nullptr == contextPtr->decoderContext.get())
    {
        ML_LOG(Error, "BMSProviderGetCurrentPosition failed. Reason: contextPtr->decoderContext was null.");
        return MLResult_InvalidParam;
    }

    int64_t positionUs = 0;
    MLResult result = contextPtr->decoderContext->GetPosition(positionUs);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderGetCurrentPosition failed to get position. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    *position = positionUs / 1000;

    return MLResult_Ok;
}

MLResult BMSProviderGetMetadata(MLMusicServiceTrackType track, MLMusicServiceMetadata* pMetadata, void* context)
{
    if (nullptr == context)
    {
        ML_LOG(Error, "BMSProviderGetMetadata failed. Reason: context was null.");
        return MLResult_InvalidParam;
    }
    Context* contextPtr = (Context*)context;

    if (nullptr == contextPtr->decoderContext.get())
    {
        ML_LOG(Error, "BMSProviderGetMetadata failed. Reason: contextPtr->decoderContext was null.");
        return MLResult_InvalidParam;
    }

    if (nullptr == pMetadata)
    {
        ML_LOG(Error, "BMSProviderGetMetadata failed. Reason: pMetadata was null.");
        return MLResult_InvalidParam;
    }

    int64_t duration_us = 0;
    MLResult result = contextPtr->decoderContext->GetDuration(duration_us);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "BMSProviderGetMetadata failed to get duration. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    if (track != MLMusicServiceTrackType_Current)
    {
        ML_LOG(Error, "BMSProviderGetMetadata failed. Reason: track was not current.");
        return MLResult_InvalidParam;
    }

    // return the part after '/' to avoid returning full path ie; return a.mp3 rather than /package/resources/a.mp3
    const size_t lastDelimiter = contextPtr->mediaUri.rfind('/');
    if (lastDelimiter == std::string::npos)
    {
        pMetadata->track_title = contextPtr->mediaUri.c_str();
    }
    else
    {
        pMetadata->track_title = &(contextPtr->mediaUri.c_str()[lastDelimiter + 1]);
    }

    pMetadata->album_name = u8"\xc2\xabNot Available In Example\xc2\xbb";
    pMetadata->album_url = u8"\xc2\xabNot Available In Example\xc2\xbb";
    pMetadata->album_cover_url = u8"\xc2\xabNot Available In Example\xc2\xbb";
    pMetadata->artist_name = u8"\xc2\xabNot Available In Example\xc2\xbb";
    pMetadata->artist_url = u8"\xc2\xabNot Available In Example\xc2\xbb";
    pMetadata->length = duration_us / 1000000; // seconds resolution

    return MLResult_Ok;
}

void BMSProviderOnEndService(void* context)
{
    SetContextCommand(context, Command::EXIT, "BMSProviderOnEndService");
}

// runs on CommandThread
MLResult PlaylistPlayIndex(Context& context, int32_t position, bool start_playback)
{
    if (!(position >= 0 && position < context.playlist.uris.size()))
    {
        ML_LOG(Error, "PlaylistPlayIndex failed. Reason: invalid position.");
        return MLResult_InvalidParam;
    }

    context.decoderContext.reset();

    // stop playing the current track - sometimes it already ended
    if (context.playbackState != MLMusicServicePlaybackState_Stopped)
    {
        context.playbackState = MLMusicServicePlaybackState_Stopped;
        MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);
    }

    context.playlist.position = position;
    if (start_playback)
    {
        context.desiredUri = context.playlist.uris[context.playlist.position];
        context.command = Command::OPEN;
    }
    else
    {
        // set the next the next track to play when start is pressed, but don't start playing
        // eg;  when repeat_album is off and the album finishes playing;
        //      the next track to play is the top one rather than replaying the last one.
        context.mediaUri = context.playlist.uris[context.playlist.position];
    }

    return MLResult_Ok;
}

// handle track transitions:
// this function is called in 3 ways
//      Command::NONE     : track completed playing naturally
//      Command::NEXT     : user pressed next
//      Command::PREVIOUS : user pressed previous
// runs on CommandThread
MLResult HandlePlaylist(Context& context, Command command)
{
    // when repeat song is on; next/prev/track completion will all play the same track
    if (context.repeatState == MLMusicServiceRepeatState_Song)
    {
        ML_LOG(Info, "%s() line: %d RepeatState_Song track: \"%s\"", __FUNCTION__, __LINE__,
            context.playlist.uris[context.playlist.position].c_str());

        int32_t position = context.playlist.position; // next track to play is the current one

        PlaylistPlayIndex(context, position, true);
    }
    else if (context.shuffleState == MLMusicServiceShuffleState_On)
    {
        // when shuffle is on; next/prev/track completion will start playing a randomly selected track
        // shuffle takes precedence over repeat_album
        // here we will make all of them behave the same since replicating exact VLC behavior would complicate code and gain little
        int32_t position = context.playlist.position;
        while (position == context.playlist.position)
        {
            srand(time(nullptr));
            position = rand() % context.playlist.uris.size();
        }

        ML_LOG(Info, "%s() line: %d shuffle_on: playing a random track: %d in playlist: \"%s\"",
            __FUNCTION__, __LINE__, position, context.playlist.uris[position].c_str());

        PlaylistPlayIndex(context, position, true);
    }
    else
    {
        // if current track is not the last one in playlist; track completion/next will start playing the next track in playlist
        if (command == Command::NONE || command == Command::NEXT)
        {
            if (context.playlist.position + 1 < context.playlist.uris.size())
            {
                ML_LOG(Info, "%s() line: %d a track - which is not the last one - has completed playing", __FUNCTION__, __LINE__);
                // a track - which is not the last one - has completed playing
                // continue with the next track in playlist
                int32_t position = context.playlist.position + 1;

                ML_LOG(Info, "%s() line: %d continue with the next track: %d in playlist: \"%s\"",
                    __FUNCTION__, __LINE__, position, context.playlist.uris[position].c_str());

                PlaylistPlayIndex(context, position, true);
            }
            else
            {
                ML_LOG(Info, "%s() line: %d last track in playlist has completed playing", __FUNCTION__, __LINE__);
                // last track in playlist has completed playing
                // if the current track is the last one in playlist, behavior is determined by repeat_album
                if (context.repeatState == MLMusicServiceRepeatState_Album)
                {
                    // start from the beginning if repeat_album is set
                    int32_t position = 0;

                    ML_LOG(Info, "%s() line: %d continue with the first track: %d in playlist: \"%s\"",
                        __FUNCTION__, __LINE__, position, context.playlist.uris[position].c_str());

                    PlaylistPlayIndex(context, position, true);
                }
                else
                {
                    // if current track is the last one and repeat is off;
                    if (command == Command::NONE)
                    {
                        // move to a stopped state but retain the last track played
                        PlaylistPlayIndex(context, context.playlist.position, false);
                    }
                    else
                    {
                        // if next button is pressed; do nothing, keep playing current track
                    }
                }
            }
        }
        else if (command == Command::PREVIOUS)
        {
            // if current track is not the first one in playlist; pressing previous button will start playing the previous track in playlist
            if (context.playlist.position - 1 >= 0)
            {
                int32_t position = context.playlist.position - 1;

                ML_LOG(Info, "%s() line: %d continue with the previous track: %d in playlist: \"%s\"",
                    __FUNCTION__, __LINE__, position, context.playlist.uris[position].c_str());

                PlaylistPlayIndex(context, position, true);
            }
            else
            {
                // if the current track is the first one in playlist, previous button will restart the current track
                // but it will never move to last track regardless of repeat_album
                int32_t position = 0;

                ML_LOG(Info, "%s() line: %d pressing previous while playing the first track will restart it: %d in playlist: \"%s\"",
                    __FUNCTION__, __LINE__, position, context.playlist.uris[position].c_str());

                PlaylistPlayIndex(context, position, true);
            }
        }
    }

    return MLResult_Ok;
}

// note: must run on a separate thread because main() is blocked at MLMusicServiceProviderStart()
// all these commands run in the same thread ie; CommandThread handles decode/demux/render operations
void CommandThread(Context& context)
{
    ML_LOG(Info, "%s() line: %d enter", __FUNCTION__, __LINE__);

    MLResult result = MLResult_Ok;
    bool running = true;
    while (running)
    {
        switch (context.command)
        {

        case Command::OPEN:
            ML_LOG(Info, "%s() line: %d Command::OPEN desiredUri: \"%s\"", __FUNCTION__, __LINE__, context.desiredUri.c_str());

            context.decoderContext.reset();

            try
            {
                context.decoderContext.reset(new DecoderContext(context.desiredUri.c_str(), "audio/"));
                context.mediaUri = context.desiredUri;

                MLMusicServiceProviderNotifyMetadataChange();

                context.playbackState = MLMusicServicePlaybackState_Playing;
                MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);
            }
            catch (const MLResult& result)
            {
                // DecoderContext::ctor encountered an error and already logged it
                // leave it be and wait for another call to create decoder context
                context.decoderContext.reset();
            }

            context.command = Command::NONE;
            break;

        case Command::START:
            ML_LOG(Info, "%s() line: %d Command::START", __FUNCTION__, __LINE__);

            if (context.playbackState == MLMusicServicePlaybackState_Paused)
            {
                // delegate to resume
                ML_LOG(Info, "%s() line: %d Command::START STATE_PAUSED delegate to Command::RESUME", __FUNCTION__, __LINE__);
                context.command = Command::RESUME;
                continue;
            }
            else if (context.playbackState == MLMusicServicePlaybackState_Stopped)
            {
                if (context.mediaUri.size() > 0)
                {
                    // media uri never opened or alternatively stopped
                    if (context.decoderContext.get() == nullptr)
                    {
                        try
                        {
                            context.decoderContext.reset(new DecoderContext(context.mediaUri.c_str(), "audio/"));

                            MLMusicServiceProviderNotifyMetadataChange();

                            context.playbackState = MLMusicServicePlaybackState_Playing;
                            MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);
                        }
                        catch (const MLResult& result)
                        {
                            // DecoderContext::ctor encountered an error and already logged it
                            // leave it be and wait for another call to create decoder context
                            context.decoderContext.reset();
                        }
                    }
                }
                else
                {
                    ML_LOG(Error, "%s() line: %d Command::START error! no media uri specified", __FUNCTION__, __LINE__);
                }
            }

            context.command = Command::NONE;
            break;

        case Command::PAUSE:
            ML_LOG(Info, "%s() line: %d Command::PAUSE", __FUNCTION__, __LINE__);

            // perform pause by not calling demux and decode, codec does not know being in paused mode - no need to flush codec buffers
            // trying to pause by calling MediaCodecStop(), then MediaCodecStart() does not work
            // do not flush codec here, because we want to use the samples and frames currently in media pipeline when resumed

            MLMusicServiceProviderFlushAudioOutput();

            context.playbackState = MLMusicServicePlaybackState_Paused;
            MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);

            context.command = Command::NONE;
            break;

        case Command::RESUME:
            ML_LOG(Info, "%s() line: %d Command::RESUME", __FUNCTION__, __LINE__);

            // you can only resume in paused state, not while playing or stopped states
            if (context.playbackState == MLMusicServicePlaybackState_Paused)
            {
                context.playbackState = MLMusicServicePlaybackState_Playing;
                MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);
            }

            context.command = Command::NONE;
            break;

        case Command::SEEK:
            ML_LOG(Info, "%s() line: %d Command::SEEK", __FUNCTION__, __LINE__);

            MLMusicServiceProviderFlushAudioOutput();
            if (nullptr != context.decoderContext.get())
            {
                result = context.decoderContext->Seek((int64_t)context.desiredSeekPosition * 1000);
            }

            context.command = Command::NONE;
            break;

        case Command::STOP:
            ML_LOG(Info, "%s() line: %d Command::STOP", __FUNCTION__, __LINE__);

            MLMusicServiceProviderFlushAudioOutput();

            context.decoderContext.reset();
            context.playbackState = MLMusicServicePlaybackState_Stopped;
            MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);

            context.command = Command::NONE;
            break;

        case Command::NEXT:
            ML_LOG(Info, "%s() line: %d Command::NEXT", __FUNCTION__, __LINE__);

            MLMusicServiceProviderFlushAudioOutput();
            MLMusicServiceProviderNotifyStatus(MLMusicServiceStatus_NextTrack);

            context.command = Command::NONE;
            HandlePlaylist(context, Command::NEXT);
            break;

        case Command::PREVIOUS:
            ML_LOG(Info, "%s() line: %d Command::PREVIOUS", __FUNCTION__, __LINE__);

            MLMusicServiceProviderFlushAudioOutput();
            MLMusicServiceProviderNotifyStatus(MLMusicServiceStatus_PrevTrack);

            context.command = Command::NONE;
            HandlePlaylist(context, Command::PREVIOUS);
            break;

        case Command::EXIT:
            // user or the system signaled exit
            ML_LOG(Info, "%s() line: %d Command::EXIT", __FUNCTION__, __LINE__);

            MLMusicServiceProviderFlushAudioOutput();

            if (context.playbackState != MLMusicServicePlaybackState_Stopped)
            {
                context.playbackState = MLMusicServicePlaybackState_Stopped;
                MLMusicServiceProviderNotifyPlaybackStateChange(context.playbackState);
            }
            context.command = Command::NONE;
            running = false;
            break;

        default:
            break;
        }

        if (MLMusicServicePlaybackState_Playing == context.playbackState &&
            nullptr != context.decoderContext.get())
        {
            int32_t samplesDemuxed = 0;
            context.decoderContext->ProcessInput(samplesDemuxed);

            int32_t framesDecoded = 0;
            context.decoderContext->ProcessOutput(framesDecoded);

            bool decodeCompleted = false;
            result = context.decoderContext->GetDecodeCompleted(decodeCompleted);

            if (MLResult_Ok == result && decodeCompleted)
            {
                HandlePlaylist(context, Command::NONE);
            }

            // demuxer and decoder works without timeouts, hence we have to sleep to avoid infinite loop
            // because we do not wait at all for demuxing/decoding we can add a meager one
            // do not wait at all when you just grabbed a decoded frame, because there is now an input slot for sure
            if (samplesDemuxed == 0 && framesDecoded == 0)
            {
                usleep(1000);
            }
        }
        else
        {
            // no command to process and no demux/decode is going on
            // the sleep length here only determines reaction time to a user command on terminal
            usleep(1000);
        }
    }

    context.decoderContext.reset();
    context.playlist.uris.clear();

    // thread returns context pointer at success, nullptr at failure
    // - when context pointer is received as nullptr, we have no other pointer value to return
    ML_LOG(Info, "%s() line: %d exit return: %d", __FUNCTION__, __LINE__, result);
}

int main(int argc __unused, char* argv[] __unused)
{
    ML_LOG(Info, "%s() line: %d enter", __FUNCTION__, __LINE__);

    Playlist playlist;

    Context context =
    {
        Command::NONE,                       // command
        MLMusicServicePlaybackState_Stopped, // playbackState
        nullptr,                             // decoder context
        "",                                  // mediaUri
        MLMusicServiceRepeatState_Off,       // repeatState
        MLMusicServiceShuffleState_Off,      // shuffleState
        "",                                  // desiredUri
        0,                                   // desiredSeekPosition
        playlist,                            // playlist
    };

    MLMusicServiceProviderImplementation musicServiceImpl =
    {
        &BMSProviderSetAuthString,
        &BMSProviderSetUrl,
        &BMSProviderSetPlaylist,
        &BMSProviderStart,
        &BMSProviderStop,
        &BMSProviderPause,
        &BMSProviderResume,
        &BMSProviderSeek,
        &BMSProviderNext,
        &BMSProviderPrevious,
        &BMSProviderSetShuffle,
        &BMSProviderSetRepeat,
        &BMSProviderSetVolume,
        &BMSProviderGetTrackLength,
        &BMSProviderGetCurrentPosition,
        &BMSProviderGetMetadata,
        &BMSProviderOnEndService,
        &context
    };

    MLResult result = MLResult_UnspecifiedFailure;
    ML_LOG(Info, "%s() line: %d MLMusicServiceProviderCreate", __FUNCTION__, __LINE__);

    result = MLMusicServiceProviderCreate(&musicServiceImpl);
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "main call to MLMusicServiceProviderCreate failed. Reason: %s.", MLMediaResultGetString(result));
        return result;
    }

    std::thread commandWorker(CommandThread, std::ref(context));

    // This call blocks and executes the callbacks specified in MLMusicServiceProviderCreate
    // commandWorker will (eventually) finished when BMSProviderOnEndService is called
    // Note: This should match the visible_name field of provider component in manifest.xml
    result = MLMusicServiceProviderStart("ExampleMusicProvider");
    ML_LOG(Info, "%s() line: %d MLMusicServiceProviderStart returned: %s", __FUNCTION__, __LINE__, MLMediaResultGetString(result));
    if (MLResult_Ok != result)
    {
        ML_LOG(Error, "main call to MLMusicServiceProviderStart failed. Reason: %s", MLMediaResultGetString(result));
        return EXIT_FAILURE;
    }

    commandWorker.join();

    ML_LOG(Info, "%s() line: %d exit return: %s", __FUNCTION__, __LINE__, MLMediaResultGetString(result));
    return EXIT_SUCCESS;
}
