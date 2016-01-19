using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Windows.Kinect;
using System;

public class ButterFlyState : FSMState
{

    GameProcess gameProcess;

    public bool isCatchOneButterfly = false;
    public bool isTouching = false;
    public float touchingTime = 0;
    private CoordinateMapper coordinateMapper = null;
    bool canTouch = false;
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

    private void Reader_FrameArrived(BodyFrameArrivedEventArgs e)
    {
    }
    public override void DoBeforeEntering()
    {
        canTouch = false;
        gameProcess.lenovoCumputer.gameObject.SetActive(true);

        touchOneButterFlyTime = 0;


        //gameProcess.lenovoCumputer.transform.position = new Vector3(6.31f, -0.72f, 0);
        //gameProcess.lenovoCumputer.transform.rotation = Quaternion.Euler(21.34876f, 204.3775f, 0);

//         LeanTween.value(gameProcess.lenovoCumputer, 6.31f, -5.8f, 1).setOnUpdate(
//            (float v) =>
//            {
//                gameProcess.lenovoCumputer.transform.position = new Vector3(v, -0.72f, 0);
//            }).setOnComplete(() =>
//            {
// 
//                var pos = (GameObject.Find("ParticleControllerPos") as GameObject).transform.position;
//                var rotation = (GameObject.Find("ParticleControllerPos") as GameObject).transform.rotation;
//                gameProcess.GetComponent<ButterFlyController>().pc1.gameObject.transform.position = pos;
//                gameProcess.GetComponent<ButterFlyController>().pc1.gameObject.transform.rotation = rotation;
//                gameProcess.GetComponent<ButterFlyController>().Play();
// 
//            });


//         LeanTween.value(gameProcess.lenovoCumputer, 204.3775f, 105.4469f, 1).setOnUpdate(
//            (float v) =>
//            {
//                gameProcess.lenovoCumputer.transform.rotation = Quaternion.Euler(21.34876f, v, 0);
//            }).setOnComplete(() =>
//            {
// 
//            });

        var pos = (GameObject.Find("ParticleControllerPos") as GameObject).transform.position;
        var rotation = (GameObject.Find("ParticleControllerPos") as GameObject).transform.rotation;
        gameProcess.GetComponent<ButterFlyController>().pc1.gameObject.transform.position = pos;
        gameProcess.GetComponent<ButterFlyController>().pc1.gameObject.transform.rotation = rotation;
        gameProcess.GetComponent<ButterFlyController>().Play();


        butterFlyState = FlyState.emitting;


        mono.StartCoroutine(WaitForCanTouch());
    }
    IEnumerator WaitForCanTouch()
    {
        yield return new WaitForSeconds(5);
        canTouch = true;
    }
    public override void DoBeforeLeaving()
    {
        isCatchOneButterfly = false;
        gameProcess.lenovoCumputer.gameObject.SetActive(false);

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

        gameProcess.RenderToImage();

        if (butterFlyState == FlyState.emitting)
        {
            CheckTouch();

        }
        else if (butterFlyState == FlyState.touch)
        {
            butterFlyState = FlyState.catched;
            gameProcess.GetComponent<ButterFlyController>().Stop();
            gameProcess.msgText.text = "ok";
//             LeanTween.value(gameProcess.lenovoCumputer, -5.8f, -15.8f, 1).setOnUpdate(
//           (float v) =>
//           {
//               gameProcess.lenovoCumputer.transform.position = new Vector3(v, -0.72f, 0);
//           }).setOnComplete(() =>
//           {
//               isCatchOneButterfly = true;
// 
//           });
            isCatchOneButterfly = true;
      
        }
        else if (butterFlyState == FlyState.catched)
        {
            //电脑左移
        }
    }


    float touchOneButterFlyTime = 0;
    enum FlyState
    {
        emitting,
        touch,
        catched
    }

    FlyState butterFlyState = FlyState.emitting;
    void CheckTouch()
    {
        ParticleController particle = ParticleController.instance;
        if (KinectPlayerAnalyst.instance.GetPrimaryUserID() != 0 && ParticleController.instance)
        {
            gameProcess.msgText.text = "detected";
            Vector2 pos = KinectPlayerAnalyst.instance.GetRightHandPositionV2(KinectPlayerAnalyst.instance.GetPrimaryUserID());

            Rect rectRightHand = new Rect(0, 0, Screen.width / 15f, Screen.width / 15f);
            rectRightHand.center = new Vector2(pos.x, pos.y);

            Rect rectLeftHand = new Rect(0, 0, Screen.width / 15f, Screen.width / 15f);
            rectLeftHand.center = new Vector2(pos.x, pos.y);

            bool detected = false;
            for (int i = 0; i < particle.activeParticles.Count; i++)
            {
                float touchedTime = 0.4f;
                if (particle.activeParticles[i].GetComponent<ParticleButterfly>().life>3)
                {
                    continue;
                }
                if (rectRightHand.Contains(Camera.main.WorldToScreenPoint(particle.activeParticles[i].transform.position)))
                {
                    touchOneButterFlyTime += Time.deltaTime;
                    gameProcess.msgText.text = touchOneButterFlyTime.ToString();
                    detected = true;
                    if (touchOneButterFlyTime > touchedTime)
                    {
                        Vector3 particlePos = particle.activeParticles[i].transform.position;
                        GameObject go = GameObject.Instantiate(gameProcess.butterFlyCatchedEffect) as GameObject;
                        go.transform.position = particlePos;
                        particle.activeParticles[i].SetActive(false);

                        gameProcess.touchedColor = particle.activeParticles[i].GetComponent<ParticleButterfly>().color;
                        butterFlyState = FlyState.touch;

                    }
                    break;
                }
                if (rectLeftHand.Contains(Camera.main.WorldToScreenPoint(particle.activeParticles[i].transform.position)))
                {
                    touchOneButterFlyTime += Time.deltaTime;
                    gameProcess.msgText.text = touchOneButterFlyTime.ToString();
                    detected = true;
                    if (touchOneButterFlyTime > touchedTime)
                    {
                        Vector3 particlePos = particle.activeParticles[i].transform.position;
                        GameObject go = GameObject.Instantiate(gameProcess.butterFlyCatchedEffect) as GameObject;
                        go.transform.position = particlePos;
                        particle.activeParticles[i].SetActive(false);

                        gameProcess.touchedColor = particle.activeParticles[i].GetComponent<ParticleButterfly>().color;
                        butterFlyState = FlyState.touch;

                    }
                    break;
                }
            }
            if (detected == false)
            {
                touchOneButterFlyTime = 0;
            }
        }
        else
        {
            gameProcess.msgText.text = "no body";
        }
    }





}
