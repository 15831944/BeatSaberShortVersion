using UnityEngine;
using VRTK;

public class bl_TiltWindow : MonoBehaviour
{
	public Vector2 range = new Vector2(5f, 3f);
    private VRTK_Pointer rightHandPointer;//右手柄Pointer组件
    Transform mTrans;
	Quaternion mStart;
	Vector2 mRot = Vector2.zero;

	void Start ()
	{
		mTrans = transform;
		mStart = mTrans.localRotation;
        //当射线停留的时候
        rightHandPointer.GetComponent<VRTK_DestinationMarker>().DestinationMarkerHover += Test_DestinationMarkerHover;
    }

	void Update ()
	{
        //Vector3 pos = Input.mousePosition;
            //float halfWidth = Screen.width * 0.5f;
            //float halfHeight = Screen.height * 0.5f;
            //float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
            //float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
            //mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);
            //mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);
	}
    private void Test_DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
    {
        Vector3 pos = e.target.position;
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;
        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
        float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);
        mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);

    }
}