using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bgmManager : MonoBehaviour {

    //获取所有音乐的名字
    List<string> allMusicName = new List<string>();

    //设置当前音乐名称
    private static string currentAudioName = "";


    //设置单例变量
    public static bgmManager instance;


    //设置属性
    public static bgmManager Instance
    {
        get
        {
            return instance;
        }
    }

    //将声音放置字典中方便管理
    private Dictionary<string, AudioClip> _bgmDictionary;

    private void Awake()
    {
        //实例化
        instance = this;
        //加载资源存的所有音频资源
        LoadAudio();
    }
    void Start()
    {
        
    }
    private void LoadAudio()
    {
        //初始化字典
        _bgmDictionary = new Dictionary<string, AudioClip>();
        //本地加载
        AudioClip[] audioArray = Resources.LoadAll<AudioClip>("Song");

        //存放到字典
        foreach (AudioClip item in audioArray)
        {
            _bgmDictionary.Add(item.name, item);
            allMusicName.Add(item.name);
        }
        Debug.Log(_bgmDictionary.Count);
    }
    private AudioClip getbgm(string audioName)
    {
        return _bgmDictionary[audioName];

    }
    //播放音乐
    public void playbgm(string audioName)
    {
        if (currentAudioName != audioName)
        {
            if (_bgmDictionary.ContainsKey(audioName))
            {
                
                this.GetComponent<AudioSource>().Stop();
                this.GetComponent<AudioSource>().clip = getbgm(audioName);
                this.GetComponent<AudioSource>().Play();
                currentAudioName = audioName;
            }  
        }
    }
    //关闭音乐
    public void stopbgm()
    {
        if (this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().Stop();
        }
    }

    //获取所有的音乐名称
    public List<string> getAllMusicName()
    {
        return allMusicName;

    }
    //检测是否有这首bgm，如果没有，展示的文本和照片不发生变化
    public bool checkMusicSwitched(string audioName)
    {
        return _bgmDictionary.ContainsKey(audioName);
    }

    //获取当前音乐的名字
    public string getcurrentAudioName()
    {
        return currentAudioName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
