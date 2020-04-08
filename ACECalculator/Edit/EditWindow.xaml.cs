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
    public partial class EditStorm : Window
    {
        MainWindow MnWindow;
        public EditStorm(MainWindow MainWindow) // shitty i know
        {
            InitializeComponent();
            MnWindow = MainWindow;
            StormIntensityNode mostrecent = (StormIntensityNode)MnWindow.StormIntensities.Items[MnWindow.StormIntensities.SelectedIndex];
            IntensityTextBox.Text = mostrecent.Intensity.ToString();
            mostrecent = null; // populates the textbox with the most recent intensity
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            for (int i = 0; i < MnWindow.StormIntensities.Items.Count; i++) // oof
            {
                StormIntensityNode sin = (StormIntensityNode)MnWindow.StormIntensities.Items[i];
                if (MnWindow.StormIntensities.SelectedIndex == i)
                {
                    try
                    {
                        double origIntensity = sin.Intensity;
                        sin.Intensity = Convert.ToDouble(IntensityTextBox.Text);
                        double origACE = sin.ACE;
                        
                        sin.ACE = MnWindow.GenACE(sin.Intensity, MnWindow.IntensityMeasure); // todo: check mode.
                        // loop through everything

                        double tempTotal = sin.Total - origACE; // so it doesn't add the original intensity in addition to the new.  

                        for (int j = MnWindow.StormIntensities.SelectedIndex; j < MnWindow.StormIntensities.Items.Count; j++)
                        {
                            StormIntensityNode sin_shittycode = (StormIntensityNode)MnWindow.StormIntensities.Items[j];
                            // ADD updateall function.
                            tempTotal += sin_shittycode.ACE; // this is a mess but it works.
                            sin_shittycode.Total = tempTotal;
                        }

                        MnWindow.StormIntensities.Items.Refresh();
                        this.Close();
                        return;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Error: Cannot change the intensity to something that is not a number.", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
    }
}
