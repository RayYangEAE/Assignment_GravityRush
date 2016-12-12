using UnityEngine;
using System;
using System.Collections;

public class HideBlockTransparent : MonoBehaviour {

	private Vector3 CameraDir;
	RaycastHit[] hit;
	private GameObject[] LastHitObject;
	private GameObject[] HitObject;
	private int HitNum;
	private int MatNum;
	private int TotalMatNum;
	private int HitRealNum;
	private int[] LastHitMatIndex;
	private String[,] LastShaderString;


	void Start () {
		LastHitObject = new GameObject[10];
		HitObject = new GameObject[10];
		LastHitMatIndex = new int[10];
		LastShaderString = new string[10,10];

	}

	void SetMaterialTransparency(GameObject HitObj){

		Renderer HitRenderer = HitObj.GetComponent<Renderer> ();
		TotalMatNum = HitRenderer.sharedMaterials.Length;

		for (MatNum = 0; MatNum < TotalMatNum; MatNum++) {
			HitRenderer.materials [MatNum].shader = Shader.Find("Custom/DoubleSided_Transparent");
			Color tempColor = HitRenderer.materials[MatNum].color;
			tempColor.a *= 0.4f;
			HitRenderer.materials[MatNum].color = tempColor;
		}

	}

	void ClearMaterialTransparency(GameObject LastHitObj){

		Renderer LastHitRenderer = LastHitObj.GetComponent<Renderer> ();

		for (MatNum = 0; MatNum < TotalMatNum; MatNum++) 
		{
			LastHitRenderer.materials [MatNum].shader = Shader.Find (LastShaderString [HitNum, MatNum]);
			LastShaderString [HitNum, MatNum] = null;
			Color tempColor = LastHitRenderer.materials [MatNum].color;
			tempColor.a *= 2.5f;
			LastHitRenderer.materials [MatNum].color = tempColor;
			//Debug.Log (LastHitObj.GetComponent<Renderer> ().materials [MatNum].shader);
		}

	}
		
	bool NotExistsInArray(int HitIndex, GameObject TempHitObj){
		bool NotExist = true;
		for (int i = 0; i < HitIndex; i++) {
			if (hit [i].collider.gameObject == TempHitObj) {
				NotExist = false;
			}
			Debug.Log (hit [i].collider.gameObject);
		}
		return NotExist;
	}

	void Update () {
		CameraDir = -transform.forward;
		//Debug.DrawLine (transform.parent.gameObject.transform.position, transform.position, Color.red);
		hit = Physics.RaycastAll (transform.parent.gameObject.transform.position, CameraDir, transform.localPosition.magnitude);
		for (HitNum = 0; HitNum < 10; HitNum++) {
			if (LastHitObject [HitNum] != null) {
				ClearMaterialTransparency (LastHitObject [HitNum]);
				LastHitObject [HitNum] = null;
			}
		}
		for (HitNum = 0; HitNum < hit.Length; HitNum++) {

			if ((hit [HitNum].collider.gameObject.tag != "Ground") &&
			    (hit [HitNum].collider.gameObject.tag != "Player") &&
				(hit [HitNum].collider.gameObject.tag != "Star") &&
				NotExistsInArray(HitNum, hit [HitNum].collider.gameObject)) {
				HitObject [HitNum] = hit [HitNum].collider.gameObject;
			} else
				HitObject [HitNum] = null;
			if (HitObject [HitNum] != null) {
				LastHitObject [HitNum] = HitObject [HitNum];
				for (int i = 0; i < LastHitObject [HitNum].GetComponent<Renderer>().sharedMaterials.Length; i++) {
					LastShaderString [HitNum, i] = LastHitObject [HitNum].GetComponent<Renderer> ().materials [i].shader.name;
					//Debug.Log (LastShaderString [HitNum, i]);
				}
				//Debug.Log (LastRenderer[HitNum].materials [0].color.a);
				SetMaterialTransparency (HitObject [HitNum]);
			}
			HitObject [HitNum] = null;
		}

	}
}
