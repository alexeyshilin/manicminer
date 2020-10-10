using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class HorizontalGuardianRenderer : IRenderer
{
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private RoomData _data;
    private IList<Mob> _mobs;

    public HorizontalGuardianRenderer(RoomData data, IList<Mob> mobs)
    {
        _data = data;
        _mobs = mobs;
    }

    public void Init(Com.SloanKelly.ZXSpectrum.SpectrumScreen screen)
    {
        _screen = screen;
    }

    public void Draw()
    {
        _screen.RowOrderSprite();

        foreach (var g in _mobs)
        {
            if (g.Attribute == 0) continue;

            byte[] graphic = _data.GuardianGraphics[g.Frame];

            _screen.FillAttribute(g.X, g.Y, 2, 2, g.Attribute.GetInk(), g.Attribute.GetPaper());
            _screen.DrawSprite(g.X, g.Y, 2, 2, graphic);
        }
    }
}
//}
