using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    bool facingRight = true;
    private Rigidbody2D rb;
    public float speed = 15f;
    public float jumpForce = 175f;
    private float maxSpeed = 3f;
    private bool canJump = false;

    Transform playerGraphics; // reference to graphics for changing direction
    Animator animator; // reference to the character animator

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerGraphics = transform.FindChild("Graphics"); // get graphics object
        if(playerGraphics == null){
            Debug.LogError("No Graphics Objects as a child of Player");
        }
	}
	
	// Update is called once per frame
	void Update () {
        // moving right
	    if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(Vector2.right * speed);
            if(!facingRight){ // change direction of sprite
                Flip();
            }
        }
        // moving left
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(-Vector2.right * speed);
            if(facingRight){ // change direction of sprite
                Flip();
            }
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        // jumping
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

    void Flip (){
        // change the way the player is facing
        facingRight = !facingRight;

        Vector3 theScale = playerGraphics.localScale;
        theScale.x *= -1;
        playerGraphics.localScale = theScale;
    }
}
