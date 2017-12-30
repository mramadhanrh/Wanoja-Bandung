using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler {

    public TilesetCollection myTileSetCollection;
    public LevelCollection myLevelCollection;
    public EnemyCollection myEnemyCollection;
    public Sprite background;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        SetCollection();
        GameData.sceneTarget = "Loading";
        Loading.Instance.LoadingLoad();
    }

    void SetCollection()
    {
        LevelInfo.tilesetCollection = myTileSetCollection;
        LevelInfo.levelCollection = myLevelCollection;
        LevelInfo.enemyCollection = myEnemyCollection;
        LevelInfo.background = background;
    }
}
