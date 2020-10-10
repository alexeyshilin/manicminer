using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class MinerWillyRenderer : IRenderer
{
    private MinerWilly _player;
    private RoomData _data;
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    public MinerWillyRenderer(MinerWilly player, RoomData data)
    {
        _player = player;
        _data = data;
    }

    public void Init(Com.SloanKelly.ZXSpectrum.SpectrumScreen screen)
    {
        _screen = screen;
    }

    public void Draw()
    {
        //throw new NotImplementedException();

        _screen.RowOrderSprite();

        byte[] graphic = _player.Frames[_player.Frame];

        int attr = _data.Attributes[_player.Y * 32 + _player.X];
        attr &= 0xF8; // 11111000
        //attr |= _player.Attribute.GetInk();
        attr |= 7; // always white

        //screen.SetAttribute(key.Position.X, key.Position.Y, key.Attr, 0, true, false);
        Com.SloanKelly.ZXSpectrum.ZXAttribute attribute = new Com.SloanKelly.ZXSpectrum.ZXAttribute((byte)attr);

        _screen.FillAttribute(_player.X, _player.Y, 2, 2, attribute);
        _screen.DrawSprite(_player.X, _player.Y, 2, 2, graphic);
    }
}
//}
