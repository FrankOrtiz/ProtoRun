using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Keep variables sepperated by datatype vs unity component
		// Variables in alphabetical order
	// leave useful values as public for debugging
	public bool canDash;
	public bool canWallJump;
	public float currentSpeed;
	public float dashSpeed;
	public float gravForce;
	public float groundCheckRadius;
	public bool isGrounded;
	public float jumpSpeed;
	public float moveSpeed;
	public bool isOnWall;

	public Transform groundCheck;
	private Rigidbody2D rigidbody;
	private RigidbodyConstraints2D startingConstraints;
	public LayerMask whatIsGround;
	private Animator animator;

	void Awake () {

	}

	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		currentSpeed = moveSpeed;
		startingConstraints = rigidbody.constraints;
	}	
	// Update is called once per frame
	void Update () {

		isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);

		if (isGrounded) {
			currentSpeed = moveSpeed;
			isOnWall = false;
		}

		if (isOnWall) {
			currentSpeed = 0;
			canDash = false;
			rigidbody.velocity = new Vector2(moveSpeed, 0f);
			gravForce = 1;
		}
						
		if(gravForce < 1) gravForce += Time.deltaTime*2;
		if(gravForce > 1) gravForce = 0.6f;
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y - gravForce);
		if(rigidbody.velocity.y > 70) rigidbody.velocity = new Vector2(rigidbody.velocity.x, 70);

		AutoRunHandler ();
		JumpHandler ();
		DashHandler ();

		animator.SetBool ("On Wall", isOnWall);
		animator.SetBool ("Grounded", isGrounded);

	}

	void OnCollisionExit2D(Collision2D collision){

		if (collision.gameObject.tag == "Wall") {
			isOnWall = false;
			canWallJump = false;
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

		if (collision.gameObject.tag == "Wall") {
			transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
			if (!isGrounded) {
				currentSpeed = 0;
				isOnWall = true;
				canWallJump = true;
			}
		}
	}

	void AutoRunHandler(){
		if (transform.localScale.x > 0) // When facing right, move right
		{
			rigidbody.velocity = new Vector3 (currentSpeed, rigidbody.velocity.y, 0f);
		} 
		else if (transform.localScale.x < 0) // When facing left, move left
		{
			rigidbody.velocity = new Vector3 (-currentSpeed, rigidbody.velocity.y, 0f);
		} else {
			rigidbody.velocity = new Vector3 (0, rigidbody.velocity.y, 0f);
		}
	}

	void JumpHandler(){
		
		if (Input.GetButtonDown ("Jump") && (isGrounded || isOnWall)) {
			currentSpeed = moveSpeed;
			isOnWall = false;
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
			canDash = true;
		}
	}

	void DashHandler(){

		if (Input.GetButtonDown ("Jump") && !isGrounded && canDash && !canWallJump) {
			animator.SetTrigger ("Dash");
			canDash = false;
		}

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("bug_dash")) {
			currentSpeed = dashSpeed;
			rigidbody.mass = 0;
			rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		} else {
			currentSpeed = moveSpeed;
			rigidbody.mass = 1;
			rigidbody.constraints = startingConstraints;
		}
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Kill Plane"){
			gameObject.SetActive (false);
		}		
	}
}


// TODO
	// use ground check radius for canDash ***=bug=*** dashes on floor if second jump is delayed
