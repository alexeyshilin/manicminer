using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//namespace Assets.Scripts.ZXSpectrum
//{
public class CharScreen : MonoBehaviour
{
    class TextMessage
    {
        public string Message { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public TextMessage(string msg, int x, int y)
        {
            Message = msg;
            X = x;
            Y = y;
        }
    }

    List<TextMessage> messages;

    Texture2D tex;

    byte[] charSet;

    [Tooltip("The char set resource file name")]
    public string charSetResources = "charset";

    [Tooltip("The pixel perfect material")]
    public Material pixelPerfect;

    public SpriteRenderer target;

    public void Cls(bool clearMessages = false)
    {
        if(clearMessages) messages.Clear();

        //Color[] fill = new Color[256 * 192];
        Color[] fill = tex.GetPixels();
        //Array.ForEach(fill, (c)=>c=Color.black);
        Array.ForEach(fill, (c) => c = new Color(0, 0, 0, 0));

        tex.SetPixels(fill);
        tex.Apply();
    }

    public void PrintAt(string msg, int x, int y) // x<=31, y<=23
    {
        messages.Add(new TextMessage(msg, x, y));
    }

    void Awake()
    {
        messages = new List<TextMessage>();

        LoadCharSet();
        CreateTexture();

        target.material = pixelPerfect;
    }

    public void ApplyText()
    {
        Cls();

        //Color[] pixels = tex.GetPixels();
        List<Color> pixels = new List<Color>(tex.GetPixels());

        foreach (var msg in messages)
        {
            //PrintMessage(pixels, msg.Message, msg.X * 8, msg.Y * 8);
            PrintMessage(pixels, msg.Message, msg.X * 8, (23 - msg.Y) * 8);
        }

        //tex.SetPixels(pixels);
        tex.SetPixels(pixels.ToArray());
        tex.Apply();

        //target.sprite = Sprite.Create(Apply(), new Rect(0,0,256,192), Vector2.zero, 1);
        //target.sprite = Sprite.Create(tex, new Rect(0, 0, 256, 192), Vector2.zero, 1);
        target.sprite = Sprite.Create(tex, new Rect(0, 0, 256, 192), new Vector2(0, 1), 1);
    }

    /*
    Texture2D Apply()
    {
        ApplyText();

        return tex;
        return null;
    }
    */

    //private void PrintMessage(Color[] pixels, string msg, int x, int y)
    private void PrintMessage(List<Color> pixels, string msg, int x, int y)
    {
        int ptr = (y * 32*8) + x;

        foreach(var ch in msg)
        {
            int ptrCopy = ptr;

            int offestIntoCharset = (ch - ' ')*8;

            for(int c=0; c<8; c++)
            {
                //int lineValue = offestIntoCharset + c;
                int lineValue = offestIntoCharset + (7-c);

                for (int i = 0; i < 8; i++)
                {
                    int pow = 7 - i;
                    byte mask = (byte)(1 << pow);

                    if ((charSet[lineValue] & mask) == mask)
                    {
                        pixels[ptr + i] = Color.white;
                    }
                }

                ptr += 32*8; // +256
            }

            ptr = ptrCopy;
            ptr += 8;
        }
    }

    private void CreateTexture()
    {
        //tex = new Texture2D(256, 192);

        tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

        Cls(true);
    }

    private void LoadCharSet()
    {
        //TextAsset ta = Resources.Load<TextAsset>(charSetResources);
        var ta = Resources.Load<TextAsset>(charSetResources);

        charSet = new byte[ta.bytes.Length];

        Array.Copy(ta.bytes, charSet, ta.bytes.Length);
    }
}
//}
