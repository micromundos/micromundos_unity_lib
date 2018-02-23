using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinControl : MonoBehaviour {

	public float speed;
	Rigidbody rb;
	public Vector3 dir;

	bool detected;
	public bool onRoad;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		dir = Quaternion.Euler (transform.eulerAngles) * Vector3.left;
		Vector3 mov = dir.normalized * speed * 0.5f;
		transform.position = transform.position + mov;

	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		/*if (Input.GetMouseButtonDown (0)) {
			Vector3 pos =Input.mousePosition;
			pos.z = 10;
			ClientData.Instance.GetObstacleAt (Camera.main.ScreenToWorldPoint(pos));
		}*/

		if (!onRoad) {
			if (!detected && ClientData.Instance.GetObstacleAt (transform.position)) {
				dir = new Vector3 (dir.x * -1, dir.y * -1, 0f);
				transform.Rotate (180, 180, 0);
				//speed *= 1.1f;
				detected = true;
			} else {
				detected = false;
			}
		}

		Vector3 mov = dir.normalized * speed * Time.deltaTime;
		rb.MovePosition (transform.position + mov);


	}

	public void SetDir(Vector3 v){
		transform.eulerAngles = v;
		dir = Quaternion.Euler (transform.eulerAngles) * Vector3.left;
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
