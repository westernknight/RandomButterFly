using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class PlayerTakePictureState : FSMState
{
 
    GameProcess gameProcess;
    string picName = "";
    bool isShot = false;
    public PlayerTakePictureState(MonoBehaviour mono)
    {
        stateID = StateID.PlayerTakePicture;
        this.mono = mono;
        gameProcess = (GameProcess)mono;
       
    }
 
    public override void DoBeforeEntering()
    {
        gameProcess.kinectBkImagePlane.SetActive(true);

  
        gameProcess.timeText.gameObject.SetActive(true);
        gameProcess.timeText.ResetAndStart((float)gameProcess.config.capturePhotoTime);
        gameProcess.pictureNameText.gameObject.SetActive(true);
        
        picName = gameProcess.config.taskCount + "," + UnityEngine.Random.Range(0, 10) + ".png";
        gameProcess.pictureNameText.text = picName;
    }

    public override void DoBeforeLeaving()
    {
        gameProcess.timeText.gameObject.SetActive(false);
        for (int i = 0; i < gameProcess.playerModels.Count; i++)
        {
            gameProcess.playerModels[i].SetActive(false);
        }
        gameProcess.pictureNameText.gameObject.SetActive(false);
        gameProcess.kinectBkImagePlane.SetActive(false);
        isShot = false;
    }
    void SavePicture()
    {
        
        Debug.Log("ka cha");
        var gp = ((GameProcess)mono);
        gameProcess.timeText.gameObject.SetActive(false);
        gameProcess.pictureNameText.gameObject.SetActive(false);
        Application.CaptureScreenshot(   Path.Combine(gp.config.savePicturePath,      picName), 0);


        gameProcess.config.taskCount++;
        gameProcess.SaveConfig();
       
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (isShot)
        {
           // gameProcess.SetTransition(StateID.LenovoModelRotation);
        }
    }
  
    public override void Act(GameObject player, GameObject npc)
    {

        if (gameProcess.timeText.isTimeOut && isShot == false)
        {
            isShot = true;
            SavePicture();
        }
        if (isShot==false)
        {
            gameProcess.RenderToImage(gameProcess.takePictureImage);
        }
    }
   
}
