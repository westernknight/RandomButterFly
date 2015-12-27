using UnityEngine;
using System.Collections;
using System;
using RootMotion.FinalIK;
using Windows.Kinect;

public class SpecialController : MonoBehaviour
{



    public GameObject sholderLeft;
    public GameObject sholderRight;

    public GameObject Hip_Center;
    public GameObject Spine;
    public GameObject Neck;
    public GameObject Head;
    public GameObject Shoulder_Left;
    public GameObject Elbow_Left;
    public GameObject Wrist_Left;
    public GameObject Hand_Left;
    public GameObject Shoulder_Right;
    public GameObject Elbow_Right;
    public GameObject Wrist_Right;
    public GameObject Hand_Right;
    public GameObject Hip_Left;
    public GameObject Knee_Left;
    public GameObject Ankle_Left;
    public GameObject Foot_Left;
    public GameObject Hip_Right;
    public GameObject Knee_Right;
    public GameObject Ankle_Right;
    public GameObject Foot_Right;
    public GameObject Spine_Shoulder;
    public GameObject Hand_Tip_Left;
    public GameObject Thumb_Left;
    public GameObject Hand_Tip_Right;
    public GameObject Thumb_Right;


    void Start()
    {

    }


    void Update()
    {

    }
    void LateUpdate()
    {
        if (!Spine.activeSelf)
        {
            return;

        }
        if (sholderLeft)
        {
            sholderLeft.transform.rotation = Shoulder_Left.transform.rotation;
        }

    }

    public void DetectByName(Transform parent)
    {
        Hip_Center = parent.FindChild("00_Hip_Center").gameObject;
        Spine = parent.FindChild("01_Spine").gameObject;
        Neck = parent.FindChild("02_Neck").gameObject;
        Head = parent.FindChild("03_Head").gameObject;
        Shoulder_Left = parent.FindChild("04_Shoulder_Left").gameObject;
        Elbow_Left = parent.FindChild("05_Elbow_Left").gameObject;
        Wrist_Left = parent.FindChild("06_Wrist_Left").gameObject;
        Hand_Left = parent.FindChild("07_Hand_Left").gameObject;
        Shoulder_Right = parent.FindChild("08_Shoulder_Right").gameObject;
        Elbow_Right = parent.FindChild("09_Elbow_Right").gameObject;
        Wrist_Right = parent.FindChild("10_Wrist_Right").gameObject;
        Hand_Right = parent.FindChild("11_Hand_Right").gameObject;
        Hip_Left = parent.FindChild("12_Hip_Left").gameObject;
        Knee_Left = parent.FindChild("13_Knee_Left").gameObject;
        Ankle_Left = parent.FindChild("14_Ankle_Left").gameObject;
        Foot_Left = parent.FindChild("15_Foot_Left").gameObject;
        Hip_Right = parent.FindChild("16_Hip_Right").gameObject;
        Knee_Right = parent.FindChild("17_Knee_Right").gameObject;
        Ankle_Right = parent.FindChild("18_Ankle_Right").gameObject;
        Foot_Right = parent.FindChild("19_Foot_Right").gameObject;
        Spine_Shoulder = parent.FindChild("20_SpineShoulder").gameObject;
        Hand_Tip_Left = parent.FindChild("21_Hand_Tip_Left").gameObject;
        Thumb_Left = parent.FindChild("22_Thumb_Left").gameObject;
        Hand_Tip_Right = parent.FindChild("23_Hand_Tip_Right").gameObject;
        Thumb_Right = parent.FindChild("24_Thumb_Right").gameObject;
    }
}
