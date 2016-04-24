using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    bool facingRight = false;
	bool canMove = true;
	int timer = 30;
    private Rigidbody2D rb;
    public float speed = 15f;
    public float jumpForce = 175f;
    private float maxSpeed = 3f;
    private bool canJump = false;
	float myWidth;
	public float invincibleTimeAfterHurt = 2;
	public LayerMask playerMask;
	Transform myTrans;
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

		myTrans = this.transform;
		//myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	// Update is called once per frame
	void Update () {

        // moving right
	    if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && rb.velocity.x < maxSpeed && canMove)
        {
            rb.AddForce(Vector2.right * speed);
            if(!facingRight){ // change direction of sprite
                Flip();
            }
        }
        // moving left
		if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && rb.velocity.x > -maxSpeed && canMove)
        {
            rb.AddForce(-Vector2.right * speed);
            if(facingRight){ // change direction of sprite
                Flip();
            }
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        // jumping
		if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump && canMove)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canJump = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
		if (!canMove) {
			timer--;
			if (timer <= 0) {
				canMove = true;
				timer = 30;
			}
		}
    }

	void FixedUpdate(){
		//Check to see if there is ground in front of us before moving
		Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * -0.15f + Vector2.up * -0.15f;
		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down * 0.35f);
		bool isGrounded = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down * 0.35f, playerMask);

		if (isGrounded) {
			canJump = true;
		} else {
			canJump = false;
		}
	}

	void Hurt(){
		//Change Health here
		rb.velocity = Vector2.zero;
		canMove = false;
		StartCoroutine(HurtBlinker());
		if (facingRight) { // change direction of sprite
			rb.AddForce (-Vector2.right * 100f);
		} else {
			rb.AddForce (Vector2.right * 100f);
		}
		rb.AddForce(Vector2.up * jumpForce);
	}

	IEnumerator HurtBlinker(){
		//Ignore Enemy Collisions
		int enemyLayer = LayerMask.NameToLayer ("Enemy");
		int playerLayer = LayerMask.NameToLayer ("Player");
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer);

		//Start blinking animation
		animator.SetLayerWeight(1,1);

		//Wait
		yield return new WaitForSeconds(invincibleTimeAfterHurt);

		//Stop animation and let collisions resume
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer,false);
		animator.SetLayerWeight(1,0);
	}

    void OnCollisionEnter2D(Collision2D col)
    {
		Enemy enemy = col.collider.GetComponent<Enemy> ();
		if (enemy != null) {
			Hurt();
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
