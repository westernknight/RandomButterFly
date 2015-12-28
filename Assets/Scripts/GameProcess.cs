



using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

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


    GameObject player;

    public GlobalStructure config = new GlobalStructure();
    public KinectInterop.SensorData sensorData = null;
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
        StartCoroutine(InitKinect());
        
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
    IEnumerator InitKinect()
    {
        while (sensorData == null)
        {
            sensorData = KinectManager.Instance.sensorData;
            yield return null;
        }
        
        StartCoroutine(Shot());
        MakeFSM();
    }

    IEnumerator Shot()
    {
        while (KinectInterop.PollColorFrame(sensorData) == false)
        {
            Debug.Log("yield return null");
            yield return null;
        }
        Texture2D usersClrTex = new Texture2D(sensorData.colorImageWidth, sensorData.colorImageHeight, TextureFormat.RGBA32, false);

        usersClrTex.LoadRawTextureData(sensorData.colorImage);
        usersClrTex.Apply();
        kinectBkImage.overrideSprite = Sprite.Create(usersClrTex, new Rect(0, 0, usersClrTex.width, usersClrTex.height), new Vector2(0.5f, 0.5f));

    }

    public void SetTransition(StateID t) { fsm.PerformTransition(t); }
    private void InitParam()
    {
        //to do
        //lenovoBkImage = GameObject.Find("lenovoBkImage").GetComponent<Image>();
    }
    private void MakeFSM()
    {

        lenovoModelRotationState = new LenovoModelRotationState(this);
        lenovoModelRotationState.AddTransition(StateID.ButterFly);
        lenovoModelRotationState.AddTransition(StateID.ModelControl);

        butterFlyState = new ButterFlyState(this);
        butterFlyState.AddTransition(StateID.LenovoModelRotation);
        butterFlyState.AddTransition(StateID.ModelControl);


        modelControlState = new ModelControlState(this);
        modelControlState.AddTransition(StateID.PlayerTakePicture);


        takePictureState = new PlayerTakePictureState(this);
        takePictureState.AddTransition(StateID.ButterFly);




        fsm = new FSMSystem();
        fsm.AddState(lenovoModelRotationState);
        fsm.AddState(takePictureState);

        fsm.AddState(modelControlState);

        fsm.AddState(butterFlyState);





    }


    // Update is called once per frame
    void Update()
    {
        if (fsm != null)
        {
            fsm.CurrentState.Reason(player, gameObject);
            fsm.CurrentState.Act(player, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shot());
        }
    }

 

}
