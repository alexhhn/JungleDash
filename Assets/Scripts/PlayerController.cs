using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float jumpPower;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	private int count;				//Integer to store the number of pickups collected so far.

	private Collider2D playerCollider;
	private bool isFlipped;
	private float yPosition;
	private bool dead;

	// jumping animation variables
	public Transform groundCheckTransform;
	Animator animator;
	private bool grounded;
	public LayerMask groundCheckLayerMask;

	// UI variables
	public GameObject feedbackPanel;
	public Text feedbackText;


	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
		playerCollider = GetComponent<Collider2D> ();
		yPosition = 0;
		isFlipped = false;
		animator = GetComponent<Animator>();
		grounded = true;

		// disable the feedback panel when the game starts
		feedbackPanel.SetActive (false);
		feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (0.1f, 0f, false);

	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{

		UpdateGroundedStatus();


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


	// called when player collides with any objects
	void OnTriggerEnter2D(Collider2D collider){
		
		if(collider.gameObject.CompareTag("Minions")){
			// trigger only if player is not flying

//			animator.SetTrigger("isAttacking");
			KillMinion(collider);
		}

		if(collider.gameObject.CompareTag("Danger")){
			// trigger only if player is not flying
			animator.SetBool("isDead", true);
			print ("meet a danger");
		}

		if(collider.gameObject.CompareTag("jumpInst")) {
			feedbackPanel.SetActive (true);
			feedbackPanel.GetComponent<Image>().CrossFadeAlpha(1, 0.3f, true);
			feedbackText.text = "Press Space to Jump";
			StartCoroutine(Wait (2,feedbackPanel,feedbackText));
		
		}
	}


	// called when player exist collision with any objects
	void OnTriggerExit2D(Collider2D collider){
		if(collider.gameObject.CompareTag("Minions")){
			Destroy(collider.gameObject);

		}
	}

	void KillMinion(Collider2D minionCollider){
		// Dont need to destroy, just play die animation
		Animator minionAnimator = minionCollider.gameObject.GetComponent<Animator>();
		minionAnimator.SetTrigger("isAttacked");
	}

	IEnumerator Wait(float duration, GameObject gameObject, Text text)
	{
		//This is a coroutine
		yield return new WaitForSeconds(duration);   //Wait
		gameObject.GetComponent<Image>().CrossFadeAlpha(0, 0.5f, true);
		text.CrossFadeAlpha (0, 0.5f, true);

	}

}
