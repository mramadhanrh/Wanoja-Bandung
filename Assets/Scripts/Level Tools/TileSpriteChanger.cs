using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    MainBlock,
    TileBlock
}

public class TileSpriteChanger : MonoBehaviour {

    public TileType myTileType;
    List<SpriteRenderer> mySprite = new List<SpriteRenderer>();

	// Use this for initialization
	void Start () {
        SetSprite();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSprite()
    {
        CollectSpriteRenderer();
        ChangeSprite();
    }

    void CollectSpriteRenderer()
    {
        mySprite.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    void ChangeSprite()
    {
        int spriteIndex = (myTileType == TileType.MainBlock) ? 4 : 1;

        foreach (SpriteRenderer _mySprite in mySprite)
        {
            _mySprite.sprite = LevelInfo.tilesetCollection.tileSet[spriteIndex];
        }
    }
}
