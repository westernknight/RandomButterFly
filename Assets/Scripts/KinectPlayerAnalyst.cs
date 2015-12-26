using UnityEngine;
using System.Collections;

public class KinectPlayerAnalyst : MonoBehaviour
{

    Vector3 playerOffset;

    public Vector3 PlayerOffset
    {
        get { return playerOffset; }
        set { playerOffset = value; }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetPlayerCount()
    {
        return 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public Vector3 GetPlayerPosition(int userID)
    {
        return Vector3.zero;
    }
    public bool IsHandDetected()
    {
        return false;
    }
    public Vector3 GetHandPosition(int userID)
    {
        return Vector3.zero;
    }
}
