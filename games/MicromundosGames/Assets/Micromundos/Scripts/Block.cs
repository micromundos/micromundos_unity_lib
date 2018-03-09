using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Block{
	
	public int id; 
	public Vector2 loc; //normalized [0,1]
	public float radio; //vector<ofVec2f> corners; //normalized [0,1]
	public Vector2 dir; //normalized len vec
	public float angle; //radians
	public Vector2 loc_i; //interpolated loc
	public Vector2 dir_i; //interpolated dir
	public float angle_i; //interpolated angle
	public float radio_i; //interpolated radio

}
