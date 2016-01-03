using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class AdjustmentState : FSMState
{
 
    GameProcess gameProcess;
    string picName = "";
    bool isShot = false;
    public AdjustmentState(MonoBehaviour mono)
    {
        stateID = StateID.Adjustment;
        this.mono = mono;
        gameProcess = (GameProcess)mono;
       
    }
 
    public override void DoBeforeEntering()
    {

        gameProcess.takePictureImage.gameObject.SetActive(true);
  
    }

    public override void DoBeforeLeaving()
    {
        gameProcess.takePictureImage.gameObject.SetActive(false);
  
        
    }
 

    public override void Reason(GameObject player, GameObject npc)
    {
       
    }
  
    public override void Act(GameObject player, GameObject npc)
    {

   
        gameProcess.RenderToImage(gameProcess.takePictureImage);
    }
   
}
