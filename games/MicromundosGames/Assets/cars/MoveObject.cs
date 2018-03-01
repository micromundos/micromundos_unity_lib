using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

	public float acceleration;
	float speed = 0;
	public float MaxSpeed;

	void Start () {

	}

	public void OnUpdate()
	{
		speed += acceleration*Time.deltaTime;
		if (speed > speed)
			speed = speed;
		transform.Translate ((Vector3.up * speed) * Time.deltaTime, Space.Self);	
	}
	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
