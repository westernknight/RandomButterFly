using UnityEngine;
using System.Collections;
using System;




#if true
public class GlobalStructure
{
    public string bkImagePath="e:/";
   
    public string realityBKPath = ".";
    public string savePicturePath = "e:/";

    public int maxPlayerCount=2;

    public double playModelTime = 30;
    public double capturePhotoTime = 5;

    public string model1Position="0,0,0";
    public string model2Position = "0,0,0";
    public string model3Position = "0,0,0";

    public double model1Scale = 1;
    public double model2Scale = 1;
    public double model3Scale = 1;

    public bool isLenovoStateBkAndButterflyStateBkIsTheSame = false;

    public int taskCount = 0;
}
#else
public struct GlobalStructure
{
    public string bkImagePath = "e:/";
    public string realityBKPath = ".";
    public int maxPlayerCount = 2;
    public float playModelTime = 30;
    public float capturePhotoTime = 5;


    public string model1Position = "0,0,0";
    public string model2Position = "0,0,0";
    public string model3Position = "0,0,0";

    public float model1Scale = 1;
    public float model2Scale = 1;
    public float model3Scale = 1;
}
#endif
public class Utility
{
    public static string Vector3ToString(Vector3 vec)
    {
        return vec.x.ToString("f2") + " " + vec.y.ToString("f2") + " " + vec.z.ToString("f2");
    }
    public static Vector3 StringToVector3(string sz)
    {
        string[] tmp = sz.Split(',');
        Vector3 vec = new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), float.Parse(tmp[2]));
        return vec;
    }

    
}