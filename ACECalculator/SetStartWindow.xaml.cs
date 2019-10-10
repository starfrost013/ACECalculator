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

namespace ACECalculator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SetStartWindow : Window
    {

        MainWindow MnWindow;

        public SetStartWindow(MainWindow MainWindow)
        {
            InitializeComponent();
            MnWindow = MainWindow;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateTime = new DateTime(Convert.ToInt32(YearBox.Text), Convert.ToInt32(MonthBox.Text), Convert.ToInt32(DateBox.Text), Convert.ToInt32(TimeBox.Text), 0, 0);
                MnWindow.CurrentDateTime = dateTime;

                int addBackFactor = 0;
                for (int i = 0; i < MnWindow.StormIntensities.Items.Count; i++)
                {
                    StormIntensityNode sin = (StormIntensityNode)MnWindow.StormIntensities.Items[i]; // subtract 6 each time.
                    sin.DateTime = MnWindow.CurrentDateTime.ToString();
                    MnWindow.CurrentDateTime = MnWindow.CurrentDateTime.AddHours(6);
                    addBackFactor += 6;

                }
                MnWindow.StormIntensities.Items.Refresh();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Error: An invalid date or time was entered.", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: An invalid date or time was entered.", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            this.Close(); // closes the window which implicitly destroys the class and thus makes it not unsafe hopefully!
        }
    }
}
