using UnityEngine;
using System.Collections;

public enum EnemyState{
	Patrol,
	Chase,
	Attack,
	Hit,
	Stunned,
	Death
}

public class Enemy : MonoBehaviour {

	EnemyState current = EnemyState.Patrol;
	float patrolSpeed = 2f;
	float chaseSpeed = 2.75f;
	float myWidth, myHeight;
	int viewDistance = 4; //how far the enemy can see
	int attackDistance = 1;
	int health = 8;
	public int damage = 1;
	Transform player;
	public LayerMask enemyMask;
	Rigidbody2D myBody;
	Transform myTrans;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
		myBody = this.GetComponent<Rigidbody2D>();
		SpriteRenderer mySprite = this.GetComponent<SpriteRenderer> ();
		myWidth = mySprite.bounds.extents.x;
		myHeight = mySprite.bounds.extents.y;
		myTrans = this.transform;
	}

	// Update is called once per frame
	void FixedUpdate () {

		//Check to see if there is ground in front of us before moving
		Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + Vector2.up * (myHeight - 0.4f);
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
		ChangeState ();
	}

	void Attack(){

	}

	void ChangeState(){

		//Patrol or Chase
		if(Vector2.Distance(myTrans.position,player.position) < viewDistance && current != EnemyState.Death){
			current = EnemyState.Chase;
		} else if(current == EnemyState.Chase && current != EnemyState.Death){
			current = EnemyState.Patrol;
		}

		//Attacking
		if(Vector2.Distance(myTrans.position,player.position) < attackDistance && current != EnemyState.Death){
			current = EnemyState.Attack;
		}

		//Death
		if(health <= 0){
			current = EnemyState.Death;
		}
	}
}
