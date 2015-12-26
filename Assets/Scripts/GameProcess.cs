



using UnityEngine;
using System.Collections;

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

    FSMSystem fsm;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        MakeFSM();
    }
    public void SetTransition(StateID t) { fsm.PerformTransition(t); }
    private void MakeFSM()
    {

        


        LenovoModelRotationState lenovo = new LenovoModelRotationState(this);
        lenovo.AddTransition(StateID.ButterFly);
        lenovo.AddTransition(StateID.ModelControl);

        ButterFlyState butter = new ButterFlyState(this);
        butter.AddTransition(StateID.LenovoModelRotation);

        ModelControlState model = new ModelControlState(this);
        model.AddTransition(StateID.PlayerTakePicture);


        PlayerTakePictureState takePicture = new PlayerTakePictureState(this);
        takePicture.AddTransition(StateID.ButterFly);
 



        fsm = new FSMSystem();
        fsm.AddState(model);

        fsm.AddState(butter);
        fsm.AddState(lenovo);
        
        fsm.AddState(takePicture);


    }
    // Update is called once per frame
    void Update()
    {
        fsm.CurrentState.Reason(player, gameObject);
        fsm.CurrentState.Act(player, gameObject);
    }
}
