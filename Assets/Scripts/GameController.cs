﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController: MonoBehaviour{
		public static GameController instance;
		// public Text scoreText;
		// public GameObject gameOverText;
		//
		private int score = 0;
		public bool gameOver;
		public bool playerWon;
		public float runSpeed;	//Floating point variable to store the player's movement speed.


		void Awake() {
			//If we don't currently have a game controll ...
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy (gameObject);
		}

		public void PlayerDied() {
			gameOver = true;
		}
}
