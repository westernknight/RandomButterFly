using UnityEngine;
using System.Collections;

public class PlayerTakePictureState : FSMState
{

    public PlayerTakePictureState(MonoBehaviour mono)
    {
        stateID = StateID.PlayerTakePicture;
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
