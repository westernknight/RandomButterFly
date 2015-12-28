using UnityEngine;
using System.Collections;
using System.IO;
using Windows.Kinect;
using System.Runtime.InteropServices;
using System;

public class KinectSensorManager
{
    /*
    private static byte[] screenShotByteList;
    private KinectSensor m_pKinectSensor = null;
    private BodyFrameReader m_pBodyFrameReader = null;
    private ColorFrameReader m_pColorFrameReader = null;

    public static KinectSensorManager instance = null;
    private CoordinateMapper coordinateMapper = null;

    private IntPtr screenShotData;
    public void InitializeDefaultSensor()
    {
        m_pKinectSensor = KinectSensor.GetDefault();

        if (m_pKinectSensor != null)
        {
          
                screenShotByteList = new byte[1920 * 1080 * 4];
            


            m_pKinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            m_pColorFrameReader = m_pKinectSensor.ColorFrameSource.OpenReader();



            m_pBodyFrameReader = m_pKinectSensor.BodyFrameSource.OpenReader();
            this.coordinateMapper = this.m_pKinectSensor.CoordinateMapper;
            if (m_pBodyFrameReader != null)
            {
                m_pBodyFrameReader.FrameArrived += this.Reader_FrameArrived;
            }

            if (m_pKinectSensor.IsOpen == false)
            {
                m_pKinectSensor.Open();
            }
        }

        if (m_pKinectSensor == null)
        {
            UnityEngine.Debug.LogError("No ready Kinect found!");
        }
    }
    private void Sensor_IsAvailableChanged( IsAvailableChangedEventArgs e)
    {
        // on failure, set the status text
        Debug.Log(m_pKinectSensor.IsAvailable);
    }
    private void Reader_FrameArrived( BodyFrameArrivedEventArgs e)
    {
    }
    private void Reader_ColorFrameArrived( ColorFrameArrivedEventArgs e)
    {
        using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
        {
            if (colorFrame != null)
            {
                FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                {

                    try
                    {
                        if ((colorFrameDescription.Width == 1920) && (colorFrameDescription.Height == 1080))
                        {
                       
                            GCHandle hObject = GCHandle.Alloc(screenShotByteList, GCHandleType.Pinned);

                            screenShotData = hObject.AddrOfPinnedObject();


                            colorFrame.CopyConvertedFrameDataToIntPtr(
                               screenShotData,
                               (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Rgba);

                            byte[] screenShotByte = screenShotByteList;
                            for (int y = 0; y < 1080 / 2; y++)
                            {
                                for (int x = 0; x < 1920; x++)
                                {
                                    byte b = screenShotByte[x * 4 + 4 * 1920 * y];
                                    byte g = screenShotByte[x * 4 + 4 * 1920 * y + 1];
                                    byte r = screenShotByte[x * 4 + 4 * 1920 * y + 2];
                                    byte a = screenShotByte[x * 4 + 4 * 1920 * y + 3];

                                    screenShotByte[x * 4 + 4 * 1920 * y] = screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1))];
                                    screenShotByte[x * 4 + 4 * 1920 * y + 1] = screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1)) + 1];
                                    screenShotByte[x * 4 + 4 * 1920 * y + 2] = screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1)) + 2];
                                    screenShotByte[x * 4 + 4 * 1920 * y + 3] = screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1)) + 3];


                                    screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1))] = b;
                                    screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1)) + 1] = g;
                                    screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1)) + 2] = r;
                                    screenShotByte[x * 4 + 4 * 1920 * (1080 - (y + 1)) + 3] = a;
                                }
                            }


                        }
                        else
                        {
                            Debug.Log("screen shot error.kinect description not 1920*1080");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    m_pColorFrameReader.FrameArrived -= Reader_ColorFrameArrived;


                }
            }
        }
    }
    public  void Shot()
    {

    }
    /// <summary>
    /// 捕捉图像后，获得它的Texture2D
    /// </summary>
    /// <param name="index">buffer索引</param>
    /// <returns></returns>
    public Texture2D GetScreenShotTexture()
    {
        

        if (screenShotByteList != null)
        {
            Texture2D t = new Texture2D(1920, 1080, TextureFormat.RGBA32, false);
            t.LoadRawTextureData(screenShotByteList);
            t.Apply();
            return t;
        }


        return null;
    }

    public void DumpAllScreenShot()
    {

        if (screenShotByteList != null)
        {
            Texture2D t = new Texture2D(1920, 1080, TextureFormat.BGRA32, false);
            t.LoadRawTextureData(screenShotByteList);
            byte[] data = t.EncodeToPNG();
            string targetPath = Path.Combine(GameProcess.instance.config.savePicturePath, GameProcess.instance.config.taskCount + Random.Range(0, GlobalVar.SAVE_PICTURE_RANDOM_NUMBER_MAX) + ".png");
            GameProcess.instance.config.taskCount++;
            GameProcess.instance.SaveConfig();
            File.WriteAllBytes(targetPath, data);
        }

    }
     */
}
