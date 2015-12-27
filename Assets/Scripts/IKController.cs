using UnityEngine;
using System.Collections;
using System;
using Windows.Kinect;
using RootMotion.FinalIK;

public class IKController : MonoBehaviour
{

   

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


    private GameObject[] bones;



    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private Int64 initialPosUserID = 0;

    private int MoveRate = 1;
    private float XOffset, YOffset, ZOffset;
    private bool OffsetCalibrated = false;
    public float SmoothFactor = 10.0f;
    public bool VerticalMovement = false;
    //public bool MoveVertically = false;
    public bool MirroredMovement = false;
    FullBodyBipedIK ik;
    void Start()
    {
        ik = GetComponent<FullBodyBipedIK>();
        //store bones in a list for easier access
        bones = new GameObject[] {
			Hip_Center,
            Spine,
            Neck,
            Head,
            Shoulder_Left,
            Elbow_Left,
            Wrist_Left,
            Hand_Left,
            Shoulder_Right,
            Elbow_Right,
            Wrist_Right,
            Hand_Right,
            Hip_Left,
            Knee_Left,
            Ankle_Left,
            Foot_Left,
            Hip_Right,
            Knee_Right,
            Ankle_Right,
            Foot_Right,
            Spine_Shoulder,
            Hand_Tip_Left,
            Thumb_Left,
            Hand_Tip_Right,
            Thumb_Right
		};



        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }


    void Update()
    {
        KinectManager manager = KinectManager.Instance;

        // get 1st player
        Int64 userID = manager ? manager.GetPrimaryUserID() : 0;

        if (userID <= 0)
        {
            // reset the pointman position and rotation
            if (transform.position != initialPosition)
                transform.position = initialPosition;

            if (transform.rotation != initialRotation)
                transform.rotation = initialRotation;

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].gameObject.SetActive(true);

                bones[i].transform.localPosition = Vector3.zero;
                bones[i].transform.localRotation = Quaternion.identity;


            }

        }

        // set the position in space
        Vector3 posPointMan = manager.GetUserPosition(userID);
        posPointMan.z = !MirroredMovement ? -posPointMan.z : posPointMan.z;

#if false
 // store the initial position
        if (initialPosUserID != userID)
        {
            initialPosUserID = userID;
            initialPosOffset = transform.position - (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
        }

        transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
#endif

        // update the local positions of the bones
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] != null)
            {
                int joint = !MirroredMovement ? i : (int)KinectInterop.GetMirrorJoint((JointType)i);

                if (manager.IsJointTracked(userID, joint))
                {
                    bones[i].gameObject.SetActive(true);

                    Vector3 posJoint = manager.GetJointPosition(userID, joint);
                    posJoint.z = !MirroredMovement ? -posJoint.z : posJoint.z;

                    Quaternion rotJoint = manager.GetJointOrientation(userID, joint, !MirroredMovement);

                    posJoint -= posPointMan;

                    if (MirroredMovement)
                    {
                        posJoint.x = -posJoint.x;
                        posJoint.z = -posJoint.z;
                    }

                    bones[i].transform.localPosition = posJoint;
                    bones[i].transform.localRotation = rotJoint;


                }
                else
                {
                    bones[i].gameObject.SetActive(false);
                }
            }
        }
        MoveAvatar();
        UpdateEffector();

    }
    void MoveAvatar()
    {
        if (!bones[(int)JointType.SpineBase].activeSelf)
        {
            return;

        }



        // Get the position of the body and store it.
        Vector3 trans = bones[(int)JointType.SpineBase].transform.position;

        // If this is the first time we're moving the avatar, set the offset. Otherwise ignore it.
        if (!OffsetCalibrated)
        {
            OffsetCalibrated = true;

            XOffset = !MirroredMovement ? trans.x * MoveRate : -trans.x * MoveRate;
            YOffset = trans.y * MoveRate;
            ZOffset = -trans.z * MoveRate;
        }

        // Smoothly transition to the new position
        Vector3 targetPos = Kinect2AvatarPos(trans, VerticalMovement);


        transform.localPosition = SmoothFactor != 0f ?
            Vector3.Lerp(transform.localPosition, targetPos, SmoothFactor * Time.deltaTime) : targetPos;

    }

    Vector3 Kinect2AvatarPos(Vector3 jointPosition, bool bMoveVertically)
    {
        float xPos;

        if (!MirroredMovement)
            xPos = jointPosition.x * MoveRate - XOffset;
        else
            xPos = -jointPosition.x * MoveRate - XOffset;

        float yPos = jointPosition.y * MoveRate - YOffset;
        float zPos = -jointPosition.z * MoveRate - ZOffset;

        Vector3 newPosition = new Vector3(xPos, bMoveVertically ? yPos : 0f, zPos);

        return newPosition;
    }
    void UpdateEffector()
    {
        if (!bones[(int)JointType.SpineBase].activeSelf)
        {
            ik.solver.GetEffector(FullBodyBipedEffector.LeftShoulder).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftShoulder).positionWeight, 0, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.RightShoulder).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.RightShoulder).positionWeight,0, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.RightHand).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.RightHand).positionWeight, 0, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).positionWeight,0, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.LeftFoot).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftFoot).positionWeight, 0, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.RightFoot).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.RightFoot).positionWeight, 0, Time.deltaTime * SmoothFactor);

        }
        else
        {
            ik.solver.GetEffector(FullBodyBipedEffector.LeftShoulder).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftShoulder).positionWeight, 1, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.RightShoulder).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.RightShoulder).positionWeight, 1, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.RightHand).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.RightHand).positionWeight, 1, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).positionWeight, 1, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.LeftFoot).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftFoot).positionWeight, 1, Time.deltaTime * SmoothFactor);
            ik.solver.GetEffector(FullBodyBipedEffector.RightFoot).positionWeight = Mathf.Lerp(ik.solver.GetEffector(FullBodyBipedEffector.RightFoot).positionWeight, 1, Time.deltaTime * SmoothFactor);

            if (bones[(int)JointType.ShoulderLeft].activeSelf)
            {
                ik.solver.GetEffector(FullBodyBipedEffector.LeftShoulder).position = Vector3.Slerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftShoulder).position, bones[(int)JointType.ShoulderLeft].transform.position, Time.deltaTime * SmoothFactor);
            }
            if (bones[(int)JointType.HandLeft].activeSelf)
            {
                ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).position = Vector3.Slerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).position, bones[(int)JointType.HandLeft].transform.position, Time.deltaTime * SmoothFactor);
            }


            if (bones[(int)JointType.ShoulderRight].activeSelf)
            {
                ik.solver.GetEffector(FullBodyBipedEffector.RightShoulder).position = Vector3.Slerp(ik.solver.GetEffector(FullBodyBipedEffector.RightShoulder).position, bones[(int)JointType.ShoulderRight].transform.position, Time.deltaTime * SmoothFactor);

            }
            if (bones[(int)JointType.HandRight].activeSelf)
            {
                ik.solver.GetEffector(FullBodyBipedEffector.RightHand).position = Vector3.Slerp(ik.solver.GetEffector(FullBodyBipedEffector.RightHand).position, bones[(int)JointType.HandRight].transform.position, Time.deltaTime * SmoothFactor);

            }

            if (bones[(int)JointType.FootLeft].activeSelf)
            {
                ik.solver.GetEffector(FullBodyBipedEffector.LeftFoot).position = Vector3.Slerp(ik.solver.GetEffector(FullBodyBipedEffector.LeftFoot).position, bones[(int)JointType.FootLeft].transform.position, Time.deltaTime * SmoothFactor);

            }
            if (bones[(int)JointType.FootRight].activeSelf)
            {
                ik.solver.GetEffector(FullBodyBipedEffector.RightFoot).position = Vector3.Slerp(ik.solver.GetEffector(FullBodyBipedEffector.RightFoot).position, bones[(int)JointType.FootRight].transform.position, Time.deltaTime * SmoothFactor);
            }
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
