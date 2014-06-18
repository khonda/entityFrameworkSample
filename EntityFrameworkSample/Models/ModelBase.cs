using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace EntityFrameworkSample.Models
{
    abstract class ModelBase : NotificationObject
    {
        public class ModelEmployee : NotificationObject
        {

            #region Code変更通知プロパティ
            private string _Code;

            public string Code
            {
                get
                { return _Code; }
                set
                {
                    if (_Code == value)
                        return;
                    _Code = value;
                    RaisePropertyChanged("Code");
                }
            }
            #endregion


            #region Name変更通知プロパティ
            private string _Name;

            public string Name
            {
                get
                { return _Name; }
                set
                {
                    if (_Name == value)
                        return;
                    _Name = value;
                    RaisePropertyChanged("Name");
                }
            }
            #endregion


            #region MailAddress変更通知プロパティ
            private string _MailAddress;

            public string MailAddress
            {
                get
                { return _MailAddress; }
                set
                {
                    if (_MailAddress == value)
                        return;
                    _MailAddress = value;
                    RaisePropertyChanged("MailAddress");
                }
            }
            #endregion


            #region ModelDepartment変更通知プロパティ
            private ModelDepartment _ModelDepartment;

            public ModelDepartment ModelDepartment
            {
                get
                { return _ModelDepartment; }
                set
                { 
                    if (_ModelDepartment == value)
                        return;
                    _ModelDepartment = value;
                    RaisePropertyChanged("ModelDepartment");
                }
            }
            #endregion

        }

        public class ModelDepartment : NotificationObject
        {
            #region Code変更通知プロパティ
            private string _Code;

            public string Code
            {
                get
                { return _Code; }
                set
                {
                    if (_Code == value)
                        return;
                    _Code = value;
                    RaisePropertyChanged("Code");
                }
            }
            #endregion

            #region Name変更通知プロパティ
            private string _Name;

            public string Name
            {
                get
                { return _Name; }
                set
                {
                    if (_Name == value)
                        return;
                    _Name = value;
                    RaisePropertyChanged("Name");
                }
            }
            #endregion
        }

        public event EventHandler Updated = (s, e) => { };


        public List<ModelEmployee> Employees = new List<ModelEmployee>();
        public List<ModelDepartment> Departments = new List<ModelDepartment>();

        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */

        internal abstract void Load();
        internal abstract bool Insert(string code, string name, string mail, string departmentCode);
        internal abstract void Update();
        internal abstract bool Delete(string code);

        protected void OnUpdate() 
        {
            Updated(this, new EventArgs());
        }
    }
}
