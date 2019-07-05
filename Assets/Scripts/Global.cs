using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /// CubeSendRecord记录一共产生了多少个实体
        /// </summary>
        public static int CubeSendRecord = 0;
        /// <summary>
        /// 清空记录板
        /// </summary>
        public static void ClearRecord()
        {
            CubeHeatRecord = 0;
            CubeSendRecord = 0;
        }
        /// <summary>
        /// 返回击中方块的百分比
        /// </summary>
        /// <returns>返回击中的方块数占总的方块数的比例</returns>
        public float GetHeatPercent()
        {
            return CubeHeatRecord / CubeSendRecord;
        }
    }
    public class CubePara
    {
        /// <summary>
        /// 枚举物体的类型，一共有RED和BLUE对应左右手的光剑
        /// </summary>
        public enum TypeOfCube
        {
            RED = 0,
            BULE = 1,
        };
        /// <summary>
        /// 要求击中物体的方向，上下左右以及任意点
        /// </summary>
        public enum HeatPoint
        {
            UP = 0,
            DOWN = 1,
            LEFT = 2,
            RIGHT = 3,
            ANY = 4,
        };
    }
    public class SaberPara
    {
        /// <summary>
        /// 枚举光剑的类型，一共有RED和BLUE两种
        /// </summary>
        public enum TypeOfSaber
        {
            RED = 0,
            BLUE = 1,
        };
    }

}

