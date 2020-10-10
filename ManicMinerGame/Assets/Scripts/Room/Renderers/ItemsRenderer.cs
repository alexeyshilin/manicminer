using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class ItemsRenderer : IRenderer
{
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private RoomData _data;

    public ItemsRenderer(RoomData data)
    {
        _data = data;
    }

    public void Init(Com.SloanKelly.ZXSpectrum.SpectrumScreen screen)
    {
        _screen = screen;
    }

    public void Draw()
    {
        _screen.ColumnOrderSprite();
        //_screen.RowOrderSprite();

        // KEYS
        byte[] keyShape = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
        foreach (var key in _data.RoomKeys)
        {
            if (key.Attr == 255) continue;

            int attr = _data.Attributes[key.Position.Y * 32 + key.Position.X];
            attr &= 0xF8; // 11111000
            attr |= key.Attr;

            //screen.SetAttribute(key.Position.X, key.Position.Y, key.Attr, 0, true, false);
            Com.SloanKelly.ZXSpectrum.ZXAttribute attribute = new Com.SloanKelly.ZXSpectrum.ZXAttribute((byte)attr);
            _screen.SetAttribute(key.Position.X, key.Position.Y, attribute);

            //screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, keyShape);
            _screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, _data.KeyShape);
        }
        // /KEYS
    }
}
//}
