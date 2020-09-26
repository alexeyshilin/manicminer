using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CellPointExtension
{
    public static Vector3 ToVector3(this CellPoint pt)
    {
        return new Vector3(pt.x, pt.y);
    }
}