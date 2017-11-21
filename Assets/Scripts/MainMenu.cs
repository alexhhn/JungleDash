using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button startText;
	public Button settingsText;

	// Use this for initialization
	void Start () {
		startText = startText.GetComponent<Button>();
		settingsText = settingsText.GetComponent<Button>();
	}

	public void Play() {
		SceneManager.LoadScene (1);
	}
}
