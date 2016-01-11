using UnityEngine;
using System.Collections;
using System;
using System.IO;

[RequireComponent(typeof(NetworkView))]
public class GConsoleCore : MonoBehaviour
{
    public static GConsoleCore instance;
    public bool isServer = true;
    [Tooltip("只有是server下才有作用")]
    public int port = 5454;
    string serverip = "";
    bool isConnectToServer = false;
    NetworkView network;
    public Action<string> echo;
    public Action<string> act;

    StreamWriter logWriter;
    [Tooltip("只有是server下才有作用")]
    public bool isLogFile = false;
    [Tooltip("只有是server下才有作用")]
    public bool isAppendFile = false;

    void Awake()
    {
        instance = this;
        ///只有server可以记录，因为console把callback注册到ui了，除非改写GConsole的HandleUnityLog，才能写到文件去
        if (isServer)
        {
            if (isLogFile)
            {
                FileStream fs = null;
                if (isAppendFile)
                {
                    fs = File.Open(System.IO.Path.Combine(Application.persistentDataPath, "log.txt"), FileMode.Append, FileAccess.Write, FileShare.Read);
                }
                else
                {
                    fs = File.Open(System.IO.Path.Combine(Application.persistentDataPath, "log.txt"), FileMode.Create, FileAccess.Write, FileShare.Read);
                }
                logWriter = new StreamWriter(fs);
            }
            Application.RegisterLogCallback(LogCallback);
        }

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
        Echo(logstring);
    }
    // Use this for initialization
    void Start()
    {
        network = GetComponent<NetworkView>();
        if (isServer)
        {
            StartServer();
        }
    }
    public void StartServer()
    {
        NetworkConnectionError error = Network.InitializeServer(1, port, true);
        Debug.Log("server status: " + error);
    }

    public void StartClient(string ip, int port)
    {
        serverip = ip;
        this.port = port;
        NetworkConnectionError error = Network.Connect(ip, port);
        Debug.Log("connect status: " + error);
        if (error == NetworkConnectionError.NoError)
        {
            Invoke("OnConnect", 0.5f);
        }
    }
    //客户端连接成功
    void OnConnect()
    {
        if (Network.isClient)
        {
            //告诉服务器连接成功
            network.RPC("ClientConnected", RPCMode.Server);
        }
        else
        {
            Debug.Log("Connect error");
            StartCoroutine(ReconnectToServer());
        }
    }

    /// <summary>
    /// 用户连接上来
    /// </summary>
    [RPC]
    void ClientConnected()
    {
        Debug.Log("ClientConnected");
    }

    public void Act(string cmd)
    {
        network.RPC("ActRPC", RPCMode.Server, cmd);

    }
    [RPC]
    void ActRPC(string cmd)
    {
        if (act != null)
        {
            act(cmd);
        }

    }

    public void Echo(string logstring)
    {
        network.RPC("EchoRPC", RPCMode.Others, logstring);
    }
    [RPC]
    void EchoRPC(string logstring)
    {
        if (echo != null)
        {
            echo(logstring);
        }
    }
    IEnumerator ReconnectToServer()
    {
        yield return new WaitForSeconds(1);
        while (isConnectToServer == false)
        {
            Debug.Log("Try to reconnect oculus server");
            NetworkConnectionError error = Network.Connect(serverip, port);
            yield return new WaitForSeconds(1);
        }
        Invoke("OnConnect", 0.5f);
    }
    void OnDisconnectedFromServer()
    {
        isConnectToServer = false;
        StartCoroutine(ReconnectToServer());
    }
    void OnConnectedToServer()
    {
        isConnectToServer = true;
        Debug.Log("Connected to server");
    }
    void OnApplicationQuit()
    {
        if (logWriter != null)
        {
            logWriter.Close();
        }

    }
}
