



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

        ButterFlyState butter = new ButterFlyState(this);

        LenovoModelRotationState lenovo = new LenovoModelRotationState(this);
        ModelControlState model = new ModelControlState(this);
        PlayerTakePictureState takePicture = new PlayerTakePictureState(this);

        fsm = new FSMSystem();
        fsm.AddState(butter);
        fsm.AddState(lenovo);
        fsm.AddState(model);
        fsm.AddState(takePicture);
    }
    // Update is called once per frame
    void Update()
    {
        fsm.CurrentState.Reason(player, gameObject);
        fsm.CurrentState.Act(player, gameObject);
    }
}
