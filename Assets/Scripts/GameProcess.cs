



using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
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
    Texture2D texForKinectBk;
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
    public GameObject playerModel1;

    /// <summary>
    /// scene4
    /// </summary>
    public Image takePictureImage;
    

    Texture butterflyStateBk;

    bool isShottingThreadRunning = false;

    public TimeCounter timeText;
    public Text pictureNameText;
 

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {

     

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



        InitParam();
        
        StartCoroutine(LoadATexture(config.lenovoBKImagePath, lenovoBkImage));
        StartCoroutine(LoadATexture(config.butterFlyBKImagePath, butterFlyBkImage));
        StartCoroutine(WaitForKinectReady());

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
    public void RenderToImage(Image image)
    {
        if (isShottingThreadRunning == false)
        {
            StartCoroutine(Shot(image));       
        }
     
    }
    IEnumerator Shot(Image image)
    {
        isShottingThreadRunning = true;
        var sensorData = KinectPlayerAnalyst.instance.sensorData;
        while (KinectInterop.PollColorFrame(sensorData) == false)
        {
            Debug.Log("yield return null");
            yield return null;
        }
        if (usersClrTex == null)
        {
            usersClrTex = new Texture2D(sensorData.colorImageWidth, sensorData.colorImageHeight, TextureFormat.RGBA32, false);
        }


        usersClrTex.LoadRawTextureData(sensorData.colorImage);
        usersClrTex.Apply();

        image.overrideSprite = Sprite.Create(usersClrTex, new Rect(0, 0, usersClrTex.width, usersClrTex.height), new Vector2(0.5f, 0.5f));

        isShottingThreadRunning = false;

    }
    public void ShotToKinectBk()
    {

        StartCoroutine(ShotToKinectBkEnumerator());       
       
    }
    public IEnumerator ShotToKinectBkEnumerator()
    {
        var sensorData = KinectPlayerAnalyst.instance.sensorData;
        while (KinectInterop.PollColorFrame(sensorData) == false)
        {
            yield return null;
        }
        if (texForKinectBk == null)
        {
            texForKinectBk = new Texture2D(sensorData.colorImageWidth, sensorData.colorImageHeight, TextureFormat.RGBA32, false);
        }


        texForKinectBk.LoadRawTextureData(sensorData.colorImage);
        texForKinectBk.Apply();

        kinectBkImage.overrideSprite = Sprite.Create(texForKinectBk, new Rect(0, 0, texForKinectBk.width, texForKinectBk.height), new Vector2(0.5f, 0.5f));
    }
    IEnumerator WaitForKinectReady()
    {
        while (KinectPlayerAnalyst.instance.isKinectInitialized == false)
        {
            yield return null;
        }
        Debug.Log("ShotToImage");
        ShotToKinectBk();
        MakeFSM();
    }
    public void SetTransition(StateID t) { fsm.PerformTransition(t); }
    private void InitParam()
    {
        //to do
        //lenovoBkImage = GameObject.Find("lenovoBkImage").GetComponent<Image>();
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
        fsm.AddState(takePictureState);

        fsm.AddState(modelControlState);

        fsm.AddState(butterFlyState);
        fsm.AddState(adjustmentState);




    }

    public IEnumerator LoadATexture(string path,Image image)
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
        if (fsm != null)
        {
            fsm.CurrentState.Reason(player, gameObject);
            fsm.CurrentState.Act(player, gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameProcess.instance.SetTransition(StateID.ButterFly);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameProcess.instance.SetTransition(StateID.ModelControl);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameProcess.instance.SetTransition(StateID.PlayerTakePicture);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameProcess.instance.SetTransition(StateID.LenovoModelRotation);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameProcess.instance.SetTransition(StateID.Adjustment);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShotToKinectBk();
        }
    }

 

}
