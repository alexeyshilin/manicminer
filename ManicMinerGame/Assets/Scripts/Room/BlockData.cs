using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room
//{
public class BlockData
{
    public byte[] Shape { get; private set; }

    public BlockType Type { get; private set; }

    public BlockData(byte[] shape, BlockType type)
    {
        Shape = shape;
        Type = type;
    }
}
//}
