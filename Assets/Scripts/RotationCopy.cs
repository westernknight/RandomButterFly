using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotationCopy : MonoBehaviour {

    public List<Transform> reference = new List<Transform>();
    public List<Transform> model = new List<Transform>();

    List<GameObject> referenceObj = new List<GameObject>();

    void Start()
    {

        for (int i = 0; i < reference.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.rotation = model[i].transform.rotation;
            go.transform.position = new Vector3(1000, 1000, 1000);
            go.transform.SetParent(reference[i]);
            referenceObj.Add(go);
        }
    }
    void Update()
    {
        if (reference.Count == model.Count)
        {
            for (int i = 0; i < reference.Count; i++)
            {
                model[i].rotation = referenceObj[i].transform.rotation;
            }
        }
        
    }
}
