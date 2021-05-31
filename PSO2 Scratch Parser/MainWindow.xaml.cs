using System;
using System.Collections.Generic;
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
using System.Windows.Forms;

namespace PSO2_Scratch_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string AppStatus { get; set; }
        public string ContentStatus { get; set; }
        private readonly ScratchList ScratchList;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            ContentStatus = "Status: Nothing";
            AppStatus = "PSO2 Scratch Parser";

            //ScratchList scratchData = ScratchList.parseFromHTMLFile(@"C:\Users\Jimmy\Downloads\AC・SG・FUNスクラッチ _ 『PSO2』アイテムカタログ.html");
            //ScratchList scratchData = ScratchList.parseFromWebsiteURL(@"http://pso2.jp/players/catalog/scratch/ac/20210519/");
            ScratchList = new ScratchList();
        }

        public void parseHTMLBtn(Object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "HTML files (*.htm,*.html)|*htm;*.html|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ScratchList.parseFromHTMLFile(openFileDialog.FileName);
                UpdateParseControls();
            }
        }

        public void parseURLBtn(Object sender, EventArgs e)
        {
            AskUrlDialogWindow askUrlDialogWindow = new AskUrlDialogWindow();

            if (askUrlDialogWindow.ShowDialog() == true)
            {
                if (askUrlDialogWindow.URL.Length == 0)
                {
                    System.Windows.MessageBox.Show("URL needed", "Missing Scratch URL", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ScratchList.parseFromWebsiteURL(askUrlDialogWindow.URL);
                UpdateParseControls();
            }
        }

        public void saveScratchListBtn(Object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = "JSON (*.json)|*.json|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ScratchList.Write(saveFileDialog.FileName);
            }
        }

        public void downloadImageOriginalNameBtn(Object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var downloadDirectory = dialog.SelectedPath;
                    ScratchList.SaveImages(downloadDirectory, ImageNameOption.Original);
                    System.Windows.Forms.MessageBox.Show("Finish Downloading");
                }
            }
        }

        public void downloadImageJPNameBtn(Object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var downloadDirectory = dialog.SelectedPath;
                    ScratchList.SaveImages(downloadDirectory, ImageNameOption.Japanese);
                    System.Windows.Forms.MessageBox.Show("Finish Downloading");
                }
            }
        }

        public void clearScratchListBtn(Object sender, EventArgs e)
        {
            ScratchList.Clear();
            UpdateParseControls();
        }

        public void UpdateParseControls()
        {
            var isEnabled = ScratchList != null && ScratchList.Count != 0;
            downloadJPImageBtn.IsEnabled = isEnabled;
            downloadOriginalImageBtn.IsEnabled = isEnabled;
            saveBtn.IsEnabled = isEnabled;
            clearBtn.IsEnabled = isEnabled;
        }
    }
}
