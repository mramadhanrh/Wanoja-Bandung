using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileSetter : MonoBehaviour {

    public TilesetCollection ts;
    public int blockCount;

    List<SpriteRenderer> subTileSprite = new List<SpriteRenderer>();
    SpriteRenderer groundSprite;

    public void SetGroundTile()
    {
        if (transform.FindChild("Main Ground") == null || transform.FindChild("Sub Ground") == null)
        {
            SetGround();
            SetGap();
            SetGroundSubTile();
        }
        else
        {
            Debug.LogWarning("Sudah ada Main Ground");
        }
    }

    void SetGround()
    {
        GameObject mainGround = new GameObject("Main Ground");
        mainGround.layer = LayerMask.NameToLayer("Ground");
        mainGround.transform.SetParent(this.transform);
        mainGround.transform.localScale = new Vector3(16, 5, 0);
        mainGround.transform.localPosition = new Vector3(0, -4, 0);

        mainGround.AddComponent<BoxCollider2D>();
        TileSpriteChanger tsc = mainGround.AddComponent<TileSpriteChanger>();
        tsc.myTileType = TileType.MainBlock;

        groundSprite = mainGround.AddComponent<SpriteRenderer>();
        groundSprite.sprite = LevelInfo.tilesetCollection.tileSet[4];
        groundSprite.sortingOrder = 1;
    }

    void SetGap()
    {
        GameObject gap = new GameObject("Gap");
        gap.layer = LayerMask.NameToLayer("Gap");
        gap.tag = "Gap";
        gap.transform.SetParent(this.transform);

        BoxCollider2D gapCol = gap.AddComponent<BoxCollider2D>();
        gapCol.size = new Vector2(1, 13);
        gapCol.offset = new Vector2(-8, 0);

        gap.AddComponent<GapTrigger>();
    }

    void SetGroundSubTile()
    {
        GameObject subGround = new GameObject("Sub Ground");
        subGround.transform.SetParent(this.transform);
        subGround.transform.localPosition = Vector2.zero;

        TileSpriteChanger tsc = subGround.AddComponent<TileSpriteChanger>();
        tsc.myTileType = TileType.TileBlock;


        for (int i = 0; i < 16; i++)
        {
            GameObject tile = new GameObject("Tile");
            tile.transform.SetParent(subGround.transform);
            tile.transform.localPosition = new Vector2(-7.5f + i, -2);

            SpriteRenderer tileSpr = tile.AddComponent<SpriteRenderer>();
            tileSpr.sprite = LevelInfo.tilesetCollection.tileSet[1];
            tileSpr.sortingOrder = 2;
            subTileSprite.Add(tileSpr);
        }

    }

    public void SetFlyingPlatform()
    {
        GameObject flyPlatform = new GameObject("Flying Platform");
        flyPlatform.layer = LayerMask.NameToLayer("Ground");
        flyPlatform.transform.SetParent(this.transform);
        flyPlatform.transform.localPosition = Vector2.zero;

        TileSpriteChanger tsc = flyPlatform.AddComponent<TileSpriteChanger>();
        tsc.myTileType = TileType.TileBlock;

        BoxCollider2D fpCollider = flyPlatform.AddComponent<BoxCollider2D>();

        for (int i = 0; i < blockCount; i++)
        {
            GameObject tileBlock = new GameObject("Tile Block " + i.ToString());
            tileBlock.transform.SetParent(flyPlatform.transform);
            tileBlock.transform.localPosition = new Vector3(i, 0, 0);

            SpriteRenderer sprTBlock = tileBlock.AddComponent<SpriteRenderer>();
            sprTBlock.sprite = LevelInfo.tilesetCollection.tileSet[1];
        }

        fpCollider.size = new Vector2(blockCount, 1);
        fpCollider.offset = new Vector2((blockCount % 2 == 0) ? blockCount / 2 - 0.5f : blockCount / 2, 0);

#if UNITY_EDITOR
        Selection.activeGameObject = flyPlatform;
#endif
    }


    public void SetTileSet()
    {
        LevelInfo.tilesetCollection = ts;
    }

    public void ResetPlatform()
    {
        Transform[] trans = GetComponentsInChildren<Transform>();

        foreach (Transform _trans in trans)
        {
            if (_trans != this.transform && _trans != null)
                DestroyImmediate(_trans.gameObject);
        }
    }
}

public static class LevelInfo
{
    public static TilesetCollection tilesetCollection { get; set; }
    public static LevelCollection levelCollection { get; set; }
    public static EnemyCollection enemyCollection { get; set; }
    public static Sprite background { get; set; }
}


#if UNITY_EDITOR
[CustomEditor(typeof(TileSetter))]
public class TileSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileSetter ts = (TileSetter)target;

        GUILayout.Space(50f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Ground Tile"))
        {
            ts.SetTileSet();
            ts.SetGroundTile();
        }

        if (GUILayout.Button("Spawn Flying Platform"))
        {
            ts.SetTileSet();
            ts.SetFlyingPlatform();
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Reset Platform"))
        {
            ts.ResetPlatform();
        }
    }
}

#endif