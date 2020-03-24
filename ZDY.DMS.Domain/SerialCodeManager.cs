using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Application.Untils
{
    public static class SerialCodeManager
    {
        public static string GetBusinessOrderSerialCode(int number, int type)
        {
            return $"BO{DateTime.Now.ToString("yyyyMMdd")}{type}{number.ToString().PadLeft(9, '0')}";
        }

        public static string GetWarehouseOrderSerialCode(int number, int type)
        {
            return $"WO{DateTime.Now.ToString("yyyyMMdd")}{type}{number.ToString().PadLeft(9, '0')}";
        }
    }
}
