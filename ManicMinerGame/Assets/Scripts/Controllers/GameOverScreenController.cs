﻿//using System;
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
