using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject pausecanvas;
    SteamVR_TrackedObject trackdeObjec;

    void Awake(){
        //获取手柄上的这个组件  
        trackdeObjec = GetComponent<SteamVR_TrackedObject>();
    }

    // 按手柄扳手暂停游戏
    void Update () {
        //获取手柄输入  
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackdeObjec.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))//按下扳机
        {
            continue_Pause_Game();
        }
    }

    void continue_Pause_Game(){
        if(Time.timeScale == 0)
        {
            //继续游戏
            Time.timeScale = 1;
            pausecanvas.SetActive(false);
            GameObject leftSore = GameObject.Find("Blade Sword_Red");
            GameObject rightSore = GameObject.Find("Blade Sword_Blue");
            leftSore.SetActive(true);
            rightSore.SetActive(true);
        }
        else
        {
            //游戏暂停
            Time.timeScale = 0;
            pausecanvas.SetActive(true);
            GameObject leftSore = GameObject.Find("Blade Sword_Red");
            GameObject rightSore = GameObject.Find("Blade Sword_Blue");
            leftSore.SetActive(false);
            rightSore.SetActive(false);
        }
    }
}
