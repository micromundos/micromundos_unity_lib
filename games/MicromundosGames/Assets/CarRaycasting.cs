using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRaycasting : MonoBehaviour {

	Camera cam;
	public types type;


	public enum types
	{
		RIGHT,
		LEFT
	}
	void Start()
	{
		cam = GameObject.Find ("Main Camera").GetComponent<Camera>();
	}
	void Update()
	{
		Vector2 pos = Input.mousePosition; 
		Ray ray = cam.ScreenPointToRay(pos);
		RaycastHit hit;
		Physics.Raycast(cam.transform.position, Vector3.forward, out hit, 10000.0f);
		Color c;
		if(hit.collider != null) {
			Texture2D tex = (Texture2D)hit.collider.gameObject.GetComponent<MeshRenderer>().material.mainTexture; // Get texture of object under mouse pointer
			c = tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y); // Get color from texture
			print(c + " pos: " + pos);
		}
	}


}
