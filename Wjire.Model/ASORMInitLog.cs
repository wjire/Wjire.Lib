using System;
using System.ComponentModel;
using Wjire.Excel;

namespace Wjire.Model
{

    [Serializable]
    public class ASORMInitLog
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("编号")]
        public long ID { get; set; }


        /// <summary>
        /// 应用ID
        /// </summary>
        [DisplayName("应用ID")]
        public long AppID { get; set; }


        /// <summary>
        /// 初始化时间
        /// </summary>
        [DisplayName("初始化时间")]
        public DateTime CreatedAt { get; set; }


        /// <summary>
        /// 渠道应用名称
        /// </summary>
        [DisplayName("渠道应用名称")]
        public string AppName { get; set; }


        /// <summary>
        /// 初始化包名
        /// </summary>
        [DisplayName("初始化包名")]
        public string PackageName { get; set; }


        /// <summary>
        /// 设备号
        /// </summary>
        [DisplayName("设备号")]
        public string DeviceNo { get; set; }


        /// <summary>
        /// 设备品牌
        /// </summary>
        [DisplayName("设备品牌")]
        public string DeviceBrand { get; set; }


        /// <summary>
        /// 设备型号
        /// </summary>
        [DisplayName("设备型号")]
        public string UnitType { get; set; }


        /// <summary>
        /// 设备系统版本
        /// </summary>
        [DisplayName("设备系统版本")]
        public string DeviceOsVer { get; set; }


        /// <summary>
        /// 有无SIM卡
        /// </summary>
        [DisplayName("有无SIM卡")]
        public Boolean HasSim { get; set; }


        /// <summary>
        /// MAC地址
        /// </summary>
        [DisplayName("MAC地址")]
        public string MAC { get; set; }


        /// <summary>
        /// IP地址
        /// </summary>
        [DisplayName("IP地址")]
        public string IP { get; set; }


        /// <summary>
        /// 本次开机时长（秒）
        /// </summary>
        [DisplayName("本次开机时长")]
        public int BootTime { get; set; }


        /// <summary>
        /// 是否充电状态
        /// </summary>
        [DisplayName("是否充电状态")]
        public Boolean IsCharging { get; set; }


        /// <summary>
        /// 电池容量剩余（百分比）
        /// </summary>
        [DisplayName("电池容量剩余")]
        public int RemainingBattery { get; set; }


        /// <summary>
        /// 本次屏幕激活时长（秒）
        /// </summary>
        [DisplayName("本次屏幕激活时长")]
        public int ScreenActivation { get; set; }

    }

}
