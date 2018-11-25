namespace Lup.Software.Engineering.Attributes
{
    using System;

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
