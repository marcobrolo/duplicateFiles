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
using System.Windows.Shapes;
using duplicateFilesApp;
using System.IO;


namespace duplicateFilesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DupFilesWrapper dupFilesObj = new DupFilesWrapper();
        private Dictionary<string, List<FileInfo>> duplicateFiles;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBox_DupFilesInfo.Items.Clear();
            string selectedFile = listBox_DupFiles.SelectedValue.ToString();
            if (duplicateFiles.ContainsKey(selectedFile))
            {
                List<FileInfo> dupFileInfoList = duplicateFiles[selectedFile];
                foreach(var dupFileInfo in dupFileInfoList)
                {
                    listBox_DupFilesInfo.Items.Add(dupFileInfo.FullName);
                }
            }

        }

        private void btnBrowseDir(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        }

        private void btnFindDup(object sender, RoutedEventArgs e)
        {
            populateDupFiles();
        }

        private void populateDupFiles()
        {
            duplicateFiles = dupFilesObj.getDuplicateFiles();

            foreach (var dupFile in duplicateFiles.Keys)
            {
                listBox_DupFiles.Items.Add(dupFile);
            }
            
            
        }
    }
}
