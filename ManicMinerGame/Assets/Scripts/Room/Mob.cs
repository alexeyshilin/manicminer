﻿using System;
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

    public List<byte[]> Frames { get; private set; }

    public Mob(List<byte[]> frames, int startX, int startY, int left, int right, int startFrame, byte attr)
    {
        Attribute = attr;
        Frame = startFrame;
        Left = left;
        Right = right;

        X = startX;
        Y = startY;

        Frames = new List<byte[]>();
        Frames.AddRange(frames);
    }
}
//}
