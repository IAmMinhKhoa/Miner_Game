using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeviceManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Debug.LogError("deviceName: " + SystemInfo.deviceName);
		Debug.LogError("deviceModel: " + SystemInfo.deviceModel);
		Debug.LogError("deviceType: " + SystemInfo.deviceType);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
