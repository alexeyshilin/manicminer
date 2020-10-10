using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class AirSupplyRenderer : IRenderer
{
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private RoomData _data;

    public AirSupplyRenderer(RoomData data)
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

        // air supply
        for (int x = 0; x < 10; x++)
        {
            _screen.SetAttribute(x, 17, 7, 2);
        }

        for (int x = 10; x < 32; x++)
        {
            _screen.SetAttribute(x, 17, 7, 4);
        }


        byte[] airBlock = { 0, 0, 255, 255, 255, 255, 0, 0 };

        var airSupplyLength = _data.AirSupply.Length;
        var airHead = _data.AirSupply.Tip;

        for (int x = 0; x < airSupplyLength; x++)
        {
            _screen.DrawSprite(x + 4, 17, 1, 1, airBlock);
        }

        byte[] airTipBlock = new byte[] { 0, 0, (byte)airHead, (byte)airHead, (byte)airHead, (byte)airHead, 0, 0 };
        _screen.DrawSprite(4 + airSupplyLength, 17, 1, 1, airTipBlock);

        _screen.PrintMessage(0, 17, "AIR");
    }
}
//}
