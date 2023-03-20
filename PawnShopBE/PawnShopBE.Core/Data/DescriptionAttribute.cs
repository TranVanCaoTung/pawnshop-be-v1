using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Data.DescriptionAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DescriptionAttribute:Attribute
    {
        public DescriptionAttribute(string description)
        {
            this.description = description;
        }

        public string description { get; set; }
    }
}
