using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Samples.Helpers;
using SimpleJSON;

[HideInInspector]
public class ServerData : MonoBehaviour {

	string serverPath = "\\data\\backend.json";
	string configPath = "\\data\\config.json";


	int width;
	int height;
	int x;
	int y;

	string port_bin;
	string port_msg;
	string ip;

	float resize_pixels;

	[HideInInspector]
	public Vector2[] proj_pts;

	void Awake () {

		#if UNITY_EDITOR
		string filePath =  Directory.GetParent (Directory.GetParent (Application.dataPath).FullName).FullName;
		#elif UNITY_STANDALONE_WIN
		string filePath = Directory.GetParent (Directory.GetParent (Application.dataPath).FullName).FullName;
		#elif UNITY_STANDALONE_OSX
		string filePath = Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
		#endif

		Debug.Log (filePath);

		StartCoroutine(Import(filePath));

	}

	IEnumerator Import(string file){
		WWW www = new WWW ("file://" + file + serverPath);
		yield return www;
		string text = www.text;
	
		var N = JSON.Parse (text);

		width = N["monitor"]["width"].AsInt;
		height = N["monitor"]["height"].AsInt;
		x = N["monitor"]["x"].AsInt;
		y = N["monitor"]["y"].AsInt;

		resize_pixels = N["network"]["resize_pixels"].AsFloat;

		www = new WWW ("file://" + file + configPath);
		yield return www;
		text = www.text;

		N = JSON.Parse (text);

		port_bin = N["backend"]["port_bin"];
		port_msg = N["backend"]["port_msg"];
		ip = N["backend"]["ip"];

		proj_pts = new Vector2[N["calib"]["proj_pts"].AsArray.Count];
		for(int i=0;i<N["calib"]["proj_pts"].AsArray.Count;i++)
		proj_pts[i] = new Vector2(N["calib"]["proj_pts"].AsArray[i].AsArray[0].AsFloat,N["calib"]["proj_pts"].AsArray[i].AsArray[1].AsFloat);

		MicromundosManager.Instance.SetCrosses ();
		MicromundosManager.Instance.msgClient.init ();
		MicromundosManager.Instance.binClient.init ();
	}

	public string GetIP(){
			return ip;
	}

	public string GetPortBin(){
		return port_bin;
	}

	public string GetPortMsg(){
		return port_msg;
	}
}
