using System;

namespace wjire
{
    /// <summary>
    /// ASORMInitLog
    /// </summary>
    public class ASORMInitLog
    {

        /// <summary>
        /// 主键
        /// </summary>
        public long ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public long AppID { get; set; }


        /// <summary>
        /// 初始化时间
        /// </summary>
        public DateTime CreatedAt { get; set; }


        /// <summary>
        /// 渠道应用名称
        /// </summary>
        public string AppName { get; set; }


        /// <summary>
        /// 初始化包名
        /// </summary>
        public string PackageName { get; set; }


        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }


        /// <summary>
        /// 设备品牌
        /// </summary>
        public string DeviceBrand { get; set; }


        /// <summary>
        /// 设备型号
        /// </summary>
        public string UnitType { get; set; }


        /// <summary>
        /// 设备系统版本
        /// </summary>
        public string DeviceOsVer { get; set; }


        /// <summary>
        /// 有无SIM卡
        /// </summary>
        public bool HasSim { get; set; }


        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; set; }


        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }


        /// <summary>
        /// 本次开机时长（秒）
        /// </summary>
        public int BootTime { get; set; }


        /// <summary>
        /// 是否充电状态
        /// </summary>
        public bool IsCharging { get; set; }


        /// <summary>
        /// 电池容量剩余（百分比）
        /// </summary>
        public int RemainingBattery { get; set; }


        /// <summary>
        /// 本次屏幕激活时长（秒）
        /// </summary>
        public int ScreenActivation { get; set; }

    }
}
