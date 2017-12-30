using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner Instance;
    public EnemyCollection eC;

    public int waveCount;
    public int enemyDead;
    
    int enemyCount;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        eC = LevelInfo.enemyCollection;
        CallEnemy(Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
        if (enemyDead == enemyCount)
        {
            enemyDead = 0;
            GameManager.Instance.SetNextWave();
        }
	}

    void SpawnEnemy(int limit, Vector3 pos)
    {
        for (int i = 0; i < limit; i++)
        {
            Instantiate(eC.enemyGameobject[Random.Range(0, eC.enemyGameobject.Length)], new Vector3(pos.x + Random.Range(-3f, 3f),
                                                                                                    pos.y + Random.Range(0,3f), 0), Quaternion.identity);
        }
    }

    public void CallEnemy(Vector3 pos)
    {
        waveCount++;
        enemyCount = (waveCount < 3) ? 3 : (waveCount / 3) + 3;
        EnemySpawner.Instance.SpawnEnemy(enemyCount, pos);
    }
}
