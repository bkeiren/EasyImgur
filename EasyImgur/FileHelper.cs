using System;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    static class FileHelper
    {
        /// <summary>
        /// Little-endian gzip magic number
        /// </summary>
        private const ushort GZipMagicNumber = 0x8B1F;

        /// <summary>
        /// Writes <paramref name="contents"/> to a gzip-compressed file, overwriting existing files. Uses UTF-8 encoding (no BOM).
        /// </summary>
        /// <param name="location">Path of the file to write</param>
        /// <param name="contents">Text to write to file</param>
        public static void GZipWriteFile(string location, string contents)
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(contents); // UTF-8 without BOM

            using (var inStream = new MemoryStream(sourceBytes))
            using (var fs = new FileStream(location, FileMode.Create, FileAccess.Write)) // Open file for writing, overwrite existing
            using (var outStream = new GZipStream(fs, CompressionMode.Compress))
            {
                inStream.CopyTo(outStream);
            }
        }

        /// <summary>
        /// Reads a gzip-compressed file with UTF-8 encoding.
        /// </summary>
        /// <param name="location">Path of the file to read.</param>
        /// <returns>Decompressed contents of the file.</returns>
        public static string GZipReadFile(string location)
        {
            // 4096 is the default buffer size. Don't know if PeekBytes conflicts with
            // SequentialScan but there doesn't appear to be any difference.
            using (var fs = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                Stream inStream;

                // Check if file starts with the gzip magic number
                if (BitConverter.ToUInt16(fs.PeekBytes(2), 0) != GZipMagicNumber) // Could this be shorter with `?:`? Yes! Would it be legible? No!
                {
                    // Not gzip-compressed, probably old history file version in plain text
                    // or machine is big-endian. We'll worry about the latter when someone
                    // starts using the program on a big-endian system :)
                    inStream = fs; // just read directly from the file
                }
                else
                {
                    inStream = new GZipStream(fs, CompressionMode.Decompress);
                }

                using (inStream)
                using (var outStream = new MemoryStream())
                {
                    inStream.CopyTo(outStream);
                    return Encoding.UTF8.GetString(outStream.ToArray()); // UTF8 w/o BOM
                }
            }
        }

        /// <summary>
        /// Peeks an amount of bytes from the Stream as specified by <paramref name="count"/>.
        /// Returns an empty byte array if the Stream is not seekable (<see cref="Stream.CanSeek"/> set to <see langword="false"/>).
        /// </summary>
        /// <param name="stream">Stream to peek</param>
        /// <param name="count">Amount of bytes to peek</param>
        /// <returns>Peeked bytes</returns>
        public static byte[] PeekBytes(this Stream stream, int count)
        {
            var buffer = new byte[count];
            if (!stream.CanSeek)
                return buffer;

            stream.Read(buffer, 0, count);
            stream.Seek(0, SeekOrigin.Begin);
            return buffer;
        }

        public static bool CheckIfValidImageFileHeader(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                var bytes = fs.PeekBytes(4);
                var imgFormatFromHeader = GetImageFormat(bytes);
                return imgFormatFromHeader != null;
            }
        }

        public static System.Drawing.Imaging.ImageFormat GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.Bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.Png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.Tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.Jpeg;

            return null;
        }
    }
}
