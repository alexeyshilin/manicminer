using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//namespace Assets.Scripts.CollisionDetection
//{
public class BitCollision : MonoBehaviour
{
    public static bool CheckCollision2x2(int ax, int ay, byte[] aShape, int bx, int by, byte[] bShape)
    {
        return Math.Abs(ax-bx)*2<4 && Math.Abs(ay - by) * 2 < 4;
    }
}
//}
