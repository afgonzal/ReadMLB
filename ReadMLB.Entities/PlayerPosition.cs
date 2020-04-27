using System;
using System.ComponentModel;

namespace ReadMLB.Entities
{
    public enum PlayerPosition
    {
        Pitcher, Catcher, First, Second, Third, Shortstop,  LeftField, CenterField, RightField, DesignatedHitter
    }

    public enum PlayerPositionAbr
    {
        P = 1, C,
        [Description("1B")]
        FB,
        [Description("2B")]
        SB,
        [Description("3B")]
        TB, SS, LF, CF, RF, DH, RP, IF, OF, UT
    }

    public static class EnumHelper
    {
        public static T GetEnumFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }
    }
}