using UnityEngine;
using System.Collections;
using System.IO;

using System.Collections.Generic;

public class LenovoModelRotationState : FSMState
{
    GameProcess gameProcess;
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

    public override void DoBeforeEntering()
    {
        //获取背景图路径
        
  
        //AddStateAnimation;
        gameProcess.lenovoBkImage.gameObject.SetActive(true);
        gameProcess.lenovoCumputer.gameObject.SetActive(true);

        {
            Color c = gameProcess.lenovoBkImage.color;
            c.a = 0;
            gameProcess.lenovoBkImage.color = c;
        }
       

        LeanTween.value(gameProcess.lenovoBkImage.gameObject, 0, 1, 0.5f).setOnUpdate(
            (float v) =>
            {
                Color c = gameProcess.lenovoBkImage.color;
                c.a = v;
                gameProcess.lenovoBkImage.color = c;
            });
    }
  
    public override void DoBeforeLeaving()
    {
      
        lenovoModelShowTime = 0;
        //RemoveStateAnimation
        
        gameProcess.lenovoCumputer.gameObject.SetActive(false);


        LeanTween.value(gameProcess.lenovoBkImage.gameObject, 1, 0, 0.5f).setOnUpdate(
            (float v) => 
            {
                Color c = gameProcess.lenovoBkImage.color;
                c.a = v;
                gameProcess.lenovoBkImage.color = c;
            }).setOnComplete(() =>
            {
                Color c = gameProcess.lenovoBkImage.color;
                c.a = 1;
                //结束后自动随机下一张
                mono.StartCoroutine(gameProcess.LoadATexture(gameProcess.config.lenovoBKImagePath, gameProcess.lenovoBkImage));
                gameProcess.lenovoBkImage.gameObject.SetActive(false);
            });

       
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
