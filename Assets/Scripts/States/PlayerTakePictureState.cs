using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

public class PlayerTakePictureState : FSMState
{

    GameProcess gameProcess;
    string picName = "";
    bool isShot = false;
    bool isTimeOut = false;
    
    public PlayerTakePictureState(MonoBehaviour mono)
    {
        stateID = StateID.PlayerTakePicture;
        this.mono = mono;
        gameProcess = (GameProcess)mono;

    }

    public override void DoBeforeEntering()
    {
        isShot = false;
        isTimeOut = false;
        gameProcess.kinectBkImagePlane.SetActive(true);


        gameProcess.timeText.gameObject.SetActive(true);
        gameProcess.timeText.ResetAndStart((float)gameProcess.config.capturePhotoTime);
        gameProcess.pictureNameText.gameObject.SetActive(true);

        int r_index = UnityEngine.Random.Range(0, 10);
        picName = gameProcess.config.taskCount + "." + r_index + ".png";
        gameProcess.pictureNameText.text = picName;

      

        UIManager.instance.msgText.text = gameProcess.config.taskCount + "." + r_index;
        UIManager.instance.titleText.text = gameProcess.config.taskCount + "." + r_index;
        UIManager.instance.HideEach();
        UIManager.instance.playerCount = gameProcess.takePicturePlayerCount;
        UIManager.instance.ShowBaseUI();

        mono.StartCoroutine(StartCounting());

        TakePictureAnimController.instance.Play();


    }
    IEnumerator StartCounting()
    {
        yield return new WaitForSeconds(3);//for model play
        UIManager.instance.ShowArea();

        for (int i = 0; i < gameProcess.config.capturePhotoTime+1; i++)
        {
            UIManager.instance.countText.text = (gameProcess.config.capturePhotoTime - i).ToString();
    
            yield return new WaitForSeconds(1);
        }
        isTimeOut = true;
    }
    public override void DoBeforeLeaving()
    {
        gameProcess.timeText.gameObject.SetActive(false);
        for (int i = 0; i < gameProcess.playerModelsBinded.Count; i++)
        {
            gameProcess.playerModelsBinded[i].SetActive(false);
        }
        gameProcess.pictureNameText.gameObject.SetActive(false);
        //gameProcess.kinectBkImagePlane.SetActive(false);
        isShot = false;
        UIManager.instance.HideEach();

        TakePictureAnimController.instance.Hide();

    }
    void SavePicture()
    {

        Debug.Log("ka cha");
        var gp = ((GameProcess)mono);
        gameProcess.timeText.gameObject.SetActive(false);
        gameProcess.pictureNameText.gameObject.SetActive(false);




        gameProcess.config.taskCount++;
        gameProcess.SaveConfig();
        mono.StartCoroutine(Capture());
        mono.StartCoroutine(LoadATexture(Path.Combine(gp.config.savePicturePath, picName), UIManager.instance.image));
    }
    public IEnumerator LoadATexture(string path, Image image)
    {

        Debug.Log("file:///" + path);
        while (File.Exists(path) == false)
        {
            yield return null;
        }
        WWW www = new WWW("file:///" + path);
        while (www.isDone == false)
        {
            yield return null;
        }
        if (www.error == null)
        {
            var lenovoStateBk = www.texture;

            image.overrideSprite = Sprite.Create(lenovoStateBk, new Rect(0, 0, lenovoStateBk.width, lenovoStateBk.height), new Vector2(0.5f, 0.5f));
            Debug.Log("LoadATexture load sucess");
        }
        else
        {
            Debug.Log(www.error);
        }
        UIManager.instance.ShowBlack();
        UIManager.instance.ShowPictureUI();
        LeanTween.value(mono.gameObject, 0, 1, 5).setOnComplete(() => { gameProcess.SetTransition(StateID.LenovoModelRotation); });
    }
    IEnumerator Capture()
    {
        UIManager.instance.HideEach();
        yield return new WaitForEndOfFrame();
        Application.CaptureScreenshot(Path.Combine(gameProcess.config.savePicturePath, picName), 0);
        UIManager.instance.ShowBaseUI();
        for (int i = 0; i < gameProcess.playerModelsBinded.Count; i++)
        {
            Animation anim = gameProcess.playerModelsBinded[i].GetComponent<Animation>();
            anim.Stop();
        }

    }
    public override void Reason(GameObject player, GameObject npc)
    {
        if (isShot)
        {
            // gameProcess.SetTransition(StateID.LenovoModelRotation);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {

        if (isTimeOut && isShot == false)
        {
            isShot = true;

            SavePicture();
        }
        if (isShot == false)
        {
            gameProcess.RenderToImage();
        }
    }

}
