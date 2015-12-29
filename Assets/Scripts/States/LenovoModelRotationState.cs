using UnityEngine;
using System.Collections;
using System.IO;

using System.Collections.Generic;

public class LenovoModelRotationState : FSMState
{
    GameProcess gameProcess;
    private bool initBk = false;
    float lenovoModelShowTime = 0;
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
    public IEnumerator LoadATexture(string path)
    {
        Debug.Log("LoadATexture path:"+path);
        List<string> imageFileList = new List<string>();
        if (Directory.Exists(path) )
        {
            string [] filepath = Directory.GetFiles(path, "*.png");
            imageFileList.AddRange(filepath);
            filepath = Directory.GetFiles(path, "*.jpg");
            imageFileList.AddRange(filepath);

            Debug.Log(" length " + imageFileList.Count);
            for (int i = 0; i < imageFileList.Count; i++)
            {
                Debug.Log(imageFileList[i]);
            }
        }
        if (imageFileList.Count>0)
        {
           
            int index = Random.Range(0,imageFileList.Count);
            WWW www = new WWW("file:///" +imageFileList[ index]);
            Debug.Log("LoadATexture picture:" + imageFileList[index]);
            while (www.isDone == false)
            {
                yield return null;
            }
            if (www.error == null)
            {
                var lenovoStateBk = www.texture;

                gameProcess.lenovoBkImage.overrideSprite = Sprite.Create(lenovoStateBk, new Rect(0, 0, lenovoStateBk.width, lenovoStateBk.height), new Vector2(0.5f, 0.5f));
                Debug.Log("LoadATexture load sucess");
            }
        }
        
    }
    public override void DoBeforeEntering()
    {
        //获取背景图路径
        
        if (gameProcess)
        {
            mono.StartCoroutine(LoadATexture(gameProcess.config.lenovoBKImagePath));
        }
        AddStateAnimation();
    }
  
    public override void DoBeforeLeaving()
    {
        RemoveStateAnimation();
        initBk = false;
        lenovoModelShowTime = 0;
    }
    void AddStateAnimation()
    {
        gameProcess.lenovoBkImage.gameObject.SetActive(true);
        gameProcess.lenovoCumputer.gameObject.SetActive(true);
    }
    void RemoveStateAnimation()
    {
        gameProcess.lenovoBkImage.gameObject.SetActive(false);
       

    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (KinectPlayerAnalyst.instance.GetUsersCount() > 0 && lenovoModelShowTime > gameProcess.config.playModelTime)
        {
            gameProcess.SetTransition(StateID.ButterFly);
        }
       
    }

    public override void Act(GameObject player, GameObject npc)
    {
        lenovoModelShowTime += Time.deltaTime;



       
    }

}
