using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClientEvents {

	public static System.Action<int> OnBlockDetected = delegate { };
	public static System.Action<int> OnBlockExit = delegate { };
}
