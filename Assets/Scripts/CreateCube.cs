using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using System.Text;

public class CreateCube : MonoBehaviour {
    public GameObject resultcanvas;
    public static float cameraZPosition = 0.21f;//存放摄像机位置的Z坐标，用于配合计算速度
    public static float cubeZPosition  = -10.75f;//存放方块生成的位置的Z坐标，用于配合计算速度
    private float timerOne = 0f;//计时器
    public static GLOBAL_PARA.SongInfo songInfo ;
    public static GLOBAL_PARA.SongInfo.SONG songName = GLOBAL_PARA.SongInfo.SONG.AllFallsDown;
    private bool IS_PLAYING = false;
    private float collapseTime = 0;
    private string logoutUrl = "http://www.hustimis.cn:8014/Log.svc/Logout";
    private string gameRecordUrl = "http://www.hustimis.cn:8014/Statistics.svc/UploadScore";
    private float delayTime = 1f;
    /// <summary>
    /// 歌曲的BPM的1/2，可以根据不同的难度设置不同的比值
    /// </summary>
    //public static float halfBeat = 1.36f;
    private Queue<int> cubeTypes = new Queue<int>();//方块预制体编号队列
    private Queue<float> beatTime = new Queue<float>();//生成时间队列
    private Queue<float> xPosition = new Queue<float>();//对应的生成位置的X坐标队列
    private Queue<float> yPosition = new Queue<float>();//对应的生成位置的Y坐标队列
    private Queue<float> zPosition = new Queue<float>();//对应的生成位置的Z坐标队列
    private List<GLOBAL_PARA.CubePoint> cubePointsList= new List<GLOBAL_PARA.CubePoint>();

    IEnumerator POSTDATA(string url,string data)
    {
        var request = new UnityWebRequest(url,"POST");
        byte[] dataPost = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(dataPost);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 7;
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        while (!request.isDone) { }
        Debug.Log("Result: " + request.downloadHandler.text);
    }

	void Start () {
        collapseTime = Time.time;
        Debug.Log("Here");
        songInfo = new GLOBAL_PARA.SongInfo(songName);
        AudioClip audioClip = Resources.Load<AudioClip>("Song/"+songInfo.songName);
        if(songName == GLOBAL_PARA.SongInfo.SONG.AllFallsDown)
        {
            delayTime = 60 / songInfo.bPM;
        }
        if(songName == GLOBAL_PARA.SongInfo.SONG.Believer)
        {
            delayTime = 0.84f;
        }
        this.GetComponent<AudioSource>().clip = audioClip;
        this.GetComponent<AudioSource>().Play();
        //确定摄像机的位置，调整方块生成位置
        CreateCube.cameraZPosition = this.gameObject.transform.position.z;
        //CreateCube.cubeZPosition = cameraZPosition + 16f;
        //计算并设置生成速度
        Cube.speed = ((cameraZPosition - cubeZPosition) / 9) / (180 /songInfo.bPM);
        //先清空记录板然后开始记录
        GLOBAL_PARA.Game.ClearRecord();
        //加载各个队列，生成的时候把以下两行注释掉
        this.cubePointsList = LoadAllCubePoints();
        initQueue(cubePointsList);
    }
	
	void Update () {
        //deltaTime是两帧之间的间隔时间，累计超过一定的时间后产生新的方块
        timerOne += Time.deltaTime;
        //音乐结束后的处理，当作游戏结束的标志。
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            //Debug.Log("Length of List: "+cubePointsList.Count.ToString());
            //Debug.Log("Count: "+GLOBAL_PARA.Game.CubeSendRecord);
            //SaveAllCubepoint(cubePointsList);
            //Debug.Log("游戏结束");
            resultcanvas.SetActive(true);
            string gameData = JsonConvert.SerializeObject(new GLOBAL_PARA.GameRecord(GLOBAL_PARA.Game.PlayerID, 1, GLOBAL_PARA.Game.CountScore(), GLOBAL_PARA.Game.GetHeatPercent(), DateTime.Now));
            Debug.Log("UPloadData: " + gameData);
            StartCoroutine(POSTDATA(gameRecordUrl, gameData));
            //Application.Quit();
            //SceneManager.LoadScene("GameEnd");
        }

        //////按照一定的时间间隔生成物体的方法，用来建立记录时间点文件，读文件生成的时候把这个if注释掉
        //if (timerOne > (180 / songInfo.bPM) && this.GetComponent<AudioSource>().isPlaying)
        //{
        //    Vector3 position = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(8.8f, 9.5f), cubeZPosition);
        //    Instantiate(cube, position, Quaternion.identity);
        //    Debug.Log("Z position: "+position.z.ToString());
        //    int ra = Random.Range(0, 10);
        //    cubePointsList.Add(new GLOBAL_PARA.CubePoint(ra, Time.time, position.x, position.y, position.z));
        //    GLOBAL_PARA.Game.CubeSendRecord += 1;
        //    if (Random.Range(0f, 1f) < 0.3f)//产生两个并排的
        //    {
        //        if (ra <= 3)//左右并排
        //        {
        //            Debug.Log("Add one");
        //            if (ra % 2 == 0)//原来的是红色的
        //            {
        //                cubePointsList.Add(new GLOBAL_PARA.CubePoint(ra + 1, Time.time, position.x - 0.1f, position.y, position.z));
        //            }
        //            else
        //            {
        //                cubePointsList.Add(new GLOBAL_PARA.CubePoint(ra - 1, Time.time, position.x - 0.1f, position.y, position.z));
        //            }
        //        }
        //        if (4 <= ra && ra <= 7)
        //        {
        //            Debug.Log("Add one");
        //            if (ra % 2 == 0)//原来的是红色的
        //            {
        //                cubePointsList.Add(new GLOBAL_PARA.CubePoint(ra + 1, Time.time, position.x, position.y + 0.1f, position.z));
        //            }
        //            else
        //            {
        //                cubePointsList.Add(new GLOBAL_PARA.CubePoint(ra - 1, Time.time, position.x, position.y + 0.1f, position.z));
        //            }
        //        }
        //    }
        //    timerOne -= (180 / songInfo.bPM);//减去相应的时间重新计时
        //}

        //根据时间队列中的时间进行生成，先看队首的时间点是不是生成的时间点
        while (beatTime.Count > 0 && (Time.time-collapseTime) >= beatTime.Peek())
        {
            if (!IS_PLAYING && Time.time-collapseTime>=delayTime)
            {
                this.GetComponent<AudioSource>().Play();
                IS_PLAYING = true;
            }
            beatTime.Dequeue();
            float theZPosition = zPosition.Dequeue();
            Vector3 position = new Vector3(xPosition.Dequeue(), yPosition.Dequeue(), theZPosition);
            GameObject objPre = Resources.Load<GameObject>("Cube/" + ((GLOBAL_PARA.CubeType)cubeTypes.Dequeue()).ToString());
            Cube.speed = (9.6f - theZPosition) / (120f / (float)songInfo.bPM);
            Instantiate(objPre, position, objPre.transform.rotation);
            GLOBAL_PARA.Game.CubeSendRecord++;
        }
    }

    /// <summary>
    /// 将CubePoint转换成json字符串并保存到本地
    /// </summary>
    /// <param name="cubePoint">需要保存的CubePoint</param>
    public void SaveCubePoint(GLOBAL_PARA.CubePoint cubePoint)
    {
        //存储的文件名，后面有多首歌曲的话改用歌曲的名字命名
        string filePath = Application.dataPath + @"/Data/"+songInfo.songName+".json";
        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有该文件，有则打开文件，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //将对象转换成json字符串
        string objJsonStr = JsonConvert.SerializeObject(cubePoint);
        //输出文件
        sw.WriteLine(objJsonStr);
        //释放资源
        sw.Close();
        sw.Dispose();
    }

    /// <summary>
    /// 将所有的方块点写入一个json文件
    /// </summary>
    /// <param name="cPList">要写入的方块点列表</param>
    public void SaveAllCubepoint(List<GLOBAL_PARA.CubePoint> cPList)
    {
        string filePath = Application.dataPath + @"/Data/" + songInfo.songName + ".json";
        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();
        string jsonStr = JsonConvert.SerializeObject(cPList);
        sw.WriteLine(jsonStr);
        sw.Close();
        sw.Dispose();
    }
    public GLOBAL_PARA.CubePoint LoadCubePoint()
    {
        
        StreamReader streamReader = new StreamReader(Application.dataPath + @"/Data/" + songInfo.songName + ".json");
        JsonSerializer serializer = new JsonSerializer();
        serializer.NullValueHandling = NullValueHandling.Ignore;
        JsonReader reader = new JsonTextReader(streamReader);
        GLOBAL_PARA.CubePoint cubePoint = serializer.Deserialize<GLOBAL_PARA.CubePoint>(reader);
        return cubePoint;
    }

    /// <summary>
    /// 从文件中读取方块出现的点，以列表的形式返回
    /// </summary>
    /// <returns>方块生成信息的List<></returns>
    public List<GLOBAL_PARA.CubePoint> LoadAllCubePoints()
    {
        //读取文件
        StreamReader streamReader = new StreamReader(Application.dataPath + @"/Resources/Data/" + songInfo.songName + ".json");
        JsonSerializer serializer = new JsonSerializer();
        //设置忽略空值
        serializer.NullValueHandling = NullValueHandling.Ignore;
        JsonReader reader = new JsonTextReader(streamReader);
        List<GLOBAL_PARA.CubePoint> cubePoints = serializer.Deserialize<List<GLOBAL_PARA.CubePoint>>(reader);
        return cubePoints;
    }

    /// <summary>
    /// 将列表中的信息提取出来并初始化队列
    /// </summary>
    /// <param name="cubePoints">要提取信息的对象列表</param>
    private void initQueue(List<GLOBAL_PARA.CubePoint> cubePoints)
    {
        foreach(GLOBAL_PARA.CubePoint cubePoint in cubePoints)
        {
            this.beatTime.Enqueue(cubePoint.time);
            this.cubeTypes.Enqueue(cubePoint.cubeType);
            this.xPosition.Enqueue(cubePoint.x);
            this.yPosition.Enqueue(cubePoint.y);
            this.zPosition.Enqueue(cubePoint.z);
        }
    }
}
