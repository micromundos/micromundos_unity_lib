using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorate : MonoBehaviour {

	public SpriteRenderer[] all;

	public void SetOn(int id) {
		Color color = Game.Instance.GetColor (id);
		foreach (SpriteRenderer sr in all) {
			sr.color = color;
		}
	}
}
