using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剑切开方块的脚本
/// </summary>
public class Saber : MonoBehaviour {

    /// <summary>
    /// 切割面的材质
    /// </summary>
    public Material capMaterial;

    /// <summary>
    /// 剑的末端，用于判断切割方向(解决了平移与旋转）
    /// </summary>
    public GameObject endPoint;

    /// <summary>
    /// 剑的颜色属性
    /// </summary>
    public GLOBAL_PARA.TypeOfColor color;

    /// <summary>
    /// 剑的末端的上一帧位置
    /// </summary>
    private Vector3 lastPosition;

    // Use this for initialization
    void Start() {

    }

    private void Update()
    {
        lastPosition = endPoint.transform.position;
    }

    void OnCollisionEnter(Collision col)
    {
        //当Collision/Rigidbody触发另一个时调用
        GameObject victim = col.collider.gameObject;

        //若没有方块属性则返回
        if (!victim.GetComponent<Cube>()) return;

        CutCube(col);
        if( IsCutCorrect(col) )
        {
            //切割成功时
            GLOBAL_PARA.Game.CutCorrect();
        }
        else
        {
            //切割失败时
            GLOBAL_PARA.Game.RefreshCombo();
        }
    }

    /// <summary>
    /// 切割方块
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void CutCube(Collision collision)
    {
        GameObject cube = collision.collider.gameObject;
        cube.GetComponent<Collider>().enabled = false;

        //根据末端点的上一帧位置计算出切割方向
        Vector3 direction = endPoint.transform.position - lastPosition;
        //优化切割，避免切割边角
        Vector3 cutPoint = (cube.transform.position + collision.contacts[0].point) / 2;

        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(cube, cutPoint, new Vector3(direction.y, -direction.x, 0), capMaterial);

        if (!pieces[1].GetComponent<Rigidbody>())
            pieces[1].AddComponent<Rigidbody>();

        Destroy(pieces[0], 1);
        Destroy(pieces[1], 1);
    }

    /// <summary>
    /// 判断切割是否正确
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    /// <returns>正确返回True，错误返回False</returns>
    private bool IsCutCorrect(Collision collision)
    {
        GameObject victim = collision.collider.gameObject;
        Cube cube = victim.GetComponent<Cube>();

        if(cube.color != this.color)
        {
            Debug.Log(string.Format("切割错误：光剑颜色为{0}，方块颜色为{1}。", this.color , cube.color));
            return false;
        }

        if(IsHitForB(victim.transform.position, collision.contacts[0].point))
        {
            Debug.Log("不能碰撞前面或后面！");
            return false;
        }

        if (cube.hitPoint == GLOBAL_PARA.HitPoint.ANY) return true;

        if(cube.hitPoint != GetHitFrom(victim.transform.position, collision.contacts[0].point))
        {
            Debug.Log(string.Format("切割错误：切割方向为{0}，方块要求方向为{1}", GetHitFrom(victim.transform.position, collision.contacts[0].point), cube.hitPoint));
            return false;
        }

        return true;
    }

    /// <summary>
    /// 判断是否碰撞到前面或后面
    /// </summary>
    /// <param name="centerPoint">中心点</param>
    /// <param name="hitPoint">碰撞点</param>
    /// <returns>碰撞到前后面返回true，否则返回false</returns>
    private bool IsHitForB(Vector3 centerPoint, Vector3 hitPoint)
    {
        float deltaX = hitPoint.x - centerPoint.x;
        float deltaY = hitPoint.y - centerPoint.y;
        float deltaZ = hitPoint.z - centerPoint.z;

        if (Mathf.Abs(deltaZ) >= Mathf.Abs(deltaX) || Mathf.Abs(deltaZ) > Mathf.Abs(deltaY))
            return true;

        return false;
    }

    /// <summary>
    /// 判断计算碰撞方向（忽略Z轴方向）
    /// </summary>
    /// <param name="centerPoint">中心点</param>
    /// <param name="hitPoint">碰撞点</param>
    /// <returns>碰撞方向</returns>
    private GLOBAL_PARA.HitPoint GetHitFrom(Vector3 centerPoint, Vector3 hitPoint)
    {
        float deltaX = hitPoint.x - centerPoint.x;
        float deltaY = hitPoint.y - centerPoint.y;

        if( deltaX >= 0 && deltaY >= 0)
        {
            if (deltaX > deltaY) return GLOBAL_PARA.HitPoint.RIGHT;
            else return GLOBAL_PARA.HitPoint.UP;
        }

        if (deltaX >= 0 && deltaY <= 0)
        {
            if (deltaX > -deltaY) return GLOBAL_PARA.HitPoint.RIGHT;
            else return GLOBAL_PARA.HitPoint.DOWN;
        }

        if( deltaX <= 0 && deltaY >= 0)
        {
            if (-deltaX > deltaY) return GLOBAL_PARA.HitPoint.LEFT;
            else return GLOBAL_PARA.HitPoint.UP;
        }

        if(deltaX <= 0 && deltaY <= 0 )
        {
            if (-deltaX > -deltaY) return GLOBAL_PARA.HitPoint.LEFT;
            else return GLOBAL_PARA.HitPoint.DOWN;
        }

        return GLOBAL_PARA.HitPoint.ANY;
    }

}
