using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed = 15f;
    public float jumpForce = 175f;
    private float maxSpeed = 3f;
    private bool canJump = false;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(Vector2.right * speed);
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(-Vector2.right * speed);
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canJump = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Platform")
        {
            canJump = true;
        }
    }
}
