using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DebugLogRedirect : MonoBehaviour
{

    StreamWriter logWriter;
    void Awake()
    {



        FileStream fs = File.Open( System.IO.Path.Combine(Application.persistentDataPath, "log.txt"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        logWriter = new StreamWriter(fs);

        Application.RegisterLogCallback(LogCallback);


    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        string logstring = "";
        switch (type)
        {
            case LogType.Assert:
                logstring = DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]") + "LogType.Assert: " + condition + " " + stackTrace;
                break;
            case LogType.Error:
                logstring = DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]") + "LogType.Error: " + condition + " " + stackTrace;
                break;
            case LogType.Exception:
                logstring = DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]") + "LogType.Exception: " + condition + " " + stackTrace;
                break;
            case LogType.Log:
                stackTrace = "";
                logstring = DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]") + "LogType.Log: " + condition + " " + stackTrace;
                break;
            case LogType.Warning:
                stackTrace = "";
                logstring = DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]") + "LogType.Warning: " + condition + " " + stackTrace;
                break;
            default:
                break;
        }
        if (logWriter != null)
        {
            logWriter.WriteLine(logstring);

            logWriter.Flush();
        }

    }
}
