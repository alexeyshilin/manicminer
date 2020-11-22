using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.SloanKelly.ZXSpectrum;

//namespace Assets.Scripts.Room.Renderers
//{
public class StaticObjectRenderer : IRenderer
{
    private StaticObject _so;
    private SpectrumScreen _screen;

    public StaticObjectRenderer(StaticObject so)
    {
        _so = so;
    }

    public void Draw()
    {
        //throw new NotImplementedException();

        _screen.RowOrderSprite();
        _screen.DrawSpritePP(_so.X, _so.Y, 2, 2, _so.RowOffset, _so.Shape);
    }

    public void Init(SpectrumScreen screen)
    {
        //throw new NotImplementedException();

        _screen = screen;
    }
}
//}
