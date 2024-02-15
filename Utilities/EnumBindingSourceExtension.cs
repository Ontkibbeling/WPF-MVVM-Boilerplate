using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MyApp.Utilities
{
    public class EnumBindingSourceExtension: MarkupExtension
    {
        public EnumBindingSourceExtension(Type enumType) 
        { 
            if (enumType is null || !enumType.IsEnum)
                throw new Exception("EnumType must not be null and of type enum.");

            EnumType = enumType;
        }

        public Type EnumType { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Debug.WriteLine("Enum.GetValues(): " + Enum.GetValues(EnumType));
            return Enum.GetValues(EnumType);
        }
    }
}
