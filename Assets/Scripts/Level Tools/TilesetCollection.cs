using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Tileset Collection", menuName="Tile/Tileset Collection", order=2)]
public class TilesetCollection : ScriptableObject {

    [Header("Tileset Sprites")]
    [Tooltip("Masukan Sesuai Order = Upper Left to Bottom Right, From Left To Right")]

    public Sprite[] tileSet = new Sprite[0];
    
}
