using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleModelsMySql
{
    public partial class department : ICloneable
    {
        public object Clone()
        {
            return new department()
            {
                Code = this.Code,
                Name = this.Name,
            };
        }
    }
}
