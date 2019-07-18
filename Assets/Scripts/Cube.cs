using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
    private float cameraZPosition = CreateCube.cameraZPosition;//摄像机的Z坐标，没有找到特别好的办法直接获取摄像机的Z坐标，获取Z坐标是在Z轴垂直屏幕的前提下
    /// <summary>
    /// 物体运动的速度，V = S/T；s就是方块产生的XY平面距离摄像机所在XY平面的距离，t是根据歌曲的BPM算出的多少秒一拍，目前是使用了1/2的BPM
    /// </summary>
    public static float speed = -2.9411f;
    public GLOBAL_PARA.TypeOfColor color;
    public GLOBAL_PARA.HitPoint hitPoint;

	void Start () {
    }

	void Update () {
        //判断物体是不是过了视界了，过了就销毁
        if (this.gameObject.transform.position.z >= cameraZPosition)
        {
            Destroy(gameObject);
        }
        ////让物体进行移动，摄像机在后面，前面要加个负号
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
