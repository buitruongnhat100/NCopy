using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NCopy
{
    public static class FileInfoExtension
    {
        public static void XCopy(this FileInfo file, FileInfo destination, Action<int> progressCallBack)
        {
            const int _bufferSize = 1024 * 1024; // 1MB
            byte[] _buffer = new byte[_bufferSize];
            byte[] _buffer2 = new byte[_bufferSize];

            bool _swap = false;
            int _progress = 0;
            int _reportedProgress = 0;
            int _read = 0;
            long _len = file.Length;
            float _flen = _len;
            Task _writer = null;

            using (var source = file.OpenRead())
            {
                using (var dest = destination.OpenWrite())
                {
                    dest.SetLength(source.Length);
                    for (long size = 0; size < _len; size += _read)
                    {
                        if (((_progress = ((int)(size / _flen) * 100))) != _reportedProgress)
                        {
                            progressCallBack(_reportedProgress = _progress);
                        }
                        _read = source.Read(_swap ? _buffer : _buffer2, 0, _bufferSize);
                        _writer?.Wait();
                        _writer = dest.WriteAsync(_swap ? _buffer : _buffer2, 0, _read);
                        _swap = !_swap;
                    }
                    _writer?.Wait();
                    
                }
            }
        }
     }
}
