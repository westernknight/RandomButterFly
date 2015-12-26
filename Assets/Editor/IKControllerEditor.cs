using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(IKController))]
public class IKControllerEditor : Editor
{
    IKController _target;
    Transform detectObject;
    void OnEnable()
    {
        _target = target as IKController;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

       detectObject =  EditorGUILayout.ObjectField(detectObject,typeof(Transform)) as Transform;
        if (GUILayout.Button("Match"))
        {
            _target.DetectByName(detectObject);
        }
    }


}
