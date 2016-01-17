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

        gameProcess.kinectBkImagePlane.SetActive(true);
  
    }

    public override void DoBeforeLeaving()
    {
        //gameProcess.kinectBkImagePlane.SetActive(false);
  
        
    }
 

    public override void Reason(GameObject player, GameObject npc)
    {
       
    }
  
    public override void Act(GameObject player, GameObject npc)
    {

   
        gameProcess.RenderToImage();
    }
   
}
