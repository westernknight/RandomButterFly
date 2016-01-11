using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TransformCopy : MonoBehaviour
{

    public Transform reference;

   

    void ReverseCopy(Transform p1, Transform p2)
    {
        p2.localRotation = p1.localRotation;
        p2.localPosition = p1.localPosition;
        for (int i = 0; i < p1.childCount; i++)
        {
            ReverseCopy(p1.GetChild(i), p2.GetChild(i));
        }
    }
    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        ReverseCopy(reference, transform);
       
    }
}
