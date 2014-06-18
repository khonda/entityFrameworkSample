using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using EntityFrameworkSample.Models;

using System.Collections.ObjectModel;

namespace EntityFrameworkSample.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

//        ModelBase _Database = new ModelSqlServer();
        ModelBase _Database = new ModelMySql();

        public void Initialize()
        {
            Employees = new ObservableCollection<Employee>();
            Departments = new ObservableCollection<Department>();

            _Database.Updated += _Database_Updated;
            _Database.Load();
        }
        protected override void Dispose(bool disposing)
        {
            _Database.Updated -= _Database_Updated;
            base.Dispose(disposing);
        }
        void _Database_Updated(object sender, EventArgs e)
        {
            Departments.Clear();
            _Database.Departments.ForEach(dep => Departments.Add(new Department() {
                Code = dep.Code,
                Name = dep.Name,
            }));

            Employees.Clear();
            _Database.Employees.ForEach(emp => Employees.Add(new Employee() {
                Code = emp.Code,
                Name = emp.Name,
                MailAddress = emp.MailAddress,
                DepartmentName = emp.ModelDepartment.Name
            }));
        }


        #region Employees変更通知プロパティ
        private ObservableCollection<Employee> _Employees;

        public ObservableCollection<Employee> Employees
        {
            get
            { return _Employees; }
            set
            { 
                if (_Employees == value)
                    return;
                _Employees = value;
                RaisePropertyChanged("Employees");
            }
        }
        #endregion

        #region Departments変更通知プロパティ
        private ObservableCollection<Department> _Departments;

        public ObservableCollection<Department> Departments
        {
            get
            { return _Departments; }
            set
            { 
                if (_Departments == value)
                    return;
                _Departments = value;
                RaisePropertyChanged("Departments");
            }
        }
        #endregion


        #region InputCode変更通知プロパティ
        private string _InputCode;

        public string InputCode
        {
            get
            { return _InputCode; }
            set
            { 
                if (_InputCode == value)
                    return;
                _InputCode = value;
                RaisePropertyChanged("InputCode");
            }
        }
        #endregion

        #region InputName変更通知プロパティ
        private string _InputName;

        public string InputName
        {
            get
            { return _InputName; }
            set
            {   
                if (_InputName == value)
                    return;
                _InputName = value;
                RaisePropertyChanged("InputName");
            }
        }
        #endregion

        #region InputMail変更通知プロパティ
        private string _InputMail;

        public string InputMail
        {
            get
            { return _InputMail; }
            set
            { 
                if (_InputMail == value)
                    return;
                _InputMail = value;
                RaisePropertyChanged("InputMail");
            }
        }
        #endregion

        #region InputDepartmentCode変更通知プロパティ
        private string _InputDepartmentName;

        public string InputDepartmentCode
        {
            get
            { return _InputDepartmentName; }
            set
            { 
                if (_InputDepartmentName == value)
                    return;
                _InputDepartmentName = value;
                RaisePropertyChanged("InputDepartmentCode");
            }
        }
        #endregion

        #region InsertCommand
        private ViewModelCommand _InsertCommand;

        public ViewModelCommand InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            if (!_Database.Insert(InputCode, InputName, InputMail, InputDepartmentCode)) {
                Messenger.Raise(new InformationMessage("データ挿入に失敗しました", "error", "Error"));
            }
        }
        #endregion


        #region DeleteCommand
        private ListenerCommand<string> _DeleteCommand;

        public ListenerCommand<string> DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new ListenerCommand<string>(Delete);
                }
                return _DeleteCommand;
            }
        }

        public void Delete(string parameter)
        {
            if (!_Database.Delete(parameter))
            {
                Messenger.Raise(new InformationMessage("データ削除に失敗しました", "error", "Error"));
            }
        }
        #endregion




        public class Employee : ViewModel
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


            #region DepartmentName変更通知プロパティ
            private string _DepartmentName;

            public string DepartmentName
            {
                get
                { return _DepartmentName; }
                set
                { 
                    if (_DepartmentName == value)
                        return;
                    _DepartmentName = value;
                    RaisePropertyChanged("DepartmentName");
                }
            }
            #endregion

        }

        public class Department : ViewModel
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
    }
}
