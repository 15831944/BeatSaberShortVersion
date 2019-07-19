using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintorHide_MusicButton : MonoBehaviour
{
    public int x_initial = 0;
    public int y_initial = 0;
    public int width = 160;
    public int height = 30;
    public int interval = 50;
    private GameObject panel;
    private GameObject button;
    private GameObject text;
    private GameObject img;
    private Button btn;
    private int MusicListNum;
    private int MusicListIndex;
    private List<string> allMusicName;

    private void Start()
    {
        allMusicName = bgmManager.instance.getAllMusicName();
        MusicListNum = (allMusicName.Count % 4) == 0 ? allMusicName.Count / 4 : allMusicName.Count / 4 + 1;
        Debug.Log(MusicListNum);
        MusicListIndex = 0;
        ; PaintButton(allMusicName, MusicListIndex);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeButtonNext()
    {
        MusicListIndex = (MusicListIndex + 1) % MusicListNum;
        Debug.Log("MusicListIndex:" + MusicListIndex);
        HideButton();
        PaintButton(allMusicName, MusicListIndex);
    }
    public void changeButtonLast()
    {
        if (MusicListIndex == 0)
        {
            MusicListIndex =  MusicListNum -1;
        }
        else
        {
            MusicListIndex = (MusicListIndex - 1) % MusicListNum;
        }
        Debug.Log("MusicListIndex:" + MusicListIndex);
        HideButton();
        PaintButton(allMusicName, MusicListIndex);
    }
    private void PaintButton(List<string> allMusicName, int MusicListIndex)
    {
        panel = GameObject.Find("SelectMusic");
        int initial_index = 4 * MusicListIndex;
        for (int i = initial_index; i < initial_index + 4 && i < allMusicName.Count; i++)
        {
            //新增button  new GameObject（string 标记名称,typeof(新增的类型),typeof(内部相关属性).....）
            button = new GameObject(allMusicName[i], typeof(Button), typeof(Image));
            //设置button的父节点
            button.transform.SetParent(panel.transform);

            //新增点击事件
            button.AddComponent<ClickSwitchMusic>();


            //通过btn对按钮的属性进行修改
            btn = button.GetComponent<Button>();

            //设置按钮鼠标悬浮和点击颜色改变
            btn.targetGraphic = button.GetComponent<Graphic>();  //设置按钮的targetGraphic为自身
            ColorBlock cb = new ColorBlock();
            cb.normalColor = new Color(255, 255, 255, 0);
            cb.highlightedColor = new Color(58 / 255.0f, 155 / 255.0f, 202 / 255.0f);
            cb.pressedColor = new Color(255/ 255.0f, 255 / 255.0f, 255 / 255.0f,0);
            cb.disabledColor = new Color(1, 134 / 255f, 0, 1);
            cb.colorMultiplier = 1;
            btn.colors = cb;

            //设置按钮的RectTransform属性
            RectTransform btn_rectTransform = btn.GetComponent<RectTransform>();
            //设置posx,posy,posz
            btn_rectTransform.anchoredPosition3D = new Vector3(-4, 113 - 65 * (i - initial_index), 0);
            //设置width,height
            btn_rectTransform.sizeDelta = new Vector2(250, 50);

            //为按钮新增text属性
            text = new GameObject("Text", typeof(Text));
            text.transform.SetParent(button.transform);
            //获取Button上的text的句柄
            Text btn_text = text.GetComponent<Text>();
            btn_text.text = allMusicName[i];
            //设置文本框的位置
            btn_text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            btn_text.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
            //设置文字字体、颜色、大小
            btn_text.color = Color.white;
            btn_text.fontSize = 20;
            Font font = Resources.Load("FontStyle/Lato-Black", typeof(Font)) as Font;
            btn_text.font = font;
            //上下左右居中
            btn_text.alignment = TextAnchor.MiddleCenter;



            //增加音乐图片
            img = new GameObject("image", typeof(Image));
            img.transform.SetParent(button.transform);
            //设置图片属性
            Image musicImg = img.GetComponent<Image>();
            Sprite tem = musicImgManager.instance.getImage(allMusicName[i]);
            if (tem != null)
            {
                musicImg.sprite = tem;
            }
            //设置图片位置
            RectTransform musicRect = musicImg.rectTransform;
            //设置posx,posy,posz
            musicRect.anchoredPosition3D = new Vector3(-125, 0, 0);
            //设置width,height
            musicRect.sizeDelta = new Vector2(50, 50);

        }
    }
    //使一部分Button删除
    private void HideButton()
    {
        panel = GameObject.Find("SelectMusic");
        Debug.Log("删除");
        GameObject obj = GameObject.Find("SelectMusic"); // 当前组件的列表内容对象
        foreach (Transform child in obj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
