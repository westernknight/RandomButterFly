using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinectModelCreator : MonoBehaviour
{

    public List<GameObject> modelPrefabs = new List<GameObject>();

    //用于获取骨骼位置并应用到模型
    List<GameObject> modelRef = new List<GameObject>();
    //用于获取modelRef model的骨骼位置应用到ik
    List<GameObject> modelIK = new List<GameObject>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameObject CreateModel(Vector3 pos)
    {
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public bool DeleteModel(GameObject go)
    {
        return false;
    }
   
}
