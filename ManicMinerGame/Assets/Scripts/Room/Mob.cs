using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room
//{
public class Mob : MovableObject
{
    public Mob(GuardianHorizontal g)
    {
        Attribute = g.Attribute;
        //startX = g.StartX;
        //startY = g.StartY;
        //startFrame = g.StartFrame;
        Frame = g.StartFrame;
        Left = g.Left;
        Right = g.Right;

        X = g.StartX;
        Y = g.StartY;
    }
}
//}
