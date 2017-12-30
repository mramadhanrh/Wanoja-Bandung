using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    void Attack();
    void Damaged(float inDamage);
    void Patrol();
    void Chase();
    void Death();
}

public enum EnemyState
{
    Patrol,
    Chase
}

public class Enemy : MonoBehaviour {

    public ButaBehaviour myBehaviour;
    public EntityInfo myInfo;

	// Use this for initialization
	void Start () {
        myBehaviour = new ButaBehaviour(this.gameObject, myInfo);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        myBehaviour.SetMethods();
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            myBehaviour.Attack();
        }
    }
}

public class ButaBehaviour : Component, IEnemy
{
    Rigidbody2D myRb;

    EnemyState myState = EnemyState.Patrol;

    Status myStatus;

    GameObject character;

    RaycastHit2D hit, gapHit;

    EntityInfo myInfo;

    Vector2 ecCross;
    float mySpeed = 2f;
    public float dir = 1;

    public ButaBehaviour(GameObject go, EntityInfo _myInfo)
    {
        myInfo = _myInfo;
        myInfo.SetCharacter();
        myStatus = new Status(myInfo.myAttribute);

        myRb = go.GetComponent<Rigidbody2D>();
        character = Character.Instance.gameObject;
    }

    #region IEnemy Implementation

    public void Attack()
    {
        Character.Instance.myBehaviour.Damaged(myStatus.damage, dir);
        myRb.AddForce(new Vector2(-dir * 300, 0));
    }

    public void Damaged(float inDamage)
    {
        if (Character.Instance != null)
        {
            Vector2 localDir = myRb.transform.position - Character.Instance.transform.position;
            localDir.Normalize();
            float normalizeMagnitudeX = (localDir.x > 0) ? 1 : -1;
            myRb.AddForce(new Vector2(normalizeMagnitudeX * 400, 0));
            dir = -normalizeMagnitudeX;
            myStatus.health -= inDamage;
        }
    }

    public void Patrol()
    {
        if (hit.collider != null)
            myRb.AddForce(new Vector2(0, 20), ForceMode2D.Force);

        if (gapHit.collider != null)
            dir = -dir;

        myRb.transform.Translate(new Vector2(dir * mySpeed * Time.fixedDeltaTime, 0));
    }

    public void Chase()
    {
        if (hit.collider != null)
            myRb.AddForce(new Vector2(0, 30), ForceMode2D.Force);

        dir = (Mathf.Round(ecCross.normalized.x) == 0) ? 1 : Mathf.Round(ecCross.normalized.x);

        myRb.transform.Translate(new Vector2(ecCross.normalized.x * mySpeed * Time.fixedDeltaTime, 0));
    }

    public void Death()
    {
        if (myStatus.health <= 0)
        {
            Instantiate(GameManager.Instance.coin.gameObject, myRb.transform.position, Quaternion.identity);
            EnemySpawner.Instance.enemyDead++;
            GameData.myScore++;
            Destroy(myRb.gameObject);
        }
    }

    #endregion

    void SetRaycast()
    {
        hit = Physics2D.Raycast(new Vector3(myRb.transform.position.x, myRb.transform.position.y, 0), new Vector2(dir, 0), 1f, 1 << 8 );
        gapHit = Physics2D.Raycast(new Vector3(myRb.transform.position.x, myRb.transform.position.y, 0), new Vector2(dir, 0), 1f, 1 << 11);
    }

    void SetState()
    {
        if (character.gameObject != null)
        {
            ecCross = character.transform.position - myRb.transform.position;

            if (ecCross.magnitude <= 3f)
            {
                myState = EnemyState.Chase;
            }
            else
            {
                myState = EnemyState.Patrol;
            }
        }
        else
        {
            myState = EnemyState.Patrol;
        }
    }

    void SetFacing()
    {
        myRb.transform.localScale = new Vector3(-dir, myRb.transform.localScale.y, myRb.transform.localScale.z);
    }

    public void SetMethods()
    {
        SetState();
        SetRaycast();
        SetFacing();
        Death();

        if (myState == EnemyState.Chase)
            Chase();
        else if (myState == EnemyState.Patrol)
            Patrol();
    }
}
