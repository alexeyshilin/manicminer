using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    int[,] _attrs;

    public string RoomName { get; set; }

    public int[,] Attributes { get { return _attrs; } }

    public Dictionary<int, Sprite> Blocks { get; private set; }

    public List<RoomKey> RoomKeys { get; private set; }

    public CellPoint StartPoint { get; set; }

    public RoomData()
    {
        _attrs = new int[16, 32];
        Blocks = new Dictionary<int, Sprite>();
        RoomKeys = new List<RoomKey>();
    }

    public void SetAttr(int x, int y, int attr)
    {
        //_attrs[15-y, x] = attr;
        _attrs[y, x] = attr;
    }
}
