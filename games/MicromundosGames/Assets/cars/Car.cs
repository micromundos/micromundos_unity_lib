using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

	public int id;
	public states state;
	public MoveObject move;
	public TurnObject turnObject;
	public RotatingLoop rotatingLoop;
	public GameObject targetRight;
	public GameObject targetLeft;

	public enum states
	{
		MOVE,
		IDLE,
		ROTATING
	}
	public void SetID(int id)
	{
		GetComponent<Colorate> ().SetOn (id);
		this.id = id;
	}
	void Update () {
		CheckHits ();
		if (state == states.ROTATING)
			turnObject.OnUpdate ();
		else if (state == states.MOVE)
			move.OnUpdate();
		if (Mathf.Abs (transform.position.x) > 15 || Mathf.Abs (transform.position.y) > 15)
			Game.Instance.RemoveCar (this);
	}
	void OnTriggerEnter2D(Collider2D other)
	{				
		Flag flag = other.GetComponent<Flag> ();
		if (flag != null)
			flag.SetOn (id);
		
		ActivateOnTouch activateOnTouch = other.GetComponent<ActivateOnTouch> ();
		if (activateOnTouch != null)
			activateOnTouch.SetOn ();

		Bomb bomb = other.GetComponent<Bomb> ();
		Turn turn = other.GetComponent<Turn> ();
		RotationArrow rotationArrow = other.GetComponent<RotationArrow> ();

		if (turn != null) {	
			transform.position = other.transform.position;
			Vector3 rot = other.transform.eulerAngles;
			turnObject.Init (rot.z);
			RotateTo ();
		} else if (rotationArrow != null) {
			rotatingLoop.speed = rotationArrow.speed;
			rotatingLoop.enabled = true;
		} else if (bomb != null && bomb.carID== id)
			Game.Instance.RemoveCar (this);
	}
	void OnTriggerExit2D(Collider2D other)
	{		
		RotationArrow rotationArrow = other.GetComponent<RotationArrow> ();
		if (rotationArrow != null) {
			rotatingLoop.enabled = false;
		}
	}
	public void OnStateDone()
	{
		state = states.MOVE;
	}
	void RotateTo()
	{
		move.SetSpeed (0.7f);
		state = states.ROTATING;
	}
	bool madeFirstCheck;
	void CheckHits()
	{
		bool isAvailablePoint = MicromundosManager.Instance.isPointBlocked (transform.position);
		//print (madeFirstCheck + " isBlocked: " + isAvailablePoint);
		if (madeFirstCheck == false && !isAvailablePoint) {
			Game.Instance.RemoveCar (this);
			return;
		}
		madeFirstCheck = true;

		bool rightIsAvailable = MicromundosManager.Instance.isPointBlocked (targetRight.transform.position);
		bool leftIsAvailable = MicromundosManager.Instance.isPointBlocked (targetLeft.transform.position);

		if (!rightIsAvailable)
			Turn (true);
		else if (!leftIsAvailable)
			Turn (false);
	}
	void Turn(bool turnLeft)
	{
		move.ResetSpeed (0.5f);
		Vector3 rot = transform.localEulerAngles;
		if (turnLeft)
			rot.z += 4;
		else
			rot.z -= 4;
		transform.localEulerAngles = rot;
	}
}
