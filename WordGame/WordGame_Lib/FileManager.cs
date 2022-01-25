using System.Diagnostics;
using System.IO;

namespace WordGame_Lib
{
    // Converted from TileContainer.cs in the Monogame dll to allow me to read AND write from disk
    public static class FileManager
    {
        static FileManager()
        {
            sBaseDirectory = string.Empty;
        }

        public static void RegisterBaseDirectory(string iBaseDirectoryPath)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(sBaseDirectory), "Already registered a base directory?");

            sBaseDirectory = iBaseDirectoryPath;
        }

        // ReSharper disable once InconsistentNaming
        public static bool TryOpenStreamReadSafe(string iFileName, out Stream oStream)
        {
            var stream = OpenStreamRead(iFileName);
            
            oStream = stream;
            
            return stream != null;
        }

        // ReSharper disable once InconsistentNaming
        public static bool TryOpenStreamWriteSafe(string iFileName, out Stream oStream)
        {
            var stream = OpenStreamWrite(iFileName);

            oStream = stream;

            return stream != null;
        }

        private static string sBaseDirectory;

        private static Stream OpenStreamRead(string iFileName)
        {
            if (!string.IsNullOrWhiteSpace(sBaseDirectory))
            {
                var path = Path.Combine(sBaseDirectory, iFileName);

                if (File.Exists(path))
                    return File.OpenRead(path);
                else
                    return null;
            }
            else
            {
                Debug.Fail($"Failed to open read stream at {iFileName}");
                return null;
            }
        }

        private static Stream OpenStreamWrite(string iFileName)
        {
            if (!string.IsNullOrWhiteSpace(sBaseDirectory))
            {
                var path = Path.Combine(sBaseDirectory, iFileName);

                if (File.Exists(path))
                    return File.OpenWrite(path);
                else
                {
                    var parentDirectory = Path.GetDirectoryName(path);
                    if (!string.IsNullOrWhiteSpace(parentDirectory))
                    {
                        Directory.CreateDirectory(parentDirectory);
                        return File.Create(path);
                    }
                    else
                    {
                        Debug.Fail($"Failed to open read stream at {iFileName}");
                        return null;
                    }
                }
            }
            else
            {
                Debug.Fail($"Failed to open read stream at {iFileName}");
                return null;
            }
        }
    }
}
