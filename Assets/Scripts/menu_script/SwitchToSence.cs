using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToSence : MonoBehaviour {
    private static string bgmName;  //为当前选中音乐的名称
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //切换页面
    public void ToPlaySence()
    {
        bgmManager.instance.stopbgm();
        bgmName = bgmManager.instance.getcurrentAudioName();
        GLOBAL_PARA.SongInfo.SONG songSame = (GLOBAL_PARA.SongInfo.SONG)GLOBAL_PARA.SongInfo.SONG.Parse(typeof(GLOBAL_PARA.SongInfo.SONG), bgmName);
        Debug.Log("跳出");
        //SampleScene为需要转换至的场景的名称
        SceneManager.LoadScene("Back_CubeDistance");
    }


}
