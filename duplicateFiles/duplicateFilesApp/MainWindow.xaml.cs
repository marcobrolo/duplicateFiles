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
using System.Windows.Forms;
using System.Drawing;


namespace duplicateFilesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DupFilesWrapper dupFilesObj = new DupFilesWrapper();
        private Dictionary<string, List<FileInfo>> duplicateFiles;
        private string directoryPath = "D:\\";

        public MainWindow()
        {
            InitializeComponent();
        }

        // CLICK EVEN FOR THE DUPLICATE FILE LISTBOX, CLICKING EVEN WILL SHOW THE USER
        // ALL THE PATHS TO THE DUPLICATE FILE
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBox_DupFilesInfo.Items.Clear();
            string selectedFile = listBox_DupFiles.SelectedValue.ToString();
            if (duplicateFiles.ContainsKey(selectedFile))
            {
                List<FileInfo> dupFileInfoList = duplicateFiles[selectedFile];
                foreach(var dupFileInfo in dupFileInfoList)
                {
                    //listBox_DupFilesInfo.Items.Add(GetCompactedString(dupFileInfo.FullName,32));
                    listBox_DupFilesInfo.Items.Add(dupFileInfo.FullName);
                }
            }

        }

        // DOUBLE CLICK EVENT FOR THE LIST BOX DISPLAYING ALL THE PATHS TO A SELECTED DUPLICATE FILE
        // DOUBLE CLICKING THIS WOULD OPEN UP EXPLORER TO THE DIRECTORY OF THE SELECTED PATH
        private void dupFileListBoxDblClick(Object sender, System.Windows.Input.MouseEventArgs e)
        {
            Console.WriteLine("click");
            string selectedFilePath = listBox_DupFilesInfo.SelectedValue.ToString();


            System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(selectedFilePath));

        }

        // FUNCTION TO ALLOW USERS TO SELECT THE DIRECTORY TO RUN THE PROGRAM IN
        private void btnBrowseDir(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Console.WriteLine(dialog.SelectedPath);
                directoryPath = dialog.SelectedPath.Replace(@"\", @"\\");
            }
            
        }

        // FUNCTION TO INITIATE THE DUPLICATE FILE SEARCH
        private void btnFindDup(object sender, RoutedEventArgs e)
        {
            populateDupFiles();
        }

        private void populateDupFiles()
        {
            duplicateFiles = dupFilesObj.getDuplicateFiles(directoryPath);

            foreach (var dupFile in duplicateFiles.Keys)
            {
                listBox_DupFiles.Items.Add(dupFile);
            }
        }

        // COMPACT STRING FUNCTION FOUND FROM 
        // http://stackoverflow.com/questions/1764204/how-to-display-abbreviated-path-names-in-net
        // USED TO SHORTEN THE DIPLAYING OF THE STRING, HOWEVER MIGHT NOT BE USED SINCE WE WANT TO 
        // USE FULL PATH FOR THE DOUBLE CLICK EVEN
        private static string GetCompactedString(
        string stringToCompact, int maxWidth)
        {
            // Copy the string passed in since this string will be
            // modified in the TextRenderer's MeasureText method
            Font font = new Font("Arial", 14, System.Drawing.FontStyle.Regular);

            string compactedString = string.Copy(stringToCompact);
            var maxSize = new System.Drawing.Size(maxWidth, 0);
            var formattingOptions = TextFormatFlags.PathEllipsis
                                  | TextFormatFlags.ModifyString;
            TextRenderer.MeasureText(compactedString, font, maxSize, formattingOptions);
            return compactedString;
        }
    }
}
