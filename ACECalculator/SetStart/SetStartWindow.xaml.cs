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
                // V1.4: If it ain't broke, don't fix it! (build 222)
                
                if (Ver14_DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Error: You must select a date!", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MnWindow.CurrentDateTime = (DateTime)Ver14_DatePicker.SelectedDate;

                // add the time

                MnWindow.CurrentDateTime = MnWindow.CurrentDateTime.AddHours(Convert.ToInt32(Ver14_HoursBox.Text));
                MnWindow.CurrentDateTime = MnWindow.CurrentDateTime.AddMinutes(Convert.ToInt32(Ver14_HoursBox.Text));


                int addBackFactor = 0; 
                for (int i = 0; i < MnWindow.StormIntensities.Items.Count; i++)
                {
                    StormIntensityNode sin = (StormIntensityNode)MnWindow.StormIntensities.Items[i]; // subtract 6 each time.
                    sin.DateTime = MnWindow.CurrentDateTime;
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
                return; 
            }

            this.Close(); // closes the window which implicitly destroys the class and thus makes it not unsafe hopefully!
        }

    }
}
