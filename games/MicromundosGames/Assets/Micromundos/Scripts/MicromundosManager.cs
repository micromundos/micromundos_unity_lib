using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Syphon;

public class MicromundosManager : MonoBehaviour {

	const string PREFAB_PATH = "ClientData";    
	static MicromundosManager mInstance = null;

	[HideInInspector]
	public ServerData serverData;
	[HideInInspector]
	public MsgClient msgClient;
	[HideInInspector]
	public BinClient binClient;

	public string appName;
	public Camera mainCamera;
	public GameObject cross;
	public GameObject calib;
	public GameObject backendTex;


	public static MicromundosManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = FindObjectOfType<MicromundosManager>();

				if (mInstance == null)
				{
					GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
					mInstance = go.GetComponent<MicromundosManager>();
					go.transform.localPosition = new Vector3(0, 0, 0);
				}
			}
			return mInstance;
		}
	}

	void Awake()
	{

		if (!mInstance)
			mInstance = this;
		//otherwise, if we do, kill this thing
		else
		{
			Destroy(this.gameObject);
			return;
		}

		serverData = GetComponent<ServerData> ();
		msgClient = GetComponent<MsgClient> ();
		binClient = GetComponent<BinClient> ();

		//backendTex.SetActive (false);

		if (mainCamera == null)
			mainCamera = Camera.main;


		//DontDestroyOnLoad(this.gameObject);

	}

	void Update(){
		msgClient.Process ();
		if (msgClient.PixReady()) 
			binClient.Process(msgClient.PixWidth(), msgClient.PixHeight(), msgClient.PixChan());

	}

	public void SetCrosses(){
		RectTransform canvasT = calib.transform as RectTransform;
		for (int i = 0; i < serverData.proj_pts.Length; i++) {
			GameObject go = Instantiate (cross);

			go.transform.SetParent (canvasT);
			RectTransform rt = go.transform as RectTransform;
			rt.position = new Vector2 (canvasT.rect.width*serverData.proj_pts[i].x,canvasT.rect.height*serverData.proj_pts[i].y);
		}

		calib.SetActive (false);
	}

	public void ShowCalib(bool show){
		calib.SetActive (show);
	}

	public Vector3 GetBlockRotation(int id){
		return new Vector3 (0f, 0f, Mathf.Rad2Deg * msgClient.GetBlock (id).angle_i);
	}

	public Vector3 GetBlockPosition(int id){
		return Camera.main.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			10f));
	}

	public Vector3 GetBlockPosition(int id, Camera cam){
			return cam.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			10f));
	}

	public Vector3 GetBlockPositionAtFarPlane(int id){
		return Camera.main.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			Camera.main.farClipPlane));		
	}

	public Vector3 GetBlockPositionAtNearPlane(int id){
		return Camera.main.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			Camera.main.nearClipPlane));		
	}

	public Vector3 GetBlockPositionAtZ(int id, float z){
		return Camera.main.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			z));		
	}

	public Vector3 GetBlockPositionAtFarPlane(int id, Camera cam){
		return cam.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			cam.farClipPlane));		
	}

	public Vector3 GetBlockPositionAtNearPlane(int id, Camera cam){
		return cam.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			cam.nearClipPlane));		
	}

	public Vector3 GetBlockPositionAtZ(int id, float z, Camera cam){
		return cam.ViewportToWorldPoint (new Vector3 (msgClient.GetBlock (id).loc_i.x,
			1f-msgClient.GetBlock (id).loc_i.y,
			z));		
	}

	public bool IsBlock(int id){
		return msgClient.GetBlock (id) != null;
	}

	public Texture2D GetTexture(){
		return binClient.GetTexture ();
	}

	public bool isPointBlocked(Vector3 pos){
		Vector3 uv = Camera.main.WorldToViewportPoint (pos);
		//print (uv);
		return binClient.IsPixelFill (uv.x, uv.y);
	}

	public void AddSyphon(){
		#if UNITY_EDITOR_OSX
		SyphonServer f = mainCamera.gameObject.AddComponent<SyphonServer> ();
		mainCamera.gameObject.name = serverData.GetSyphonClientName ();
		//f.renderMode = Funnel.Funnel.RenderMode.PreviewOnGameView;
		f.enabled = false;
		//mainCamera.gameObject.AddComponent<Syphon> ();
		//mainCamera.gameObject.GetComponent<Syphon> ().runInEditMode = true;
		//mainCamera.gameObject.AddComponent<SyphonServerTexture> ();
		
		#elif UNITY_STANDALONE_OSX
		SyphonServer f = mainCamera.gameObject.AddComponent<SyphonServer> ();
		mainCamera.gameObject.name = serverData.GetSyphonClientName ();
		//f.renderMode = Funnel.Funnel.RenderMode.PreviewOnGameView;
		f.enabled = false;
		//mainCamera.gameObject.AddComponent<Syphon> ();
		//mainCamera.gameObject.AddComponent<SyphonServerTexture> ();
		
		#endif
	}

	public void SetActiveSyphonServer(string active){
		Debug.Log (appName + "==" + active);
		bool enabled = active == appName ? true : false;
		Time.timeScale = enabled ? 1 : 0;
		mainCamera.gameObject.GetComponent<SyphonServer> ().enabled=enabled;
	}
}
