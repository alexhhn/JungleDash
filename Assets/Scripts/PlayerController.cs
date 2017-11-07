﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float jumpPower;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	private int count;				//Integer to store the number of pickups collected so far.

	public LayerMask GetGround;
	// private bool onGround;
	private Collider2D playerCollider;
	private bool isFlipped;
	private float yPosition;

	// jumping animation variables
	public Transform groundCheckTransform;
	Animator animator;
	private bool grounded;
	public LayerMask groundCheckLayerMask;



	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
		playerCollider = GetComponent<Collider2D> ();
		yPosition = 0;
		isFlipped = false;
		// onGround = false;
		animator = GetComponent<Animator>();
		grounded = true;

	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{

		UpdateGroundedStatus();

		// onGround = playerCollider.IsTouchingLayers(GetGround);

		// Player moves forward automatically
		rb2d.velocity = new Vector2(GameController.instance.runSpeed, rb2d.velocity.y);


	 	// Player is jumping
		bool isJumping = Input.GetKeyDown(KeyCode.Space);
		if (isJumping && grounded) {
			if (isFlipped) {
				rb2d.velocity = new Vector2 (0, jumpPower * -1);
			} else {
				rb2d.velocity = new Vector2 (0, jumpPower);
			}
		}

		if (grounded) {
			Flip ();
		}
	}


	void Flip() {
		if(Input.GetKeyDown(KeyCode.S)) {
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1,transform.localScale.z);
			if(isFlipped) {
				yPosition = 9.37f;
				isFlipped = false;
			}  else {
				yPosition = 4.0f;
				isFlipped = true;
			}
			transform.position = new Vector3(transform.position.x, yPosition,transform.position.z);

			rb2d.gravityScale = rb2d.gravityScale * -1;
		}
	}

	void UpdateGroundedStatus() {
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
		animator.SetBool("grounded", grounded);
	}

}
