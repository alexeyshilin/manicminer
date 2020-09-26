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

    public int roomId; // 0-19

    public Transform target;

    [Tooltip("The material that has the pixel perfect check box")]
    public Material pixelPerfect;

    public Sprite minerStartTemp; // TODO: remove for prod

    public Sprite roomKeyTemp;

    IEnumerator Start()
    {
        store = GetComponent<RoomStore>();

        while(!store.IsReady)
        {
            yield return null;
        }

        RoomData data = store.Rooms[roomId];

        for(int y=0; y<16; y++)
        {
            for(int x=0; x<32; x++)
            {
                int attr = data.Attributes[y,x];
                if(attr != 0)
                {
                    Sprite block = data.Blocks[attr];

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

                    AddSprite(string.Format("({0}, {1})", x, y), new Vector3(x, y), block);
                }
            }
        }

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

        CellPoint pt = data.StartPoint;
        //AddSprite("Miner Willy Start", new Vector3(pt.x, pt.y), minerStartTemp);
        AddSprite("Miner Willy Start", pt.ToVector3(), minerStartTemp);

        foreach(var key in data.RoomKeys)
        {
            AddSprite("Key", key.Position.ToVector3(), roomKeyTemp);
        }
    }

    protected void AddSprite(string name, Vector3 pos, Sprite sprite)
    {
        GameObject go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.material = pixelPerfect;
        go.transform.SetParent(target);
        go.transform.localPosition = new Vector3(pos.x * 8, pos.y * -8);
    }
}