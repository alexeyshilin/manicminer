//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomRenderer))]
[RequireComponent(typeof(RoomStore))]
public class GameOverScreenController : MonoBehaviour, IScoreInformation
{
    enum GameOverState
    {
        BootDrop,
        GameOver
    }

    //private bool isRunning = true;
    private GameOverState gameOverState = GameOverState.BootDrop;
    private StaticObject boot;
    private StaticObject plynth;
    private GameOverTextRenderer gameOverText;

    //public int Score => throw new NotImplementedException();
    public int Score
    {
        get; private set;
    }

    //public int HiScore => throw new NotImplementedException();
    public int HiScore
    {
        get; private set;
    }

    IEnumerator Start()
    {
        var store = GetComponent<RoomStore>();
        var roomRenderer = GetComponent<RoomRenderer>();

        roomRenderer.clearScreen = false;

        PlayerPrefs.SetInt("_room", 0);
        var roomId = PlayerPrefs.GetInt("_room");

        while (!store.IsReady)
        {
            yield return null;
        }

        gameOverText = new GameOverTextRenderer() { X = 10, Y = 6 };

        boot = new StaticObject(store.Rooms[2].SpecialGraphics[0], 15, 0);
        plynth = new StaticObject(store.Rooms[1].SpecialGraphics[0], 15, 14);

        var roomData = store.Rooms[0]; // TODO: game over room

        Mob minerWilly = new Mob(store.MinerWillySprites, 15, 12, 4, 0, 0, 7);
        minerWilly.Frame = 2;

        var renderers = new List<IRenderer>();

        renderers.Add(new MinerWillyRenderer(minerWilly, store.Rooms[roomId]));
        renderers.Add(new StaticObjectRenderer(boot));
        renderers.Add(new StaticObjectRenderer(plynth));
        renderers.Add(new AirSupplyRenderer(roomData));
        renderers.Add(new RoomNameRenderer(roomData));
        renderers.Add(new PlayerScoreRenderer(this));
        renderers.Add(gameOverText);

        roomRenderer.Init(renderers);

        StartCoroutine(FlashTheScreen(roomRenderer));
        StartCoroutine(DropTheBoot(boot));
        StartCoroutine(DrawTheScreen(roomRenderer));
    }

    IEnumerator FlashTheScreen(RoomRenderer roomRenderer)
    {
        //throw new NotImplementedException();

        int paper = 1;

        while (gameOverState == GameOverState.BootDrop)
        {
            yield return new WaitForSeconds(0.01f);
            roomRenderer.FloodFill(paper);
            paper++;
            if (paper == 8) paper = 1;
        }

        roomRenderer.FloodFill(0);
        StartCoroutine(FlashGameOverText());
    }

    private IEnumerator FlashGameOverText()
    {
        //throw new NotImplementedException();

        var ink = 1;
        var inks = new int[GameOverTextRenderer.GameOver.Length];

        float time = 3f;

        while(time>0)
        {
            for(int i=0; i<inks.Length; i++)
            {
                inks[i] = ink++;
                if(ink > 7)
                {
                    ink = 1;
                }
            }

            gameOverText.Ink = inks;
            gameOverText.Active = true;

            yield return null;

            time += Time.deltaTime;
        }
    }

    private IEnumerator DropTheBoot(StaticObject boot)
    {
        //throw new NotImplementedException();

        while(true)
        {
            yield return new WaitForSeconds(0.01f);

            boot.RowOffset += 2;
            if(boot.RowOffset >= 8)
            {
                boot.RowOffset = 0;
                boot.Y++;
            }

            if(boot.Y==12)
            {
                gameOverState = GameOverState.GameOver;
                yield break;
            }
        }
    }

    IEnumerator DrawTheScreen(RoomRenderer roomRenderer)
    {
        //throw new NotImplementedException();

        while (true)
        {
            roomRenderer.DrawScreen();
            yield return null;
        }
    }
}
