using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    bool facingRight = false;
	bool canMove = true;
	int timer = 30;
    private Rigidbody2D rb;
    public float speed = 15f;
	public int health = 8;
	private int startHealth;
    public float jumpForce = 175f;
    private float maxSpeed = 3f;
    private bool canJump = false;
	float myWidth;
	public float invincibleTimeAfterHurt = 2;
	public LayerMask playerMask;
	Transform myTrans;
    Transform playerGraphics; // reference to graphics for changing direction
    Animator animator; // reference to the character animator

	public Sprite halfBone;
	public Sprite fullBone;
	public Image[] bones;

	public UnityEvent zeroHealth;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		startHealth = health;
        playerGraphics = transform.FindChild("Graphics"); // get graphics object
        if(playerGraphics == null){
            Debug.LogError("No Graphics Objects as a child of Player");
        }

		myTrans = this.transform;
		//myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	// Update is called once per frame
	void Update () {
        switch (TheManager.Instance.gameState)
        {
            case GameState.Playing:
                ProcessInput();
                break;
            case GameState.Paused:
            case GameState.Cutscene:
                break;
            default:
                break;
        }
    }

    void ProcessInput()
    {
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

		switch (health) {
		case 8:
			bones [0].enabled = bones [1].enabled = bones [2].enabled = bones [3].enabled = true;
			bones [0].sprite = bones [1].sprite = bones [2].sprite = bones[3].sprite = fullBone;
			break;
		case 7:
			bones [0].enabled = bones [1].enabled = bones [2].enabled = bones [3].enabled = true;
			bones [0].sprite = bones [1].sprite = bones [2].sprite = fullBone;
			bones [3].sprite = halfBone;
			break;
		case 6:
			bones [0].enabled = bones [1].enabled = bones [2].enabled = true;
			bones [3].enabled = false;
			bones [0].sprite = bones [1].sprite = bones [2].sprite = fullBone;
			break;
		case 5:
			bones [0].enabled = bones [1].enabled = bones [2].enabled = true;
			bones [3].enabled = false;
			bones [0].sprite = bones [1].sprite = fullBone;
			bones [2].sprite = halfBone;
			break;
		case 4:
			bones [0].enabled = bones [1].enabled = true;
			bones [2].enabled = bones [3].enabled = false;
			bones [0].sprite = bones [1].sprite = fullBone;
			break;
		case 3:
			bones [0].enabled = bones [1].enabled = true;
			bones [2].enabled = bones [3].enabled = false;
			bones [0].sprite = fullBone;
			bones [1].sprite = halfBone;
			break;
		case 2:
			bones [0].enabled = true;
			bones[1].enabled = bones [2].enabled = bones [3].enabled = false;
			bones [0].sprite = fullBone;
			break;
		case 1:
			bones [0].enabled = true;
			bones[1].enabled = bones [2].enabled = bones [3].enabled = false;
			bones [0].sprite = halfBone;
			break;
		}
    }

	void FixedUpdate(){
		//Check to see if there is ground in front of us before moving
		Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * 0 + Vector2.up * -0.15f;
		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down * 0.35f);
		bool isGrounded = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down * 0.35f, playerMask);

		if (isGrounded) {
			canJump = true;
		} else {
			canJump = false;
		}
	}

	void Hurt(int damage){

		health -= damage;

		if (health <= 0) {
			zeroHealth.Invoke ();
			health = startHealth;
		}

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
		switch (col.collider.transform.name) {
			case "enemy1":
				Enemy enemy = col.collider.GetComponent<Enemy> ();
				Hurt (enemy.damage);
				break;
			case "dogbowl":
				HealPlayer (8);
				Destroy (col.gameObject);
				break;
			case "treat":
				HealPlayer (4);
				Destroy (col.gameObject);
				break;
			case "bone":
				HealPlayer (2);
				Destroy (col.gameObject);
				break;
			default:
				break;
		}
    }

	void OnTriggerStay2D(Collider2D col){
		//stop completely and attack if touching the player
		if (col.transform.tag == "Enemy" && col.GetComponent<Enemy>().isAttacking) {
			Hurt (2);
		}
	}

    void Flip (){
        // change the way the player is facing
        facingRight = !facingRight;

        Vector3 theScale = playerGraphics.localScale;
        theScale.x *= -1;
        playerGraphics.localScale = theScale;
    }

	void HealPlayer (int healAmt)
	{
		health += healAmt;

		if (health > startHealth)
		{
			health = startHealth;
		}
	}
}
