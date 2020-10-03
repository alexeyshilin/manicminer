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
    enum GameState
    {
        Playing,
        MoveToNextCavern,
        Dead
    }

    private int score = 0;
    private int hiScore = 100;
    private const string ScoreFormat = "High Score {0:000000}   Score {1:000000}";

    private RoomData roomData;
    //private bool gameOver;
    private GameState state;
    private MinerWilly minerWilly;
    private List<Mob> mobs = new List<Mob>();
    private byte[] keyColours = new byte[] { 3, 6, 5, 4 }; // magenta, yellow, cyan, green
    private int currentKeyColour = 0;

    public Camera mainCamera;

    [Tooltip("The room number (0-19)")]
    public int roomId; // 0-19

    //public RoomData RoomData { get{return roomData;} }

    IEnumerator Start()
    {
        //UnityEngine.PlayerPrefs.SetInt("_room", 0);
        if (roomId < 0)
        {
            //roomId = 0;
            roomId = UnityEngine.PlayerPrefs.GetInt("_room"); // check PlayerPrefs._room
        }
        else
        {
            roomId = UnityEngine.PlayerPrefs.GetInt("_room");
            score = UnityEngine.PlayerPrefs.GetInt("_score");
        }

        var store = GetComponent<RoomStore>();
        var roomRenderer = GetComponent<RoomRenderer>();

        while (!store.IsReady)
        {
            yield return null;
        }

        //RoomData data = store.Rooms[roomId];
        roomData = store.Rooms[roomId];

        // miner willy
        minerWilly = new MinerWilly(store.MinerWillySprites, roomData.StartPoint.X, roomData.StartPoint.Y, 4, 0, 0, 7);

        // horizontal guardians
        roomData.HorizontalGuardians.ForEach(g=>mobs.Add(new Mob(g)));

        // conveyor shape
        foreach(var block in roomData.Blocks.Values)
        {
            if(block.Type==BlockType.Conveyor)
            {
                roomData.ConveyorShape = block.Shape;
                break;
            }
        }

        mainCamera.backgroundColor = Com.SloanKelly.ZXSpectrum.ZXColour.Get(roomData.BorderColour);

        StartCoroutine(DrawScreen(roomRenderer, roomData));
        StartCoroutine(LoseAir(roomData));
        StartCoroutine(MoveMinerWilly(minerWilly, roomData));
        StartCoroutine(CycleColours(roomData.RoomKeys));
        StartCoroutine(AnimateConveyor(roomData));
        StartCoroutine(CheckPortalCollision(roomData));
        StartCoroutine(EndOfCavernCheck(roomData));

        if ((roomId >= 0 && roomId <= 6) || roomId==9 || roomId==15)
        {
            StartCoroutine(BidirectionalSprites());
        }
    }

    IEnumerator EndOfCavernCheck(RoomData roomData)
    {
        //throw new NotImplementedException();

        while(state == GameState.Playing) yield return null;

        if(state == GameState.MoveToNextCavern)
        {
            yield return MoveToNextCavern(roomData);
        }
        else
        {
            yield return GivePlayerTheBoot();
        }

        //yield return null;
    }

    IEnumerator GivePlayerTheBoot()
    {
        //throw new NotImplementedException();
        yield return null;
    }

    IEnumerator MoveToNextCavern(RoomData data)
    {
        //throw new NotImplementedException();

        while(roomData.AirSupply.Length > 0)
        {
            // lose air
            //yield return new WaitForSeconds(0.0001f);

            data.AirSupply.Tip = (byte)(data.AirSupply.Tip << 1);
            data.AirSupply.Tip = (byte)(data.AirSupply.Tip & 0xff);

            if (roomData.AirSupply.Length > 0 && data.AirSupply.Tip == 0)
            {
                data.AirSupply.Length--;
                data.AirSupply.Tip = 255;
            }
            // /lose air

            score += 10;

            //yield return null;
            yield return new WaitForSeconds(0.0001f);
        }

        // move to next cavern
        roomId++;
        UnityEngine.PlayerPrefs.SetInt("_room", roomId);
        UnityEngine.PlayerPrefs.SetInt("_score", score);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        //yield return null;
    }

    IEnumerator CheckPortalCollision(RoomData roomData)
    {
        //throw new NotImplementedException();

        while (state == GameState.Playing)
        {
            var touch = BitCollision.CheckCollision2x2(
                 minerWilly.X, minerWilly.Y, minerWilly.Frames[minerWilly.Frame]
                ,roomData.Portal.X, roomData.Portal.Y, roomData.Portal.Shape
            );

            if (touch)
            {
                //fin
                //gameOver = true;
                state = GameState.MoveToNextCavern;
            }

            yield return null;
        }

        //yield return null;
    }

    IEnumerator AnimateConveyor(RoomData roomData)
    {
        //throw new NotImplementedException();
        //yield return null;

        float speed = 0.1f;

        while (state == GameState.Playing)
        {
            byte[] tmp = roomData.ConveyorShape;

            if(roomData.ConveyorDirection==ConveyorDirection.Left)
            {
                tmp[0] = RotateLeft(tmp[0]);
                tmp[1] = RotateRight(tmp[1]);
                tmp[2] = RotateLeft(tmp[2]);
                tmp[3] = RotateRight(tmp[3]);
            }
            else
            if (roomData.ConveyorDirection == ConveyorDirection.Right)
            {
                tmp[0] = RotateRight(tmp[0]);
                tmp[1] = RotateLeft(tmp[1]);
                tmp[2] = RotateRight(tmp[2]);
                tmp[3] = RotateLeft(tmp[3]);
            }

            roomData.ConveyorShape = tmp;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private byte RotateRight(byte v)
    {
        //throw new NotImplementedException();

        byte tmp = (byte)(v&1);
        v = (byte)(v >> 1);

        tmp = (byte)(tmp << 7);
        return (byte)(v|tmp);
    }

    private byte RotateLeft(byte v)
    {
        //throw new NotImplementedException();

        byte tmp = (byte)(v & 0x80);
        v = (byte)(v << 1);

        tmp = (byte)(tmp >> 7);
        return (byte)(v | tmp);
    }

    private IEnumerator CycleColours(List<RoomKey> roomKeys)
    {
        //throw new NotImplementedException();
        //yield return null;

        while (state == GameState.Playing)
        {
            foreach(var key in roomKeys)
            {
                key.Attr = keyColours[currentKeyColour];
            }

            currentKeyColour++;
            currentKeyColour %= keyColours.Length;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator DrawScreen(RoomRenderer renderer, RoomData data)
    {
        while(state == GameState.Playing || state==GameState.MoveToNextCavern)
        {
            string scoreInfo = string.Format(ScoreFormat, hiScore, score);
            //renderer.DrawScreen(data, scoreInfo);
            renderer.DrawScreen(data, minerWilly, mobs, scoreInfo);
            yield return null;
        }
    }

    private IEnumerator LoseAir(RoomData data)
    {
        while (state == GameState.Playing)
        {
            // lose air
            yield return new WaitForSeconds(1);

            data.AirSupply.Tip = (byte)(data.AirSupply.Tip << 1);
            data.AirSupply.Tip = (byte)(data.AirSupply.Tip & 0xff);

            if (data.AirSupply.Tip == 0)
            {
                data.AirSupply.Length--;
                data.AirSupply.Tip = 255;

                //gameOver = !(data.AirSupply.Length >= 0);
                if( data.AirSupply.Length < 0 )
                {
                    state = GameState.Dead;
                }
            }
            // /lose air
        }
    }

    private IEnumerator MoveMinerWilly(MinerWilly minerWilly, RoomData data)
    {
        //throw new NotImplementedException();
        //yield return null;

        //float speed = 0.25f;
        //float speed = 0.125f;
        float speed = 0.1f;

        while (state == GameState.Playing)
        {
            // walls
            int attrRight = data.Attributes[minerWilly.Y * 32 + (minerWilly.X + 1)];
            int attrLeft = data.Attributes[minerWilly.Y * 32 + (minerWilly.X - 1)];

            bool wallRight = data.Blocks[attrRight].Type == BlockType.Wall;
            bool wallLeft = data.Blocks[attrLeft].Type == BlockType.Wall;
            // /walls

            bool move = false;

            if (Input.GetKey(KeyCode.W))
            {
                if (minerWilly.Frame > 3)
                {
                    minerWilly.Frame -= 4;
                }

                //if (!wallRight || (wallRight && minerWilly.Frame != 4))
                if (!wallRight)
                    minerWilly.Frame += 1;

                if (minerWilly.Frame > 3)
                {
                    minerWilly.Frame = 0;
                    if (!wallRight) minerWilly.X++;
                }

                move = true;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                if (minerWilly.Frame < 4)
                {
                    minerWilly.Frame += 4;
                }

                if (!wallLeft || (wallLeft && minerWilly.Frame != 4))
                    minerWilly.Frame -= 1;

                if (minerWilly.Frame < 4)
                {
                    minerWilly.Frame = 7;
                    if (!wallLeft) minerWilly.X--;
                }

                move = true;
            }

            if (move)
            {
                yield return new WaitForSeconds(speed);
            }
            else
            {
                yield return null;
            }

            //yield return null;
        }
    }

    private IEnumerator BidirectionalSprites()
    {
        //yield return null;

        foreach(var m in mobs)
        {
            m.FrameDirection = m.Frame < 4 ? 1 : -1;
        }

        while (state == GameState.Playing)
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
