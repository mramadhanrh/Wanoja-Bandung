using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            EnemySpawner.Instance.CallEnemy(new Vector3(CameraFollow.Instance.myCamera.targetPosition.x,
                                                        CameraFollow.Instance.myCamera.targetPosition.y, 0));
            CameraFollow.Instance.myCamera.AllowToMove();
            GameManager.Instance.tileManager.ReAssign();
            GameManager.Instance.tileManager.SetAllNotTrigger();
            CameraFollow.Instance.showArrow = false;
        }
    }
}
