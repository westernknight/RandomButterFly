using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
public class LenovoGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{

    public void UserDetected(long userId, int userIndex)
    {
        KinectPlayerAnalyst manager = KinectPlayerAnalyst.instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
    }
    public void UserLost(long userId, int userIndex)
    {
    }
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
        float progress, JointType joint, Vector3 screenPos)
    {
    }

   
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
        JointType joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        //		if(gesture == KinectGestures.Gestures.Click)
        //			sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);

        
        var getModel = GameProcess.instance.GetModelByUserID(userId);
        if (getModel)
        {
            getModel.GetComponent<RotationCopy>().Jump();
            Debug.LogError(sGestureText);
        }
        return true;
    }



    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
         JointType joint)
    {
        return true;
    }
}
