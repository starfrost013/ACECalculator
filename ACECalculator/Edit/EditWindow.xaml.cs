using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACECalculator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditStorm : Window
    {
        MainWindow mnWindow;

        public EditStorm()
        {
            InitializeComponent();
            mnWindow = (MainWindow)Owner;
            StormIntensityNode mostrecent = (StormIntensityNode)mnWindow.StormIntensities.Items[mnWindow.StormIntensities.SelectedIndex];
            IntensityTextBox.Text = mostrecent.Intensity.ToString();
            mostrecent = null; // populates the textbox with the most recent intensity
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            for (int i = 0; i < mnWindow.StormIntensities.Items.Count; i++) // oof
            {
                StormIntensityNode sin = (StormIntensityNode)mnWindow.StormIntensities.Items[i];
                if (mnWindow.StormIntensities.SelectedIndex == i)
                {
                    try
                    {
                        double origIntensity = sin.Intensity;
                        sin.Intensity = Convert.ToDouble(IntensityTextBox.Text);
                        double origACE = sin.ACE;
                        
                        sin.ACE = mnWindow.GenACE(sin.Intensity); // todo: check mode.
                        // loop through everything

                        double tempTotal = sin.Total - origACE; // so it doesn't add the original intensity in addition to the new.  

                        for (int j = mnWindow.StormIntensities.SelectedIndex; j < mnWindow.StormIntensities.Items.Count; j++)
                        {
                            StormIntensityNode sin_shittycode = (StormIntensityNode)mnWindow.StormIntensities.Items[j];
                            // ADD updateall function.
                            tempTotal += sin_shittycode.ACE; // this is a mess but it works.
                            sin_shittycode.Total = tempTotal;
                        }

                        mnWindow.StormIntensities.Items.Refresh();
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
