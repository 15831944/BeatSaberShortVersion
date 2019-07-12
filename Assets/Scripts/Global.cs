using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace GLOBAL_PARA
{
    public class Global
    {
        
    }
    public class Game
    {
        /// <summary>
        /// CubeHeatRecord记录被正确击中的物体的数量
        /// </summary>
        public static int CubeHeatRecord = 0;

        /// <summary>
        /// CubeSendRecord记录目前产生了多少个实体
        /// </summary>
        public static int CubeSendRecord = 0;

        /// <summary>
        /// CubeSumRecord记录当前歌曲要产生的实体总数
        /// </summary>
        public static int CubeSumRecord = 0;

        /// <summary>
        /// CurrentComboRecord记录当前的连击数
        /// </summary>
        public static int CurrentComboRecord = 0;

        /// <summary>
        /// MaxComboRecord记录游玩的最大连击数
        /// </summary>
        public static int MaxComboRecord = 0;

        /// <summary>
        /// 清空记录板
        /// </summary>
        public static void ClearRecord()
        {
            CubeHeatRecord = 0;
            CubeSendRecord = 0;
            CubeSumRecord = 0;
            CurrentComboRecord = 0;
            MaxComboRecord = 0;
        }

        /// <summary>
        /// 返回击中方块的百分比
        /// </summary>
        /// <returns>返回击中的方块数占总的方块数的比例</returns>
        public static float GetHeatPercent()
        {
            return CubeHeatRecord / CubeSendRecord;
        }

        /// <summary>
        /// 切割成功时
        /// </summary>
        public static void CutCorrect()
        {
            CubeHeatRecord++;
            CurrentComboRecord++;
        }

        /// <summary>
        /// 切割失败时刷新连击数
        /// </summary>
        public static void RefreshCombo()
        {
            if (MaxComboRecord < CurrentComboRecord)
                MaxComboRecord = CurrentComboRecord;
            CurrentComboRecord = 0;
        }
    }

    public class CubePoint
    {
        public string cubeType { get; set; }//对应预制体前面的编号
        public float time { get; set; }//在游戏出现的时间点
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public CubePoint()
        {

        }
        public CubePoint(string cubeType, float time,float x,float y,float z)
        {
            this.cubeType = cubeType;
            this.time = time;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }


    /// <summary>
    /// 枚举光剑和方块的类型，一共有RED和BLUE两种
    /// </summary>
    public enum TypeOfColor
    {
        RED = 0,
        BLUE = 1,
    };

    /// <summary>
    /// 要求击中物体的方向，上下左右以及任意点
    /// </summary>
    public enum HitPoint
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
        ANY = 4,
    };

}

