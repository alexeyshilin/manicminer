//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStore : MonoBehaviour
{
    public string snapshotFile = "manicminer";
    private List<RoomData> _rooms;
    private List<byte[]> _sprites = new List<byte[]>();

    public IList<RoomData> Rooms { get { return _rooms; } }

    public List<byte[]> MinerWillySprites { get { return _sprites; } }

    public bool IsReady { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _rooms = new List<RoomData>();

        using (SnapshotImporter importer = new SnapshotImporter(snapshotFile) )
        {
            int offset = 45056;

            for(int i=0; i<20; i++)
            {
                importer.Seek(offset);

                ImportRoom(importer, IsSpecialRoom(i));

                offset += 1024;
            }

            importer.Seek(33280); // 33280-33535 - sprites for Miner Willy
            for(int i=0; i<8; i++)
            {
                byte[] sprite = importer.ReadBytes(32);
                _sprites.Add(sprite);
            }
        }

        IsReady = true;
    }

    private bool IsSpecialRoom(int id)
    {
        return (id >= 0 && id <= 2) || id == 4;
    }

    void ImportRoom(SnapshotImporter importer, bool hasSpecialGraphics)
    {
        //throw new NotImplementedException();

        RoomData data = new RoomData();

        /*
        for(int y=0; y<16; y++)
        {
            for(int x=0; x<32; x++)
            {
                data.SetAttr(x, y, importer.Read());
            }
        }
        */

        byte[] buf = importer.ReadBytes(512);
        int ii = 0;

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                //data.SetAttr(x, y, buf[i]);
                data.Attributes[ii] = buf[ii];
                ii++;
            }
        }

        data.RoomName = importer.ReadString(32);

        for(int i=0; i<8; i++)
        {
            byte attr = importer.Read();

            ////SpriteTexture tex = new SpriteTexture(8, 8, Vector2.zero);
            //SpriteTexture tex = new SpriteTexture(8, 8, new Vector2(0,1));
            //tex.Clear(new Color(0,0,0,0));
            //for(int y=0; y<8; y++)
            //{
            //    tex.SetLine(y, importer.Read());
            //}

            //data.Blocks[attr] = tex.Apply();

            byte[] blockData = importer.ReadBytes(8);
            data.Blocks[attr] = new BlockData(blockData, (BlockType)i);

            // for debug only
            /*
            GameObject go = new GameObject();
            go.transform.position = new Vector3(i*8, _rooms.Count*8, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = data.Blocks[attr];
            */
            // /for debug only
        }

        importer.Read(); // y-offset
        importer.Read(); // starts at
        importer.Read(); // direction facing
        importer.Read(); // always 0

        // start position
        short startPos = importer.ReadShort();
        CellPoint pos = new CellPoint(startPos.GetX(), startPos.GetY());
        data.StartPoint = pos;

        importer.Read(); // always 0 ???

        //importer.ReadBytes(4); // (623-626) conveyor belt
        data.ConveyorDirection = (ConveyorDirection)importer.Read();
        importer.ReadBytes(3);

        importer.Read(); // border color

        // positions of the items (keys)
        bool addKey = true;
        for(int j=0; j<5; j++)
        {
            addKey = true; // reset value (set default)

            byte attr = importer.Read();
            //if (attr == 255) continue;
            //if (attr == 0) continue;
            //if (attr == 255) addKey = false;
            //if (attr == 0) addKey = false;

            byte secondGfxBuf = importer.Read();
            short keyPosRaw = importer.ReadShort();
            CellPoint keyPos = new CellPoint(keyPosRaw.GetX(), keyPosRaw.GetY());

            importer.Read(); // dummy byte

            if(addKey)
                data.RoomKeys.Add( new RoomKey(attr, keyPos));
        }

        importer.Read(); // dummy
        importer.Read(); // dummy always =255

        // PORTAL
        byte portalColour = importer.Read(); // 655
        byte[] portalShape = importer.ReadBytes(32); // 656-687 (=687-656+1)
        short portalPositionRaw = importer.ReadShort(); // 688-691
        importer.ReadShort(); // skip short
        data.AddPortal(portalColour, portalShape, portalPositionRaw.GetX(), portalPositionRaw.GetY());
        // /PORTAL

        data.KeyShape = importer.ReadBytes(8); // ITEMS (keys) shape

        byte airFirst = importer.Read();
        byte airSize = (byte)(airFirst - 32 - 4);
        byte airPixel = importer.Read();

        data.AirSupply = new AirSupply() { Length = airSize, Tip = airPixel };

        // GUARDIANS
        // horizontal guardians(offset 702-732)
        for(int i=0; i<4; i++)
        {
            // todo: check black square in room 1
            GuardianHorizontal gh = new GuardianHorizontal();
            gh.Attribute = importer.Read();
            var pos_tmp = importer.ReadShort();
            gh.StartX = pos_tmp.GetX();
            gh.StartY = pos_tmp.GetY();
            //gh.StartPoint = new CellPoint(pos_tmp.GetX(), pos_tmp.GetY());
            importer.Read(); // ignore
            gh.StartFrame = importer.Read();
            gh.Left = importer.Read() & 0x1f;
            gh.Right = importer.Read() & 0x1f;

            if (gh.Attribute != 255)
            {
                data.HorizontalGuardians.Add(gh);
            }
        }

        importer.ReadBytes(3); // offset 730 - 255, 731-0, 732-0

        if (hasSpecialGraphics)
        {
            importer.ReadBytes(3); // ignore offset 733, 734, 735

            // SPECIAL GRAPHICS (offset 736-767)
            byte[] specialGraphic = importer.ReadBytes(32);
            data.SpecialGraphics.Add(specialGraphic);
            // /SPECIAL GRAPHICS
        }
        else
        {
            // vertical guardians(offset 702-732)
            for (int i = 0; i < 4; i++)
            {
                GuardianHorizontal gh = new GuardianHorizontal();
                gh.Attribute = importer.Read();
                var pos_tmp = importer.ReadShort();
                gh.StartX = pos_tmp.GetX();
                gh.StartY = pos_tmp.GetY();
                //gh.StartPoint = new CellPoint(pos_tmp.GetX(), pos_tmp.GetY());
                importer.Read(); // ignore
                gh.StartFrame = importer.Read();
                gh.Left = importer.Read() & 0x1f;
                gh.Right = importer.Read() & 0x1f;

                if (gh.Attribute != 255)
                {
                    //data.VerticalGuardians.Add(gh);
                }
            }
        }
        // /GUARDIANS

        // GUARDIAN GRAPHICS (offset 768-1023)
        for (int i=0; i<8; i++)
        {
            byte[] shape = importer.ReadBytes(32);
            data.GuardianGraphics.Add(shape);
        }
        // /GUARDIAN GRAPHICS


        _rooms.Add(data);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
