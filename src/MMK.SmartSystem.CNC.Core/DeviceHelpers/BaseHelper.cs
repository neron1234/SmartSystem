using MMK.SmartSystem.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class BaseHelper
    {
        public string Ip { get; set; }

        public ushort Port { get; set; }

        public ushort TimeOut { get; set; }

        public short LaserLibraryCuttingDataQuantity { get; set; } = 10;

        public short LaserLibraryEdgeCuttingDataQuantity { get; set; } = 5;

        public short LaserLibraryPiercingDataQuantity { get; set; } = 3;

        public short LaserLibrarySlopeControlDataQuantity { get; set; } = 5;

        public ushort LaserCommentLineCount { get; set; } = 20;

        public BaseHelper()
        {
            LaserLibraryCuttingDataQuantity = MMKSmartSystemWebCommonConsts.LaserLibraryCuttingDataQuantity;
            LaserLibraryEdgeCuttingDataQuantity = MMKSmartSystemWebCommonConsts.LaserLibraryEdgeCuttingDataQuantity;
            LaserLibraryPiercingDataQuantity = MMKSmartSystemWebCommonConsts.LaserLibraryPiercingDataQuantity;
            LaserLibrarySlopeControlDataQuantity = MMKSmartSystemWebCommonConsts.LaserLibrarySlopeControlDataQuantity;

        }
        public short BuildConnect(ref ushort flib)
        {
            Focas1.cnc_freelibhndl(flib);

            var ret = Focas1.cnc_allclibhndl3(Ip, Port, TimeOut, out flib);
            return ret;
        }

        public void FreeConnect(ushort flib)
        {
            Focas1.cnc_freelibhndl(flib);
            flib = 0;
        }

        protected static string GetGeneralErrorMessage(short ret)
        {
            string res = null;

            switch (ret)
            {
                case -16:
                    res = "连接失败";
                    break;
                case -15:
                    res = "NO DLL";
                    break;
                case -11:
                    res = "BUS ERROR";
                    break;
                case -7:
                    res = "系统版本不匹配";
                    break;
                case -6:
                    res = "发生意外错误";
                    break;
                case -2:
                    res = "操作期间执行了复位或者停止";
                    break;
                case -1:
                    res = "系统忙碌";
                    break;
                case 1:
                    res = "功能不可用";
                    break;
                case 2:
                    res = "数据长度错误或者数据值错误";
                    break;
                case 3:
                    res = "数据NUMBER错误";
                    break;
                case 4:
                    res = "数据属性错误";
                    break;
                case 5:
                    res = "数据错误";
                    break;
                case 6:
                    res = "没有选项功能";
                    break;
                case 7:
                    res = "写保护";
                    break;
                case 8:
                    res = "CNC内存溢出";
                    break;
                case 9:
                    res = "参数设定错误";
                    break;
                case 10:
                    res = "内存为空或者内存已满";
                    break;
                case 11:
                    res = "路径设定错误";
                    break;
                case 12:
                    res = "CNC操作方式错误,或者CNC运行中";
                    break;
                case 13:
                    res = "CNC拒绝，请检查相关状态";
                    break;
                case 14:
                    res = "数据服务器错误";
                    break;
                case 15:
                    res = "请消除相关报警";
                    break;
                case 16:
                    res = "CNC停止或者急停";
                    break;
                case 17:
                    res = "数据保护";
                    break;
                default:
                    res = " 其他错误";
                    break;
            }

            return res;
        }

    }
}
