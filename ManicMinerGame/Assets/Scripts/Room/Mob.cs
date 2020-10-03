using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room
//{
public class Mob
{
    // private
    int startFrame;
    int startX;
    int startY;

    // public
    public byte Attribute { get; set; }
    public int Left { get; set; }
    public int Right { get; set; }
    public int Frame { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public int FrameDirection { get; set; }

    public Mob(GuardianHorizontal g)
    {
        Attribute = g.Attribute;
        startX = g.StartX;
        startY = g.StartY;
        startFrame = g.StartFrame;
        Frame = startFrame;
        Left = g.Left;
        Right = g.Right;

        X = startX;
        Y = startY;
    }
}
//}
