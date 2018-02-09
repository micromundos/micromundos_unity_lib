using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Samples.Helpers;


public class ServerData : MonoBehaviour {

	public string serverPath = "\\data\\server.yml";
	public string configPath = "\\data\\config.yml";


	public int width;
	public int height;
	public int x;
	public int y;

	public string port_bin;
	public string port_msg;
	public string ip;

	public float resize_pixels;

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

	void Start () {

		#if UNITY_EDITOR
		string filePath = Directory.GetParent (Application.dataPath).FullName;
		#elif UNITY_STANDALONE_WIN
		string filePath = Directory.GetParent(Application.dataPath).FullName;
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

		ClientData.Instance.SetCrosses ();
		ClientData.Instance.msgClient.init ();
	}
}
