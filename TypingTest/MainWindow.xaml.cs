using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TypingTest.Model;
using TypingTest.ViewModel;

namespace TypingTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region fields
        public static Int32 _attempts = 100; 
        public static String _txtbText = "fJar@Se69";

        private static String _path;
        private static String _folderName;
        private static String _fileName;
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(params string[] namesOfProperties)
        {
            if (PropertyChanged != null)
            {
                foreach (var prop in namesOfProperties)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
        #endregion

        #region Showed collection
        ObservableCollection<DataChunk> empty = new ObservableCollection<DataChunk>() { };
        ObservableCollection<DataChunk> dataChunkCollection = new ObservableCollection<DataChunk>();
        Boolean _isEmpty = true;
        #endregion

        #region Constructors and inits
        public MainWindow()
        {
            InitializeComponent();
            InitPath();
            ResetStats();

            keys.Add(Key.Space, new Stopwatch());
            keys.Add(Key.Enter, new Stopwatch());
            keys.Add(Key.RightAlt, new Stopwatch());
            keys.Add(Key.LeftAlt, new Stopwatch());

            UpdateAttemptsOnTxtbInfo();
        }
        public void InitPath()
        {
            _folderName = "TypingTest";
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _folderName;
            Directory.CreateDirectory(path);
            _path = path;
            _fileName = DateTime.Now.ToString().Replace(' ', '-').Replace(':', ';') + ".txt";
        }

        public void ResetStats()
        {
            tbTypingField.Text = "";
            dataChunkCollection = new ObservableCollection<DataChunk>();

            if (_isEmpty)
                lbKeysInfo.ItemsSource = empty;
            else
                lbKeysInfo.ItemsSource = dataChunkCollection;
        }
        #endregion

        #region Keys and stopwatches
        Stopwatch next = new Stopwatch();

        Dictionary<Key, KeyInfo> keysInfo = new Dictionary<Key, KeyInfo>();

        Stopwatch spaceWatch = new Stopwatch();

        Stopwatch swnext = new Stopwatch();
        Stopwatch swhold = new Stopwatch();


        Dictionary<Key, Stopwatch> keys = new Dictionary<Key, Stopwatch>();

        Int32 textLenght;
        Stopwatch swlast = new Stopwatch();
        #endregion

        #region Catching biometric data
        private void OnTextChangedHandler(object sender, TextChangedEventArgs e)
        {
            if(tbTypingField.Text.Length != 0) //nie wylapujhe pierwszej!!
            {
                swlast.Stop();
                Debug.WriteLine($"Key {tbTypingField.Text[^1]}");
                if (tbTypingField.Text.Length > textLenght)
                {
                    //collectionWithPressedKeys.Add($"{tbTypingField.Text[^1]} speed: {swlast.ElapsedMilliseconds}");
                    dataChunkCollection.Add(new DataChunk()
                    {
                        DataType = DataType.KeyPressedSpeed, 
                        Key = tbTypingField.Text[^1].ToString(), 
                        TimeMs = swlast.ElapsedMilliseconds,
                    });
                }
                else
                {
                    //collectionWithPressedKeys.Add($"BACKSPACE speed: {swlast.ElapsedMilliseconds}");
                    dataChunkCollection.Add(new DataChunk()
                    {
                        DataType = DataType.KeyPressedSpeed,
                        Key = "BACKSPACE", 
                        TimeMs = swlast.ElapsedMilliseconds,
                    });
                }

                swlast.Reset();
                swlast.Start();

                if (tbTypingField.Text[^1] == ' ')
                    keys[Key.Space].Start();
                if (tbTypingField.Text[^1] == '\n')
                    keys[Key.Enter].Start();


                textLenght = tbTypingField.Text.Length;
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (!keys.ContainsKey(e.Key))
                keys.Add(e.Key, new Stopwatch());
            keys[e.Key].Start();
        }

        private void OnKeyUpHandler(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            Debug.WriteLine("dupasof");

            if (keys.ContainsKey(e.Key))
            { 
                keys[e.Key].Stop();
                //collectionWithPressedKeys.Add($"{e.Key} hold: {keys[e.Key].ElapsedMilliseconds}");
                dataChunkCollection.Add(new DataChunk()
                {
                    DataType = DataType.KeyHoldedTime, 
                    Key = e.Key.ToString(),
                    TimeMs = keys[e.Key].ElapsedMilliseconds,
                });
                keys[e.Key].Reset();
            }
        }
        #endregion

        #region Button press
        void cHideShowClick(object sender, RoutedEventArgs e)
        {
            if(_isEmpty)
            {
                lbKeysInfo.ItemsSource = dataChunkCollection;
                _isEmpty = false;
            }
            else
            {
                lbKeysInfo.ItemsSource = empty;
                _isEmpty = true;
            }
        }

        void cConfirmResult(object sender, RoutedEventArgs e)
        {
            List<DataChunk> dataChunksList = dataChunkCollection.ToList(); 
            List<KeyData> keyDataList = Parser.Parse(dataChunksList);

            if(!IsAttemptValid())
            {
                MessageBox.Show("Tekst zawiera błędy, bądź użyto backspace'a, proszę spróbować ponownie");
                ResetStats();
                return;
            }


            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(_path, _fileName), true))
            {
                foreach (var data in keyDataList)
                    outputFile.WriteLine(data);
                outputFile.WriteLine(":");
            }

            _attempts--;
            UpdateAttemptsOnTxtbInfo();
            ResetStats();

            if(_attempts<=0)
            {
                //using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(_path, _fileName), true))
                //    outputFile.WriteLine("$");
                MessageBox.Show("Eksperyment zakończony! Wyniki które należy przesłać znajdują się w 'Moje dokumenty' w folderze 'TypingTest'. Prześlij go osobie od której dostałeś poniższy program. ");
                Application.Current.Shutdown();
            }
            //else
            //    MessageBox.Show($"Pozostałe próby: {_attempts + 1}");
        }

        private bool IsAttemptValid()
        {
            if (tbTypingField.Text != _txtbText)
                return false;

            List<DataChunk> dataChunkList = dataChunkCollection.ToList();
            dataChunkList.RemoveAt(0);
            foreach(DataChunk dataChunk in dataChunkList)
            {
                if (dataChunk.Key == "BACKSPACE")
                    return false;
            }

            return true;
        }

        void UpdateAttemptsOnTxtbInfo()
        {
            txtbInfo.Text = $"Proszę przepisać tekst: {_txtbText} \nPozostałe próby: {_attempts}";
        }

        #endregion


    }
}