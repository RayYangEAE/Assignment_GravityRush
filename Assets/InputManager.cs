using System;
using System.Collections;
using UnityEngine;
using CrossPlatformInput;


namespace CrossPlatformInput
{
	public class InputManager : MonoBehaviour
	{
		public float speed = 1.0f;
		public float jumpspeed = 1.0f;
		public float jumpcd = 1.0f;
		private bool jumpbool = true;
		private int jumpcount = 0;
		private bool keepdrop = false;
		private float x;
		private float z;
		private float Rotatex;
		private float Rotatez;
		private bool changeGbool = true;
		public float changeGcd = 5.0f;
		private int changeGcount = 0;
		private bool rotateGbool = false;
		public float rotateSpeed = 1.0f;
		private Vector3 currentPosition;
		private Rigidbody Rigidbodyref;
		private gravityonYaxis ScriptGravityYaxis;
		private SnapToGround ScriptSnapToGround;
		private Vector3 SnapDirection;
		private float Angle_normal_up;
		private float MouseXMove;

		void Start()
		{
			Rigidbodyref = this.gameObject.GetComponent<Rigidbody> ();
			ScriptGravityYaxis = this.gameObject.GetComponent<gravityonYaxis> ();
			ScriptSnapToGround = this.gameObject.GetComponent<SnapToGround> ();
			Rigidbodyref.constraints = RigidbodyConstraints.FreezeRotation;
		}

		IEnumerator waitjumpcd() {
			jumpbool = false;
			yield return new WaitForSeconds(jumpcd);
			jumpbool = true;
		}

		IEnumerator waitchangeGcd() {
			changeGbool = false;
			yield return new WaitForSeconds(changeGcd);
			changeGbool = true;
		}

		//脱离地面，移向上方
		IEnumerator detachGround() {
			Rigidbodyref.useGravity= false;
			Rigidbodyref.velocity= transform.up*6;
			yield return new WaitForSeconds(0.3f);
			Rigidbodyref.velocity=Vector3.zero;
			Rigidbodyref.detectCollisions = false;
		}
			
		void JumpAddForce()
		{
			Rigidbodyref.AddForce(transform.up*jumpspeed);
		}

		//凡是碰撞，表示jump完成
		void OnCollisionEnter(Collision collision) {
			keepdrop = false;
		}

		//取得人物与地面normal角度，角度太大人物将不再snap在地面上
		void OnCollisionStay(Collision collision)
		{
			if ((!keepdrop) && (changeGcount == 0))
			{
				SnapDirection = ScriptSnapToGround.GetSurfaceNormal(Rigidbodyref);
				Angle_normal_up = Vector3.Angle (SnapDirection,transform.up);
				//Debug.Log (SnapDirection);
				//Debug.DrawRay(Rigidbodyref.position, -SnapDirection*300, Color.red);
				if ((Angle_normal_up <= 55) && (Angle_normal_up >= -55))
					Rigidbodyref.velocity = -SnapDirection * 1;
			}
		}

		void jumpEvent()
		{
			if (Input.GetAxis ("Jump") > 0) {
				if (jumpbool) {
					jumpcount++;
					if (jumpcount <= 2) {
						//Debug.Log (jumpcount);
						Rigidbodyref.velocity = Vector3.zero;
						JumpAddForce ();
						keepdrop = true;
						StartCoroutine (waitjumpcd ());
					}
				}
			}
			if (!keepdrop)
			{
				jumpcount = 0;
			}
		}

		void movementEvent()
		{
			if (changeGcount != 1) 
			{
				z = Input.GetAxis ("Vertical") * Time.deltaTime * speed;
				x = Input.GetAxis ("Horizontal") * Time.deltaTime * speed;
				transform.Translate (x, 0.0f, z);
			}
		}
			
		//按1下q，漂浮，改重力方向; 按第二次，落下。 
		void changeG()
		{
			if (Input.GetKeyDown("q"))
			{
				if (changeGbool) {
					changeGcount++;
					if (changeGcount == 1) 
					{
						rotateGbool = true;
						SnapDirection = Vector3.zero;
						keepdrop = true;
						StartCoroutine (detachGround());
					}
					if (changeGcount == 2) 
					{
						changeGcount = 0;
						rotateGbool = false;
						Rigidbodyref.detectCollisions = true;
						Rigidbodyref.useGravity= true;
						ScriptGravityYaxis.ChangeGravity();
						StartCoroutine (waitchangeGcd ());
					}
				}
			}
		}

		void changeGDirection()
		{
			if (rotateGbool) 
			{
				Rotatez = 5*Input.GetAxis ("MouseX")*rotateSpeed*Time.deltaTime;
				Rotatex = 5*Input.GetAxis ("MouseY")*rotateSpeed*Time.deltaTime;
				transform.Rotate(Rotatex , 0, Rotatez);
			}
		}

		void changeFacing()
		{
			if (Input.GetMouseButton (1)) {
				MouseXMove = Input.GetAxis ("MouseX");
				if ((MouseXMove != 0) && (changeGcount == 0)) {
					Rigidbodyref.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
					transform.Rotate (0, 5 * MouseXMove * rotateSpeed * Time.deltaTime, 0);
				}
			} else 
			{	
				Rigidbodyref.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}

		void Update()
		{
			movementEvent ();
			jumpEvent ();
			changeG ();
			changeGDirection ();
			changeFacing ();
		}
			
	}
}
