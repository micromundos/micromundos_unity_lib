using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnObject : MonoBehaviour {

	[HideInInspector]
	public float rotateTo;
	public float speedToRotate;
	public float speedToAdvance; 

	void Start () {
		
	}
	bool turnLeft;
	public void Init(float _rotateTo)
	{		
		if (_rotateTo > (transform.eulerAngles.z + 180) || ((transform.eulerAngles.z + 180) > 360) ) {
			turnLeft = true;
		} else {
			turnLeft = false;
		}

		this.rotateTo = _rotateTo;
	}
	public void OnUpdate()
	{
		if (Mathf.Abs (transform.localEulerAngles.z - rotateTo) < 2)
			SendMessage ("OnStateDone", SendMessageOptions.DontRequireReceiver);
		else {
			transform.localEulerAngles = Vector3.Lerp (transform.localEulerAngles, new Vector3 (0, 0, rotateTo), 0.1f);
//			Vector3 rot = transform.localEulerAngles;
//			if(turnLeft)
//				rot.z -= speedToRotate*Time.deltaTime;
//			else
//				rot.z += speedToRotate*Time.deltaTime;
//			transform.localEulerAngles = rot;
			//transform.Translate ((Vector3.up * speedToAdvance *Time.deltaTime) * Time.deltaTime, Space.Self);	

		}
	}
}
