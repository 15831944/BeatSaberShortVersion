using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//用来控制物体的平移的脚本
public class Cube : MonoBehaviour {
    private float cameraPosition = -3.45f;
    private float speed = 4.0f;
	void Start () {
        //调试用，尚未完成
        Debug.Log("Start");
        Debug.Log(Time.time);
    }
	
	void Update () {
        
        if(this.gameObject.transform.position.z == cameraPosition)
        {
            Debug.Log("End");
            Debug.Log(Time.time);
        }
        if (this.gameObject.transform.position.z <= cameraPosition)
        {
            Debug.Log("End");
            Debug.Log(Time.time);
            Destroy(gameObject);
        }
        //Translate是平移函数
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);
	}
}
