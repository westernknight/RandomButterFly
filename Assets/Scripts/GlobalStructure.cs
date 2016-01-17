using UnityEngine;
using System.Collections;
using System;




public class GlobalStructure
{
    public string lenovoBKImagePath = "e:/kinectBk";
    public string butterFlyBKImagePath = "e:/butterflyBk";
   
    public string realityBKPath = ".";
    public string savePicturePath = "e:/kinectImage";

    public int maxPlayerCount=2;

    public int touchButterflyTime = 3;

    public double playModelTime = 8;
    public double modelControlTime = 10;
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
public class GlobalVar
{
    public static int SAVE_PICTURE_RANDOM_NUMBER_MAX = 10;
    public static Vector3 POSITION_OUT_OF_WORLD = new Vector3(1000, 1000, 1000);
}
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


