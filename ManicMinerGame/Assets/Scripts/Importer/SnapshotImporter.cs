using System;
using System.IO;
using System.Text;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class SnapshotImporter : IDisposable
{
    // https://www.icemark.com/dataformats/manic/mmformat.htm

    MemoryStream _ms;
    BinaryWriter _writer;
    BinaryReader _reader;

    public SnapshotImporter(string filename)
    {
        TextAsset asset = Resources.Load<TextAsset>(filename);

        _ms = new MemoryStream();
        _writer = new BinaryWriter(_ms);
        _reader = new BinaryReader(_ms);

        //_writer.Write(asset.bytes);
        _writer.Write(asset.bytes, 27, 49152);
        _writer.Flush();

        _ms.Seek(0, SeekOrigin.Begin);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public byte Read()
    {
        return _reader.ReadByte();
    }

    public byte[] ReadBytes(int length)
    {
        //return _reader.ReadBytes(length);
        byte[] buf = _reader.ReadBytes(length);
        return buf;
    }

    public string ReadString(int length)
    {
        //return String.Empty;
        byte[] _buf = _reader.ReadBytes(length);
        return Encoding.ASCII.GetString(_buf, 0, length);
    }

    public short ReadShort()
    {
        return _reader.ReadInt16();
    }
    internal void Seek(int offset)
    {
        offset = offset - 16384;
        //_ms.Seek(27 + offset, SeekOrigin.Begin);
        _ms.Seek(offset, SeekOrigin.Begin);
    }

    protected void Dispose(bool disposed)
    {
        // cleanup
    }
}
