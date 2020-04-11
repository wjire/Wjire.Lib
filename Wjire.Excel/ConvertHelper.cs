using System;

namespace Wjire.Excel
{
    public static class ConvertHelper
    {
        public static object Convert(object value, Type conversionType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                if (value == DBNull.Value)
                {
                    return null;
                }

                if (underlyingType.IsEnum)
                {
                    value = Enum.Parse(underlyingType, value.ToString());
                }

                return System.Convert.ChangeType(value, underlyingType);
            }
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }

            if (value == null || (value is string valueString && string.IsNullOrWhiteSpace(valueString)))
            {
                if (typeof(ValueType).IsAssignableFrom(conversionType))
                {
                    return Activator.CreateInstance(conversionType);
                }
            }
            return System.Convert.ChangeType(value, conversionType);
        }
    }
}
