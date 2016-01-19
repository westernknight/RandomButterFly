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
        gameProcess.GetComponent<ButterFlyController>().Stop();
        gameProcess.lenovoCumputer.gameObject.SetActive(false);
        Debug.Log("ModelControlState DoBeforeEntering");

        //根据model1的Y轴位置决定地板位置

        gameProcess.choiseModels.Clear();
        //create models; to prepare

        int randomNumber = gameProcess.touchedColor;
        //randomNumber = 0;
        for (int i = 0; i < gameProcess.playerModelCount; i++)
        {
            gameProcess.choiseModels.Add(gameProcess.playerModelsBinded[randomNumber * 2 + i]);
        }


        if (KinectPlayerAnalyst.instance.GetUsersCount() == 1)
        {
            gameProcess.choiseModels[0].SetActive(true);

        }

        else if (KinectPlayerAnalyst.instance.GetUsersCount() == 2)
        {
            gameProcess.choiseModels[1].SetActive(true);

        }


        for (int i = 0; i < gameProcess.choiseModels.Count; i++)
        {
            gameProcess.choiseModels[i].GetComponent<Animation>().enabled = false;
            gameProcess.choiseModels[i].GetComponent<RotationCopy>().enabled = true;
        }

        for (int i = 0; i < gameProcess.choiseModels.Count; i++)
        {
            gameProcess.choiseModels[i].transform.position = Utility.StringToVector3(gameProcess.config.model1Position);

            gameProcess.choiseModels[i].GetComponent<RotationCopy>().InitPosition(gameProcess.playerModelsBinded[0].transform.position);
            gameProcess.choiseModels[i].transform.rotation = Quaternion.Euler(0, 180, 0);
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

        
        mono.StartCoroutine(ModelControlTime());

    }
    void AddUser(Int64 userid)
    {
        for (int i = 0; i < gameProcess.choiseModels.Count; i++)
        {
            gameProcess.choiseModels[i].SetActive(false);
        }
        foreach (KeyValuePair<Int64, GameObject> kvp in gameProcess.modelMap)
        {
            kvp.Value.SetActive(true);
        }
    }
    void RemoveUser(Int64 userid)
    {
        for (int i = 0; i < gameProcess.choiseModels.Count; i++)
        {
            gameProcess.choiseModels[i].SetActive(false);
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
        for (int i = 0; i < gameProcess.choiseModels.Count; i++)
        {
            gameProcess.choiseModels[i].SetActive(false);
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

    IEnumerator ModelControlTime()
    {
        yield return new WaitForSeconds((float)gameProcess.config.modelControlTime);
        //to do change butterfly

        int playerCount = 0;
        foreach (KeyValuePair<Int64, GameObject> kvp in gameProcess.modelMap)
        {
            GameObject go = GameObject.Instantiate(gameProcess.butterflyPrefab) as GameObject;
            go.transform.position = kvp.Value.transform.position;
            kvp.Value.SetActive(false);
            LeanTween.value(mono.gameObject, 0, 1, 5).setOnUpdate((float v) =>
            {
                go.transform.position = go.transform.position + go.transform.forward * Time.deltaTime * 5;
                go.transform.rotation = gameProcess.GetComponent<ButterFlyController>().pc1.transform.rotation;


            });
            playerCount++;
        }
        if (playerCount<1)
        {
            playerCount = 1;
        }
        gameProcess.takePicturePlayerCount = playerCount;
        yield return new WaitForSeconds(3);
        gameProcess.SetTransition(StateID.PlayerTakePicture);
    }
    public override void Reason(GameObject player, GameObject npc)
    {
        
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
        gameProcess.SetImage(gameProcess.texShotted);
        //gameProcess.RenderToImage();

    }
}
