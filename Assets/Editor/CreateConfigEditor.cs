using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class CreateConfigEditor : EditorWindow
{

    [MenuItem("Project Tools/CreateJsonConfig ")]
    static void Init()
    {
        CreateConfigEditor window = (CreateConfigEditor)EditorWindow.GetWindow(typeof(CreateConfigEditor));
        window.Show();

    }
    void OnGUI()
    {

        if (GUILayout.Button("Create"))
        {
            FileInfo fi = new FileInfo(Path.Combine(Application.dataPath, "config.txt"));
            StreamWriter sw = new StreamWriter(fi.Create());
            string jsonConfig = LitJson.JsonMapper.ToJson(new GlobalStructure());
            sw.Write(jsonConfig);
            sw.Close();

        }

    }
}
