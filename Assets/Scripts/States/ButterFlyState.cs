using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Windows.Kinect;

public class ButterFlyState : FSMState
{
    private bool initBk = false;
    GameProcess gameProcess;

    public bool catchOneButterfly = false;
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
    public IEnumerator LoadATexture(string path)
    {
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

            int index = Random.Range(0, imageFileList.Count);
            WWW www = new WWW("file:///" + imageFileList[index]);
            Debug.Log("LoadATexture picture:" + imageFileList[index]);
            while (www.isDone == false)
            {
                yield return null;
            }
            if (www.error == null)
            {
                var lenovoStateBk = www.texture;

                gameProcess.butterFlyBkImage.overrideSprite = Sprite.Create(lenovoStateBk, new Rect(0, 0, lenovoStateBk.width, lenovoStateBk.height), new Vector2(0.5f, 0.5f));
                Debug.Log("LoadATexture load sucess");
            }
        }

    }
    private void Reader_FrameArrived( BodyFrameArrivedEventArgs e)
    {
    }
    public override void DoBeforeEntering()
    {
        if (gameProcess)
        {
            mono.StartCoroutine(LoadATexture(gameProcess.config.butterFlyBKImagePath));
        }
        AddStateAnimation();
    }
  
    public override void DoBeforeLeaving()
    {
        catchOneButterfly = false;
        initBk = false;
        RemoveStateAnimation();
    }
    void AddStateAnimation()
    {
        gameProcess.butterFlyBkImage.gameObject.SetActive(true);
    }
    void RemoveStateAnimation()
    {
        gameProcess.butterFlyBkImage.gameObject.SetActive(false);

    }
    public override void Reason(GameObject player, GameObject npc)
    {
        if (catchOneButterfly)
        {
            gameProcess.SetTransition(StateID.ModelControl);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
    }
}
