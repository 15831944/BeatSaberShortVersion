using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using EzySlice;

/// <summary>
/// 剑切开方块的脚本
/// </summary>
public class Saber : MonoBehaviour {

    /// <summary>
    /// 切割正确时的音效
    /// </summary>
    public AudioClip correctTrriger;

    /// <summary>
    /// 切割错误时的音效
    /// </summary>
    public AudioClip errorTrriger;

    /// <summary>
    /// 切割面的材质
    /// </summary>
    public Material sectionMaterial;

    /// <summary>
    /// 剑的末端，用于判断切割方向(解决了平移与旋转）
    /// </summary>
    public GameObject endPoint;

    /// <summary>
    /// 剑的起始端，用于发射射线
    /// </summary>
    public GameObject startPoint;

    /// <summary>
    /// 与另一把剑碰撞的火花特效
    /// </summary>
    public GameObject saberSpark;

    /// <summary>
    /// 切割方块的火花特效
    /// </summary>
    public GameObject cutSpark;

    /// <summary>
    /// 剑的颜色属性
    /// </summary>
    public GLOBAL_PARA.TypeOfColor color;

    /// <summary>
    /// 震动反馈的强度
    /// </summary>
    public float hapticForce;

    /// <summary>
    /// 剑的末端的上一帧位置
    /// </summary>
    private Vector3 lastPosition;

    // Use this for initialization
    void Start() {
        saberSpark.SetActive(false);
    }

    private void Update()
    {
        //记录末端上一帧位置信息
        lastPosition = endPoint.transform.position;

        //判断是否碰撞到另一把光剑
        Vector3 startPosition = startPoint.transform.position;
        Vector3 endPosition = endPoint.transform.position;
        Vector3 saberDirection = endPosition - startPosition;
        RaycastHit hit;

        if(Physics.Raycast(startPosition, saberDirection,out hit, saberDirection.magnitude))
        {
            GameObject victim = hit.collider.gameObject;

            if (victim.GetComponent<Saber>())
            {
                saberSpark.SetActive(true);
                saberSpark.transform.position = hit.point;
                return;
            }
        }
        saberSpark.SetActive(false);
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
            AudioSource.PlayClipAtPoint(correctTrriger, victim.transform.position);

            //Debug.Log(victim.transform.position.z);

            //VRTK震动反馈
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(transform.parent.gameObject), hapticForce);
        }
        else
        {
            //切割失败时
            GLOBAL_PARA.Game.RefreshCombo();
            //TODO 目前声音太大，需要在实际运行时调整音量
            AudioSource.PlayClipAtPoint(errorTrriger, victim.transform.position ,0.5f);
            //VRTK震动反馈
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(transform.parent.gameObject), 1.5f * hapticForce);
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
        Vector3 centerPoint = cube.transform.position;
        Vector3 colPoint = collision.contacts[0].point;
        Vector3 cutPoint = (centerPoint + colPoint) / 2;

        //调用切割方块的的函数
        GameObject[] pieces = cube.SliceInstantiate(cutPoint, new Vector3(direction.y, -direction.x, 0), sectionMaterial);
        Destroy(cube);

        //添加刚体与溶解特效
        pieces[0].AddComponent<Rigidbody>();
        pieces[1].AddComponent<Rigidbody>();
        pieces[0].AddComponent<CubeDissolve>();
        pieces[1].AddComponent<CubeDissolve>();

        #region  优化弹开效果，并根据方向生成火花
        float angleX = Vector2.SignedAngle(Vector2.right, new Vector2(direction.x, direction.y));
        Vector3 sparkPosition = Vector3.zero;
        switch (GetHitFrom(centerPoint, colPoint))
        {
            case GLOBAL_PARA.HitPoint.LEFT:
                //上侧方块向上弹开
                pieces[1].GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f), ForceMode.Impulse);
                //找到切割面的中心点
                sparkPosition = new Vector3(centerPoint.x, colPoint.y + (centerPoint.x - colPoint.x) * direction.y / direction.x, centerPoint.z);
                break;

            case GLOBAL_PARA.HitPoint.RIGHT:
                //上侧方块向上弹开
                pieces[0].GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f), ForceMode.Impulse);
                //找到切割面的中心点
                sparkPosition = new Vector3(centerPoint.x, colPoint.y + (centerPoint.x - colPoint.x) * direction.y / direction.x, centerPoint.z);

                break;

            case GLOBAL_PARA.HitPoint.DOWN:
                //右侧方块向右弹开
                pieces[0].GetComponent<Rigidbody>().AddForce(new Vector3(0.8f, 0), ForceMode.Impulse);
                //左侧方块向左弹开
                pieces[1].GetComponent<Rigidbody>().AddForce(new Vector3(-0.8f, 0), ForceMode.Impulse);
                //找到切割面的中心点
                sparkPosition = new Vector3(colPoint.x + (centerPoint.y - colPoint.y) * direction.x / direction.y, centerPoint.y,centerPoint.z);

                break;

            case GLOBAL_PARA.HitPoint.UP:
                //左侧方块向左弹开
                pieces[0].GetComponent<Rigidbody>().AddForce(new Vector3(-0.8f, 0), ForceMode.Impulse);
                //右侧方块向右弹开
                pieces[1].GetComponent<Rigidbody>().AddForce(new Vector3(0.8f, 0), ForceMode.Impulse);
                //找到切割面的中心点
                sparkPosition = new Vector3(colPoint.x + (centerPoint.y - colPoint.y) * direction.x / direction.y, centerPoint.y, centerPoint.z);

                break;
        }
        Destroy(Instantiate(cutSpark, sparkPosition, Quaternion.Euler(new Vector3(90 - angleX, 90, 0))), 0.5f);
        #endregion

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

        if (collision.contacts[0].normal.z == -1 )
        {
            Debug.Log("不能碰撞方块的前面！");
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
    /// 判断计算碰撞方向（忽略Z轴方向）
    /// </summary>
    /// <param name="centerPoint">中心点</param>
    /// <param name="hitPoint">碰撞点</param>
    /// <returns>碰撞方向</returns>
    private GLOBAL_PARA.HitPoint GetHitFrom(Vector3 centerPoint, Vector3 hitPoint)
    {
        float deltaX = hitPoint.x - centerPoint.x;
        float deltaY = hitPoint.y - centerPoint.y;

        //朝向-z方向，左右方向相反
        if( deltaX >= 0 && deltaY >= 0)
        {
            if (deltaX > deltaY) return GLOBAL_PARA.HitPoint.LEFT;
            else return GLOBAL_PARA.HitPoint.UP;
        }

        if (deltaX >= 0 && deltaY <= 0)
        {
            if (deltaX > -deltaY) return GLOBAL_PARA.HitPoint.LEFT;
            else return GLOBAL_PARA.HitPoint.DOWN;
        }

        if( deltaX <= 0 && deltaY >= 0)
        {
            if (-deltaX > deltaY) return GLOBAL_PARA.HitPoint.RIGHT;
            else return GLOBAL_PARA.HitPoint.UP;
        }

        if(deltaX <= 0 && deltaY <= 0 )
        {
            if (-deltaX > -deltaY) return GLOBAL_PARA.HitPoint.RIGHT;
            else return GLOBAL_PARA.HitPoint.DOWN;
        }

        return GLOBAL_PARA.HitPoint.ANY;
    }

}
