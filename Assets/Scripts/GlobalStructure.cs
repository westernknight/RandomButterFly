using UnityEngine;
using System.Collections;
using System;




public class GlobalStructure
{
    public string model1Position;


    public static string Vector3ToString(Vector3 vec)
    {
        return vec.x.ToString("f2") + " " + vec.y.ToString("f2") + " " + vec.z.ToString("f2");
    }
    public static Vector3 StringToVector3(string sz)
    {
        string[] tmp = sz.Split(' ');
        Vector3 vec = new Vector3(float.Parse(tmp[0]),float.Parse(tmp[1]),float.Parse(tmp[2]));
        return vec;
    }
}
