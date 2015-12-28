using UnityEngine;
using System.Collections;

public class ModelControlState : FSMState
{
    GameProcess gameProcess;
    public ModelControlState(MonoBehaviour mono)
    {
        stateID = StateID.ModelControl;
        this.mono = mono;
        gameProcess = mono as GameProcess;
    }
    public float secondElapse = 0;
    public override void DoBeforeEntering()
    {
        Debug.Log("ModelControlState DoBeforeEntering");
        //create models; to prepare
        
        AddStateAnimation();
    }
  
    public override void DoBeforeLeaving()
    {
        secondElapse = 0;
        RemoveStateAnimation();
    }
    void AddStateAnimation()
    {
        gameProcess.playerModel1.SetActive(true);
        gameProcess.playerModel1.transform.position = Utility.StringToVector3(gameProcess.config.model1Position);
        gameProcess.playerModel1.GetComponentInChildren<AvatarController>().Start();
        KinectManager.Instance.canUpdateAvatar = true;
        gameProcess.kinectBkImage.gameObject.SetActive(true);
    }
    void RemoveStateAnimation()
    {
        KinectManager.Instance.canUpdateAvatar = false;
        gameProcess.kinectBkImage.gameObject.SetActive(false);

    }
    public override void Reason(GameObject player, GameObject npc)
    {
        if (secondElapse>30)
        {
            gameProcess.SetTransition(StateID.PlayerTakePicture);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        secondElapse += Time.deltaTime;
    }
}
