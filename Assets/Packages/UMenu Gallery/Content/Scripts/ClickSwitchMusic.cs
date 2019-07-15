using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickSwitchMusic : MonoBehaviour {
    // Use this for initialization
    void Start()
    {
        //默认播放I need you
        //string defaultAudio = "I need you";
        //bgmManager.instance.playbgm(defaultAudio);
    }

    // Update is called once per frame
    void Update()
    {
        //获取当前被点击的按钮
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if (obj != null)
        {
            if (bgmManager.instance.checkMusicSwitched(obj.name))
            {
                //获取被点击按钮的名字
                string EventName = obj.name;

                //更改展示的音乐图片和名字
                Image musicImg = GameObject.Find("showImage").GetComponent<Image>();
                Sprite sp = musicImgManager.instance.getImage(EventName);
                if (sp != null)
                {
                    musicImg.sprite = sp;
                }

                Text showName = GameObject.Find("showName").GetComponent<Text>();
                showName.text = EventName;
                //播放音乐
                bgmManager.instance.playbgm(EventName);

            }  
        }
    }
}
