using UnityEngine;
using System.Collections;

public class SnapToGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 GetSurfaceNormal(Rigidbody PlayerRigid)
	{
		RaycastHit hitGround;
		Vector3 hitGroundDirection = -PlayerRigid.transform.up;
		if (Physics.Raycast (PlayerRigid.position, hitGroundDirection, out hitGround)) {
			return hitGround.normal;
		} else {
			return Vector3.zero;
		}
	}
}
