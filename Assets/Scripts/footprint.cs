using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footprint : MonoBehaviour {

    public float y;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Camera mainCamera;
        GameObject gameObject = GameObject.Find("MainCamera");
        mainCamera = gameObject.GetComponent<Camera>();
        Vector3 player_postion = mainCamera.transform.position;
        float x = player_postion.x;
        float z = player_postion.z;
        

        //GameObject jio = GameObject.Find("Jio");
        this.transform.position.Set(x,y,z);

    }
}
