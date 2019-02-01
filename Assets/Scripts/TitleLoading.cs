using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleLoading : MonoBehaviour {

	InputField inputField;
	static public string inputValue;

	// Use this for initialization
	void Start () {
		inputField = GetComponent<InputField> ();
	}

	public void InputLogger(){
		inputValue = inputField.text;
		print (inputValue);
	
	}

	
	// Update is called once per frame
	void Update () {
		inputValue = inputField.text;
	}
	public void Onclick(){
		SceneManager.LoadScene ("MainScene");

	}

}
