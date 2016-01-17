using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public GameObject canvas;


    public GameObject baseUI;
    public GameObject countDownPanel;
    public Image countDownImage;
    public Text countText;

    public GameObject msgBox;
    public GameObject takePictureArea;
    public Text msgText;

    public GameObject blackMask;

    public GameObject pictureUI;
    public GameObject title;
    public GameObject imageFrame;
    public Text titleText;

    public Image image;

    public List<Sprite> countDownList = new List<Sprite>();
    void Awake()
    {
        instance = this;
    }
    #if false
    public void Hide()
    {
        canvas.SetActive(false);
    }
    public void Show()
    {
        canvas.SetActive(true);
    }
    #endif

    public void HideEach()
    {
        HideBaseUI();
        HideBlack();
        HidePictureUI();
    }

    public void ShowEach()
    {
        baseUI.SetActive(true);
        blackMask.SetActive(true);
        pictureUI.SetActive(true);
    }

    public void HideBaseUI()
    {
        baseUI.SetActive(false);
    }
    public void HideBlack()
    {
        blackMask.SetActive(false);
    }
    public void HidePictureUI()
    {
        pictureUI.SetActive(false);
    }
   public void ShowBaseUI()
    {
        baseUI.SetActive(true);
    }
    public void ShowBlack()
    {
        blackMask.SetActive(true);
    }
    public void ShowPictureUI()
    {
        //to do 
        pictureUI.SetActive(true);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
