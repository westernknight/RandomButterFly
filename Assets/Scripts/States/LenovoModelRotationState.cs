using UnityEngine;
using System.Collections;

public class LenovoModelRotationState : FSMState
{
    public LenovoModelRotationState(MonoBehaviour mono)
    {
        stateID = StateID.LenovoModelRotation;
        this.mono = mono;
    }

    public override void DoBeforeEntering()
    {
        
    }
  
    public override void DoBeforeLeaving()
    {
    }

    public override void Reason(GameObject player, GameObject npc)
    {
    }

    public override void Act(GameObject player, GameObject npc)
    {
    }

}
