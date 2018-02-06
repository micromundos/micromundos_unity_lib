using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;
using WebSocketSharp;

public class WebSockeClient : MonoBehaviour {
	
	public string ip = "ws://192.168.0.39";
	public string port_bin = "9999";
	public string port_msg = "9998";

	WebSocket ws;

	// Use this for initialization
	void Start () {
		
		Debug.Log ("Start at ws://"+ip+":"+port_msg+"/");

		ws = new WebSocket ("ws://"+ip+":"+port_msg+"/");

		ws.OnOpen += OnOpen;
		ws.OnMessage += OnMessage;
		ws.OnError += OnError;
		ws.OnClose += OnClose;

		ws.Connect ();
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

	void OnError(object sender, ErrorEventArgs e){
		Debug.Log ("WebSocket Error: "+e.Message);
	}

	void OnClose(object sender, CloseEventArgs e){
		string msg = String.Format ("WebSocket Close ({0})", e.Code) + " : " + e.Reason;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
