//using System;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Com.SloanKelly.ZXSpectrum.SpectrumScreen))]
public class RoomRenderer : MonoBehaviour
{
    // TODO: refactoring needed
    //int airHead = 252;
    //int airSupplyLength = 27;
    // end

    //int score = 0;
    //int hiScore = 100;
    //const string ScoreFormat = "High Score {0:000000}   Score {1:000000}"; // TODO: do not live it here

    //RoomStore store;

    Com.SloanKelly.ZXSpectrum.SpectrumScreen screen;

    //public int roomId; // 0-19

    public Transform target;

    [Tooltip("The material that has the pixel perfect check box")]
    public Material pixelPerfect;

    public Sprite minerStartTemp; // TODO: remove for prod

    public Sprite roomKeyTemp;

    // HACK: this does not belog here
    //public CharScreen charScreen;

    //void Awake()
    //{
    //    Start();
    //}

    void Start()
    {
        //store = GetComponent<RoomStore>();

        //while(!store.IsReady)
        //{
        //    yield return null;
        //}

        screen = GetComponent<Com.SloanKelly.ZXSpectrum.SpectrumScreen>();
        var sr = target.GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(screen.Texture, new Rect(0, 0, 256, 192), new Vector2(0, 1), 1f);

        //RoomData data = store.Rooms[roomId];

        //airHead = data.AirSupply.Tip;
        //airSupplyLength = data.AirSupply.Length;

        //StartCoroutine(DrawScreen(data));
        //StartCoroutine(LoseAir());
    }

    public void DrawScreen(RoomData data, string playerScore)
    {
        //while (airSupplyLength > 0)
        //{
        screen.ClearX(7, 0, false);
        //////////////////////////////////////

        DrawRoom(data);

        DrawMinerWilly(data);

        DrawItems(data); // keys

        DrawHorizontalGuardians(data);

        DrawPortal(data);

        /*
        GameObject start = new GameObject("Miner Willy Start");
        var ss = start.AddComponent<SpriteRenderer>();
        ss.sprite = minerStartTemp;
        ss.material = pixelPerfect;

        CellPoint pt = data.StartPoint;

        start.transform.SetParent(target);
        //start.transform.localPosition = new Vector3(pt.x*8, -128 + pt.y*8);
        start.transform.localPosition = new Vector3(pt.x * 8, pt.y * -8);
        */

        DrawRoomTitle(data);

        DrawAairSupply(data);

        DrawScore(playerScore);

        /*
        CellPoint pt = data.StartPoint;
        //AddSprite("Miner Willy Start", new Vector3(pt.x, pt.y), minerStartTemp);
        AddSprite("Miner Willy Start", pt.ToVector3(), minerStartTemp);

        foreach(var key in data.RoomKeys)
        {
            AddSprite("Key", key.Position.ToVector3(), roomKeyTemp);
        }


        // HACK: this does not belong here
        charScreen.PrintAt(data.RoomName, 0, 16);
        charScreen.ApplyText();
        */

        //////////////////////////////////////
        //yield return null;
        //}
    }

    public void DrawScreen(RoomData data, MinerWilly minerWilly, IList<Mob> mobs, string playerScore)
    {
        screen.ClearX(7, 0, false);

        DrawMinerWilly(minerWilly);
        DrawRoom(data);
        //DrawMinerWilly(minerWilly);
        DrawItems(data); // keys
        DrawHorizontalGuardians(mobs, data);
        DrawPortal(data);
        DrawRoomTitle(data);
        DrawAairSupply(data);
        DrawScore(playerScore);
    }

    private void DrawMinerWilly(RoomData data)
    {
        throw new NotImplementedException();
    }

    private void DrawMinerWilly(MinerWilly m)
    {
        //throw new NotImplementedException();

        screen.RowOrderSprite();

        byte[] graphic = m.Frames[m.Frame];

        screen.FillAttribute(m.X, m.Y, 2, 2, m.Attribute.GetInk(), m.Attribute.GetPaper());
        screen.DrawSprite(m.X, m.Y, 2, 2, graphic);
    }

    private void DrawScore(string playerScore)
    {
        // Score
        for (int x = 0; x < 32; x++)
        {
            screen.SetAttribute(x, 19, 6, 0);
        }

        //screen.PrintMessage(0, 19, string.Format(ScoreFormat, hiScore, score));
        screen.PrintMessage(0, 19, playerScore);
        // /Score
    }

    private void DrawAairSupply(RoomData data)
    {
        // air supply
        for (int x = 0; x < 10; x++)
        {
            screen.SetAttribute(x, 17, 7, 2);
        }

        for (int x = 10; x < 32; x++)
        {
            screen.SetAttribute(x, 17, 7, 4);
        }


        byte[] airBlock = { 0, 0, 255, 255, 255, 255, 0, 0 };

        var airSupplyLength = data.AirSupply.Length;
        var airHead = data.AirSupply.Tip;

        for (int x = 0; x < airSupplyLength; x++)
        {
            screen.DrawSprite(x + 4, 17, 1, 1, airBlock);
        }

        byte[] airTipBlock = new byte[] { 0, 0, (byte)airHead, (byte)airHead, (byte)airHead, (byte)airHead, 0, 0 };
        screen.DrawSprite(4 + airSupplyLength, 17, 1, 1, airTipBlock);

        screen.PrintMessage(0, 17, "AIR");
    }

    private void DrawRoomTitle(RoomData data)
    {
        // room title
        for (int x = 0; x < 32; x++)
        {
            screen.SetAttribute(x, 16, 0, 6);
        }

        screen.PrintMessage(0, 16, data.RoomName);
    }

    private void DrawItems(RoomData data)
    {
        // KEYS
        byte[] keyShape = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
        foreach (var key in data.RoomKeys)
        {
            if (key.Attr == 255) continue;

            int attr = data.Attributes[key.Position.Y * 32 + key.Position.X];
            attr &= 0xF8; // 11111000
            attr |= key.Attr;

            //screen.SetAttribute(key.Position.X, key.Position.Y, key.Attr, 0, true, false);
            Com.SloanKelly.ZXSpectrum.ZXAttribute attribute = new Com.SloanKelly.ZXSpectrum.ZXAttribute((byte)attr);
            screen.SetAttribute(key.Position.X, key.Position.Y, attribute);

            //screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, keyShape);
            screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, data.KeyShape);
        }
        // /KEYS
    }

    private void DrawPortal(RoomData data)
    {
        for (int py = 0; py < 2; py++)
        {
            for (int px = 0; px < 2; px++)
            {
                screen.SetAttribute(data.Portal.X + px, data.Portal.Y + py, data.Portal.Attr);
            }
        }

        screen.RowOrderSprite();
        screen.DrawSprite(data.Portal.X, data.Portal.Y, 2, 2, data.Portal.Shape);

        screen.ColumnOrderSprite();
    }

    private void DrawRoom(RoomData data)
    {
        screen.ColumnOrderSprite();

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                //int attr = data.Attributes[y,x];
                int attr = data.Attributes[y * 32 + x];

                if (attr != 0)
                {
                    if (!data.Blocks.ContainsKey(attr)) continue; // hack for room #19

                    //Sprite block = data.Blocks[attr];

                    /*
                    GameObject go = new GameObject(string.Format("({0}, {1})", x, y));

                    var sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = block;
                    sr.material = pixelPerfect;

                    go.transform.SetParent(target);

                    //go.transform.position = new Vector3(x * 8, y * 8, 0);
                    //go.transform.localPosition = new Vector3(x*8, -192 + y*8, 0);
                    //go.transform.localPosition = new Vector3(x * 8, -128 + y * 8, 0);
                    go.transform.localPosition = new Vector3(x * 8, y * -8, 0);
                    */

                    // AddSprite(string.Format("({0}, {1})", x, y), new Vector3(x, y), block);

                    int ink = attr.GetInk();
                    int paper = attr.GetPaper();
                    bool bright = attr.IsBright();
                    bool flashing = attr.IsFlashing();

                    screen.SetAttribute(x, y, ink, paper, bright, flashing);
                    //screen.DrawSprite(x, y, 1, 1, data.Blocks[attr]);

                    if(data.Blocks[attr].Type == BlockType.Conveyor)
                    {
                        screen.DrawSprite(x, y, 1, 1, data.ConveyorShape);
                    }
                    else
                    {
                        screen.DrawSprite(x, y, 1, 1, data.Blocks[attr].Shape);
                    }
                }
            }
        }

        data.Portal.Attr.Flashing = true; // Flashing!!!

        //screen.SetAttribute(data.Portal.X, data.Portal.Y, data.Portal.Attr);
        //screen.DrawSprite(data.Portal.X, data.Portal.Y, 2, 2, data.Portal.Shape);
    }

    private void DrawHorizontalGuardians(RoomData data)
    {
        screen.RowOrderSprite();

        foreach (var g in data.HorizontalGuardians)
        {
            //byte[] graphic = new byte[32];
            //for (int i = 0; i < graphic.Length; i++)
            //    graphic[i] = 255;

            //byte[] graphic = data.SpecialGraphics[0];
            byte[] graphic = data.GuardianGraphics[0];

            screen.FillAttribute(g.StartX, g.StartY, 2, 2, g.Attribute.GetInk(), g.Attribute.GetPaper());
            screen.DrawSprite(g.StartX, g.StartY, 2, 2, graphic);
        }
    }

    private void DrawHorizontalGuardians(IList<Mob> mobs, RoomData data)
    {
        screen.RowOrderSprite();

        foreach (var g in mobs)
        {
            byte[] graphic = data.GuardianGraphics[g.Frame];

            screen.FillAttribute(g.X, g.Y, 2, 2, g.Attribute.GetInk(), g.Attribute.GetPaper());
            screen.DrawSprite(g.X, g.Y, 2, 2, graphic);
        }
    }


    /*
    private IEnumerator LoseAir()
    {
        while(airSupplyLength>0)
        {
            yield return new WaitForSeconds(1);

            airHead = airHead << 1;
            airHead = airHead & 0xff;

            if (airHead == 0)
            {
                airSupplyLength--;
                airHead = 255;
            } 
        }
    }
    */

    /*
    protected void AddSprite(string name, Vector3 pos, Sprite sprite)
    {
        GameObject go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.material = pixelPerfect;
        go.transform.SetParent(target);
        go.transform.localPosition = new Vector3(pos.x * 8, pos.y * -8);
    }
    */
}