using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ButterFlyController : MonoBehaviour {

    public ParticleController pc1;

    public void Play()
    {
        //pc1.enabled = true;
        pc1.playbackTime = 5;
        pc1.PlayEmit();
    }
    public void Stop()
    {
        pc1.StopEmit();
        
    }
}
