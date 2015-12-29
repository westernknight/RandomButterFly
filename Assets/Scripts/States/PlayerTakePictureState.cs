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
        gameProcess.kinectBkImage.gameObject.SetActive(false);
        gameProcess.takePictureImage.gameObject.SetActive(true);
  
        gameProcess.timeText.gameObject.SetActive(true);
        gameProcess.timeText.ResetAndStart((float)gameProcess.config.capturePhotoTime);
        gameProcess.pictureNameText.gameObject.SetActive(true);
        
        picName = gameProcess.config.taskCount + "," + UnityEngine.Random.Range(0, 10) + ".png";
        gameProcess.pictureNameText.text = picName;
    }

    public override void DoBeforeLeaving()
    {
        gameProcess.timeText.gameObject.SetActive(false);
        gameProcess.takePictureImage.gameObject.SetActive(false);
        gameProcess.playerModel1.SetActive(false);
        gameProcess.pictureNameText.gameObject.SetActive(false);
        
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
            gameProcess.SetTransition(StateID.LenovoModelRotation);
        }
    }
  
    public override void Act(GameObject player, GameObject npc)
    {

        if (gameProcess.timeText.isTimeOut && isShot == false)
        {
            isShot = true;
            SavePicture();
        }
        gameProcess.RenderToImage(gameProcess.takePictureImage);
    }
   
}
