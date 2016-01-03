using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Windows.Kinect;
using System;

public class ButterFlyState : FSMState
{
    private bool initBk = false;
    GameProcess gameProcess;

    public bool isCatchOneButterfly = false;
    public bool isTouching = false;
    public float touchingTime = 0;
    private CoordinateMapper coordinateMapper = null;
    public ButterFlyState(MonoBehaviour mono)
    {
        stateID = StateID.ButterFly;
        this.mono = mono;
        gameProcess = mono as GameProcess;
    }


    /// <summary>
    /// 参数只是路径，随机读取路径里面的一张图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    
    private void Reader_FrameArrived( BodyFrameArrivedEventArgs e)
    {
    }
    public override void DoBeforeEntering()
    {

        gameProcess.butterFlyBkImage.gameObject.SetActive(true);
        gameProcess.butterFly.gameObject.SetActive(true);


        {
            Color c = gameProcess.butterFlyBkImage.color;
            c.a = 0;
            gameProcess.butterFlyBkImage.color = c;
        }
        LeanTween.value(gameProcess.butterFlyBkImage.gameObject, 0, 1, 0.5f).setOnUpdate(
            (float v) =>
            {
                Color c = gameProcess.butterFlyBkImage.color;
                c.a = v;
                gameProcess.butterFlyBkImage.color = c;
            });
    }
  
    public override void DoBeforeLeaving()
    {
        isCatchOneButterfly = false;
        initBk = false;
        gameProcess.lenovoCumputer.gameObject.SetActive(false);
        gameProcess.cursor.gameObject.SetActive(false);
        isTouching = false;
        touchingTime = 0;

        gameProcess.butterFly.gameObject.SetActive(false);




        LeanTween.value(gameProcess.butterFlyBkImage.gameObject, 1, 0, 0.5f).setOnUpdate(
           (float v) =>
           {
               Color c = gameProcess.butterFlyBkImage.color;
               c.a = v;
               gameProcess.butterFlyBkImage.color = c;
           }).setOnComplete(() =>
           {
               Color c = gameProcess.butterFlyBkImage.color;
               c.a = 1;
               //结束后自动随机下一张
               mono.StartCoroutine(gameProcess.LoadATexture(gameProcess.config.butterFlyBKImagePath, gameProcess.butterFlyBkImage));
               gameProcess.butterFlyBkImage.gameObject.SetActive(false);
           });

    }
 
    public override void Reason(GameObject player, GameObject npc)
    {
        if (isCatchOneButterfly)
        {
            gameProcess.SetTransition(StateID.ModelControl);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        KinectPlayerAnalyst kinect = KinectPlayerAnalyst.instance;
        Int64 userID = kinect.GetPrimaryUserID();
    
        if (kinect.GetRightHandPosition(userID)!=Vector3.zero)
        {
            Vector3 handPos = kinect.GetRightHandPosition(userID);
            float cursorX = handPos.x / 0.5f * Screen.width/2;
            float cursorY = (handPos.y-1.3f) / 0.3f * Screen.height/2;
            gameProcess.cursor.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(cursorX, cursorY, -20);
            gameProcess.cursor.gameObject.SetActive(true);
            
        }
        else
        {
            gameProcess.cursor.gameObject.SetActive(false);
        }
        CheckTouch();


    }
    void CheckTouch()
    {

        Ray ray = new Ray(gameProcess.cursorCube.transform.position, gameProcess.cursorCube.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo) && gameProcess.cursor.gameObject.activeSelf)
        {
            isTouching = true;
        }
        else
        {
            isTouching = false;
            touchingTime = 0;
        }
        if (isTouching)
        {
            touchingTime += Time.deltaTime;
            if (touchingTime > gameProcess.config.touchButterflyTime)
            {
                isCatchOneButterfly = true;
            }
        }
    }
}
