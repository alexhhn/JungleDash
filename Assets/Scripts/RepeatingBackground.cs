using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	private BoxCollider2D groundCollider;
	private float groundHorizontalLength;
	private float width;


	// Use this for initialization
	void Start () {
		width = GetComponent<SpriteRenderer>().bounds.size.x;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.x < -width) {
			RePositionBackground ();
		}

	}

	private void RePositionBackground () {
		Vector2 groundOffset = new Vector2(width * 2f,0);
		transform.position = (Vector2)transform.position + groundOffset;
	}
}
