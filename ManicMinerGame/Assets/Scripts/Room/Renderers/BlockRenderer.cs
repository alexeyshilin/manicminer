using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class BlockRenderer : IRenderer
{
    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private RoomData _data;

    public BlockRenderer(RoomData data)
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

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                //int attr = data.Attributes[y,x];
                int attr = _data.Attributes[y * 32 + x];

                if (attr != 0)
                {
                    if (!_data.Blocks.ContainsKey(attr)) continue; // hack for room #19

                    //Sprite block = data.Blocks[attr];

                    /*
                    GameObject go = new GameObject(string.Format("({0}, {1})", x, y));

                    var sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = block;
                    sr.material = pixelPerfect;

                    go.transform.SetParent(target);

                    //go.transform.position = new Vector3(x * 8, y * 8, 0);
                    //go.transform.localPosition = new Vector3(x*8, -192 + y*8, 0);
                    //go.transform.localPosition = new Vector3(x * 8, -128 + y * 8, 0);
                    go.transform.localPosition = new Vector3(x * 8, y * -8, 0);
                    */

                    // AddSprite(string.Format("({0}, {1})", x, y), new Vector3(x, y), block);

                    int ink = attr.GetInk();
                    int paper = attr.GetPaper();
                    bool bright = attr.IsBright();
                    bool flashing = attr.IsFlashing();

                    _screen.SetAttribute(x, y, ink, paper, bright, flashing);
                    //screen.DrawSprite(x, y, 1, 1, data.Blocks[attr]);

                    if (_data.Blocks[attr].Type == BlockType.Conveyor)
                    {
                        _screen.DrawSprite(x, y, 1, 1, _data.ConveyorShape);
                    }
                    else
                    {
                        _screen.DrawSprite(x, y, 1, 1, _data.Blocks[attr].Shape);
                    }
                }
            }
        }

        _data.Portal.Attr.Flashing = true; // Flashing!!!

        //screen.SetAttribute(data.Portal.X, data.Portal.Y, data.Portal.Attr);
        //screen.DrawSprite(data.Portal.X, data.Portal.Y, 2, 2, data.Portal.Shape);
    }
}
//}
