using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using SampleModelsMySql;

namespace EntityFrameworkSample.Models
{
    internal class ModelMySql : ModelBase
    {
        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */

        internal override void Load()
        {
            var departments = new List<Tuple<string, string>> 
            {
                new Tuple<string,string>("01","Department01"),
                new Tuple<string,string>("02","Department02"),
                new Tuple<string,string>("03","Department03"),
                new Tuple<string,string>("04","Department04"),
                new Tuple<string,string>("05","Department05"),
            };

            using (var db = new samplemodelsEntities())
            {
                departments.ForEach(dept =>
                {
                    if (db.department.FirstOrDefault(d => d.Code == dept.Item1) == null)
                    {
                        db.department.Add(new department()
                        {
                            Code = dept.Item1,
                            Name = dept.Item2,
                        });
                    }
                });
                db.SaveChanges();

                Departments.AddRange(db.department.ToList().Select(d => new ModelDepartment() { Code = d.Code, Name = d.Name }));
                Employees.AddRange(db.employee.ToList().Select(e => new ModelEmployee() { Code = e.Code, Name = e.Name, MailAddress = e.MailAddress, ModelDepartment = Departments.First(d => d.Code == e.department.Code) }));
            }
            OnUpdate();
        }

        internal override bool Insert(string code, string name, string mail, string departmentCode)
        {
            using (var db = new samplemodelsEntities())
            {
                try
                {
                    //重複チェック
                    if (db.employee.Any(e => e.Code == code || e.MailAddress == mail))
                    {
                        return false;
                    }

                    var dept = db.department.First(d => d.Code == departmentCode);

                    var employee = new employee()
                    {
                        Code = code,
                        Name = name,
                        MailAddress = mail,
                        department = dept,
                    };
                    db.employee.Add(employee);
                    db.SaveChanges();

                    Employees.Add(new ModelEmployee() { Code = code, Name = name, MailAddress = mail, ModelDepartment = Departments.First(d => d.Code == dept.Code) });
                    OnUpdate();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        internal override void Update()
        {
            //ToDo:
        }

        internal override bool Delete(string code)
        {
            using (var db = new samplemodelsEntities())
            {
                try
                {
                    //重複チェック
                    var emp = db.employee.FirstOrDefault(e => e.Code == code);
                    if (emp == null)
                    {
                        return false;
                    }

                    db.employee.Remove(emp);
                    db.SaveChanges();


                    Employees.Remove(Employees.First(e => e.Code == code));
                    OnUpdate();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
