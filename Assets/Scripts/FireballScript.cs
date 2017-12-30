using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {
    Rigidbody2D rb;
    Transform target;

    public float speed = 5f;
    public float rotateSpeed = 200f;

    Vector2 direction;
    float rotateAmount;

	// Use this for initialization
	void Start () {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

        if(targets.Length > 0)
            target = targets[Random.Range(0, targets.Length)].transform;

        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target != null)
        {
            direction = (Vector2)target.position - rb.position;

            direction.Normalize();

            rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            coll.gameObject.GetComponent<AIEnemy>()._myAIBehaviour.Damaged(150);
            Destroy(gameObject);
        }
    }
}
