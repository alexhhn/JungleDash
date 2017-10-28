using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float jumpPower;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	private int count;				//Integer to store the number of pickups collected so far.

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsground;
	private bool onGround;


	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();

	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		// onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsground);

		// Player moves forward automatically
		rb2d.velocity = new Vector2(GameController.instance.runSpeed, rb2d.velocity.y);

	 	// Player is jumping
		bool isJumping = Input.GetKey(KeyCode.Space);
		if(isJumping) {
			rb2d.velocity = new Vector2(0, jumpPower);
		}

		// if (Input.GetKey(KeyCode.Space) && onGround)
		// {
		// 	rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpPower);
		// }
	}
}
