using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//namespace Assets.Scripts.Controllers
//{
[RequireComponent(typeof(RoomStore))]
[RequireComponent(typeof(RoomRenderer))]
[RequireComponent(typeof(Com.SloanKelly.ZXSpectrum.SpectrumScreen))]
public class GameController : MonoBehaviour
{
    int score = 0;
    int hiScore = 100;
    const string ScoreFormat = "High Score {0:000000}   Score {1:000000}";

    private RoomData roomData;
    private bool gameOver;

    List<Mob> mobs = new List<Mob>();

    [Tooltip("The room number (0-19)")]
    public int roomId; // 0-19

    //public RoomData RoomData { get{return roomData;} }

    IEnumerator Start()
    {
        var store = GetComponent<RoomStore>();
        var roomRenderer = GetComponent<RoomRenderer>();

        while (!store.IsReady)
        {
            yield return null;
        }

        //RoomData data = store.Rooms[roomId];
        roomData = store.Rooms[roomId];

        roomData.HorizontalGuardians.ForEach(g=>mobs.Add(new Mob(g)));

        StartCoroutine(DrawScreen(roomRenderer, roomData));
        StartCoroutine(LoseAir(roomData));

        if((roomId >= 0 && roomId <= 6) || roomId==9 || roomId==15)
        {
            StartCoroutine(BidirectionalSprites());
        }
    }

    IEnumerator DrawScreen(RoomRenderer renderer, RoomData data)
    {
        while(!gameOver)
        {
            string scoreInfo = string.Format(ScoreFormat, hiScore, score);
            //renderer.DrawScreen(data, scoreInfo);
            renderer.DrawScreen(data, mobs, scoreInfo);
            yield return null;
        }
    }

    private IEnumerator LoseAir(RoomData data)
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(1);

            data.AirSupply.Tip = (byte)(data.AirSupply.Tip << 1);
            data.AirSupply.Tip = (byte)(data.AirSupply.Tip & 0xff);

            if (data.AirSupply.Tip == 0)
            {
                data.AirSupply.Length--;
                data.AirSupply.Tip = 255;

                gameOver = !(data.AirSupply.Length >= 0);
            }
        }
    }

    private IEnumerator BidirectionalSprites()
    {
        //yield return null;

        foreach(var m in mobs)
        {
            m.FrameDirection = m.Frame < 4 ? 1 : -1;
        }

        while (!gameOver)
        {
            yield return new WaitForSeconds(0.1f); // 0.25f 0.125f 0.1f

            foreach (var m in mobs)
            {
                m.Frame += m.FrameDirection;

                // left to right
                if (m.FrameDirection>0 && m.Frame>3)
                {
                    m.Frame = 0;
                    m.X += m.FrameDirection;

                    // end?
                    if (m.X>m.Right)
                    {
                        m.X = m.Right;
                        m.FrameDirection *= -1;
                        m.Frame = 7;
                    }
                }

                // right to left
                if (m.FrameDirection <0 && m.Frame < 4)
                {
                    m.Frame = 7;
                    m.X += m.FrameDirection;

                    if (m.X < m.Left)
                    {
                        m.X = m.Left;
                        m.FrameDirection *= -1;
                        m.Frame = 0;
                    }
                }
            }
        }
    }
}
//}
