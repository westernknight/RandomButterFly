using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
public class TakePictureAnimController : MonoBehaviour
{

    public static TakePictureAnimController instance;
    List<GameObject> choiseModes = new List<GameObject>();
    public List<GameObject> animModels = new List<GameObject>();
    int playerCount = 2;
    GameProcess gameProcess;

    public Action poseFinishEvent;
    public List<Vector3> targetPositions = new List<Vector3>();
    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        gameProcess = GameProcess.instance;
        for (int i = 0; i < animModels.Count; i++)
        {
            animModels[i].SetActive(false);
        }

        targetPositions.Add(Utility.StringToVector3(gameProcess.config.model1Position));
        targetPositions.Add(Utility.StringToVector3(gameProcess.config.model2Position));
        targetPositions.Add(Utility.StringToVector3(gameProcess.config.model3Position));
    }
    public void Play()
    {
        playerCount = gameProcess.takePicturePlayerCount;

 
        choiseModes.Clear();

        Random.seed = System.Environment.TickCount;
        int c = Random.Range(0, 2);
        choiseModes.Add(animModels[c]);
        choiseModes.Add(animModels[1 - c]);

        for (int i = 0; i < playerCount; i++)
        {
            Vector3 vec = targetPositions[0];
            vec.x = 20;
            choiseModes[i].SetActive(true);
            choiseModes[i].transform.position = vec;
            Vector3 targetPos = targetPositions[i];
            GameObject model = choiseModes[i];

            Animation anim = choiseModes[i].GetComponent<Animation>();
            anim.Play("run");
            anim.wrapMode = WrapMode.Loop;
            LeanTween.value(gameObject, vec, targetPos, 3).setDelay(i * 0.3f).setOnUpdate((Vector3 v) =>
            {
                model.transform.position = v;
            }).setOnComplete(() =>
            {       
                int index = Random.Range(0, 3);
                if (index == 0)
                {
                    anim.Play("pose1");
                }
                else if (index == 1)
                {
                    anim.Play("pose2");
                }
                else if (index == 2)
                {
                    anim.Play("pose3");
                }
                anim.wrapMode = WrapMode.Once;
            });



        }

    }

    public void Hide()
    {
        for (int i = 0; i < animModels.Count; i++)
        {
            animModels[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
