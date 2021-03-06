﻿



using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class GameProcess : MonoBehaviour
{
    public static GameProcess instance;
    /// <summary>
    /// 电脑模型旋转  必须有一定时间
    /// 旋转切换
    /// 蝴蝶飞过     人不在，切换到电脑模型旋转
    /// 飞过切换
    /// 动作捕捉
    /// 捕捉切换
    /// 模型摆姿势
    /// 
    /// </summary>

    public FSMSystem fsm;

    public LenovoModelRotationState lenovoModelRotationState;
    public ButterFlyState butterFlyState;
    public ModelControlState modelControlState;
    public PlayerTakePictureState takePictureState;
    public AdjustmentState adjustmentState;

    Texture2D usersClrTex;
    public Texture2D texShotted;
    GameObject player;

    public GlobalStructure config = new GlobalStructure();
    /// <summary>
    /// scene1
    /// </summary>
    public Image lenovoBkImage;
    public GameObject lenovoCumputer;


    /// <summary>
    /// scene2
    /// </summary>
    public Image butterFlyBkImage;
    public GameObject butterFlyModelPrefab;
    public Image cursor;
    public GameObject cursorCube;
    public Image butterFly;

    /// <summary>
    /// scene3
    /// </summary>
    public Image kinectBkImage;
    public GameObject kinectBkImagePlane;
    public List<GameObject> playerModelsBinded = new List<GameObject>();

    public int playerModelCount = 2;

    [HideInInspector]
    public List<GameObject> choiseModels = new List<GameObject>();
    /// <summary>
    /// scene4
    /// </summary>
    public Image takePictureImage;

    public Text msgText;

    public GameObject butterflyPrefab;

    Texture butterflyStateBk;

    bool isShottingThreadRunning = false;
    public Vector3 floorPosition;

    public TimeCounter timeText;
    public Text pictureNameText;

    /// <summary>
    /// 人物ID对应模型
    /// </summary>
    public Dictionary<Int64, GameObject> modelMap = new Dictionary<Int64, GameObject>();

    /// <summary>
    /// 由粒子系统创建
    /// </summary>
    public GameObject butterFlyCatchedEffect;
    [HideInInspector]
    public int touchedColor =0;
    [HideInInspector]
    public int takePicturePlayerCount = 1;
    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {

       

        for (int i = 0; i < playerModelsBinded.Count; i++)
        {
            playerModelsBinded[i].SetActive(false);
        }
        KinectPlayerAnalyst.instance.addPlayer += (Int64 userid) =>
            {
                for (int i = 0; i < choiseModels.Count; i++)
                {
                    if (choiseModels[i].activeSelf == false)
                    {

                        modelMap.Add(userid, choiseModels[i]);                        
                        break;
                    }
                }
            };
        KinectPlayerAnalyst.instance.removePlayer += (Int64 userid) =>
        {
            
            modelMap.Remove(userid);
            
        };
        FileInfo fi = new FileInfo(Path.Combine(Application.streamingAssetsPath, "config.json"));
        if (fi.Exists)
        {
            StreamReader sr = new StreamReader(fi.OpenRead());
            try
            {
                config = LitJson.JsonMapper.ToObject<GlobalStructure>(sr.ReadLine());
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }
            if (sr != null)
            {
                sr.Close();
            }
        }
        else
        {
            StreamWriter sw = new StreamWriter(fi.Create());
            if (sw != null)
            {
                sw.Write(LitJson.JsonMapper.ToJson(config));
                sw.Close();
            }

        }
        Debug.Log(LitJson.JsonMapper.ToJson(config));

        floorPosition = Utility.StringToVector3(config.model1Position);

        InitParam();
        //设置地面
        Vector3 pos = Utility.StringToVector3(config.model1Position);
        GameObject.Find("TransparentFloor").transform.position = new Vector3(0, pos.y, 0);
       
        StartCoroutine(WaitForKinectReady());

    }
    public GameObject GetModelByUserID(long userid)
    {
        if (modelMap.ContainsKey(userid))
        {
            return modelMap[userid];
        }
        return null;
    }
    public void SaveConfig()
    {
        FileInfo fi = new FileInfo(Path.Combine(Application.streamingAssetsPath, "config.json"));
        StreamWriter sw = new StreamWriter(fi.Create());
        if (sw != null)
        {
            sw.Write(LitJson.JsonMapper.ToJson(config));
            sw.Close();
        }
    }
    public void SetImage(Texture t)
    {
        kinectBkImagePlane.renderer.material.mainTexture = t;
    }
    byte[] correctColorImageData;
    public void RenderToImage()
    {
        if (correctColorImageData == null)
        {
            correctColorImageData = new byte[1920 * 1080 * 4];
        }
        if (isShottingThreadRunning == false)
        {
            var sensorData = KinectPlayerAnalyst.instance.sensorData;
            if (KinectInterop.PollColorFrame(sensorData))
            {
                if (usersClrTex == null)
                {
                    usersClrTex = new Texture2D(sensorData.colorImageWidth, sensorData.colorImageHeight, TextureFormat.RGBA32, false);
                }

                for (int y = 0; y < 1080; y++)
                {
                    for (int x = 0; x < 1920; x++)
                    {
                        correctColorImageData[x * 4 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 1920 * 4 * (1080 - (y + 1))];
                        correctColorImageData[x * 4 + 1 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 1 + 1920 * 4 * (1080 - (y + 1))];
                        correctColorImageData[x * 4 + 2 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 2 + 1920 * 4 * (1080 - (y + 1))];
                        correctColorImageData[x * 4 + 3 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 3 + 1920 * 4 * (1080 - (y + 1))];
                    }
                }

                usersClrTex.LoadRawTextureData(correctColorImageData);
                usersClrTex.Apply();

                //image.overrideSprite = Sprite.Create(usersClrTex, new Rect(0, 0, usersClrTex.width, usersClrTex.height), new Vector2(0.5f, 0.5f));
                kinectBkImagePlane.renderer.material.mainTexture = usersClrTex;
            }

        }


    }

    public void CapturePicture()
    {
        StartCoroutine(ShotToKinectBkEnumerator());

    }

    byte[] correctColorImageDataCapture;
    public IEnumerator ShotToKinectBkEnumerator()
    {

        if (correctColorImageDataCapture == null)
        {
            correctColorImageDataCapture = new byte[1920 * 1080 * 4];
        }
        var sensorData = KinectPlayerAnalyst.instance.sensorData;
        isShottingThreadRunning = true;
        while (KinectInterop.PollColorFrame(sensorData) == false)
        {
            yield return null;
        }
        isShottingThreadRunning = false;
        if (texShotted == null)
        {
            texShotted = new Texture2D(sensorData.colorImageWidth, sensorData.colorImageHeight, TextureFormat.RGBA32, false);
        }
        for (int y = 0; y < 1080; y++)
        {
            for (int x = 0; x < 1920; x++)
            {
                correctColorImageDataCapture[x * 4 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 1920 * 4 * (1080 - (y + 1))];
                correctColorImageDataCapture[x * 4 + 1 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 1 + 1920 * 4 * (1080 - (y + 1))];
                correctColorImageDataCapture[x * 4 + 2 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 2 + 1920 * 4 * (1080 - (y + 1))];
                correctColorImageDataCapture[x * 4 + 3 + 1920 * 4 * y] = sensorData.colorImage[x * 4 + 3 + 1920 * 4 * (1080 - (y + 1))];
            }
        }

        texShotted.LoadRawTextureData(correctColorImageDataCapture);
        texShotted.Apply();
        //kinectBkImage.overrideSprite = Sprite.Create(texShotted, new Rect(0, 0, texShotted.width, texShotted.height), new Vector2(0.5f, 0.5f));
    }
    public void SetKinectBkShottedPicture()
    {
        Debug.Log("SetKinectBkShottedPicture");
        kinectBkImagePlane.renderer.material.mainTexture = texShotted;
    }
    IEnumerator WaitForKinectReady()
    {
        while (KinectPlayerAnalyst.instance.isKinectInitialized == false)
        {
            yield return null;
        }
        Debug.Log("ShotToImage");
        CapturePicture();


        MakeFSM();
    }
    public void SetTransition(StateID t) { fsm.PerformTransition(t); }

    private Windows.Kinect2.KinectSensor m_pKinectSensor = null;
    private void InitParam()
    {
        //to do
        //lenovoBkImage = GameObject.Find("lenovoBkImage").GetComponent<Image>();

        m_pKinectSensor = Windows.Kinect2.KinectSensor.GetDefault();
        if (m_pKinectSensor != null)
        {
            
        }
    }
  
    private void MakeFSM()
    {
        adjustmentState = new AdjustmentState(this);
        adjustmentState.AddTransition(StateID.LenovoModelRotation);
        adjustmentState.AddTransition(StateID.ButterFly);
        adjustmentState.AddTransition(StateID.ModelControl);
        adjustmentState.AddTransition(StateID.PlayerTakePicture);

        lenovoModelRotationState = new LenovoModelRotationState(this);
        lenovoModelRotationState.AddTransition(StateID.ButterFly);
        lenovoModelRotationState.AddTransition(StateID.ModelControl);
        lenovoModelRotationState.AddTransition(StateID.Adjustment);

        butterFlyState = new ButterFlyState(this);
        butterFlyState.AddTransition(StateID.LenovoModelRotation);
        butterFlyState.AddTransition(StateID.ModelControl);
        butterFlyState.AddTransition(StateID.Adjustment);

        modelControlState = new ModelControlState(this);
        modelControlState.AddTransition(StateID.PlayerTakePicture);
        modelControlState.AddTransition(StateID.Adjustment);

        takePictureState = new PlayerTakePictureState(this);
        takePictureState.AddTransition(StateID.LenovoModelRotation);
        takePictureState.AddTransition(StateID.Adjustment);



        fsm = new FSMSystem();
        fsm.AddState(lenovoModelRotationState);
        fsm.AddState(modelControlState);
       
     
        fsm.AddState(butterFlyState);
        
        fsm.AddState(takePictureState);

        fsm.AddState(adjustmentState);




    }

   
    public IEnumerator LoadARandomTexture(string path, Image image)
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

            int index = UnityEngine.Random.Range(0, imageFileList.Count);
            WWW www = new WWW("file:///" + imageFileList[index]);
            Debug.Log("LoadATexture picture:" + imageFileList[index]);
            while (www.isDone == false)
            {
                yield return null;
            }
            if (www.error == null)
            {
                var lenovoStateBk = www.texture;

                image.overrideSprite = Sprite.Create(lenovoStateBk, new Rect(0, 0, lenovoStateBk.width, lenovoStateBk.height), new Vector2(0.5f, 0.5f));
                Debug.Log("LoadATexture load sucess");
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    
        
 
       

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CapturePicture();
        }
        if (fsm != null)
        {
            fsm.CurrentState.Reason(player, gameObject);
            fsm.CurrentState.Act(player, gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameProcess.instance.SetTransition(StateID.LenovoModelRotation);

        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameProcess.instance.SetTransition(StateID.ModelControl);

        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameProcess.instance.SetTransition(StateID.PlayerTakePicture);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameProcess.instance.SetTransition(StateID.Adjustment);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Break();
        }
        #if false
 if (KinectPlayerAnalyst.instance.GetPrimaryUserID() != 0 && ParticleController.instance)
        {
            msgText.text = "detect";
            Vector2 pos = KinectPlayerAnalyst.instance.GetRightHandPositionV2(KinectPlayerAnalyst.instance.GetPrimaryUserID());

            Rect r = new Rect(0, 0, Screen.width / 10f, Screen.width / 10f);
            r.center = new Vector2(pos.x, pos.y);

            for (int i = 0; i < ParticleController.instance.activeParticles.Count; i++)
            {
                if (r.Contains(Camera.main.WorldToScreenPoint(ParticleController.instance.activeParticles[i].transform.position)))
                {
                    msgText.text = "find";
                    break;
                }
                else
                {
                    msgText.text = "miss\n" + r + "\n" + Camera.main.WorldToScreenPoint(ParticleController.instance.activeParticles[i].transform.position).ToString(); ;
                }
            }
        }
        else
        {
            msgText.text = "no person detect";
        }
 #endif
    }



}
#if false
    if ( KinectPlayerAnalyst.instance.GetPrimaryUserID()!=0)
        {
            Vector2 pos = KinectPlayerAnalyst.instance.GetRightHandPositionV2(KinectPlayerAnalyst.instance.GetPrimaryUserID());
    
            Rect r = new Rect(0, 0, Screen.width / 10f, Screen.width / 10f);
            r.center = new Vector2(pos.x, pos.y);

            if (r.Contains(Camera.main.WorldToScreenPoint(GameObject.Find("Cube").transform.position)))
            {
                msgText.text = "find";
            }
            else
            {
                msgText.text = "miss";
            }
           
        }
#endif