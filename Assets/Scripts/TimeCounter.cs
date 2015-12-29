using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeCounter : MonoBehaviour
{

    Text timeText;
    float elapseTime = 0;
    const float readDelay = 0.5f;
    float elapseReadDelay = 2;
    bool running = false;
    public bool isTimeOut = false;

    public Action timeOutEvent;

    float setCounter = 30;
    void Awake()
    {
        timeText = GetComponent<Text>();
        Reset(setCounter);
    }
    void Start()
    {
       
       
    }
    public void Reset(float setTime)
    {
        timeText.color = new Color(201 / 255.0f, 220 / 255.0f, 48 / 255.0f);

        elapseTime = 0;
        elapseReadDelay = readDelay;

        float nowTime = setTime;
        setCounter = setTime;
        string a = ((int)(nowTime) / 10).ToString();
        string b = ((int)(nowTime) % 10).ToString();
        string c = ((int)((nowTime) * 10) % 10).ToString();
        string d = ((int)((nowTime) * 100) % 10).ToString();
        timeText.text = a + b + "." + c + d;
        isTimeOut = false;
    }
    public void ResetAndStart(float setTime)
    {
        timeText.color = new Color(201/255.0f, 220 / 255.0f, 48 / 255.0f);
        running = true;
        elapseTime = 0;
        elapseReadDelay = readDelay;

        float nowTime = setTime;
        setCounter = setTime;
        string a = ((int)(nowTime) / 10).ToString();
        string b = ((int)(nowTime) % 10).ToString();
        string c = ((int)((nowTime) * 10) % 10).ToString();
        string d = ((int)((nowTime) * 100) % 10).ToString();
        timeText.text = a + b + "." + c + d;
        isTimeOut = false;
    }
    public void Pause()
    {
        running = false;
    }
    public void Resume()
    {
        running = true;
    }
    void TimeOut()
    {
        Debug.Log("TimeOut");
        isTimeOut = true;
        if (timeOutEvent != null)
        {
            timeOutEvent();
        }

    }
    void Update()
    {
        if (running)
        {
            if (elapseReadDelay < 0)
            {
                elapseTime += Time.deltaTime;
                if (setCounter - elapseTime > 0)
                {
                    float nowTime = setCounter - elapseTime;
                    string a = ((int)(nowTime) / 10).ToString();
                    string b = ((int)(nowTime) % 10).ToString();
                    string c = ((int)((nowTime) * 10) % 10).ToString();
                    string d = ((int)((nowTime) * 100) % 10).ToString();
                    timeText.text = a + b + "." + c + d;
                    if (nowTime < setCounter / 3.0f)
                    {
                        timeText.color = new Color(232 / 255.0f, 110 / 255.0f, 95 / 255.0f);
                    }
                }
                else
                {
                    timeText.text = "00.00";
                    running = false;
                    TimeOut();
                }
            }
            else
            {
                elapseReadDelay -= Time.deltaTime;
            }

        }
    }

}
