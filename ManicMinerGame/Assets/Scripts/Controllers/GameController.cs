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

        StartCoroutine(DrawScreen(roomRenderer, roomData));
        StartCoroutine(LoseAir(roomData));
    }

    IEnumerator DrawScreen(RoomRenderer renderer, RoomData data)
    {
        while(!gameOver)
        {
            string scoreInfo = string.Format(ScoreFormat, hiScore, score);
            renderer.DrawScreen(data, scoreInfo);
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
}
//}
