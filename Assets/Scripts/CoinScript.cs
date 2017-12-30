using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

    [SerializeField]
    bool isFollowing;

    Rigidbody2D rb;

    CircleCollider2D cc;
    Vector2 direction;

    float rotateAmount;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        cc.isTrigger = true;
        isFollowing = false;
        rb.AddForce(new Vector2(Random.Range(-1, 2), 1) * 300);
        Invoke("SetFollow", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (isFollowing && Character.Instance != null)
        {
            cc.isTrigger = true;

            rb.gravityScale = 0;

            direction = ((Vector2)Character.Instance.transform.position + new Vector2(0, 0.2f)) - (Vector2)transform.position;

            rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * 300f;

            rb.velocity = transform.up * 5f;
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
            DestroyCoin();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
            DestroyCoin();
    }

    void DestroyCoin()
    {
        GameData.myCoin++;
        SoundFX.Instance.PlaySFX("Ping");
        Destroy(this.gameObject);   
    }

    public void SetFollow()
    {
        isFollowing = true;
    }
}
