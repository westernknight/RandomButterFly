using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LateRotationFollow : MonoBehaviour
{

    public List<Transform> reference = new List<Transform>();
    public List<Transform> objects = new List<Transform>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < reference.Count; i++)
        {
            objects[i].rotation = reference[i].rotation;
        }
    }
}
