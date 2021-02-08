using System;
using System.Linq;


namespace Acb.Shield.MqError
{
    public class MqErrorPageDto
    {
        public string Id { get; set; }
        
        public string CreatedTime { get; set; }

        public DateTime CreatedTimeDt => ConvertStringToDateTime(CreatedTime);

        public string HandlerFullName { get; set; }

        public string Queue { get; set; }

        public string Event { get; set; }

        public string ErrorMessage { get; set; }

        public string ModifyTime { get; set; }

        /// <summary> 0:未处理 1:已处理 </summary>
        public int State { get; set; }

        public string StateCn => State == 0 ? "未处理" : "已处理";

        public string Remark { get; set; }


        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
