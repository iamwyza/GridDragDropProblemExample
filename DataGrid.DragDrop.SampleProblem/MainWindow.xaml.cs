using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DataGrid.DragDrop.SampleProblem.Annotations;

namespace DataGrid.DragDrop.SampleProblem
{
    /// <summary>
    /// Sample program to show problems when changing with "CanUserAddRows" via binding + drag drop
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Vm vm = new Vm
            {
                Field = new Field("TopField1")
                {
                    FileFields = new ObservableCollection<FileField>
                    {
                        new FileField {FieldName = "Test1"},
                        new FileField {FieldName = "Test2"}
                    }
                }
            };

            DataContext = vm;

        }
    }

    public class Vm : INotifyPropertyChanged
    {
        private Field _field;

        public Field Field
        {
            get => _field;
            set
            {
                _field = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class Field : INotifyPropertyChanged
    {
        private string _fieldName;
        private ObservableCollection<FileField> _fileFields;

        public bool IsAdvanced => FileFields.Count > 1;


        public string FieldName
        {
            get => _fieldName;
            set
            {
                _fieldName = value;
                OnPropertyChanged(nameof(FieldName));
            }
        }

        public ObservableCollection<FileField> FileFields
        {
            get => _fileFields;
            set
            {
                _fileFields = value;
                FileFields.CollectionChanged += FileFields_CollectionChanged;
                OnPropertyChanged(nameof(FileFields));
                OnPropertyChanged(nameof(IsAdvanced));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public Field(string fieldName)
        {
            FieldName = fieldName;
            FileFields = new ObservableCollection<FileField> { new FileField { FieldName = string.Empty } };
            FileFields.CollectionChanged += FileFields_CollectionChanged;
            foreach (FileField field in FileFields)
            {
                field.PropertyChanged += PropertyChanged;
            }

        }

        private void FileFields_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += PropertyChanged;
            }

            OnPropertyChanged(nameof(IsAdvanced));

        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class FileField : INotifyPropertyChanged
    {
        private string _fieldName;
        private string _translationName;



        public string FieldName
        {
            get => _fieldName;
            set
            {
                _fieldName = value;
                OnPropertyChanged(nameof(FieldName));
            }
        }

        public string TranslationName
        {
            get => _translationName;
            set
            {
                _translationName = value;
                OnPropertyChanged(nameof(TranslationName));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

}
