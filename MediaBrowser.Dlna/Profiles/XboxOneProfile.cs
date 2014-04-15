﻿using MediaBrowser.Model.Dlna;
using System.Xml.Serialization;

namespace MediaBrowser.Dlna.Profiles
{
    [XmlRoot("Profile")]
    public class XboxOneProfile : DefaultProfile
    {
        public XboxOneProfile()
        {
            Name = "Xbox One";
            XDlnaDoc = "DMS-1.50";

            Identification = new DeviceIdentification
            {
                ModelName = "Xbox One",
                FriendlyName = "Xbox-SystemOS"
            };

            TranscodingProfiles = new[]
            {
                new TranscodingProfile
                {
                    Container = "mp3",
                    AudioCodec = "mp3",
                    Type = DlnaProfileType.Audio
                },
                new TranscodingProfile
                {
                    Container = "ts",
                    VideoCodec = "h264",
                    AudioCodec = "ac3",
                    Type = DlnaProfileType.Video,
                    EstimateContentLength = true
                }
            };

            DirectPlayProfiles = new[]
            {
                new DirectPlayProfile
                {
                    Container = "ts",
                    VideoCodec = "h264",
                    AudioCodec = "ac3",
                    Type = DlnaProfileType.Video
                },
                new DirectPlayProfile
                {
                    Container = "avi",
                    VideoCodec = "mpeg4",
                    AudioCodec = "ac3,mp3",
                    Type = DlnaProfileType.Video
                },
                new DirectPlayProfile
                {
                    Container = "avi",
                    VideoCodec = "h264",
                    AudioCodec = "aac",
                    Type = DlnaProfileType.Video
                },
                new DirectPlayProfile
                {
                    Container = "mp4,mov,mkv",
                    VideoCodec = "h264,mpeg4",
                    AudioCodec = "aac,ac3",
                    Type = DlnaProfileType.Video
                },
                new DirectPlayProfile
                {
                    Container = "asf",
                    VideoCodec = "wmv2,wmv3,vc1",
                    AudioCodec = "wmav2,wmapro",
                    Type = DlnaProfileType.Video
                },
                new DirectPlayProfile
                {
                    Container = "asf",
                    AudioCodec = "wmav2,wmapro,wmavoice",
                    Type = DlnaProfileType.Audio
                },
                new DirectPlayProfile
                {
                    Container = "mp3",
                    AudioCodec = "mp3",
                    Type = DlnaProfileType.Audio
                },
                new DirectPlayProfile
                {
                    Container = "jpeg",
                    Type = DlnaProfileType.Photo
                }
            };

            ContainerProfiles = new[]
            {
                new ContainerProfile
                {
                    Type = DlnaProfileType.Video,
                    Container = "mp4,mov",

                    Conditions = new []
                    {
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.Equals,
                            Property = ProfileConditionValue.Has64BitOffsets,
                            Value = "false",
                            IsRequired = false
                        }
                    }
                }
            };

            CodecProfiles = new[]
            {
                new CodecProfile
                {
                    Type = CodecType.Video,
                    Codec = "mpeg4",
                    Conditions = new []
                    {
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.Width,
                            Value = "1920"
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.Height,
                            Value = "1080"
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.VideoFramerate,
                            Value = "30",
                            IsRequired = false
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.VideoBitrate,
                            Value = "5120000",
                            IsRequired = false
                        }
                    }
                },

                new CodecProfile
                {
                    Type = CodecType.Video,
                    Codec = "h264",
                    Conditions = new []
                    {
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.Width,
                            Value = "1920"
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.Height,
                            Value = "1080"
                        }
                    }
                },

                new CodecProfile
                {
                    Type = CodecType.Video,
                    Codec = "wmv2,wmv3,vc1",
                    Conditions = new []
                    {
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.Width,
                            Value = "1920"
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.Height,
                            Value = "1080"
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.VideoFramerate,
                            Value = "30",
                            IsRequired = false
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.VideoBitrate,
                            Value = "15360000",
                            IsRequired = false
                        }
                    }
                },

                new CodecProfile
                {
                    Type = CodecType.VideoAudio,
                    Codec = "ac3,wmav2,wmapro",
                    Conditions = new []
                    {
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.AudioChannels,
                            Value = "6",
                            IsRequired = false
                        }
                    }
                },

                new CodecProfile
                {
                    Type = CodecType.VideoAudio,
                    Codec = "aac",
                    Conditions = new []
                    {
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.LessThanEqual,
                            Property = ProfileConditionValue.AudioChannels,
                            Value = "2",
                            IsRequired = false
                        },
                        new ProfileCondition
                        {
                            Condition = ProfileConditionType.Equals,
                            Property = ProfileConditionValue.AudioProfile,
                            Value = "lc",
                            IsRequired = false
                        }
                    }
                }
            };
            
            ResponseProfiles = new[]
            {
                new ResponseProfile
                {
                    Container = "avi",
                    MimeType = "video/avi",
                    Type = DlnaProfileType.Video
                }
            };
        }
    }
}