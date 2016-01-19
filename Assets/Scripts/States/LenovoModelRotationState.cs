using UnityEngine;
using System.Collections;
using System.IO;

using System.Collections.Generic;
using System;

public class LenovoModelRotationState : FSMState
{
    GameProcess gameProcess;
    float lenovoModelShowTime = 0;
    bool playAnim = true;
    public LenovoModelRotationState(MonoBehaviour mono)
    {
        stateID = StateID.LenovoModelRotation;
        this.mono = mono;
        gameProcess = mono as GameProcess;
    }


    /// <summary>
    /// 参数只是路径，随机读取路径里面的一张图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>

    public override void DoBeforeEntering()
    {
        //获取背景图路径

        gameProcess.kinectBkImagePlane.SetActive(true);
        //AddStateAnimation;
        //gameProcess.lenovoBkImage.gameObject.SetActive(true);
        gameProcess.lenovoCumputer.gameObject.SetActive(true);
   
        //gameProcess.lenovoCumputer.transform.position = new Vector3(36.31f, -0.72f, 0);
        //gameProcess.lenovoCumputer.transform.rotation = Quaternion.Euler(21.34876f, 204.3775f, 0);


       // LeanTween.value(mono.gameObject, 36, 6.31f, 3).setOnUpdate((float v) => { gameProcess.lenovoCumputer.transform.position = new Vector3(v, -0.72f, 0); });
    
        playAnim = true;
        //mono.StartCoroutine(PlayComputerAnim());

      
        ComputerRotateComplete();

        //background
        string path = gameProcess.config.lenovoBKImagePath;
        Debug.Log("LoadATexture path:" + path);
        List<string> imageFileList = new List<string>();
        if (Directory.Exists(path))
        {
            string[] filepath = Directory.GetFiles(path, "*.png");
            imageFileList.AddRange(filepath);
            filepath = Directory.GetFiles(path, "*.jpg");
            imageFileList.AddRange(filepath);

            Debug.Log(" length " + imageFileList.Count);
            for (int i = 0; i < imageFileList.Count; i++)
            {
                Debug.Log(imageFileList[i]);
            }
        }
        if (imageFileList.Count > 0)
        {

            int index = UnityEngine.Random.Range(0, imageFileList.Count);
            WWW www = new WWW("file:///" + imageFileList[index]);
            Debug.Log("LoadATexture picture:" + imageFileList[index]);
            while (www.isDone == false)
            {
               
            }
            if (www.error == null)
            {
                gameProcess.SetImage(www.texture);
            }
        }
    }


 void ComputerRotateComplete()
    {
        LeanTween.value(mono.gameObject, 0, 360, 3).setOnUpdate((float v) => { gameProcess.lenovoCumputer.transform.rotation = Quaternion.Euler(0, 90 - v, 0); }).setOnComplete(() => {

            if (playAnim == true)
            {
                ComputerRotateComplete();
            }
            else
            {
                GameProcess.instance.SetTransition(StateID.ButterFly);
            }
            
        });
    }
    public override void DoBeforeLeaving()
    {
      
        lenovoModelShowTime = 0;
        //RemoveStateAnimation
        
        gameProcess.lenovoCumputer.gameObject.SetActive(false);


       
    }

    IEnumerator PlayComputerAnim()
    {
        Animation anim = GameObject.Find("BJB").GetComponent<Animation>();
        anim["Take 001"].time = 1;
        while (playAnim)
        {
            if (anim.isPlaying == false && playAnim)
            {
                anim.Play("Take 001");
            }
            yield return null;
        }
        while (anim.isPlaying)
        {
            yield return null;
        }
       
        GameProcess.instance.SetTransition(StateID.ButterFly);
    }
 


    public override void Reason(GameObject player, GameObject npc)
    {
        if (KinectPlayerAnalyst.instance.GetUsersCount() > 0 && lenovoModelShowTime > gameProcess.config.playModelTime)
        {
            playAnim = false;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            playAnim = false;
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        lenovoModelShowTime += Time.deltaTime;


        //gameProcess.RenderToImage();
       
    }

}
