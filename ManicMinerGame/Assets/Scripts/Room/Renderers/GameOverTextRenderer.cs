//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Com.SloanKelly.ZXSpectrum;
//using UnityEngine;

//namespace Assets.Scripts.Room.Renderers
//{
public class GameOverTextRenderer : IRenderer
{
    //private const string GameOver = "Game    Over";
    public static readonly string GameOver = "Game    Over";

    private SpectrumScreen _screen;

    public int[] Ink { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public bool Active { get; set; }

    public void Draw()
    {
        //throw new NotImplementedException();

        if (!Active) return;

        for(int i=0; i<GameOver.Length; i++)
        {
            if(i<4 || i>8)
            {
                _screen.SetAttribute(X+i, Y, Ink[i], 0);
            }
        }

        _screen.OrDraw();
        _screen.PrintMessage(X, Y, GameOver);
        _screen.OverwriteDraw();
    }

    public void Init(SpectrumScreen screen)
    {
        //throw new NotImplementedException();

        _screen = screen;
    }
}
//}
