using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace ACECalculator
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Assembly Assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo FileVersion = FileVersionInfo.GetVersionInfo(Assembly.Location); // get this program's location
            Version.Text = $"Version {FileVersion.ProductVersion} (build {FileVersion.FileBuildPart})";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            AbtWindow.Close();
        }

        private void Version_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Version.Text = "8/30/21";
        }

        // Aug 18 2021
        // Didn't expect to be updating this again but it's used on the hhw resources page 
        // so we need to do some changes (to fix dead links old names etc)
        private void StartWebsite(object Sender, RequestNavigateEventArgs e) => Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
    }
}
