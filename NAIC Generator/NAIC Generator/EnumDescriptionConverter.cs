using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace naic
{
    /**
    \brief
        Converts an enum value name to
        its description.

        Heavily inspired by solution on
        StackOverflow.

        See:
        https://stackoverflow.com/questions/3985876
    */
    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum enumObj)
        {
            // Get field information from enum
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            // Get list of attributes for enum
            object[] attributeArray = fieldInfo.GetCustomAttributes(false);

            // Make sure attributes exist
            if (attributeArray.Length == 0)
            {
                // They don't. Return
                // the regular enum name.
                return enumObj.ToString();
            }
            else
            {
                // Iterate each attribute
                foreach(object attrib in attributeArray)
                {
                    // Make sure attribute is
                    // a description
                    if(attrib.GetType() == typeof(DescriptionAttribute))
                    {
                        // It is. Return it.
                        return ((DescriptionAttribute)attrib).Description;
                    }
                }
            }

            // No description
            // Return enum string
            return enumObj.ToString();
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get enum value
            Enum myEnum = (Enum)value;

            // Get enum description for
            // value
            string description = GetEnumDescription(myEnum);

            // Return description
            return description;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return empty string
            return string.Empty;
        }
    }
}
