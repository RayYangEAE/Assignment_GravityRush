using UnityEngine;
using System.Collections;

public class AddScore : MonoBehaviour {

	public int ScoreToWin = 10;
	private int CurrentScore = 0;

	void GetScore() 
	{
		CurrentScore++;
		Debug.Log ("Score: "+ CurrentScore);
		if (CurrentScore >= ScoreToWin) 
		{
			Debug.Log ("Win!");
		}
	}
		
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Star")
		{
			Destroy (collision.gameObject);
			GetScore();
		}	
	}

}
