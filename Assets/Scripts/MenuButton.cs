using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum ButtonName
{
    Play,
    Upgrade,
    Exit,
    Back,
    Panel,
    ClosePanel,
    Url
}

public class MenuButton : MonoBehaviour, IPointerClickHandler {

    public ButtonName myButton;
    public GameObject ScrollView;
    public string mapUrl;
	// Use this for initialization
	void Start () {
        if(ScrollView != null)
            ScrollView.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (myButton == ButtonName.Play)
        {
            GameData.sceneTarget = "MainGame";
        }
        else if (myButton == ButtonName.Upgrade)
        {
            GameData.sceneTarget = "Upgrade";
        }
        else if(myButton == ButtonName.Exit)
        {
            Application.Quit();
        }
        else if (myButton == ButtonName.Back)
        {
            Time.timeScale = 1;
            GameData.sceneTarget = "MainMenu";
        }
        else if (myButton == ButtonName.Panel)
        {
            ScrollView.SetActive(true);
            return;
        }
        else if (myButton == ButtonName.ClosePanel)
        {
            ScrollView.SetActive(false);
            return;
        }
        else if (myButton == ButtonName.Url)
        {
            Application.OpenURL(mapUrl);
            return;
        }
        Loading.Instance.LoadingLoad();
    }
}
