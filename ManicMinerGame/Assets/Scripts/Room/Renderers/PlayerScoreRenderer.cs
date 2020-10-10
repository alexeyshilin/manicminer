using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Assets.Scripts.Room.Renderers
//{
public class PlayerScoreRenderer : IRenderer
{
    const string ScoreFormat = "High Score {0:000000}   Score {1:000000}";

    private Com.SloanKelly.ZXSpectrum.SpectrumScreen _screen;

    private IScoreInformation _data;

    public PlayerScoreRenderer(IScoreInformation data)
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

        // Score
        for (int x = 0; x < 32; x++)
        {
            _screen.SetAttribute(x, 19, 6, 0);
        }

        //screen.PrintMessage(0, 19, string.Format(ScoreFormat, hiScore, score));
        string playerScore = string.Format(ScoreFormat, _data.HiScore, _data.Score);
        _screen.PrintMessage(0, 19, playerScore);
        // /Score
    }
}
//}
