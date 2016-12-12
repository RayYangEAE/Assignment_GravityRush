using UnityEngine;
using System.Collections;

public class gravityonYaxis : MonoBehaviour {
	public float gravitych = 9.8f;
	//private CrossPlatformInput.InputManager ScriptInputManager;

    void Start () {
		ChangeGravity ();
	}

	void Update () {
	}

	public void ChangeGravity()
	{
		Physics.gravity = -transform.up * gravitych;
	}
		
}
