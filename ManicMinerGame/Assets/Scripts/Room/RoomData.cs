using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    //int[,] _attrs;
    int[] _attrs;

    public string RoomName { get; set; }

    //public int[,] Attributes { get { return _attrs; } }
    public int[] Attributes { get { return _attrs; } }

    //public Dictionary<int, Sprite> Blocks { get; private set; }
    public Dictionary<int, byte[]> Blocks { get; private set; }

    public List<RoomKey> RoomKeys { get; private set; }

    public CellPoint StartPoint { get; set; }

    public Portal Portal {get; set;}

    public byte[] KeyShape { get; set; }

    //public int AirSupply { get; set; }
    public AirSupply AirSupply { get; set; }

    public RoomData()
    {
        //_attrs = new int[16, 32];
        //Blocks = new Dictionary<int, Sprite>();
        //RoomKeys = new List<RoomKey>();

        _attrs = new int[32 * 16];
        Blocks = new Dictionary<int, byte[]>();
        RoomKeys = new List<RoomKey>();
    }

    public void SetAttr(int x, int y, int attr)
    {
        //_attrs[15-y, x] = attr;
        //_attrs[y, x] = attr;
        _attrs[y * 32 + x] = attr;
    }

    public void AddPortal(byte colour, byte[] shape, int x, int y)
    {
        //throw new NotImplementedException();

        Portal = new Portal(colour, shape, x, y);
    }
}
