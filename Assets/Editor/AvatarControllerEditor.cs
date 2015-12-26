using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(AvatarController))]
public class AvatarControllerEditor : Editor
{

    AvatarController _target;
    void OnEnable()
    {
        _target = target as AvatarController;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Match"))
        {
            _target.AutoDetectReferences();

        }
    }
}
