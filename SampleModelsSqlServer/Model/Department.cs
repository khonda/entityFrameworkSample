using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public partial class Department : ICloneable
    {
        public object Clone()
        {
            return new Department() {
                Code = this.Code,
                Name = this.Name,
            };
        }
    }
}
