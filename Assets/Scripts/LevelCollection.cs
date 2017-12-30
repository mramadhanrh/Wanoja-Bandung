using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Collection", menuName = "Tile/Level Collection", order = 1)]
public class LevelCollection : ScriptableObject
{
    public GameObject[] myTile;
}
