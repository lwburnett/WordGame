using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordGame_Lib
{
    // Converted from TileContainer.cs in the Monogame dll to allow me to read AND write from disk
    public static class FileManager
    {
        public static void RegisterOpenStreamReadCallback(Func<string, Stream> iCallback)
        {
            sOpenStreamReadCallback = iCallback;
        }

        public static void RegisterOpenStreamWriteCallback(Func<string, Stream> iCallback)
        {
            sOpenStreamWriteCallback = iCallback;
        }

        public static bool TryOpenStreamReadSafe(string iFileName, out Stream oStream)
        {
            var stream = sOpenStreamReadCallback(iFileName);

            oStream = stream;

            return stream != null;
        }

        public static bool TryOpenStreamWriteSafe(string iFileName, out Stream oStream)
        {
            var stream = sOpenStreamWriteCallback(iFileName);

            oStream = stream;

            return stream != null;
        }

        private static Func<string, Stream> sOpenStreamReadCallback;
        private static Func<string, Stream> sOpenStreamWriteCallback;

        // static FileManager()
        // {
        //     sLocation = string.Empty;
        //     PlatformInit();
        // }
        //
        // public static Stream OpenStreamRead(string iFileName)
        // {
        //     if (string.IsNullOrEmpty(iFileName))
        //         throw new ArgumentNullException(nameof(iFileName));
        //     var safeName = !Path.IsPathRooted(iFileName) ? 
        //         NormalizeRelativePath(iFileName) : 
        //         throw new ArgumentException("Invalid filename. TitleContainer.OpenStream requires a relative path.", iFileName);
        //
        //     Stream stream;
        //     try
        //     {
        //         stream = PlatformOpenStreamRead(safeName);
        //         if (stream == null)
        //             throw FileNotFoundException(iFileName, null);
        //     }
        //     catch (FileNotFoundException)
        //     {
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new FileNotFoundException(iFileName, ex);
        //     }
        //     return stream;
        // }
        //
        // public static Stream OpenStreamWrite(string iFileName)
        // {
        //     if (string.IsNullOrEmpty(iFileName))
        //         throw new ArgumentNullException(nameof(iFileName));
        //     var safeName = !Path.IsPathRooted(iFileName) ?
        //         NormalizeRelativePath(iFileName) :
        //         throw new ArgumentException("Invalid filename. TitleContainer.OpenStream requires a relative path.", iFileName);
        //
        //     Stream stream;
        //     try
        //     {
        //         stream = PlatformOpenStreamWrite(safeName);
        //         if (stream == null)
        //             throw FileNotFoundException(iFileName, null);
        //     }
        //     catch (FileNotFoundException)
        //     {
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new FileNotFoundException(iFileName, ex);
        //     }
        //     return stream;
        // }
        //
        // private static string sLocation;
        //
        // private static void PlatformInit()
        // {
        //     if (Directory.Exists(sLocation))
        //         return;
        //     sLocation = AppDomain.CurrentDomain.BaseDirectory;
        // }
        //
        // private static Exception FileNotFoundException(string iName, Exception iInner) => new FileNotFoundException($"Error loading \"{iName}\". File not found.", iInner);
        //
        // internal static string NormalizeRelativePath(string iName) => new Uri("file:///" + UrlEncode(iName)).LocalPath.Substring(1).Replace(NotSeparator, Separator);
        //
        // private static Stream PlatformOpenStreamRead(string iSafeName) => File.OpenRead(Path.Combine(sLocation, iSafeName));
        // private static Stream PlatformOpenStreamWrite(string iSafeName) => File.OpenWrite(Path.Combine(sLocation, iSafeName));
        //
        // private static string UrlEncode(string iUrl)
        // {
        //     var utF8Encoding = new UTF8Encoding();
        //     var stringBuilder = new StringBuilder(utF8Encoding.GetByteCount(iUrl) * 3);
        //     foreach (var ch in iUrl)
        //     {
        //         if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || Array.IndexOf<char>(UrlSafeChars, ch) != -1)
        //         {
        //             stringBuilder.Append(ch);
        //         }
        //         else
        //         {
        //             foreach (var num in utF8Encoding.GetBytes(ch.ToString()))
        //             {
        //                 stringBuilder.Append("%");
        //                 stringBuilder.Append(num.ToString("X"));
        //             }
        //         }
        //     }
        //     return stringBuilder.ToString();
        // }
        //
        // private static readonly char[] UrlSafeChars = new char[8]
        // {
        //     '.',
        //     '_',
        //     '-',
        //     ';',
        //     '/',
        //     '?',
        //     '\\',
        //     ':'
        // };
        // public static readonly char ForwardSlash = '/';
        // //public static readonly string ForwardSlashString = new string(ForwardSlash, 1);
        // public static readonly char BackwardSlash = '\\';
        // public static readonly char NotSeparator = (int)Path.DirectorySeparatorChar == (int)BackwardSlash ? ForwardSlash : BackwardSlash;
        // public static readonly char Separator = Path.DirectorySeparatorChar;
    }
}
