



using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class GameProcess : MonoBehaviour
{

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

    public LenovoModelRotationState lenovo;
    public ButterFlyState butter;
    public ModelControlState model;
    public PlayerTakePictureState takePicture;


    public GameObject player;

    public GlobalStructure config = new GlobalStructure();

    public Image lenovoBkImage;
    Texture butterflyStateBk;
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

        lenovo = new LenovoModelRotationState(this);
        lenovo.AddTransition(StateID.ButterFly);
        lenovo.AddTransition(StateID.ModelControl);

        butter = new ButterFlyState(this);
        butter.AddTransition(StateID.LenovoModelRotation);

        model = new ModelControlState(this);
        model.AddTransition(StateID.PlayerTakePicture);


        takePicture = new PlayerTakePictureState(this);
        takePicture.AddTransition(StateID.ButterFly);




        fsm = new FSMSystem();
        fsm.AddState(takePicture);
        fsm.AddState(lenovo);
        fsm.AddState(model);

        fsm.AddState(butter);


       


    }

    
    // Update is called once per frame
    void Update()
    {
        if (fsm != null)
        {
            fsm.CurrentState.Reason(player, gameObject);
            fsm.CurrentState.Act(player, gameObject);
        }

    }

    void OnGUI()
    {
        if (fsm.CurrentStateID == StateID.PlayerTakePicture)
        {
            takePicture.OnGUI();
        }
       
    }

}
