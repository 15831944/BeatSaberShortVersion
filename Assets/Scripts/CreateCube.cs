using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateCube : MonoBehaviour {
    public static float cameraZPosition = 1f;//存放摄像机位置的Z坐标，用于配合计算速度
    public static float cubeZPosition  = 3f;//存放方块生成的位置的Z坐标，用于配合计算速度
    private float timerOne = 0f;//计时器
    /// <summary>
    /// 歌曲的BPM的1/2，可以根据不同的难度设置不同的比值
    /// </summary>
    public static float halfBeat = 1.36f;
    //public AudioSource theAudio ;
    public GameObject cube;
	void Start () {
        //theAudio = GetComponent<AudioSource>();
        //theAudio.Play();
        //确定摄像机的位置
        CreateCube.cameraZPosition = this.gameObject.transform.position.z;
        Debug.Log("Z position: " + this.gameObject.transform.position.z);
        //初始化生成一个物体
        Instantiate(cube, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.3f, 0.9f), cubeZPosition), Quaternion.identity);
        Debug.Log("INit OVER");
        GLOBAL_PARA.Game.ClearRecord();
        GLOBAL_PARA.Game.CubeSendRecord += 1;
    }
	
	void Update () {
        //deltaTime是两帧之间的间隔时间，累计超过一定的时间后产生新的方块
        timerOne += Time.deltaTime;
        //音乐结束后的处理，当作游戏结束的标志。
        //if (!theAudio.isPlaying)
        //{
        //    Debug.Log("游戏结束");
        //    //SceneManager.LoadScene("GameEnd");
        //}
        if (timerOne > halfBeat)
        {
            Instantiate(cube, new Vector3(Random.Range(0f, 2f), Random.Range(0f, 1f), cubeZPosition), Quaternion.identity);
            GLOBAL_PARA.Game.CubeSendRecord += 1;
            timerOne -= halfBeat;//减去相应的时间重新计时
        }
    }
}
