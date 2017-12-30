using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICharacter
{
    void Attack();
    void Move();
    void Jump();
    void Damaged(float inDamage, float dir);
    void Death();
}

public class Character : MonoBehaviour {

	public static Character Instance;

    public EntityInfo myInfo;

    public GameObject deathParticle;

    [HideInInspector]
    public CharacterBehaviour myBehaviour;

    public GameObject ResultBoard;

    AudioSource ac;

    public bool isWin;
    public GameObject fireball;

    void Awake()
    {
		Instance = this;
        ac = GetComponent<AudioSource>();
        myBehaviour = new CharacterBehaviour(this.gameObject, myInfo, ac);
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        myBehaviour.SetMethods();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Gap" && col.GetComponent<BoxCollider2D>().isTrigger)
            GetComponent<Rigidbody2D>().AddForce(new Vector2(20, 20));

        if (col.tag == "Enemy")
        {
            col.gameObject.GetComponent<AIEnemy>()._myAIBehaviour.Damaged(0);
        }
    }

    public void InstantiateDeath()
    {
        Instantiate(deathParticle.gameObject, transform.position, Quaternion.identity);
    }

}

public class CharacterBehaviour : Component, ICharacter
{
    GameObject character;
    public Animator characterAnimator;
    Rigidbody2D characterRb;
    EntityInfo myInfo;
    Status myStatus;
    AudioSource ac;
    ParticleSystem asuhParticle, asihParticle;
    GameObject asihCollider;

    int attackState;
    int facing = 1;
    float mSpeed;
    float airTime;
    float attackTime;
    float stunTime;
    float hitRange = 0.8f;

    RaycastHit2D jumpHit;
    RaycastHit2D attackHit;

    Vector3 mousePos;
    Vector3 threshold = new Vector2(0f, -0.5f);

    #region ICharacter implementation

    public CharacterBehaviour(GameObject go, EntityInfo _myInfo, AudioSource _ac)
    {
        character = go;
        characterAnimator = character.GetComponentInChildren<Animator>();
        characterRb = character.GetComponent<Rigidbody2D>();

        myInfo = _myInfo;
        myInfo.SetCharacter();
        myStatus = new Status(myInfo.myAttribute);

        ac = _ac;

        asuhParticle = go.transform.FindChild("Healing Particle").GetComponent<ParticleSystem>();
        asihParticle = go.transform.FindChild("Ring Shield").GetComponent<ParticleSystem>();
        asihCollider = go.transform.FindChild("Ring Collider").gameObject;
        asihCollider.SetActive(false);
    }

    public void Attack()
    {
        if (attackTime >= 0.1f)
        {
            characterAnimator.SetInteger("attackState", attackState);
            characterAnimator.SetTrigger("isAttack");

            if (attackHit.collider != null)
            {
                attackHit.collider.gameObject.GetComponent<AIEnemy>()._myAIBehaviour.Damaged(myStatus.damage);
                //myAudioSource.clip = myAudioClip[1];
                SoundFX.Instance.PlaySFX("Slash_Hit");

                CameraFollow.Instance.StartShaking();

                if(myStatus.gauge <= myInfo.myStatus.gauge)
                    myStatus.gauge += myInfo.myAttribute.intellegence * 2f;
            }
            else
            {
                SoundFX.Instance.PlaySFX("Slash");
                //myAudioSource.clip = myAudioClip[0];
            }

            //myAudioSource.Play();

            attackTime = 0;

            attackState++;

            if (attackState > 2)
            {
                attackState = 0;
            }
        }
    }

    public void Move()
    {
        if (stunTime >= 0.25f)
        {
            //mSpeed = Input.GetAxis("Horizontal");
            mSpeed = Control.direction;

            if (mSpeed != 0)
            {
                characterAnimator.SetBool("isRunning", true);
                ac.enabled = true;
            }
            else
            {
                characterAnimator.SetBool("isRunning", false);
                ac.enabled = false;
            }

            if (mSpeed > 0f)
            {
                facing = 1;
                SetFacing();
            }
            else if (mSpeed < 0f)
            {
                facing = -1;
                SetFacing();
            }

        
            characterRb.transform.Translate(new Vector2(mSpeed * 3 * Time.fixedDeltaTime, 0));

        }
            //characterRb.velocity = new Vector2(mSpeed * 3, characterRb.velocity.y);
    }

    public void Jump()
    {
        if (jumpHit.collider != null)
        {
            characterAnimator.SetTrigger("doJump");
            characterRb.AddForce(Vector2.up * 540, ForceMode2D.Force);
            airTime = 0;
            Debug.Log("Jump");
        }
    }

    public void Damaged(float inDamage, float dir)
    {
        if (stunTime >= 0.25f)
        {
            facing = -(int)dir;
            SetFacing();
            characterAnimator.SetTrigger("doDamaged");
            characterRb.AddForce(new Vector2(dir * 400, 0), ForceMode2D.Force);
            stunTime = 0;
            myStatus.health -= inDamage;
            CameraFollow.Instance.StartShaking();
        }
    }

    public void Damaged(float inDamage)
    {
        if (stunTime >= 0.25f)
        {
            SetFacing();
            characterAnimator.SetTrigger("doDamaged");
            characterRb.AddForce(new Vector2(-facing * 400, 0), ForceMode2D.Force);
            stunTime = 0;
            myStatus.health -= inDamage;
            CameraFollow.Instance.StartShaking();
        }
    }

    public void Death()
    {
        if (myStatus.health <= 0)
        {
            ResultScript.Instance.SetGameOver();
            Character.Instance.InstantiateDeath();
            Destroy(character);
        }
    }
    #endregion

    void SetRay()
    {
        jumpHit = Physics2D.Raycast(
                    new Vector3(character.transform.position.x,
                    character.transform.position.y + 0.2f, 0),
                    Vector2.down, 0.2f, 1 << 8);

        attackHit = Physics2D.Raycast(
                    new Vector3(character.transform.position.x,
                    character.transform.position.y + 0.5f, 0),
                    new Vector2(facing, 0), hitRange, 1 << 9);
    }

    void SetDrop()
    {
        if (characterRb.velocity.y < 0)
        {
            characterRb.AddForce(Vector2.down * 40, ForceMode2D.Force);
        }
    }

    void SetFacing()
    {
        character.transform.localScale = new Vector3(
        facing,
        character.transform.localScale.y,
        character.transform.localScale.z);
    }

    public float getHealth()
    {
        return myStatus.health / myInfo.myStatus.health;
    }

    public float getGauge()
    {
        return myStatus.gauge / myInfo.myStatus.gauge;
    }

    public void SetTimer()
    {
        if (stunTime <= 0.5f)
            stunTime += Time.fixedDeltaTime;

        if (airTime <= 0.5f)
            airTime += Time.fixedDeltaTime;

        if (attackTime <= 0.12f)
            attackTime += Time.fixedDeltaTime;
    }

    public void SetMethods()
    {
        SetTimer();
        SetRay();
        SetDrop();
        SetFacing();

        Death();
        Move();

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Swipe.Instance.IsSwiping(SwipeDirection.Up) && mousePos.x > threshold.x && mousePos.y > threshold.y)
            Jump();

        if(Input.GetKeyUp(KeyCode.Space))
            Attack();

        //if(Input.GetKey(KeyCode.UpArrow))
        //    Jump();

        if (myStatus.gauge > myInfo.myStatus.gauge)
            myStatus.gauge = myInfo.myStatus.gauge;

        if (myStatus.health > myInfo.myStatus.health)
            myStatus.health = myInfo.myStatus.health;
    }

    float asihTimeDelay;

    public void StartSkillAsih()
    {
        if (myStatus.gauge > 0)
        {
            if (!asihParticle.isPlaying)
            {
                asihTimeDelay = 0;
                asihParticle.Play();
            }
            else
            {
                if(asihTimeDelay <= 1.4f)
                    asihTimeDelay += Time.fixedDeltaTime;
            }
            Debug.Log(asihTimeDelay + "" + asihParticle.isPlaying);
            if (asihTimeDelay >= 1.3f)
            {
                asihCollider.SetActive(true);
                myStatus.gauge -= ((myInfo.myAttribute.intellegence / 2) * 1.2f) * Time.deltaTime;
            }
        }
        else
        {
            StopSkillAsih();
        }
    }

    public void StopSkillAsih()
    {
        asihParticle.Stop();
        asihCollider.SetActive(false);
    }

    public void StartSkillAsuh()
    {
        if (myStatus.health < myInfo.myStatus.health && myStatus.gauge > 0)
        {
            if(!asuhParticle.isPlaying)
                asuhParticle.Play();

            myStatus.health += ((myInfo.myAttribute.intellegence / 2) * 4f) * Time.deltaTime;
            myStatus.gauge -= ((myInfo.myAttribute.intellegence / 2) * 1.5f) * Time.deltaTime;
        }
        else
        {
            StopSkillAsuh();
        }
    }

    public void StopSkillAsuh()
    {
        asuhParticle.Stop();
    }

    public void StartSkillAsah()
    {
        if (Character.Instance != null && myStatus.gauge >= 30)
        {
            SoundFX.Instance.PlaySFX("Fireball");
            myStatus.gauge -= 20;
            Instantiate(Character.Instance.fireball.gameObject, Character.Instance.transform.position, Quaternion.identity);
        }
    }
}