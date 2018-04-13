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

public class MsgClient : MonoBehaviour {

	WebSocket ws;

	bool locked, received;
	string message;
	int _pix_w, _pix_h, _pix_chan;
	//public List<Block> _blocks;
	public Dictionary<int,Block> _blocks;
	bool _calib_enabled,_binary_enabled,_syphon_enabled;
	public string _juego_active;

	[HideInInspector]
	public Text fps;
	float lastTime;

	float reconnectDelay = 5;
	float reconnectTime;

	// Use this for initialization
	public void init () {

		string url = MicromundosManager.Instance.serverData.GetIP() + ":" + MicromundosManager.Instance.serverData.GetPortMsg() + "/";

		Debug.Log (url);

		//_blocks = new List<Block> ();
		_blocks = new Dictionary<int,Block> ();

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
		else {
			fps.text = "fps: " + (1.0f / (Time.realtimeSinceStartup-lastTime));
			lastTime = Time.realtimeSinceStartup;
		}
		Parse ();
		received=false;
		locked=false;
		return true;
	}

	void Parse(){
		string[] data = message.Split ('_');

		foreach (string seccion in data) {
			string[] s = seccion.Split (':');
			if (s[0] == "pixels")
				ParsePixData (seccion);
			else if (s[0] == "net")
				ParseNetData (seccion);
			else if (s[0] == "calib")
				ParseCalibData (seccion);
			else if (s[0] == "bloques")
				ParseBlocks (seccion.Split (':') [1].Split (';'), _blocks);
			else if (s[0] == "juegos")
				ParseJuegos (seccion);
		}
		
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

	void ParseNetData(string data_str){
		string[] data = data_str.Split(':');
		if (data.Length > 1){
			string[] d = data[1].Split('#');
			_binary_enabled = d2i(d[0])>0?true:false;
			bool syphon = d2i(d[1])>0?true:false;
			if (syphon) {
				if (!_syphon_enabled)
					MicromundosManager.Instance.AddSyphon ();
				_syphon_enabled = syphon;
			}
		}
	}

	void ParseCalibData(string calibStr){
		string[] cd = calibStr.Split(':');
		if (cd.Length > 1)
		{
			string[] cdata = cd[1].Split('#');
			bool state = d2i(cdata[0])>0?true:false;
			if (_calib_enabled != state)
				MicromundosManager.Instance.ShowCalib (state);
			_calib_enabled = state;
		}
	}

	void ParseJuegos(string juegosStr){
		string[] active = juegosStr.Split(':');
		if (active.Length > 1) {
			string j = active [1].Split ('=') [1];
			if (j != _juego_active) {
				_juego_active = j;
				MicromundosManager.Instance.SetActiveSyphonServer (_juego_active);
			}
		}
	}

	//void ParseBlocks(string[] bloques_str, List<Block> blocks){
	void ParseBlocks(string[] bloques_str, Dictionary<int, Block> blocks){
		Dictionary<int,bool> cur = new Dictionary<int, bool> ();

		foreach (string b in bloques_str)
		{
			string[] bdata = b.Split('#');
			if (bdata.Length == 0 || bdata[0] == "")
				continue;
			int id = d2i(bdata[0]);
			cur[id] = true;
			if (!blocks.ContainsKey(id)){
			//if (!blocks.Contains (x => x.id == id)) {
				MakeBlock (id, bdata, blocks);
			} else {
				//UpdateBlock (id, bdata, blocks.Find (x => x.id == id));
				UpdateBlock (id, bdata, blocks[id]);
			}
		}

		Dictionary<int,Block>.KeyCollection k = blocks.Keys;
		for (int i = k.Count - 1; i >= 0; i--) {			
			//if (!cur.ContainsKey (blocks [i].id)) {
			if (!cur.ContainsKey (k.ElementAt(i))) {
				//ClientEvents.OnBlockExit(blocks [i].id);
				//blocks.RemoveAt (i);
				ClientEvents.OnBlockExit(k.ElementAt(i));
				blocks.Remove(k.ElementAt(i));
			}
		}

		/*foreach (int i in blocks.Keys) {
			if (!cur.ContainsKey (i)) {
				ClientEvents.OnBlockExit(i);
				blocks.Remove(i);
			}
		}*/
	}

	//void MakeBlock(int id, string[] bdata, List<Block> block)
	void MakeBlock(int id, string[] bdata, Dictionary<int,Block> block)
	{

		Block b = new Block();

		SetBlock(id, bdata, b);

		b.loc_i = b.loc;
		b.dir_i = b.dir;
		b.angle_i = b.angle;

		block[b.id] = b;
		//block.Add (b);

		ClientEvents.OnBlockDetected (id);
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
		//return _blocks.Find (x => x.id == id);
		if (_blocks != null) {
			if (_blocks.ContainsKey (id))
				return _blocks [id];
			else
				return null;
		} else {
			return null;
		}
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
		if (ws != null) {
			if (ws.ReadyState != WebSocketState.Open) {
				TryReconnect ();
			}
		}
	}

	void TryReconnect(){
		if (reconnectDelay < reconnectTime) {
			ws.Close ();
			ws.Connect ();
			Debug.Log ("try reconnect");
			reconnectTime = 0;
		} else {
			reconnectTime += Time.deltaTime;
		}			
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
	public bool SyphonEnabled() { return _syphon_enabled; }
	public bool BinaryEnabled() { return _binary_enabled; }

	public bool PixReady()
	{
		return _pix_w != 0 && _pix_h != 0 && _pix_chan != 0;
	}
}
