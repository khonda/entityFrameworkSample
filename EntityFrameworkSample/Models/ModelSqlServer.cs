using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using ClassLibrary1;

namespace EntityFrameworkSample.Models
{
    internal class ModelSqlServer : ModelBase
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

            using (var db = new SampleModelContainer())
            {
                departments.ForEach(dept =>
                {
                    if (db.Departments.FirstOrDefault(d => d.Code == dept.Item1) == null)
                    {
                        db.Departments.Add(new Department()
                        {
                            Code = dept.Item1,
                            Name = dept.Item2,
                        });
                    }
                });
                db.SaveChanges();

                Departments.AddRange(db.Departments.ToList().Select(d => new ModelDepartment(){ Code = d.Code, Name = d.Name}));
                Employees.AddRange(db.Employees.ToList().Select(e => new ModelEmployee() { Code = e.Code, Name = e.Name, MailAddress = e.MailAddress, ModelDepartment = Departments.First(d => d.Code == e.Department.Code) }));
            }
            OnUpdate();
        }

        internal override bool Insert(string code, string name, string mail, string departmentCode) 
        {
            using (var db = new SampleModelContainer()) {
                try
                {
                    //重複チェック
                    if (db.Employees.Any(e => e.Code == code || e.MailAddress == mail))
                    {
                        return false;
                    }

                    var dept = db.Departments.First(d => d.Code == departmentCode);

                    var employee = new Employee()
                    {
                        Code = code,
                        Name = name,
                        MailAddress = mail,
                        Department = dept,
                    };
                    db.Employees.Add(employee);
                    db.SaveChanges();

                    Employees.Add(new ModelEmployee() { Code = code, Name = name, MailAddress = mail, ModelDepartment = Departments.First(d => d.Code == dept.Code) });
                    OnUpdate();
                }
                catch {
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
            using (var db = new SampleModelContainer())
            {
                try
                {
                    //重複チェック
                    var emp = db.Employees.FirstOrDefault(e => e.Code == code);
                    if (emp == null) {
                        return false;
                    }

                    db.Employees.Remove(emp);
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
