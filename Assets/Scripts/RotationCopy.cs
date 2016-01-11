using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotationCopy : MonoBehaviour
{

    public List<Transform> reference = new List<Transform>();
    public List<Transform> model = new List<Transform>();

    List<GameObject> referenceObj = new List<GameObject>();

    public Transform referenceModel;
    public AvatarController avatarController;
    Vector3 initialPosition;

    public Transform leftPoint;
    public Transform rightPoint;


    bool isJumping = false;

    float jumpOffsetY = 0;
    Transform ReverseFindChild(Transform t, string name)
    {
        if (t.name == name)
        {
            return t;
        }
        for (int i = 0; i < t.childCount; i++)
        {

            Transform tc = ReverseFindChild(t.GetChild(i), name);
            if (tc != null)
            {
                return tc;
            }

        }
        return null;
    }
    void AddReference(string name)
    {
        Transform t = ReverseFindChild(referenceModel, name);
        if (t != null)
        {
            reference.Add(t);
            model.Add(null);
        }
    }
    public void FillReference_Editor()
    {
        reference.Clear();
        model.Clear();
        AddReference("joint_HipLT");
        AddReference("joint_KneeLT");
        AddReference("joint_FootLT");
        AddReference("joint_HipRT");
        AddReference("joint_KneeRT");
        AddReference("joint_FootRT");
        AddReference("joint_ShoulderLT");
        AddReference("joint_ElbowLT");
        AddReference("joint_HandLT");
        AddReference("joint_ShoulderRT");
        AddReference("joint_ElbowRT");
        AddReference("joint_HandRT");
        AddReference("joint_Neck");
        AddReference("joint_Head");
        AddReference("joint_Pelvis");

        
    }
    public void InitPosition(Vector3 pos)
    {
        initialPosition = pos;
    }
    void Awake()
    {
        initialPosition = transform.position;
   
        for (int i = 0; i < reference.Count; i++)
        {
            GameObject go = new GameObject();
            if (model[i] != null)
            {
                go.transform.rotation = model[i].transform.rotation;
            }
            go.transform.position = reference[i].position;
            go.transform.SetParent(reference[i]);
            referenceObj.Add(go);
        }
    }
    void Start()
    {
       

       

    }

    public void Jump()
    {
        if (isJumping == false && gameObject.activeSelf)
        {
            isJumping = true;
            LeanTween.value(gameObject, 0, 1, 0.2f).setOnUpdate((float v) =>
            {

                jumpOffsetY = v;

            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1, 0, 0.2f).setOnUpdate((float v) =>
                {

                    jumpOffsetY = v;

                }).setOnComplete(() =>
                {                    
                    isJumping = false;

                });

            });
        }

    }
    void Update()
    {
        for (int i = 0; i < reference.Count; i++)
        {
            if (model[i] != null)
            {
                model[i].rotation = referenceObj[i].transform.rotation;

            }
        }
        if (avatarController!=null)
        {
            transform.position = initialPosition + avatarController.offsetPosition;//x z 
           
        }
        if (leftPoint!=null && rightPoint !=null)
        {
            float footOffset = leftPoint.position.y < rightPoint.position.y ? leftPoint.position.y : rightPoint.position.y;
            transform.position = new Vector3(transform.position.x, -footOffset, transform.position.z);//y



        }

        transform.position = transform.position + Vector3.up * jumpOffsetY;//jump

    }
}
