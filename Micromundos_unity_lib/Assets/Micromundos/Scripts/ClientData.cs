using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientData : MonoBehaviour {

	const string PREFAB_PATH = "ClientData";    
	static ClientData mInstance = null;

	public ServerData serverData;
	public MsgClient msgClient;
	public BinClient binClient;

	public GameObject cross;
	public GameObject calib;
	public GameObject backendTex;


	public static ClientData Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = FindObjectOfType<ClientData>();

				if (mInstance == null)
				{
					GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
					mInstance = go.GetComponent<ClientData>();
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

		DontDestroyOnLoad(this.gameObject);

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
		return new Vector3 (0f, 0f, Mathf.Rad2Deg * ClientData.Instance.msgClient.GetBlock (id).angle_i);
	}

	public Vector3 GetBlockPositionAtFarPlane(int id){
		return Camera.main.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
			1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
			Camera.main.farClipPlane));		
	}

	public Vector3 GetBlockPositionAtNearPlane(int id){
		return Camera.main.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
			1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
			Camera.main.nearClipPlane));		
	}

	public Vector3 GetBlockPositionAtZ(int id, float z){
		return Camera.main.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
			1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
			z));		
	}

	public Vector3 GetBlockPositionAtFarPlane(int id, Camera cam){
		return cam.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
			1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
			cam.farClipPlane));		
	}

	public Vector3 GetBlockPositionAtNearPlane(int id, Camera cam){
		return cam.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
			1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
			cam.nearClipPlane));		
	}

	public Vector3 GetBlockPositionAtZ(int id, float z, Camera cam){
		return cam.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
			1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
			z));		
	}

	public bool IsBlock(int id){
		return ClientData.Instance.msgClient.GetBlock (id) != null;
	}
}
