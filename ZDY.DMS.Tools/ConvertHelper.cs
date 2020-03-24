using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ZDY.DMS.Tools
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 把数字转换为大写
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns></returns>
        public static string NumberToUpper(int num)
        {
            String str = num.ToString();
            string rstr = "";
            int n;
            for (int i = 0; i < str.Length; i++)
            {
                n = Convert.ToInt16(str[i].ToString());//char转数字,转换为字符串，再转数字
                switch (n)
                {
                    case 0: rstr = rstr + "〇"; break;
                    case 1: rstr = rstr + "一"; break;
                    case 2: rstr = rstr + "二"; break;
                    case 3: rstr = rstr + "三"; break;
                    case 4: rstr = rstr + "四"; break;
                    case 5: rstr = rstr + "五"; break;
                    case 6: rstr = rstr + "六"; break;
                    case 7: rstr = rstr + "七"; break;
                    case 8: rstr = rstr + "八"; break;
                    default: rstr = rstr + "九"; break;


                }

            }
            return rstr;
        }

        /// <summary>
        /// 日转化为大写
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string DayToUpper(int day)
        {
            if (day < 20)
            {
                return NumberToUpper(day);
            }
            else
            {
                String str = day.ToString();
                if (str[1] == '0')
                {
                    return NumberToUpper(Convert.ToInt16(str[0].ToString())) + "十";

                }


                else
                {
                    return NumberToUpper(Convert.ToInt16(str[0].ToString())) + "十"
                        + NumberToUpper(Convert.ToInt16(str[1].ToString()));
                }
            }
        }

        /// <summary>
        /// 月转化为大写
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string MonthToUpper(int month)
        {
            if (month < 10)
            {
                return NumberToUpper(month);
            }
            else
                if (month == 10) { return "十"; }

                else
                {
                    return "十" + NumberToUpper(month - 10);
                }
        }

        /// <summary>
        /// 日期转换为大写
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateToUpper(System.DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            return NumberToUpper(year) + "年" + MonthToUpper(month) + "月" + DayToUpper(day) + "日";
        }

        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="money">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string MoneyToUpper(decimal money)
        {
            string strUpperMum = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string strNumUnit = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string strOfNum = "";    //从原num值中取出的值 
            string strNum = "";    //数字的字符串形式 
            string strReturnUpper = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int sumLength;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            money = Math.Round(Math.Abs(money), 2);    //将num取绝对值并四舍五入取2位小数 
            strNum = ((long)(money * 100)).ToString();        //将num乘100并转换成字符串形式 
            sumLength = strNum.Length;      //找出最高位 
            if (sumLength > 15) { return "溢出"; }
            strNumUnit = strNumUnit.Substring(15 - sumLength);   //取出对应位数的strNumUnit的值。如：200.55,sumLength为5所以strNumUnit=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < sumLength; i++)
            {
                strOfNum = strNum.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(strOfNum);      //转换为数字 
                if (i != (sumLength - 3) && i != (sumLength - 7) && i != (sumLength - 11) && i != (sumLength - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (strOfNum == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (strOfNum != "0" && nzero != 0)
                        {
                            ch1 = "零" + strUpperMum.Substring(temp * 1, 1);
                            ch2 = strNumUnit.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = strUpperMum.Substring(temp * 1, 1);
                            ch2 = strNumUnit.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (strOfNum != "0" && nzero != 0)
                    {
                        ch1 = "零" + strUpperMum.Substring(temp * 1, 1);
                        ch2 = strNumUnit.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (strOfNum != "0" && nzero == 0)
                        {
                            ch1 = strUpperMum.Substring(temp * 1, 1);
                            ch2 = strNumUnit.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (strOfNum == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (sumLength >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = strNumUnit.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (sumLength - 11) || i == (sumLength - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = strNumUnit.Substring(i, 1);
                }
                strReturnUpper = strReturnUpper + ch1 + ch2;

                if (i == sumLength - 1 && strOfNum == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    strReturnUpper = strReturnUpper + '整';
                }
            }
            if (money == 0)
            {
                strReturnUpper = "零元整";
            }
            return strReturnUpper;
        }

        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="money">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string MoneyToUpper(string money)
        {
            try
            {
                decimal num = Convert.ToDecimal(money);
                return MoneyToUpper(num);
            }
            catch
            {
                return "非数字形式";
            }
        }

        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="money">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string MoneyToUpper(int money)
        {
            try
            {
                decimal num = Convert.ToDecimal(money);
                return MoneyToUpper(num);
            }
            catch
            {
                return "非数字形式";
            }
        }

        /// <summary>    
        /// 将泛型集合类转换成DataTable    
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="propertyName">需要返回的列的列名</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ToDataTable<T>(this IList<T> collection, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);
            DataTable result = new DataTable();
            if (collection.Count > 0)
            {
                PropertyInfo[] propertys = collection[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < collection.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(collection[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(collection[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>    
        /// 将泛型集合类转换成DataTable    
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    
        /// <param name="list">集合</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ToDataTable<T>(this IList<T> collection)
        {
            return ToDataTable(collection, null);
        }

        /// <summary>    
        /// DataTable 转换为List 集合    
        /// </summary>    
        /// <typeparam name="TResult">类型</typeparam>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表    
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口    
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合    
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例    
                T ob = new T();
                //找到对应的数据  并赋值    
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.    
                oblist.Add(ob);
            }
            return oblist;
        }

        /// <summary>
        /// DataTable 转换为JSON 字符串 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJSON(this DataTable dt)
        {
            var JsonString = new System.Text.StringBuilder();
            if (dt.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
            }
            return JsonString.ToString();
        }
    }
}
