using UnityEngine;
using System.Collections;

public class KinectPlayerAnalyst : MonoBehaviour
{
    public static KinectPlayerAnalyst instance;
    Vector3 playerOffset;

    public Vector3 PlayerOffset
    {
        get { return playerOffset; }
        set { playerOffset = value; }
    }
    int playerCount = 0;
    void Awake()
    {
        instance = this;
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
        return playerCount;
    }
    public void Debug_SetPlayerCount()
{
    playerCount = 100;
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
