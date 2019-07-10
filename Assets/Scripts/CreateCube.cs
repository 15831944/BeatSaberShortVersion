using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

public class CreateCube : MonoBehaviour {
    public static float cameraZPosition = 1f;//存放摄像机位置的Z坐标，用于配合计算速度
    public static float cubeZPosition  = 3f;//存放方块生成的位置的Z坐标，用于配合计算速度
    private float timerOne = 0f;//计时器
    /// <summary>
    /// 歌曲的BPM的1/2，可以根据不同的难度设置不同的比值
    /// </summary>
    public static float halfBeat = 1.36f;
    private Queue<string> cubeTypes = new Queue<string>();//方块预制体编号队列
    private Queue<float> beatTime = new Queue<float>();//生成时间队列
    private Queue<float> xPosition = new Queue<float>();//对应的生成位置的X坐标队列
    private Queue<float> yPosition = new Queue<float>();//对应的生成位置的Y坐标队列
    private Queue<float> zPosition = new Queue<float>();//对应的生成位置的Z坐标队列
    public AudioSource theAudio ;
    public GameObject cube;
    private List<GLOBAL_PARA.CubePoint> cubePointsList= new List<GLOBAL_PARA.CubePoint>();
	void Start () {
        //theAudio = GetComponent<AudioSource>();
        //theAudio.Play();
        //确定摄像机的位置
        CreateCube.cameraZPosition = this.gameObject.transform.position.z;
        //先清空记录板然后开始记录
        GLOBAL_PARA.Game.ClearRecord();
        GLOBAL_PARA.Game.CubeSendRecord += 1;
        ////将产生的信息加入列表
        //GLOBAL_PARA.CubePoint cubePoint = new GLOBAL_PARA.CubePoint(GLOBAL_PARA.CubePara.TypeOfCube.BULE,GLOBAL_PARA.CubePara.HeatPoint.ANY,Time.time,start.x,start.y,start.z);
        //cubePointsList.Add(cubePoint);
        //加载各个队列
        this.cubePointsList = LoadAllCubePoints();
        initQueue(cubePointsList);
    }
	
	void Update () {
        //deltaTime是两帧之间的间隔时间，累计超过一定的时间后产生新的方块
        timerOne += Time.deltaTime;

        //音乐结束后的处理，当作游戏结束的标志。
        if (!theAudio.isPlaying)
        {
            Debug.Log("Length of List: "+cubePointsList.Count.ToString());
            Debug.Log("Count: "+GLOBAL_PARA.Game.CubeSendRecord);
            SaveAllCubepoint(cubePointsList);
            Debug.Log("游戏结束");
            Application.Quit();
            //SceneManager.LoadScene("GameEnd");
        }
        /*
         * 按照一定的时间间隔生成物体的方法，用来建立记录时间点文件
        if (timerOne > halfBeat)
        {
            Vector3 position = new Vector3(Random.Range(0f, 2f), Random.Range(0f, 1f), cubeZPosition);
            Instantiate(cube, position, Quaternion.identity);
            int ra = Random.Range(0, 9);
            GLOBAL_PARA.CubePoint cubePoint = new GLOBAL_PARA.CubePoint(ra.ToString(), Time.time, position.x, position.y, position.z);
            cubePointsList.Add(cubePoint);
            GLOBAL_PARA.Game.CubeSendRecord += 1;
            timerOne -= halfBeat;//减去相应的时间重新计时
        }
        */
        //根据时间队列中的时间进行生成，先看队首的时间点是不是
        if (beatTime.Count > 0 && Time.time >= beatTime.Peek())
        {
            beatTime.Dequeue();
            Vector3 position = new Vector3(xPosition.Dequeue(), yPosition.Dequeue(), zPosition.Dequeue());
            GameObject objPre = Resources.Load<GameObject>("Cube" + cubeTypes.Dequeue());
            Instantiate(objPre, position, Quaternion.identity);
        }
    }

    /// <summary>
    /// 将CubePoint转换成json字符串并保存到本地
    /// </summary>
    /// <param name="cubePoint">需要保存的CubePoint</param>
    public void SaveCubePoint(GLOBAL_PARA.CubePoint cubePoint)
    {
        //存储的文件名，后面有多首歌曲的话改用歌曲的名字命名
        string filePath = Application.dataPath + @"/Data/CubePoint.json";
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
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 将所有的方块点写入一个json文件
    /// </summary>
    /// <param name="cPList">要写入的方块点列表</param>
    public void SaveAllCubepoint(List<GLOBAL_PARA.CubePoint> cPList)
    {
        string filePath = Application.dataPath + @"/Data/CubePoint.json";
        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();
        string jsonStr = JsonConvert.SerializeObject(cPList);
        sw.WriteLine(jsonStr);
        sw.Close();
        sw.Dispose();
        AssetDatabase.Refresh();
    }
    public GLOBAL_PARA.CubePoint LoadCubePoint()
    {
        
        StreamReader streamReader = new StreamReader(Application.dataPath + @"/Data/CubePoint.json");
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
        StreamReader streamReader = new StreamReader(Application.dataPath + @"/Data/CubePoint.json");
        JsonSerializer serializer = new JsonSerializer();
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
