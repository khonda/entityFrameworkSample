using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleModelsMySql
{
    public partial class employee : ICloneable
    {
        public object Clone()
        {
            return new employee()
            {
                Code = this.Code,
                Name = this.Name,
                MailAddress = this.MailAddress,
                department = this.department.Clone() as department
            };
        }
    }
}
