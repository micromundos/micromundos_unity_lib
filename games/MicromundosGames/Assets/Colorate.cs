using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorate : MonoBehaviour {

	public SpriteRenderer[] all;
	public TrailRenderer tr;

	public void SetOn(int id) {
		Color color = Game.Instance.GetColor (id);
		foreach (SpriteRenderer sr in all) {
			sr.color = color;
		}

		if (tr != null) {
			tr.enabled = false;
			Invoke ("Delayed", 0.2f);
			color.a = 1;
			tr.startColor = color;
			color.a = 0;
			tr.endColor = color;
		}
		
	}
	void Delayed()
	{
		tr.enabled = true;
	}
}
