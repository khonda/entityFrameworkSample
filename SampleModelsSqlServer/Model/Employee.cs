using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public partial class Employee : ICloneable
    {

        public object Clone()
        {
            return new Employee() {
                Code = this.Code,
                Name = this.Name,
                MailAddress = this.MailAddress,
                Department = this.Department.Clone() as Department
            };
        }
    }
}
