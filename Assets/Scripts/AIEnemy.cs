using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIBehaviour
{
    void Patrol();
    void Attack();
    void Damaged(float damage);
    void SetState();
}

public class AIEnemy : MonoBehaviour {

    public AIEnemyObject _myAIEnemyObject;
    public EntityInfo _myEntityInfo;

    [HideInInspector]
    public AIBehaviour _myAIBehaviour;

    void Awake()
    {
        _myAIBehaviour = new AIBehaviour(_myAIEnemyObject, _myEntityInfo, this.gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        _myAIBehaviour.SetState();
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _myAIBehaviour.Damage();
        }
    }
}

public class AIBehaviour : Component, IAIBehaviour
{

    public AIEnemyObject myAI;
    public GameObject myGameobject;
    public bool isDirectionRight;

    Rigidbody2D myRb;
    RaycastHit2D groundRay;
    RaycastHit2D attackRay;
    GameObject character;
    EntityInfo myEntityInfo;
    Status myStatus;

    float timeDirection;
    float timeAttackDelay;

    public AIBehaviour(AIEnemyObject _AIObject, EntityInfo _myEntityInfo, GameObject _myGameobject)
    {
        myAI = _AIObject;

        myGameobject = _myGameobject;

        myRb = myGameobject.GetComponent<Rigidbody2D>();

        myEntityInfo = _myEntityInfo;
        myEntityInfo.SetCharacter();

        myStatus = myEntityInfo.myStatus;

        character = Character.Instance.gameObject;
    }

    #region IAIEnemy

    public void Patrol()
    {
        switch(myAI.patrolState)
        {
            case PatrolState.Normal:
                NormalPatrol();
                Crawl();
                break;
            case PatrolState.Bouncy:
                NormalPatrol();
                BouncyPatrol();
                Crawl();
                break;
        }
    }

    public void Attack()
    {
        switch (myAI.attackState)
        {
            case AttackState.Chase_Then_Attack:
                ChaseThenAttack();
                Crawl();
                break;
        }
    }

    public void Damaged(float damage)
    {
        if (Character.Instance != null)
        {
            Debug.Log(damage + " dan " + myStatus.health);
            myRb.AddForce(getDirection() * 5, ForceMode2D.Impulse);
            myStatus.health -= damage;

            if (myStatus.health <= 0)
            {
                Instantiate(GameManager.Instance.coin.gameObject, myRb.transform.position, Quaternion.identity);
                EnemySpawner.Instance.enemyDead++;
                GameData.myScore++;
                Destroy(myGameobject.gameObject);
            }
        }
    }

    public void SetState()
    {
        GroundRay();

        SetDirection();

        if (getMagnitude() < myAI.distanceThreshold)
            Attack();
        else
            Patrol();
    }

    public void SetDirection()
    {
        if (timeDirection <= myAI.duration)
        {
            timeDirection += Time.fixedDeltaTime;
        }
        else
        {
            timeDirection = 0;
            isDirectionRight = !isDirectionRight;
        }
    }
    #endregion

    #region Patrol_Behaviour

    void NormalPatrol()
    {
        int _dir = ((isDirectionRight) ? 1 : -1);
        myGameobject.transform.Translate(new Vector3((myAI.speed * Time.fixedDeltaTime) * _dir, 0, 0));
        myGameobject.transform.localScale = new Vector3(-_dir, myGameobject.transform.localScale.y, myGameobject.transform.localScale.z);
    }

    void BouncyPatrol()
    {
        if (groundRay.collider != null)
        {
            Debug.Log("bounce");
            myRb.AddForce(Vector2.up * myAI.forcePower, ForceMode2D.Impulse);
        }

        if (myRb.velocity.y < 0)
        {
            myRb.AddForce(Vector2.up * -myAI.forcePower, ForceMode2D.Impulse);
        }
    }

    void GroundRay()
    {
        groundRay = Physics2D.Raycast(new Vector3(myGameobject.transform.position.x, myGameobject.transform.position.y - 0.15f, myGameobject.transform.position.z), Vector2.down, 0.2f, 1 << 8);
    }

    #endregion

    #region Change_Behaviour

    void ChaseThenAttack()
    {
        if (attackRay != null)
        {
            if (timeAttackDelay < 1f)
            {
                timeAttackDelay += Time.fixedDeltaTime;
            }
            else
            {
                timeAttackDelay = 0;
                myRb.AddForce(-(Vector2)getDirection() * 10f, ForceMode2D.Impulse);
                myGameobject.transform.localScale = new Vector3(((getDirection().x > 0.01f) ? 1 : -1), myGameobject.transform.localScale.y, myGameobject.transform.localScale.z);
            }
        }
        else
        {
            if (myAI.patrolState == PatrolState.Bouncy)
                BouncyPatrol();

            NormalChase();
        }
    }

    void NormalChase()
    {
        myGameobject.transform.Translate(new Vector3(-getDirection().x * Time.deltaTime * myAI.speed, 0, 0));
        myGameobject.transform.localScale = new Vector3(((getDirection().x > 0.01f) ? 1 : -1), myGameobject.transform.localScale.y, myGameobject.transform.localScale.z);
    }

    void AttackRay()
    {
        attackRay = Physics2D.Raycast(new Vector3(myGameobject.transform.position.x, myGameobject.transform.position.y, myGameobject.transform.position.z), -getDirection(), 2f, 1 << 9);
    }

    Vector3 getDirection()
    {
        if (character != null)
        {
            Vector3 _dir = myGameobject.transform.position - character.transform.position;
            return _dir.normalized;
        }
        return Vector2.zero;
    }

    float getMagnitude()
    {
        if (character != null)
        {
            Vector3 _dir = myGameobject.transform.position - character.transform.position;
            return _dir.magnitude;
        }
        return 1;
    }

    #endregion

    void Crawl()
    {
        RaycastHit2D crawlHit = Physics2D.Raycast(new Vector3(myGameobject.transform.position.x, myGameobject.transform.position.y, myGameobject.transform.position.z), -getDirection(), 0.5f, 1 << 8);
        if (crawlHit.collider != null)
        {
            myRb.AddForce(Vector2.up * 40);
        }
    }

    public void Damage()
    {
        if (Character.Instance != null)
        {
            Character.Instance.myBehaviour.Damaged(myStatus.damage, -((getDirection().x > 0) ? 1 : -1));
            myRb.AddForce(getDirection() * 2, ForceMode2D.Impulse);
        }
    }
}