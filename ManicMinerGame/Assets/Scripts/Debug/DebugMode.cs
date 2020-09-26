using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    // typewriter - unlocked room selector

    public KeyCode[] phrase;

    public bool debugEnabled;

    int index = 0;
    bool deciding = false;

    float lastTime;

    /*
    // Update is called once per frame
    void Update()
    {
        if( !deciding && Input.GetKeyUp(phrase[0]) )
        {
            index++;
            print(phrase[0]);
            StartCoroutine(DoHack());
        }
    }

    IEnumerator DoHack()
    {
        float time = 0.5f;
        while(time>0)
        {
            if (index >= phrase.Length)
            {
                break;
            }

            if ( Input.GetKeyUp(phrase[index]) )
            {
                print( phrase[index] );

                index++;

                if(index == phrase.Length)
                {
                    yield break;
                }
                else
                {
                    yield return DoHack();
                }
            }

            time -= Time.deltaTime;
            yield return null;
        }

        if (index == phrase.Length)
        {
            debugEnabled = !debugEnabled;
            index = 0;
        }

        //return null;
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(phrase[index]))
        {
            print( phrase[index] );

            lastTime = Time.realtimeSinceStartup;
            index++;

            if( index==phrase.Length )
            {
                debugEnabled = !debugEnabled;
                index = 0;
            }
        }

        if( Time.realtimeSinceStartup > lastTime+0.5f )
        {
            index = 0;
        }
    }
}
