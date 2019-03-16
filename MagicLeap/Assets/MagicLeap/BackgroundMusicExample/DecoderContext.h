// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. (COMPANY) All Rights Reserved.
// Magic Leap, Inc. Confidential and Proprietary
//
// NOTICE:  All information contained herein is, and remains the property
// of COMPANY. The intellectual and technical concepts contained herein
// are proprietary to COMPANY and may be covered by U.S. and Foreign
// Patents, patents in process, and are protected by trade secret or
// copyright law.  Dissemination of this information or reproduction of
// this material is strictly forbidden unless prior written permission is
// obtained from COMPANY.  Access to the source code contained herein is
// hereby forbidden to anyone except current COMPANY employees, managers
// or contractors who have executed Confidentiality and Non-disclosure
// agreements explicitly covering such access.
//
// The copyright notice above does not evidence any actual or intended
// publication or disclosure  of  this source code, which includes
// information that is confidential and/or proprietary, and is a trade
// secret, of  COMPANY.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION,
// PUBLIC  PERFORMANCE, OR PUBLIC DISPLAY OF OR THROUGH USE  OF THIS
// SOURCE CODE  WITHOUT THE EXPRESS WRITTEN CONSENT OF COMPANY IS
// STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE LAWS AND
// INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE
// CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
// TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE,
// USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
//
// %COPYRIGHT_END%
// --------------------------------------------------------------------*/
// %BANNER_END%

#pragma once

#include <string>
#include <ml_api.h>

class DecoderContext
{
public:
    DecoderContext(const std::string& mediaUri, const std::string& trackType);
    ~DecoderContext();

    MLResult ProcessInput(int32_t& samplesDemuxed);
    MLResult ProcessOutput(int32_t& framesDecoded);
    MLResult Flush();
    MLResult Seek(int64_t timeUs);
    MLResult GetPosition(int64_t& positionUs);
    MLResult GetDuration(int64_t& durationUs);
    MLResult GetDecodeCompleted(bool& decodeCompleted);

private:
    MLHandle    _extractor;             // extractor for source (demuxer)
    MLHandle    _format;                // track format
    MLHandle    _codec;                 // track codec
    int64_t     _trackSamplesDemuxed;   // number of samples demuxed for the track
    bool        _trackSamplesEnd;       // samples demuxed for the track ended
    int64_t     _trackFramesDecoded;    // number of frames decoded for the track
    bool        _trackFramesEnd;        // frames  decoded for the track ended
    int64_t     _positionUs;            // pts of the last decoded frame in microseconds
    int64_t     _durationUs;            // format duration in microseconds

    void Release();
};
