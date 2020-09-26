using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ZXSpectrumFont
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var file = @"c:\Program Files(x86)\Fuse\roms\48.rom";
            //var file = @".\48.rom";
            var file = @"c:\projects\unity3d\ZXSpectrumFont\48.rom";

            //using (Stream s = File.OpenRead(file))
            using (var s = File.OpenRead(file))
            {
                //using (BinaryReader r = new BinaryReader(s))
                using (var r = new BinaryReader(s))
                {
                    s.Seek(15616, SeekOrigin.Begin);
                    byte[] buf = r.ReadBytes(768); // the len of the speccy char set

                    using (var o = File.OpenWrite("charset.bytes"))
                    {
                        using (var w = new BinaryWriter(o))
                        {
                            w.Write(buf);
                        }
                    }
                }
            }
        }
    }
}
