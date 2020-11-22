using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomRenderer))]
[RequireComponent(typeof(RoomStore))]
public class GameOverScreenController : MonoBehaviour
{
    private bool isRunning = true;

    IEnumerator Start()
    {
        var store = GetComponent<RoomStore>();
        var roomRenderer = GetComponent<RoomRenderer>();

        while (!store.IsReady)
        {
            yield return null;
        }

        var roomData = store.Rooms[0]; // TODO: game over room

        Mob minerWilly = new Mob(store.MinerWillySprites, 15, 12, 4, 0, 0, 7);

        var renderers = new List<IRenderer>();

        renderers.Add(new MinerWillyRenderer(minerWilly, store.Rooms[0]));

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
