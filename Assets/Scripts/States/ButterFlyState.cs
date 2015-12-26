using UnityEngine;
using System.Collections;

public class ButterFlyState : FSMState
{

    public ButterFlyState(MonoBehaviour mono)
    {
        stateID = StateID.ButterFly;
        this.mono = mono;
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("DoBeforeEntering");
        mono.StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("delay");
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
