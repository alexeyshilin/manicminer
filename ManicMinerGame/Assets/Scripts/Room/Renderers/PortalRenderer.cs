using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class PortalRenderer : IRenderer
{
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private RoomData _data;

    public PortalRenderer(RoomData data)
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

        for (int py = 0; py < 2; py++)
        {
            for (int px = 0; px < 2; px++)
            {
                _screen.SetAttribute(_data.Portal.X + px, _data.Portal.Y + py, _data.Portal.Attr);
            }
        }

        _screen.RowOrderSprite();
        _screen.DrawSprite(_data.Portal.X, _data.Portal.Y, 2, 2, _data.Portal.Shape);

        _screen.ColumnOrderSprite();
    }
}
//}
