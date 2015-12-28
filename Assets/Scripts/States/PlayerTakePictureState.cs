using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class PlayerTakePictureState : FSMState
{
    private Texture2D usersClrTex;
    private Rect usersClrRect;
    private int usersClrSize;
    GameProcess gameProcess;
    public PlayerTakePictureState(MonoBehaviour mono)
    {
        stateID = StateID.PlayerTakePicture;
        this.mono = mono;
        gameProcess = (GameProcess)mono;
       
    }
 
    public override void DoBeforeEntering()
    {

        gameProcess.kinectBkImage.gameObject.SetActive(true);
        mono.StartCoroutine(SavePicture());
        
    }

    public override void DoBeforeLeaving()
    {
        gameProcess.playerModel1.SetActive(false);
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

        Application.CaptureScreenshot(   Path.Combine(gp.config.savePicturePath,      count+","+UnityEngine.Random.Range(0,10)+".png"), 0);
    }

    public override void Reason(GameObject player, GameObject npc)
    {
    }

    public override void Act(GameObject player, GameObject npc)
    {
        var sensorData = gameProcess.sensorData;
        if (KinectInterop.PollColorFrame(sensorData))
        {
            usersClrTex.LoadRawTextureData(sensorData.colorImage);
            usersClrTex.Apply();
            gameProcess.takePictureImage.overrideSprite = Sprite.Create(usersClrTex, new Rect(0, 0, usersClrTex.width, usersClrTex.height), new Vector2(0.5f, 0.5f));
        }
    }
   
}
