using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backMenu : MonoBehaviour {

    //public GameObject pausecanvas;
    SteamVR_TrackedObject trackdeObjec;
     

    void Awake()
    {
        //获取手柄上的这个组件  
        trackdeObjec = GetComponent<SteamVR_TrackedObject>();
    }

    // 按手柄扳手暂停游戏
    void Update()
    {
        //获取手柄输入  
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackdeObjec.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))//按下扳机
        {
            SceneManager.LoadScene("Menu");
        }
  
    }
}
