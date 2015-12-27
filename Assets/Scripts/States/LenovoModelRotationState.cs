using UnityEngine;
using System.Collections;
using System.IO;

using System.Collections.Generic;

public class LenovoModelRotationState : FSMState
{
    public Texture2D lenovoStateBk;
    GameProcess gp;
    private bool initBk = false;
    public LenovoModelRotationState(MonoBehaviour mono)
    {
        stateID = StateID.LenovoModelRotation;
        this.mono = mono;
        gp = mono as GameProcess;
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
                lenovoStateBk = www.texture;
                //to do
//                 Sprite sprite = Sprite.Create(lenovoStateBk, new Rect(0, 0, 20, 20), new Vector3(0, 0));
//                 GameObject tmp = new GameObject("MySprite");
//                 tmp.AddComponent<SpriteRenderer>().sprite = sprite;
//                 
//                 gp.lenovoBkImage.overrideSprite = tmp.GetComponent<SpriteRenderer>().sprite;
//                 tmp.SetActive(false);
                Debug.Log("LoadATexture load sucess");
            }
        }
        
    }
    public override void DoBeforeEntering()
    {
        //获取背景图路径
        
        if (gp)
        {
            mono.StartCoroutine(LoadATexture(gp.config.bkImagePath));
        }
    }
  
    public override void DoBeforeLeaving()
    {
        lenovoStateBk = null;
        initBk = false;
    }

    public override void Reason(GameObject player, GameObject npc)
    {

    }

    public override void Act(GameObject player, GameObject npc)
    {
        if (lenovoStateBk!=null)
        {
            
        }
    }

}
