using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_ResultAgain : MonoBehaviour {
    public GameObject pausecanvas;
    public GameObject resultcanvas;
    SteamVR_TrackedObject trackdeObjec;
    public GameObject leftSore ;
    public GameObject rightSore ;


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
            if (resultcanvas.activeInHierarchy)
            {
                Again_Game();
            }
            else
            {
                continue_Pause_Game();
            }
        }
    }

    private void Again_Game()
    {
        resultcanvas.SetActive(false);
        SceneManager.LoadScene("Back_CubeDistance");
    }

    void continue_Pause_Game(){
        if(Time.timeScale == 0)
        {
            //继续游戏
            Time.timeScale = 1;
            pausecanvas.SetActive(false);           
            leftSore.SetActive(true);
            rightSore.SetActive(true);
        }
        else
        {
            //游戏暂停
            Time.timeScale = 0;
            pausecanvas.SetActive(true);
            leftSore.SetActive(false);
            rightSore.SetActive(false);
        }
    }
}
