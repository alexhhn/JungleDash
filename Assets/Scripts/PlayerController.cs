using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float jumpPower;
	public AudioClip hitSound;
	public AudioClip loseSound;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	private int count;				//Integer to store the number of pickups collected so far.
	private AudioSource source;
	private float volLowRange = .5f;
		private float volHighRange = 1.0f;

	private bool isFlipped;
	private float yPosition;
	private bool dead;
	public int point;
	public Text textPoint;

	// jumping animation variables
	public Transform groundCheckTransform;
	Animator animator;
	private bool grounded;
	public LayerMask groundCheckLayerMask;

	// UI variables
	public GameObject feedbackPanel;
	public Text feedbackText;
	public GameObject gameOverBoard;
	public Text gameOvetText;
	public Text killedMonsterText;


	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
		yPosition = 0;
		isFlipped = false;
		animator = GetComponent<Animator>();
		grounded = true;
		gameOverBoard.SetActive(false);
		source = GetComponent<AudioSource> ();

		// disable the feedback panel when the game starts
		feedbackPanel.SetActive (false);
		feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (0.1f, 0f, false);

		point = 0;
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		UpdateGroundedStatus();

		// Player moves forward automatically
		if(GameController.instance.runSpeed < 18) {
			GameController.instance.runSpeed = GameController.instance.runSpeed * 1.001f;
		}

		else if(GameController.instance.runSpeed > 18 && GameController.instance.runSpeed < 25) {
			GameController.instance.runSpeed = GameController.instance.runSpeed * 1.0001f;
		}


		rb2d.velocity = new Vector2(GameController.instance.runSpeed, rb2d.velocity.y);



		textPoint.text = point.ToString();
		killedMonsterText.text = point.ToString();
		GameOver();

	}

	void Update() {



		// Player is jumping
	bool isJumping = Input.GetKey(KeyCode.Space);
	if (isJumping && grounded) {
		if (isFlipped) {
			rb2d.velocity = new Vector2 (0, jumpPower * -1);
		} else {
			rb2d.velocity = new Vector2 (0, jumpPower);
		}
	}

	if (!GameController.instance.gameOver) {
		Flip ();
	}
	}


	void GameOver() {
		if(GameController.instance.gameOver == true) {
			killedMonsterText.text = point.ToString();
			rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
			gameOverBoard.SetActive(true);

			if(GameController.instance.playerWon == true) {
				gameOvetText.text = "Y o u  w i n !!";
			} else {
				gameOvetText.text = "Oh No ......";

			}

			if(Input.GetKeyDown (KeyCode.Space)) {
				//FOR PAUSE
				// GameController.instance.gameOver = false;
				// rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
				Application.LoadLevel(Application.loadedLevel);
			}
		} else {
			gameOverBoard.SetActive(false);

		}
	}

	void Flip() {
		if(Input.GetKeyDown(KeyCode.F)) {
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
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.5f, groundCheckLayerMask);
		animator.SetBool("grounded", grounded);
	}


	// called when player collides with any objects
	void OnTriggerEnter2D(Collider2D collider){

		if (collider.gameObject.CompareTag ("Minions")) {
			// trigger only if player is not flying
			if (grounded) {
				animator.SetTrigger ("isAttacking");
			}
			KillMinion (collider);
		}

		else if (collider.gameObject.CompareTag ("Danger"))

		{
			animator.SetTrigger ("isDead");
			StartCoroutine(ShowGameOver(1));
		}

		else if (collider.gameObject.CompareTag ("jumpInst")) {
			feedbackPanel.SetActive (true);
			feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (1, 0.3f, true);
			feedbackText.text = "Press Space to Jump";
			feedbackText.CrossFadeAlpha (1, 0.3f, true);

			StartCoroutine (Wait (3, feedbackPanel, feedbackText));
		}

		else if (collider.gameObject.CompareTag ("attackInst")) {
			feedbackPanel.SetActive (true);
			feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (1, 0.3f, true);
			feedbackText.text = "Run into monsters to kill them and get points";
			feedbackText.CrossFadeAlpha (1, 0.3f, true);

			StartCoroutine (Wait (6, feedbackPanel, feedbackText));
		}

		else if (collider.gameObject.CompareTag ("flipInst")) {
			feedbackPanel.SetActive (true);
			feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (1, 0.3f, true);
			feedbackText.text = "Press F to Flip";
			feedbackText.CrossFadeAlpha (1, 0.3f, true);

			StartCoroutine (Wait (3, feedbackPanel, feedbackText));
		}

		else if (collider.gameObject.CompareTag ("halfInst")) {
			feedbackPanel.SetActive (true);
			feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (1, 0.3f, true);
			feedbackText.text = "You ' re halfway there";
			feedbackText.CrossFadeAlpha (1, 0.3f, true);

			StartCoroutine (Wait (5, feedbackPanel, feedbackText));
		}

		else if (collider.gameObject.CompareTag ("almostThere")) {
			feedbackPanel.SetActive (true);
			feedbackPanel.GetComponent<Image> ().CrossFadeAlpha (1, 0.3f, true);
			feedbackText.text = "Almost there";
			feedbackText.CrossFadeAlpha (1, 0.3f, true);

			StartCoroutine (Wait (5, feedbackPanel, feedbackText));
		}


		else if (collider.gameObject.CompareTag ("Victory")) {
			animator.SetBool ("hasWon", true);
			GameController.instance.gameOver = true;
			GameController.instance.playerWon = true;
		}


		else {
			print ("Collide smt");

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
		float vol = Random.Range (volLowRange, volHighRange);
		source.PlayOneShot(hitSound,vol);
		point++;
	}

	IEnumerator Wait(float duration, GameObject gameObject, Text text)
	{
		//This is a coroutine
		yield return new WaitForSeconds(duration);   //Wait
		gameObject.GetComponent<Image>().CrossFadeAlpha(0, 0.5f, true);
		text.CrossFadeAlpha (0, 0.5f, true);

	}

	IEnumerator ShowGameOver(float duration)
	{
		//This is a coroutine
		yield return new WaitForSeconds(duration);   //Wait
		GameController.instance.PlayerDied();

	}

}
