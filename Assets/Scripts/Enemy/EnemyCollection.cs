using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Enemy Collection", menuName="Enemy/Enemy Collection", order=1)]
public class EnemyCollection : ScriptableObject {

    [Header("Put Enemy Prefab Here")]
    public GameObject[] enemyGameobject = new GameObject[1];

}
