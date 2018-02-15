using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Threading;
using WebSocketSharp;
using System.IO;
using System.Linq;

public class MsgClient : MonoBehaviour {

	WebSocket ws;

	bool locked, received;
	string message;
	int _pix_w, _pix_h, _pix_chan;
	List<Block> _blocks;
	bool _calib_enabled;

	// Use this for initialization
	public void init () {

		string url = ClientData.Instance.serverData.ip + ":" + ClientData.Instance.serverData.port_msg + "/";

		Debug.Log (url);

		_blocks = new List<Block> ();

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


		if (locked || !e.Data.IsText())
			return;
		message = e.Data;
		//Debug.Log ("Msg: "+message);
		locked = true;
		received = true;

	}

	void OnError(object sender, WebSocketSharp.ErrorEventArgs e){
		Debug.Log ("WebSocket Error: "+e.Message);
	}

	void OnClose(object sender, CloseEventArgs e){
		string msg = String.Format ("WebSocket Close ({0})", e.Code) + " : " + e.Reason;
	}

	public bool Process(){
		if (!received)
			return false;
		Parse ();
		received=false;
		locked=false;
		return true;
	}

	void Parse(){
		string[] data = message.Split ('_');

		if (data.Length > 0)
			ParsePixData(data[0]);

		if (data.Length > 1)
			ParseCalibData (data [1]);

		if (data.Length > 2)
			ParseBlocks (data[2].Split(':')[1].Split(';'), _blocks);
		
	}

	void ParsePixData(string pixStr){
		string[] pd = pixStr.Split(':');
		if (pd.Length > 1){
			string[] pix_data = pd[1].Split('#');
			Vector2 dim = d2vec(pix_data[0]);
			_pix_w = (int)dim.x;
			_pix_h = (int)dim.y;
			_pix_chan = d2i(pix_data[1]);
		}
	}

	void ParseCalibData(string calibStr){
		string[] cd = calibStr.Split(':');
		if (cd.Length > 1)
		{
			string[] cdata = cd[1].Split('#');
			bool state = d2i(cdata[0])>0?true:false;
			if (_calib_enabled != state)
				ClientData.Instance.ShowCalib (state);
			_calib_enabled = state;
		}
	}

	void ParseBlocks(string[] bloques_str, List<Block> blocks){
		Dictionary<int,bool> cur = new Dictionary<int, bool> ();

		foreach (string b in bloques_str)
		{
			string[] bdata = b.Split('#');
			if (bdata.Length == 0 || bdata[0] == "")
				continue;
			int id = d2i(bdata[0]);
			cur[id] = true;
			//if (!block.ContainsKey(id))
			if (!blocks.Contains (x => x.id == id)) {
				MakeBlock (id, bdata, blocks);
			} else {
				UpdateBlock (id, bdata, blocks.Find (x => x.id == id));
			}
		}
	}

	void MakeBlock(int id, string[] bdata, List<Block> block)
	{

		Block b = new Block();

		SetBlock(id, bdata, b);

		b.loc_i = b.loc;
		b.dir_i = b.dir;
		b.angle_i = b.angle;

		//block[b.id] = b;
		block.Add (b);
	}

	void UpdateBlock(int id, string[] bdata, Block b){
		InterporlateBlock (bdata, b);
		SetBlock (id, bdata, b); 
	}

	void SetBlock(int id, string[] bdata, Block b)
	{
		//TODO MsgClient: better deserialization
		//int id = d2i(bdata[0]);

		b.id = id;
		b.loc = d2vec(bdata[1]);
		b.dir = d2vec(bdata[2]);
		b.angle = d2f(bdata[3]);
		b.radio = d2f(bdata[4]);

		//if (bdata.size() > 4)
		//{
		//b.corners[0] = d2vec(bdata[4]);
		//b.corners[1] = d2vec(bdata[5]);
		//b.corners[2] = d2vec(bdata[6]);
		//b.corners[3] = d2vec(bdata[7]);
		//}
	}

	public Block GetBlock(int id)
	{
		return _blocks.Find (x => x.id == id);
	}

	void InterporlateBlock(string[] bdata, Block b)
	{
		Vector2 loc = d2vec(bdata[1]);
		Vector2 dir = d2vec(bdata[2]);
		int angle = (int)d2f(bdata[3]);

		b.loc_i += new Vector2 (0.2f * (loc.x - b.loc_i.x), 0.2f * (loc.y - b.loc_i.y));
		b.dir_i += new Vector2 (0.2f * (dir.x - b.dir_i.x), 0.2f * (dir.y - b.dir_i.y));
		//b.loc_i += (loc - b.loc_i) * 0.2;
		//b.dir_i += (dir - b.dir_i) * 0.2;

		b.angle_i = Mathf.Deg2Rad * Mathf.LerpAngle (Mathf.Rad2Deg * b.angle_i,Mathf.Rad2Deg*angle,0.05f);
		//b.angle_i = ofLerpRadians(b.angle_i, angle, 0.05);
	}

	// Update is called once per frame
	void Update () {

	}

	int d2i(string d){
		return int.Parse(d.Split('=')[1]);
	}

	float d2f(string d){
		return float.Parse(d.Split('=')[1]);
	}

	Vector2 d2vec(string d)
	{
		string[] vec = d.Split('=')[1].Split(',');
		return new Vector2(float.Parse(vec[0]), float.Parse(vec[1]));
	}

	public int PixWidth() { return _pix_w; }
	public int PixHeight() { return _pix_h; }
	public int PixChan() { return _pix_chan; }
	public bool CalibEnabled() { return _calib_enabled; }

	public bool PixReady()
	{
		return _pix_w != 0 && _pix_h != 0 && _pix_chan != 0;
	}
}
