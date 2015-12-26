using UnityEngine;
using System.Collections;

public class ModelControlState : FSMState
{

    public ModelControlState(MonoBehaviour mono)
    {
        stateID = StateID.ModelControl;
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
