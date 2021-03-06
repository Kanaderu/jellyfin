using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;

namespace Emby.Server.Implementations.FFMpeg
{
    public class FFMpegLoader
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationPaths _appPaths;
        private readonly ILogger _logger;
        private readonly IZipClient _zipClient;
        private readonly IFileSystem _fileSystem;
        private readonly FFMpegInstallInfo _ffmpegInstallInfo;

        public FFMpegLoader(ILogger logger, IApplicationPaths appPaths, IHttpClient httpClient, IZipClient zipClient, IFileSystem fileSystem, FFMpegInstallInfo ffmpegInstallInfo)
        {
            _logger = logger;
            _appPaths = appPaths;
            _httpClient = httpClient;
            _zipClient = zipClient;
            _fileSystem = fileSystem;
            _ffmpegInstallInfo = ffmpegInstallInfo;
        }

        public FFMpegInfo GetFFMpegInfo(IStartupOptions options)
        {
            var customffMpegPath = options.FFmpegPath;
            var customffProbePath = options.FFprobePath;

            if (!string.IsNullOrWhiteSpace(customffMpegPath) && !string.IsNullOrWhiteSpace(customffProbePath))
            {
                return new FFMpegInfo
                {
                    ProbePath = customffProbePath,
                    EncoderPath = customffMpegPath,
                    Version = "external"
                };
            }

            var downloadInfo = _ffmpegInstallInfo;

            var prebuiltFolder = _appPaths.ProgramSystemPath;
            var prebuiltffmpeg = Path.Combine(prebuiltFolder, downloadInfo.FFMpegFilename);
            var prebuiltffprobe = Path.Combine(prebuiltFolder, downloadInfo.FFProbeFilename);
            if (File.Exists(prebuiltffmpeg) && File.Exists(prebuiltffprobe))
            {
                return new FFMpegInfo
                {
                    ProbePath = prebuiltffprobe,
                    EncoderPath = prebuiltffmpeg,
                    Version = "external"
                };
            }

            var version = downloadInfo.Version;

            if (string.Equals(version, "0", StringComparison.OrdinalIgnoreCase))
            {
                return new FFMpegInfo();
            }

            var rootEncoderPath = Path.Combine(_appPaths.ProgramDataPath, "ffmpeg");
            var versionedDirectoryPath = Path.Combine(rootEncoderPath, version);

            var info = new FFMpegInfo
            {
                ProbePath = Path.Combine(versionedDirectoryPath, downloadInfo.FFProbeFilename),
                EncoderPath = Path.Combine(versionedDirectoryPath, downloadInfo.FFMpegFilename),
                Version = version
            };

            Directory.CreateDirectory(versionedDirectoryPath);

            var excludeFromDeletions = new List<string> { versionedDirectoryPath };

            if (!File.Exists(info.ProbePath) || !File.Exists(info.EncoderPath))
            {
                // ffmpeg not present. See if there's an older version we can start with
                var existingVersion = GetExistingVersion(info, rootEncoderPath);

                // No older version. Need to download and block until complete
                if (existingVersion == null)
                {
                    return new FFMpegInfo();
                }
                else
                {
                    info = existingVersion;
                    versionedDirectoryPath = Path.GetDirectoryName(info.EncoderPath);
                    excludeFromDeletions.Add(versionedDirectoryPath);
                }
            }

            // Allow just one of these to be overridden, if desired.
            if (!string.IsNullOrWhiteSpace(customffMpegPath))
            {
                info.EncoderPath = customffMpegPath;
            }
            if (!string.IsNullOrWhiteSpace(customffProbePath))
            {
                info.ProbePath = customffProbePath;
            }

            return info;
        }

        private FFMpegInfo GetExistingVersion(FFMpegInfo info, string rootEncoderPath)
        {
            var encoderFilename = Path.GetFileName(info.EncoderPath);
            var probeFilename = Path.GetFileName(info.ProbePath);

            foreach (var directory in _fileSystem.GetDirectoryPaths(rootEncoderPath)
                .ToList())
            {
                var allFiles = _fileSystem.GetFilePaths(directory, true).ToList();

                var encoder = allFiles.FirstOrDefault(i => string.Equals(Path.GetFileName(i), encoderFilename, StringComparison.OrdinalIgnoreCase));
                var probe = allFiles.FirstOrDefault(i => string.Equals(Path.GetFileName(i), probeFilename, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(encoder) &&
                    !string.IsNullOrWhiteSpace(probe))
                {
                    return new FFMpegInfo
                    {
                        EncoderPath = encoder,
                        ProbePath = probe,
                        Version = Path.GetFileName(Path.GetDirectoryName(probe))
                    };
                }
            }

            return null;
        }
    }
}
