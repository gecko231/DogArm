using UnityEngine;
using System.Collections;

public enum EnemyState{
	Patrol,
	Chase,
	Attack,
	Hit,
	Stunned,
	Death,
	Wait
}

public class Enemy : MonoBehaviour {

	EnemyState current = EnemyState.Patrol;
	float patrolSpeed = 2f;
	float chaseSpeed = 2.75f;
	float myWidth, myHeight;
	int viewDistance = 4; //how far the enemy can see
	int attackCooldown = 20;
	public bool isAttacking = false;
	int health = 8;
	public int damage = 1;
	Transform player;
	public LayerMask enemyMask;
	Rigidbody2D myBody;
	Transform myTrans;
	Animator myAnim;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
		myBody = this.GetComponent<Rigidbody2D>();
		myAnim = this.GetComponent<Animator> ();
		SpriteRenderer mySprite = this.GetComponentInChildren<SpriteRenderer> ();
		myWidth = mySprite.bounds.extents.x;
		myHeight = mySprite.bounds.extents.y;
		myTrans = this.transform;
	}

	// Update is called once per frame
	void FixedUpdate () {

		//Check to see if there is ground in front of us before moving
		Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * (myWidth - .12f) + Vector2.up * (myHeight - 0.6f);
		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down * 0.35f);
		bool isGrounded = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down * 0.35f, enemyMask);
		bool isBlocked = Physics2D.Linecast (lineCastPos, lineCastPos - myTrans.right.toVector2() * 0.05f, enemyMask);
		Debug.DrawLine (lineCastPos, lineCastPos - myTrans.right.toVector2() * 0.05f);

		if(current == EnemyState.Patrol){
			
			//If there's no ground turn around
			if (!isGrounded || isBlocked) {
				Vector3 currentRotation = myTrans.eulerAngles;
				currentRotation.y += 180;
				myTrans.eulerAngles = currentRotation;
			}

			Vector2 myVel = myBody.velocity;
			myVel.x = -myTrans.right.x * patrolSpeed;
			myBody.velocity = myVel;
		} else if(current == EnemyState.Chase){
			
			Vector3 currentRotation = myTrans.eulerAngles;
			if (player.position.x > myTrans.position.x) {
				currentRotation.y = 180;
			} else {
				currentRotation.y = 0;
			}
			myTrans.eulerAngles = currentRotation;
				
			//If there's no ground stop
			if (!isGrounded || isBlocked) {
				
			} else {
				Vector2 myVel = myBody.velocity;
				myVel.x = -myTrans.right.x * chaseSpeed;
				myBody.velocity = myVel;
			}
		}
		if (current == EnemyState.Attack) {
			myBody.velocity = Vector2.zero;
		}
		ChangeState ();
		if (current != EnemyState.Attack) {
			attackCooldown--;
		}
	}

	IEnumerator Attack(){
		myAnim.SetBool ("Idle",true);
		yield return new WaitForSeconds (0.5f);
		myAnim.SetBool ("Attacking",true);
		yield return new WaitForSeconds (0.1f);
		isAttacking = true;
		yield return new WaitForSeconds (0.2f);
		myAnim.SetBool ("Attacking", false);
		yield return new WaitForSeconds (0.1f);
		isAttacking = false;
		yield return new WaitForSeconds (0.5f);
		myAnim.SetBool ("Idle",false);
		yield return new WaitForSeconds (0.1f);
		current = EnemyState.Patrol;
	}

	void ChangeState(){

		//Patrol or Chase
		if(Vector2.Distance(myTrans.position,player.position) < viewDistance && (current != EnemyState.Death && current != EnemyState.Attack)){
			current = EnemyState.Chase;
		} else if(current == EnemyState.Chase && (current != EnemyState.Death && current != EnemyState.Attack)){
			current = EnemyState.Patrol;
		}

		//Death
		if(health <= 0){
			current = EnemyState.Death;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		//stop completely and attack if touching the player
		if (col.transform.tag == "Player" && current != EnemyState.Death && attackCooldown <= 0) {
			current = EnemyState.Attack;
			myBody.velocity = Vector2.zero;
			StartCoroutine(Attack ());
			Debug.Log ("Attacking");
		}
	}
}
