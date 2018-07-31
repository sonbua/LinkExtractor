using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Brick.IO
{
    /// <summary>
    /// @jonskeet: Collection of utility methods which operate on streams.
    /// r285, February 26th 2009: http://www.yoda.arachsys.com/csharp/miscutil/
    /// </summary>
    public static class StreamExtensions
    {
        public static string ToMd5Hash(this Stream stream)
        {
            var hash = MD5.Create().ComputeHash(stream);
            var builder = new StringBuilder();

            foreach (var byteElement in hash)
            {
                builder.Append(byteElement.ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Reads the given stream up to the end, returning the data as a byte
        /// array.
        /// </summary>
        public static byte[] ReadFully(this Stream input) => input.ReadFully(8192);

        /// <summary>
        /// Reads the given stream up to the end, returning the data as a byte
        /// array, using the given buffer size.
        /// </summary>
        public static byte[] ReadFully(this Stream input, int bufferSize)
        {
            if (bufferSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            return input.ReadFully(new byte[bufferSize]);
        }

        /// <summary>
        /// Reads the given stream up to the end, returning the data as a byte
        /// array, using the given buffer for transferring data. Note that the
        /// current contents of the buffer is ignored, so the buffer needn't
        /// be cleared beforehand.
        /// </summary>
        public static byte[] ReadFully(this Stream input, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Buffer has length of 0.");
            }

            using (var stream = new MemoryStream())
            {
                input.CopyTo(stream, buffer);

                return stream.Length == stream.GetBuffer().Length
                    ? stream.GetBuffer()
                    : stream.ToArray();
            }
        }

        /// <summary>
        /// Copies all the data from one stream into another, using the given
        /// buffer for transferring data. Note that the current contents of
        /// the buffer is ignored, so the buffer needn't be cleared beforehand.
        /// </summary>
        public static long CopyTo(this Stream input, Stream output, byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Buffer has length of 0.");
            }

            return CopyToImpl(input, output, buffer).Sum();
        }

        private static IEnumerable<long> CopyToImpl(Stream input, Stream output, byte[] buffer)
        {
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);

                yield return bytesRead;
            }
        }

        public static long WriteTo(this Stream inStream, Stream outStream)
        {
            MemoryStream memoryStream;
            
            if ((memoryStream = inStream as MemoryStream) != null)
            {
                memoryStream.WriteTo(outStream);
                return memoryStream.Position;
            }

            return WriteToImpl(inStream, outStream).Sum();
        }

        private static IEnumerable<long> WriteToImpl(Stream inStream, Stream outStream)
        {
            var buffer = new byte[4096];
            int bytesRead;
            
            while ((bytesRead = inStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                outStream.Write(buffer, 0, bytesRead);
                
                yield return bytesRead;
            }
        }
    }
}