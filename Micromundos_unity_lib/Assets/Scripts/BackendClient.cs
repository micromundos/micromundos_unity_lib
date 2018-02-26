using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;
using WebSocketSharp;
using System.IO;

public class BackendClient : MonoBehaviour {

	WebSocket ws;

	// Use this for initialization
	void Start () {

		string url = MicromundosManager.Instance.serverData.GetIP() + ":" + MicromundosManager.Instance.serverData.GetPortMsg() + "/";

		Debug.Log (url);

		ws = new WebSocket (url);

		ws.OnOpen += OnOpen;
		ws.OnMessage += OnMessage;
		ws.OnError += OnError;
		ws.OnClose += OnClose;

		ws.Connect ();
		//ws.ConnectAsync();
	}

	void OnDestroy(){
		ws.OnOpen -= OnOpen;
		ws.OnMessage -= OnMessage;
		ws.OnError -= OnError;
		ws.OnClose -= OnClose;
	}

	void OnOpen(object sender, System.EventArgs e){
		Debug.Log ("Websocket opening");
	}

	void OnMessage(object sender, MessageEventArgs e){
		string msg = !e.IsPing ? e.Data : "Received a ping.";
		Debug.Log (msg);
	}

	void OnError(object sender, WebSocketSharp.ErrorEventArgs e){
		Debug.Log ("WebSocket Error: "+e.Message);
	}

	void OnClose(object sender, CloseEventArgs e){
		string msg = String.Format ("WebSocket Close ({0})", e.Code) + " : " + e.Reason;
	}

	// Update is called once per frame
	void Update () {

	}
}
