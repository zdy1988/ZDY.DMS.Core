using System;

namespace AKCY.Tools
{
    /// <summary>
    /// BaseRandom
    /// 产生随机数
    /// 随机数管理，最大值、最小值可以自己进行设定。
    /// </summary>
    public static class RandomHelper
    {
        private static Random random = new Random(DateTime.Now.Second);

        /// <summary>
        /// 产生随机字符
        /// </summary>
        /// <returns>字符串</returns>
        public static string GetRandomString(int len = 6, string str = "")
        {
            string defaultString = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";
            if (!string.IsNullOrEmpty(str))
            {
                defaultString = str;
            }

            string returnValue = string.Empty;
            for (int i = 0; i < len; i++)
            {
                int r = random.Next(0, defaultString.Length - 1);
                returnValue += defaultString[r];
            }
            return returnValue;
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="minimum">最小值</param>
        /// <param name="maximal">最大值</param>
        /// <returns>随机数</returns>
        public static int GetRandom(int minimum = 100000, int maximal = 999999)
        {
            return random.Next(minimum, maximal);
        }

        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            return random.NextDouble();
        }

        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static void GetRandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换

            //交换的次数,这里使用数组的长度作为交换次数
            int count = arr.Length;

            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = GetRandom(0, arr.Length);
                int randomNum2 = GetRandom(0, arr.Length);

                //定义临时变量
                T temp;

                //交换两个随机数位置的值
                temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(string allChar, int codeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
    }
}
