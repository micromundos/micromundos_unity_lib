using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Samples.Helpers;

[HideInInspector]
public class ServerData : MonoBehaviour {

	string serverPath = "\\data\\server.yml";
	string configPath = "\\data\\config.yml";


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

	[Serializable]
	public class ServerYML
	{
		public Backend_server backend_server { get; set; }
		public Network network { get; set; }
		public float resize_pixels { get; set; }
	}

	[Serializable]
	public class Backend_server
	{
		public int width { get; set; }
		public int height { get; set; }
		public int x{ get; set; }
		public int y{ get; set; }
	}

	[Serializable]
	public class Network
	{
		public string port_bin { get; set; }
		public string port_msg { get; set; }
		public string ip { get; set; }
	}

	[Serializable]
	public class ConfigYML
	{
		public Projector projector { get; set; }
		public CamConfig cam { get; set; }
		public Calib calib { get; set; }
	}

	[Serializable]
	public class Projector
	{
		public int width { get; set; }
		public int height { get; set; }
		public bool fullscreen { get; set; }
		public string position { get; set; }
		public int x{ get; set; }
		public int y{ get; set; }
	}

	[Serializable]
	public class CamConfig
	{
		public int width { get; set; }
		public int height { get; set; }
		public int device_id { get; set; }
	}

	[Serializable]
	public class Calib
	{
		public string file { get; set; }
		public int tag_id { get; set; }
		public float[][] proj_pts { get; set; }
	}

	void Awake () {

		#if UNITY_EDITOR
		string filePath = Directory.GetParent (Application.dataPath).FullName;
		#elif UNITY_STANDALONE_WIN
		string filePath = Directory.GetParent(Application.dataPath).FullName;
		#elif UNITY_STANDALONE_OSX
		string filePath = Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
		#endif

		Debug.Log (filePath);

		//StartCoroutine(Import(filePath));

	}

	IEnumerator Import(string file){
		WWW www = new WWW ("file://" + file + serverPath);
		yield return www;
		string text = www.text;

		var input = new StringReader(text);

		var deserializer = new DeserializerBuilder().Build();

		var serverText = deserializer.Deserialize<ServerYML>(input);

		width = serverText.backend_server.width;
		height = serverText.backend_server.height;
		x = serverText.backend_server.x;
		y = serverText.backend_server.y;

		port_bin = serverText.network.port_bin;
		port_msg = serverText.network.port_msg;
		ip = serverText.network.ip;

		resize_pixels = serverText.resize_pixels;

		www = new WWW ("file://" + file + configPath);
		yield return www;
		text = www.text;

		input = new StringReader(text);

		deserializer = new DeserializerBuilder().Build();

		var configText = deserializer.Deserialize<ConfigYML>(input);

		proj_pts = new Vector2[configText.calib.proj_pts.Length];
		for(int i=0;i<configText.calib.proj_pts.Length;i++)
			proj_pts[i] = new Vector2(configText.calib.proj_pts[i][0],configText.calib.proj_pts[i][1]);

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
