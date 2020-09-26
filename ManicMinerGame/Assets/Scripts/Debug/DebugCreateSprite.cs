//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DebugCreateSprite : MonoBehaviour
{
    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //SpriteTexture spriteTexture = new SpriteTexture(8, 8);
        SpriteTexture spriteTexture = new SpriteTexture(8, 8, new Vector2(0, 0));
        spriteTexture.Clear( new Color(0,0,0,0) );

//        for(int i=0; i<8; i++)
//        {
//            //spriteTexture.SetLine(i, Random.Range(128, 256));
//            spriteTexture.SetLine(i, (byte) Random.Range(128, 256));
//        }

        // smiley
        spriteTexture.SetLine(0, 0x00);
        spriteTexture.SetLine(1, 0x66);
        spriteTexture.SetLine(2, 0x42);

        spriteTexture.SetLine(5, 0x42);
        spriteTexture.SetLine(6, 0x3c);
        spriteTexture.SetLine(7, 0x00);
        // /smiley

        spriteRenderer.sprite = spriteTexture.Apply();

        /*
        Texture2D smiley = new Texture2D(8, 8, TextureFormat.RGBA32, false);
        smiley.filterMode = FilterMode.Point;

        Color[] clrs = smiley.GetPixels();
        clrs[0] = Color.red;
        clrs[7] = Color.blue;
        clrs[8] = Color.green;

        smiley.SetPixels(clrs);
        smiley.Apply();

        spriteRenderer.sprite = Sprite.Create(smiley, new Rect(0, 0, 8, 8), Vector2.zero, 1f);
        */
    }
}
