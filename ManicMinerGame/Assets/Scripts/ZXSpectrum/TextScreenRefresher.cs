using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//namespace Assets.Scripts.ZXSpectrum
//{
[RequireComponent(typeof(CharScreen))]
public class TextScreenRefresher : MonoBehaviour
{
    CharScreen cs;

    private void Awake()
    {
        cs = GetComponent<CharScreen>();
    }
}
//}
