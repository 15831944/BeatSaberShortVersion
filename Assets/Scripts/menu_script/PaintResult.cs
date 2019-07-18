using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintResult : MonoBehaviour {

    // Use this for initialization
    private int MaxCombo;
    private int HeatPercent;
    public GameObject MaxComboNum;
    public GameObject CompletionNum;
    public GameObject SongnameText;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject star4;
    public GameObject star5;
    public Sprite Star;
    public Sprite StarBack;

    void Start () {
        MaxCombo = (int) GLOBAL_PARA.Game.MaxComboRecord;
        HeatPercent = (int) GLOBAL_PARA.Game.GetHeatPercent();
        //设置完成率
        CompletionNum.GetComponent<Text>().text = HeatPercent.ToString() + "%";
        //设置最大连击数
        MaxComboNum.GetComponent<Text>().text = MaxCombo.ToString();
        //设置歌曲名称
        SongnameText.GetComponent<Text>().text = CreateCube.songInfo.songName;
        //设置星星的样式
        if (HeatPercent > 0)
        {
            //点亮第一颗星
            star1.GetComponent<Image>().sprite = Star;
            if(HeatPercent >= 20)
            {
                //点亮第二颗星
                star2.GetComponent<Image>().sprite = Star;
                if (HeatPercent >= 40)
                {
                    //点亮第三颗星
                    star3.GetComponent<Image>().sprite = Star;
                    if (HeatPercent >= 60)
                    {
                        //点亮第四颗星
                        star4.GetComponent<Image>().sprite = Star;
                        if (HeatPercent >= 80)
                        {
                            //点亮第五颗星
                            star5.GetComponent<Image>().sprite = Star;
                        }
                        else
                        {
                            star5.GetComponent<Image>().sprite = StarBack;
                        }
                    }
                    else
                    {
                        star4.GetComponent<Image>().sprite = StarBack;
                    }
                }
                else
                {
                    star3.GetComponent<Image>().sprite = StarBack;
                }
            }
            else
            {
                star2.GetComponent<Image>().sprite = StarBack;
            }
        }
        else
        {
            star1.GetComponent<Image>().sprite = StarBack;
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
