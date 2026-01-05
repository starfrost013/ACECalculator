using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace ACECalculator
{
    /// <summary>
    /// starfrost's ACE Calculator
    /// 
    /// Originally created October 9, 2019 (v1.4.1: updated August 18, 2021 to update credits)
    /// 
    /// This is a very poorly written mess. One day I'll rewrite it.
    /// 
    /// Don't use this code as a tutorial!
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<StormIntensityNode> IntensityList { get; set; }

        //public int AllowSub34Kt { get; set; } // Maybe later.

        public enum IntensityMeasureType
        { 
            Knots = 0,
            Mph = 1,
        }

        public IntensityMeasureType IntensityMeasure { get; set; } // 0 = knots, 1 = mph

        public double TotalACE { get; set; } // yes

        public bool SinglePoint { get; set; } // Single Point Mode enabled

        public bool DateTimeOn { get; set; }

        public DateTime CurrentDateTime { get; set; }

        private Int32 easterEggClicks;

        public MainWindow()
        {
            InitializeComponent();
            IntensityList = new List<StormIntensityNode>();
            DateTimeOn = true; // bypasses checks
            CurrentDateTime = DateTime.Now;
            SetDateTimeVisibility(false);
        }

        private void ItCalculatesAce_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ItCalculatesAce.Text = "ã"; 
        }

        private void AddStorm_Click(object sender, RoutedEventArgs e)
        {
            AddPoint();
        }

        private void StormMenu_Reset_Click(object sender, RoutedEventArgs e)
        {
            StormIntensities.Items.Clear();
        }

        private void StormMenu_IntensityKt_Click(object sender, RoutedEventArgs e)
        {
            // return if already active
            if (IntensityMeasure == IntensityMeasureType.Knots)
            {
                StormMenu_IntensityKt.IsChecked = true;
                return;
            }

            StormMenu_IntensityMph.IsChecked = false;
            EnterStormIntensityLabel.Text = "Enter storm intensity (in knots)..."; // change the content of the enter intensity label to reflect the new measurement of wind speed.
            // convert everything

            foreach (StormIntensityNode sin in StormIntensities.Items)
            {
                sin.Intensity /= 1.151;
                sin.Intensity = RoundNearest(sin.Intensity, 5); // round it.
            }
            StormIntensities.Items.Refresh();

            IntensityMeasure = IntensityMeasureType.Knots;
        }

        private void StormMenu_IntensityMph_Click(object sender, RoutedEventArgs e)
        {
            // return if already active
            if (IntensityMeasure == IntensityMeasureType.Mph)
            {
                StormMenu_IntensityKt.IsChecked = true;
                return;
            }

            StormMenu_IntensityKt.IsChecked = false;
            EnterStormIntensityLabel.Text = "Enter storm intensity (in mph)..."; // change the content of the enter intensity label to reflect the new measurement of wind speed.

            foreach (StormIntensityNode sin in StormIntensities.Items)
            {
                sin.Intensity *= 1.151;
                sin.Intensity = RoundNearest(sin.Intensity, 5); // round it to the nearest 5mph/5kt
            }

            StormIntensities.Items.Refresh();
            IntensityMeasure = IntensityMeasureType.Mph;

        }

        private void FileMenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void StormMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (StormIntensities.SelectedIndex == -1) // do we not have anything selected?
            {
                MessageBox.Show("Error: Cannot delete a point if there are no points selected!", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning); // show a warning box
                return; // don't do anything
            }

            DeleteSelectedItems();
        }

        private void HelpMenu_About_Click(object sender, RoutedEventArgs e)
        {
            // brings up the about window.
            AboutWindow AboutWindow = new AboutWindow();
            AboutWindow.Show(); // show the about window.
        }

        private void HelpMenu_Open_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("readme.txt");
        }

        private void StormMenu_EditSelected_Click(object sender, RoutedEventArgs e)
        {
            // Shows the edit storm window.

            // if nothing is selected show an error message
            if (StormIntensities.SelectedIndex == -1)
            {
                MessageBox.Show("Error: Can't edit a point when there is nothing selected to edit!", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            EditStorm EditStorm = new EditStorm();
            EditStorm.ShowDialog();
        }

        private void SinglePointMode_Click(object sender, RoutedEventArgs e)
        {
            SinglePoint = SinglePointMode.IsChecked;
        }

        private void StormMenu_CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (StormIntensities.Items.Count == 0)
                return;

            StormIntensityNode temp = (StormIntensityNode)StormIntensities.Items[StormIntensities.Items.Count - 1]; // get the last item.
            Clipboard.SetText(temp.Total.ToString()); // set the clipboard text to the current total ACE
        }

        // Opens the set start date window.
        private void StormMenu_SetStartDate_Click(object sender, RoutedEventArgs e)
        {
            SetDateTimeVisibility(StormMenu_SetStartDate.IsChecked);
            // temporarily commented out for testing 

            if (StormMenu_SetStartDate.IsChecked)
            {
                SetStartWindow setStartWindow = new SetStartWindow(this);
                setStartWindow.ShowDialog();
            }
        }

        private void EnterKt_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                AddPoint();
                EnterKt.Text = string.Empty;
            }
        }

        private void FileMenu_Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Enter path for export",
                    DefaultExt = ".txt",
                    Filter = "Text files (*.txt)|*.txt"
                };

                saveFileDialog.ShowDialog();

                List<string> lines = new List<string>();

                foreach (StormIntensityNode sin in StormIntensities.Items)
                {
                    // write.
                    switch (IntensityMeasure)
                    {
                        case IntensityMeasureType.Knots: // The user selected knots.
                            lines.Add($"{sin.DateTime} {sin.Intensity} KT - ACE: {sin.ACE} Total: {sin.Total}");
                            continue;
                        case IntensityMeasureType.Mph: // The user selected mph.
                            lines.Add($"{sin.DateTime} {sin.Intensity} MPH - ACE: {sin.ACE} Total: {sin.Total}");
                            continue;
                    }
                }

                string[] linesArray = lines.ToArray();
                Clipboard.SetText(new StringBuilder().Append(linesArray).ToString());

                File.WriteAllLines(saveFileDialog.FileName, linesArray);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred when writing to the file: \n\n" + ex, "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void StormIntensities_RightClick_Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedItems();
        }

        private void StormIntensities_RightClick_Edit_Click(object sender, RoutedEventArgs e)
        {
            // my old code was so ugly lol
            EditStorm EStorm = new EditStorm(); 
            EStorm.Owner = this;
            EStorm.Show(); 
        }

        private void ByStarfrost_Click(object sender, RoutedEventArgs e)
        {
            easterEggClicks++;

            if (easterEggClicks == 10)
            {
                ByStarfrost.TextWrapping = TextWrapping.Wrap;   
                ByStarfrost.Text = "+☺6¶♂,▓TÈ-♦Û¶─N8‼4*┬♣•♦☺♠1♠194D411494♀123119321☻198498♦6\"5ð4@lj8/9$♂85#BfY@X6Û374ß                   ☻46 segmentation fault - core dumped\r\n";
                ByStarfrost.Height += 200;
                ByStarfrost.Width += 250;
                ByStarfrost.Margin = new Thickness(ByStarfrost.Margin.Left - 200,
                ByStarfrost.Margin.Top,
                ByStarfrost.Margin.Right,
                ByStarfrost.Margin.Bottom);

                Height += 50;
            }
        }
    }
}
