using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ButterFlyController : MonoBehaviour {

    public ParticleController pc1;

    public void Play()
    {
        pc1.enabled = true;
    }
    public void Stop()
    {
        pc1.Clean();
        pc1.enabled = false;
        
    }
}
