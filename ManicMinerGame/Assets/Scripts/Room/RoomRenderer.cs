//using System;
using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(RoomStore))]
public class RoomRenderer : MonoBehaviour
{
    RoomStore store;

    Com.SloanKelly.ZXSpectrum.SpectrumScreen screen;

    public int roomId; // 0-19

    public Transform target;

    [Tooltip("The material that has the pixel perfect check box")]
    public Material pixelPerfect;

    public Sprite minerStartTemp; // TODO: remove for prod

    public Sprite roomKeyTemp;

    // HACK: this does not belog here
    //public CharScreen charScreen;

    IEnumerator Start()
    {
        store = GetComponent<RoomStore>();

        while(!store.IsReady)
        {
            yield return null;
        }

        screen = GetComponent<Com.SloanKelly.ZXSpectrum.SpectrumScreen>();
        var sr = target.GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(screen.Texture, new Rect(0, 0, 256, 192), new Vector2(0, 1), 1f);

        RoomData data = store.Rooms[roomId];

        for(int y=0; y<16; y++)
        {
            for(int x=0; x<32; x++)
            {
                //int attr = data.Attributes[y,x];
                int attr = data.Attributes[y*32 + x];

                if (attr != 0)
                {
                    if(!data.Blocks.ContainsKey(attr)) continue; // hack for room #19

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

                    screen.SetAttribute(x ,y, ink, paper, bright, flashing);
                    screen.DrawSprite(x,y, 1,1, data.Blocks[attr]);
                }
            }
        }

        data.Portal.Attr.Flashing = true; // Flashing!!!

        //screen.SetAttribute(data.Portal.X, data.Portal.Y, data.Portal.Attr);
        //screen.DrawSprite(data.Portal.X, data.Portal.Y, 2, 2, data.Portal.Shape);

        for (int py=0; py<2; py++)
        {
            for(int px=0; px<2; px++)
            {
                screen.SetAttribute(data.Portal.X+px, data.Portal.Y+py, data.Portal.Attr);
            }
        }

        // KEYS
        byte[] keyShape = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
        foreach (var key in data.RoomKeys)
        {
            screen.SetAttribute(key.Position.X, key.Position.Y, 2, 0, true, false);
            //screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, keyShape);
            screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, data.KeyShape);
        }
        // /KEYS

        screen.RowOrderSprite();
        screen.DrawSprite(data.Portal.X, data.Portal.Y, 2, 2, data.Portal.Shape);

        screen.ColumnOrderSprite();


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

    }

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