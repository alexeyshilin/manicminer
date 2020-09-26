//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStore : MonoBehaviour
{
    public string snapshotFile = "manicminer";
    private List<RoomData> _rooms;

    public IList<RoomData> Rooms { get { return _rooms; } }

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

                ImportRoom(importer);

                offset += 1024;
            }
        }

        IsReady = true;
    }

    void ImportRoom(SnapshotImporter importer)
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
        int i = 0;

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                data.SetAttr(x, y, buf[i]);
                i++;
            }
        }

        data.RoomName = importer.ReadString(32);

        for(i=0; i<8; i++)
        {
            byte attr = importer.Read();

            SpriteTexture tex = new SpriteTexture(8, 8, Vector2.zero);
            tex.Clear(new Color(0,0,0,0));
            for(int y=0; y<8; y++)
            {
                tex.SetLine(y, importer.Read());
            }

            data.Blocks[attr] = tex.Apply();

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

        importer.ReadBytes(4); // conveyor belt

        importer.Read(); // border color

        // positions of the items (keys)
        for(int j=0; j<5; j++)
        {
            byte attr = importer.Read();
            if (attr == 255) break;
            if (attr == 0) continue;

            byte secondGfxBuf = importer.Read();
            short keyPosRaw = importer.ReadShort();
            CellPoint keyPos = new CellPoint(keyPosRaw.GetX(), keyPosRaw.GetY());

            importer.Read(); // dummy byte

            data.RoomKeys.Add( new RoomKey(attr, keyPos));
        }

        _rooms.Add(data);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
