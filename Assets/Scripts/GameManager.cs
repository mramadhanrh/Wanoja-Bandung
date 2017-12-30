using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public GameplayUI gameplayUI;
    public Image healthUI;
    public Image gaugeUI;
    public bool isPause;
    public LevelCollection myTile;
    public GameObject resultBoard;
    public GameObject coin;
    public Image background;

    [HideInInspector]
    public TileManager tileManager;

    void Awake()
    {
        resultBoard = GameObject.FindGameObjectWithTag("Result Board");
        GameData.myScore = 0;
        Instance = this;
        myTile = LevelInfo.levelCollection;
    }

	// Use this for initialization
	void Start () {
        background.sprite = LevelInfo.background;
        resultBoard.SetActive(false);
        isPause = false;
        gameplayUI = new GameplayUI(healthUI, gaugeUI);
        tileManager = new TileManager(myTile);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        gameplayUI.SetMethods();
	}

    public void SetNextWave()
    {
        tileManager.NextWaves(true);
        CameraFollow.Instance.showArrow = true;
    }
}

[System.Serializable]
public class TileManager : Component
{
    public GameObject[] myTile;
    public List<GameObject> myTileList = new List<GameObject>();

    public TileManager(LevelCollection _tileCollection)
    {
        myTile = _tileCollection.myTile;
        GenerateTile();
    }

    public void GenerateTile()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject Tile = Instantiate(myTile[Random.Range(0, myTile.Length)]);
            Tile.transform.position = new Vector3((-16 + (16 * i)), 0, 0);
            myTileList.Add(Tile);
        }
    }

    public void ReAssign()
    {
        GameObject Tile = Instantiate(myTile[Random.Range(0, myTile.Length)]);
        Destroy(myTileList[0].gameObject);
        myTileList.RemoveAt(0);
        myTileList.Add(Tile);

        myTileList[2].transform.position = new Vector3(CameraFollow.Instance.myCamera.targetPosition.x + 15.8f,
                                                            CameraFollow.Instance.myCamera.targetPosition.y,
                                                            0);   
    }

    public void NextWaves(bool colliderValue)
    {
        myTileList[2].GetComponentInChildren<GapTrigger>().gameObject.GetComponent<BoxCollider2D>().isTrigger = colliderValue;
    }

    public void SetAllNotTrigger()
    {
        foreach (GameObject tile in myTileList)
        {
            tile.GetComponentInChildren<GapTrigger>().gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}

public class GameplayUI
{
    public Image healthUI, gaugeUI;

    public GameplayUI(Image _healthUI, Image _gaugeUI)
    {
        healthUI = _healthUI;
        gaugeUI = _gaugeUI;
    }

    void SetUIValue()
    {
        healthUI.fillAmount = Character.Instance.myBehaviour.getHealth();
        gaugeUI.fillAmount = Character.Instance.myBehaviour.getGauge();
    }

    public void SetMethods()
    {
        SetUIValue();
    }
}

public static class GameData
{
    #region Score_Script
    public static int myScore { get; set; }
    public static int highScore { get; set; }

    public static int SetHighScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
            if (myScore > highScore)
            {
                highScore = myScore;
                PlayerPrefs.SetInt("highScore", highScore);
            }
        }
        else
        {
            highScore = myScore;
            PlayerPrefs.SetInt("highScore", highScore);
        }


        PlayerPrefs.Save();

        return highScore;
    }
    #endregion

    #region Coin_Script
    public static int myCoin { get; set; }
    public static int myRecentCoin { get; set; }

    public static void SetCoin()
    {
        if (PlayerPrefs.HasKey("myCoin"))
        {
            myRecentCoin = PlayerPrefs.GetInt("myCoin");
            myRecentCoin += myCoin;
        }
        else
            myRecentCoin = myCoin;

        SaveCoin(myRecentCoin);
    }

    static void SaveCoin(int _myRecentCoin)
    {
        Debug.Log(_myRecentCoin);
        myRecentCoin = _myRecentCoin;
        PlayerPrefs.SetInt("myCoin", myRecentCoin);

        PlayerPrefs.Save();
        Debug.Log(myRecentCoin);
    }

    public static void DecreaseCoin(int _val)
    {
        myRecentCoin -= _val;
        PlayerPrefs.SetInt("myCoin", myRecentCoin);
        PlayerPrefs.Save();
    }
    #endregion

    #region SceneTarget
    public static string sceneTarget { get; set; }
    #endregion

    #region Point

    public static int myPoint { get; set; }


    public static void SetPoint()
    {
        if (PlayerPrefs.HasKey("myPoint"))
            myPoint = PlayerPrefs.GetInt("myPoint");
        else
        {
            myPoint = 20;
            PlayerPrefs.SetInt("myPoint", 20);
        }

        PlayerPrefs.Save();
    }

    public static void IncreasePoint(int _val = 1)
    {
        myPoint += _val;
        PlayerPrefs.SetInt("myPoint", myPoint);
        PlayerPrefs.Save();
    }

    public static void DecreasePoint(int _val = 1)
    {
        myPoint -= _val;
        PlayerPrefs.SetInt("myPoint", myPoint);
        PlayerPrefs.Save();
    }

    #endregion

    public static void SetAllValue()
    {
        SetPoint();
        if (PlayerPrefs.HasKey("myCoin"))
            myRecentCoin = PlayerPrefs.GetInt("myCoin");
        else
            myRecentCoin = myCoin;
    }
}