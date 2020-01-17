using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;
using WebSocketSharp;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class BinClient : MonoBehaviour {

	WebSocket ws;

	bool locked, received;
	byte[] pix_data;
	public Texture2D tex;

	//[HideInInspector]
	public RawImage raw;

	float reconnectDelay = 5;
	float reconnectTime;
	public WebSocketState websocketState;

	// Use this for initialization
	public void init () {

		string url = MicromundosManager.Instance.serverData.GetIP() + ":" + MicromundosManager.Instance.serverData.GetPortBin() + "/";

		Debug.Log (url);

		ws = new WebSocket (url);

		ws.OnOpen += OnOpen;
		ws.OnMessage += OnMessage;
		ws.OnError += OnError;
		ws.OnClose += OnClose;

		//ws.Connect ();
		//ws.ConnectAsync();
		StartCoroutine(TryReconnect ());
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
		//string msg = !e.IsPing ? e.Data : "Received a ping.";

		/*if ( !args.isBinary || locked )
			return;
		pix_data = args.data.getData();
		locked = true;
		received = true;*/

		if (locked || !e.IsBinary)
			return;
		
		pix_data = e.RawData;
		locked = true;
		received = true;

	}

	void OnError(object sender, WebSocketSharp.ErrorEventArgs e){
		Debug.Log ("WebSocket Error: "+e.Message);
	}

	void OnClose(object sender, CloseEventArgs e){
		string msg = String.Format ("WebSocket Close ({0})", e.Code) + " : " + e.Reason;
	}

	// Update is called once per frame
	void Update () {
		if (ws != null) {
			websocketState = ws.ReadyState;
			if (ws.ReadyState != WebSocketState.Open) {
				StartCoroutine(TryReconnect ());
			}
		}
	}

	IEnumerator TryReconnect(){
		if (reconnectDelay < reconnectTime) {
			ws.Close ();
			//ws.Connect ();
			ws.ConnectAsync();
			Debug.Log ("try reconnect");
			reconnectTime = 0;
		} else {
			reconnectTime += Time.deltaTime;
		}
		yield return null;
	}

	public bool Process(int pix_w, int pix_h, int pix_chan){
		if (!received)
			return false;
		Parse (pix_w,pix_h,pix_chan);
		received=false;
		locked=false;
		return true;
	}

	void Parse(int pix_w, int pix_h, int pix_chan)
	{
		if (pix_data == null)
		{
			Debug.Log("pix_data = null");
			return;
		}

		if (tex == null)
			tex = new Texture2D (pix_w, pix_h,TextureFormat.Alpha8,false);
			//tex = new Texture2D (pix_w, pix_h,TextureFormat.Alpha8,false);

		/*Color[] c = new Color[pix_data.Length];
		for (int i = 0; i < pix_data.Length; i++)
			c [i] = new Color(pix_data [i],pix_data [i],pix_data [i]);*/

		tex.LoadRawTextureData (pix_data);


		//Debug.Log (pix_data.Length);

		//tex.SetPixels(c);
		//tex.LoadImage (pix_data);
		tex.Apply ();
		if (tex != null)			
			raw.texture = tex;
		/*unsigned char* pixd = reinterpret_cast<unsigned char*>(pix_data);
		pix.setFromPixels(pixd, pix_w, pix_h, pix_chan);
		tex.loadData(pix);*/
	}


	public Texture2D GetTexture(){
		return tex;
	}

	public bool IsPixelFill(float x_, float y_){		
		if (x_ < 0 || x_ > 1 || y_ < 0 || y_ > 1) {
			return false;
		} else {			
			if (tex != null) {
				int x = (int)(x_ * tex.width);
				int y = (int)((1f - y_) * tex.height);
				//print ("Tex x:"+x+" y:"+y);
				Color c = tex.GetPixel (x,y);
				//print (c.a==1);
				return c.a==1;
			} else {
				return false;
			}
		}
	}
}
