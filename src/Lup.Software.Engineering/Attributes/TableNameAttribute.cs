using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lup.Software.Engineering.Attributes
{
    public class TableNameAttribute
    {
        public class TableName : Attribute
        {
            private string name;

            public TableName(string name)
            {
                this.name = name;
            }

            public string Value => name;
        }
    }
}
