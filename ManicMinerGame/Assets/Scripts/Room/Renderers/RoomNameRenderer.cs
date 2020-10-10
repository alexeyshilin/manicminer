using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class RoomNameRenderer : IRenderer
{
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private RoomData _data;

    public RoomNameRenderer(RoomData data)
    {
        _data = data;
    }

    public void Init(Com.SloanKelly.ZXSpectrum.SpectrumScreen screen)
    {
        _screen = screen;
    }

    public void Draw()
    {
        // room title
        for (int x = 0; x < 32; x++)
        {
            _screen.SetAttribute(x, 16, 0, 6);
        }

        _screen.PrintMessage(0, 16, _data.RoomName);
    }
}
//}
