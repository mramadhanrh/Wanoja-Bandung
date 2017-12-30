using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScript : MonoBehaviour {

    public static ResultScript Instance;

    public Character character;
    public TextMeshProUGUI score_Text;
    public TextMeshProUGUI highScore_Text;

    bool isWin;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetGameOver()
    {
        highScore_Text.SetText("HighScore : " + GameData.SetHighScore().ToString());
        score_Text.SetText("Score : " + GameData.myScore.ToString());
        GameData.SetCoin();
        GameManager.Instance.resultBoard.SetActive(true);
    }
}
