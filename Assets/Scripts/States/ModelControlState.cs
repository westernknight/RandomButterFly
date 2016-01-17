using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
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

        //根据model1的Y轴位置决定地板位置
        Vector3 pos = Utility.StringToVector3(gameProcess.config.model1Position);
        GameObject.Find("TransparentFloor").transform.position = new Vector3(0, pos.y, 0);
  
        //create models; to prepare



        if (KinectPlayerAnalyst.instance.GetUsersCount() == 1)
        {
            gameProcess.playerModels[0].SetActive(true);

        }

        else if (KinectPlayerAnalyst.instance.GetUsersCount() == 2)
        {
            gameProcess.playerModels[1].SetActive(true);

        }
        for (int i = 0; i < gameProcess.playerModels.Count; i++)
        {
            gameProcess.playerModels[i].transform.position = Utility.StringToVector3(gameProcess.config.model1Position);
                                     
            gameProcess.playerModels[i].GetComponent<RotationCopy>().InitPosition(gameProcess.playerModels[0].transform.position);
            gameProcess.playerModels[i].transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        KinectPlayerAnalyst.instance.isCanUpdateAvatar = true;
        // gameProcess.kinectBkImage.gameObject.SetActive(true);
        gameProcess.kinectBkImagePlane.SetActive(true);

        gameProcess.timeText.gameObject.SetActive(true);
        gameProcess.timeText.ResetAndStart((float)gameProcess.config.playModelTime);

        foreach (KeyValuePair<Int64, GameObject> kvp in gameProcess.modelMap)
        {
            kvp.Value.SetActive(true);
        }
        KinectPlayerAnalyst.instance.addPlayer += AddUser;
        KinectPlayerAnalyst.instance.removePlayer += RemoveUser;

    }
    void AddUser(Int64 userid)
    {
        for (int i = 0; i < gameProcess.playerModels.Count; i++)
        {
            gameProcess.playerModels[i].SetActive(false);
        }
        foreach (KeyValuePair<Int64, GameObject> kvp in gameProcess.modelMap)
        {
            kvp.Value.SetActive(true);
        }
    }
    void RemoveUser(Int64 userid)
    {
        for (int i = 0; i < gameProcess.playerModels.Count; i++)
        {
            gameProcess.playerModels[i].SetActive(false);
        }
        foreach (KeyValuePair<Int64, GameObject> kvp in gameProcess.modelMap)
        {
            kvp.Value.SetActive(true);
        }
    }
    public override void DoBeforeLeaving()
    {
        secondElapse = 0;
        KinectPlayerAnalyst.instance.isCanUpdateAvatar = false;

        //gameProcess.kinectBkImagePlane.SetActive(false);
        gameProcess.timeText.gameObject.SetActive(false);
        for (int i = 0; i < gameProcess.playerModels.Count; i++)
        {
            gameProcess.playerModels[i].SetActive(false);
        }
        KinectPlayerAnalyst.instance.addPlayer -= AddUser;
        KinectPlayerAnalyst.instance.removePlayer -= RemoveUser;

        
    }
    void AddStateAnimation()
    {


    }
    void RemoveStateAnimation()
    {

    }
    public override void Reason(GameObject player, GameObject npc)
    {
        if (gameProcess.timeText.isTimeOut)
        {
            
            gameProcess.SetTransition(StateID.PlayerTakePicture);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        secondElapse += Time.deltaTime;
        //gameProcess.msgText.text = KinectPlayerAnalyst.instance.GetPrimaryUserID().ToString();


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("基础人物ID " + KinectPlayerAnalyst.instance.GetPrimaryUserID());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("index 0 ID " + KinectPlayerAnalyst.instance.GetUserIdByIndex(0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("index 1 ID " + KinectPlayerAnalyst.instance.GetUserIdByIndex(1));
        }

        if (KinectPlayerAnalyst.instance.GetPrimaryUserID()!=0)
        {
            gameProcess.msgText.text = KinectPlayerAnalyst.instance.GetUserPosition(KinectPlayerAnalyst.instance.GetPrimaryUserID()).ToString();
        }

        gameProcess.RenderToImage();

    }
}
