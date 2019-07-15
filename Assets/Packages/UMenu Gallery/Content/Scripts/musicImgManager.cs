using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicImgManager : MonoBehaviour {

    //设置单例变量
    public static musicImgManager instance;


    //设置属性
    public static musicImgManager Instance
    {
        get
        {
            return instance;
        }
    }
    //将图片放置字典中方便管理
    private Dictionary<string, Sprite> _musicImgDictionary;
    private void Awake()
    {
        //实例化
        instance = this;
        //加载资源存的所有音频资源
        LoadImg();
    }

    void Start()
    {
        
    }
    private void LoadImg()
    {
        //初始化字典
        _musicImgDictionary = new Dictionary<string, Sprite>();

        //本地加载
        Sprite[] imgArray = Resources.LoadAll<Sprite>("music_img");

        //存放到字典
        foreach (Sprite item in imgArray)
        {
            _musicImgDictionary.Add(item.name, item);
        }
        Debug.Log(_musicImgDictionary.Count);
    }
    //获取对应的照片
    public Sprite getImage(string imgName)
    {
        if (_musicImgDictionary.ContainsKey(imgName))
        {
            return _musicImgDictionary[imgName];
        }
        else
        {
            return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
