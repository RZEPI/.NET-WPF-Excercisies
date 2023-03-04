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
using System.Windows.Shapes;
using System.IO;

namespace lab2
{
    /// <summary>
    /// Interaction logic for CreateFileWindow.xaml
    /// </summary>
    public partial class CreateFileWindow : Window
    {
        private string _path;
        public CreateFileWindow(string path)
        {
            InitializeComponent();
            _path = path;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameOfItem.Text;
            string newElementPath = _path + "\\" + name;
            if(directoryRadio.IsChecked == true)
            {
                Directory.CreateDirectory(newElementPath);
            }
            if(fileRadio.IsChecked == true)
            {
                string pattern = @"^[\w-~]{1,8}\.(txt|php|html)$";
                if (System.Text.RegularExpressions.Regex.IsMatch(name, pattern))
                {
                    File.Create(newElementPath);

                    FileAttributes attributes = File.GetAttributes(newElementPath);

                    if ((bool)ReadOnlyChkBox.IsChecked)
                        attributes |= FileAttributes.ReadOnly;
                        
                    if ((bool)SystemChkBox.IsChecked)
                        attributes |= FileAttributes.System;
                    
                    if ((bool)ArchiveChkBox.IsChecked)
                        attributes |= FileAttributes.Archive;

                    if ((bool)HiddenChkBox.IsChecked)
                        attributes |= FileAttributes.Hidden;
                    
                    File.SetAttributes(newElementPath, attributes);
                    Close();
                }
                else
                    MessageBox.Show("Błąd: nieprawidłowa nazwa pliku!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
