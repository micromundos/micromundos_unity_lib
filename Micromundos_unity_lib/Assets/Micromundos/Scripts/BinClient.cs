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
	Texture2D tex;
	public RawImage raw;

	// Use this for initialization
	public void init () {

		string url = ClientData.Instance.serverData.ip + ":" + ClientData.Instance.serverData.port_bin + "/";

		Debug.Log (url);

		ws = new WebSocket (url);

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


	// Update is called once per frame
	void Update () {

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
