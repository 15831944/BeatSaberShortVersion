using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个脚本是用来生成方块的，写的时候以屏幕垂直Z轴为准
public class CreateCube : MonoBehaviour {
    private float timerOne = 0f;//计时器
    private float timeInterval = 1.46f;//产生物体的时间间隔
    public GameObject cube;
	void Start () {
        //初始可以生成也可以不生成，生成函数与下面的一样
	}
	
	// Update is called once per frame
	void Update () {
        //deltaTime是这一帧距离上一帧的时间间隔
        timerOne += Time.deltaTime;
        if (timerOne > timeInterval)
        {
            //vector3中的参数分别是x坐标，y坐标和z坐标，以其中一个轴垂直屏幕，另外两个轴就分别是物体分布的宽度和高度了
            Instantiate(cube, new Vector3(Random.Range(0f, 2f), Random.Range(0f, 1f), 3), Quaternion.identity);
            timerOne -= timeInterval;
        }
        
    }
}
