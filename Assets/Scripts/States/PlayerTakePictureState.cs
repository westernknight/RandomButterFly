using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class PlayerTakePictureState : FSMState
{
    private Texture2D usersClrTex;
    private Rect usersClrRect;
    private int usersClrSize;
    private KinectInterop.SensorData sensorData = null;
    private bool kinectInitialized = false;
    public PlayerTakePictureState(MonoBehaviour mono)
    {
        stateID = StateID.PlayerTakePicture;
        this.mono = mono;

        KinectInterop.FrameSource dwFlags = KinectInterop.FrameSource.TypeBody;
        dwFlags |= KinectInterop.FrameSource.TypeColor;
        sensorData = KinectInterop.OpenDefaultKinectSensor(dwFlags);
        if (sensorData == null)
        {
            throw new Exception("OpenDefaultKinectSensor failed");
        }
    }
    public void OnGUI()
    {
        GUI.DrawTexture(usersClrRect, usersClrTex);
    }
    public override void DoBeforeEntering()
    {

        // init color image structures
        //colorImage = new KinectInterop.ColorBuffer(true);

        // Initialize color map related stuff
        usersClrTex = new Texture2D(sensorData.colorImageWidth, sensorData.colorImageHeight, TextureFormat.RGBA32, false);

        Rect cameraRect = Camera.main.pixelRect;

        float MapsPercentWidth = (sensorData.depthImageWidth / 2) / cameraRect.width;
        float MapsPercentHeight = (sensorData.depthImageHeight / 2) / cameraRect.height;
        usersClrRect = new Rect(cameraRect.width - cameraRect.width * MapsPercentWidth, cameraRect.height, cameraRect.width * MapsPercentWidth, -cameraRect.height * MapsPercentHeight);
        usersClrSize = sensorData.colorImageWidth * sensorData.colorImageHeight;

        mono.StartCoroutine(SavePicture());
        
    }

    public override void DoBeforeLeaving()
    {
    }
    IEnumerator SavePicture()
    {
        Debug.Log("3");
        yield return new WaitForSeconds(1);
        Debug.Log("2");
        yield return new WaitForSeconds(1);
        Debug.Log("1");
        yield return new WaitForSeconds(1);
        Debug.Log("ka cha");
        var gp = ((GameProcess)mono);
        int count = gp.config.taskCount;

        Application.CaptureScreenshot(   Path.Combine(gp.config.savePicturePath,      count+UnityEngine.Random.Range(0,10)+".png"), 0);
    }

    public override void Reason(GameObject player, GameObject npc)
    {
    }

    public override void Act(GameObject player, GameObject npc)
    {
        if (KinectInterop.PollColorFrame(sensorData))
        {
            UpdateColorMap();
        }
    }
    void UpdateColorMap()
	{
		//usersClrTex.SetPixels32(colorImage.pixels);
		usersClrTex.LoadRawTextureData(sensorData.colorImage);
		usersClrTex.Apply();
	}
}
