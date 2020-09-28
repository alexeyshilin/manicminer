using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room
//{
public class Portal
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public byte[] Shape { get; private set; }
    public Com.SloanKelly.ZXSpectrum.ZXAttribute Attr { get; private set; }

    public Portal(byte colour, byte[] shape, int x, int y)
    {
        X = x;
        Y = y;
        Shape = shape;
        Attr = new Com.SloanKelly.ZXSpectrum.ZXAttribute(colour);
    }
}
//}
