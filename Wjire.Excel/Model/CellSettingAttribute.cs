using System;

namespace Wjire.Excel.Model
{
    /// <summary>
    /// 单元格设置
    /// </summary>
    public class CellSettingAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public bool IsLink { get; set; }
    }
}
