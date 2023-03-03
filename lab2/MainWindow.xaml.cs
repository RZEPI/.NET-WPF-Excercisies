
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Linq;

namespace lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var rootFolder = new DirectoryInfo(dialog.SelectedPath);
                var rootNode = new TreeViewItem { Header = rootFolder.Name, Tag = rootFolder.FullName };
                LoadFolders(rootNode, rootFolder);
                treeView.Items.Add(rootNode);
            }
        }

        private void LoadFolders(TreeViewItem parentNode, DirectoryInfo directory)
        {
            foreach (var subdir in directory.GetDirectories())
            {
                var subnode = new TreeViewItem { Header = subdir.Name, Tag = subdir.FullName };
                subnode.MouseRightButtonDown += TreeViewItem_MouseRightButtonDown;
                LoadFolders(subnode, subdir);
                parentNode.Items.Add(subnode);
            }

            foreach (var file in directory.GetFiles())
            {
                var subnode = new TreeViewItem { Header = file.Name, Tag = file.FullName };
                subnode.MouseRightButtonDown += TreeViewItem_MouseRightButtonDown;
                parentNode.Items.Add(subnode);
            }
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedTreeViewItem = treeView.SelectedItem as TreeViewItem;
            var parentOfSelectedTreeViewItem = selectedTreeViewItem.Parent as TreeViewItem;

            if (selectedTreeViewItem != null)
            {
                string tag = selectedTreeViewItem.Tag as string;

               
            }
        }

        private void TreeViewItem_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var treeViewItem = sender as TreeViewItem;

            
            if (treeViewItem != null)
            {
                string tag = treeViewItem.Tag as string;
                
                treeViewItem.Focus();
                e.Handled = true;
                var contextMenu = new ContextMenu();
                
                if (File.Exists(tag) )
                {
                    var fileOpenMenu = new MenuItem { Header = "Open" };
                    
                    fileOpenMenu.Click += OpenFile_Click;
                    contextMenu.Items.Add(fileOpenMenu);

                    var fileDeleteMenu = new MenuItem { Header = "Delete" };
                    fileDeleteMenu.Click += DeleteFile_Click;
                    contextMenu.Items.Add(fileDeleteMenu);
                }else if( Directory.Exists(tag))
                {
                    var createDirectoryMenu = new MenuItem { Header = "Create" };
                    createDirectoryMenu.Click += CreateDirectory_Click;
                    contextMenu.Items.Add(createDirectoryMenu);

                    var deleteDirectoryMenu = new MenuItem { Header = "Delete" };
                    deleteDirectoryMenu.Click += DeleteDirectory_Click;
                    contextMenu.Items.Add(deleteDirectoryMenu);
                }
                contextMenu.PlacementTarget = treeViewItem;
                treeViewItem.ContextMenu = contextMenu;
                treeViewItem.ContextMenu.IsOpen = true;
            }
        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var treeViewItem =  ((sender as MenuItem)?.Parent as ContextMenu)?.PlacementTarget as TreeViewItem;
            if(treeViewItem != null)
            {
                System.Diagnostics.Process.Start(treeViewItem.Tag as string);
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            var treeViewItem = ((sender as MenuItem)?.Parent as ContextMenu)?.PlacementTarget as TreeViewItem;
            
            if (treeViewItem != null)
            {
                string tag = treeViewItem.Tag as string;
                FileAttributes attributes = File.GetAttributes(tag);

                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    attributes &= ~FileAttributes.ReadOnly;
                    File.SetAttributes(tag, attributes);
                }
                File.Delete(treeViewItem.Tag as string);

                RefreshTreeView(treeView.Items.Cast<TreeViewItem>().FirstOrDefault().Tag as string);
            }
        }

        private void CreateDirectory_Click(object sender, RoutedEventArgs e)
        {
        }
        private void DeleteDirectory_Click(object sender, RoutedEventArgs e)
        {
            var treeViewItem = ((sender as MenuItem)?.Parent as ContextMenu)?.PlacementTarget as TreeViewItem;
            if (treeViewItem != null)
            {
                DirectoryInfo directory = new DirectoryInfo(treeViewItem.Tag as string);
                foreach (var file in directory.GetFiles())
                    file.Delete();
                Directory.Delete(treeViewItem.Tag as string);

                RefreshTreeView(treeView.Items.Cast<TreeViewItem>().FirstOrDefault().Tag as string);
            }
        }
        private void RefreshTreeView(string pathToRootFolder)
        {
            treeView.Items.Clear();
            var rootFolder = new DirectoryInfo(pathToRootFolder);
            var rootNode = new TreeViewItem { Header = rootFolder.Name, Tag = rootFolder.FullName };
            LoadFolders(rootNode, rootFolder);
            treeView.Items.Add(rootNode);
        }
    }
}
