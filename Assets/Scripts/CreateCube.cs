using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour {
    private float timerOne = 0f;//计时器
    private float halfBeat = 1.36f;//歌曲的BPM的1/2，后续可以根据不同的难度设置不同的比值
    public GameObject cube;
	void Start () {
        Instantiate(cube, new Vector3(Random.Range(0f, 2f), Random.Range(0f, 1f), 3), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        //deltaTime是两帧之间的间隔时间，累计超过一定的时间后产生新的方块
        timerOne += Time.deltaTime;
        if (timerOne > halfBeat)
        {
            Instantiate(cube, new Vector3(Random.Range(0f, 2f), Random.Range(0f, 1f), 3), Quaternion.identity);
            timerOne -= halfBeat;//减去相应的时间重新计时
        }
    }
}
