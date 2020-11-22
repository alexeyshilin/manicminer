﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomRenderer))]
[RequireComponent(typeof(RoomStore))]
public class GameOverScreenController : MonoBehaviour, IScoreInformation
{
    private bool isRunning = true;
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

        var roomId = PlayerPrefs.GetInt("_room");

        while (!store.IsReady)
        {
            yield return null;
        }

        boot = new StaticObject(store.Rooms[2].SpecialGraphics[0]);
        plynth = new StaticObject(store.Rooms[1].SpecialGraphics[0], 15, 14);

        var roomData = store.Rooms[0]; // TODO: game over room

        Mob minerWilly = new Mob(store.MinerWillySprites, 15, 12, 4, 0, 0, 7);

        var renderers = new List<IRenderer>();

        renderers.Add(new MinerWillyRenderer(minerWilly, store.Rooms[roomId]));
        renderers.Add(new StaticObjectRenderer(boot));
        renderers.Add(new StaticObjectRenderer(plynth));
        renderers.Add(new AirSupplyRenderer(roomData));
        renderers.Add(new RoomNameRenderer(roomData));
        renderers.Add(new PlayerScoreRenderer(this));

        roomRenderer.Init(renderers);

        StartCoroutine(DrawTheScreen(roomRenderer));
    }

    IEnumerator DrawTheScreen(RoomRenderer roomRenderer)
    {
        //throw new NotImplementedException();

        while (isRunning)
        {
            roomRenderer.DrawScreen();
            yield return null;
        }
    }
}