using System;
using System.Windows;

namespace ACECalculator
{
    partial class MainWindow
    {
        public double GenACE(double intensity)
        {
            double ace;
            switch (IntensityMeasure)
            {
                case IntensityMeasureType.Knots:
                    ace = Math.Pow(intensity, 2) / 10000;
                    return ace;
                case IntensityMeasureType.Mph:
                    ace = Math.Pow(RoundNearest(intensity / 1.15078, 5), 2) / 10000;
                    return ace;
                default:
                    return 0;
            }
        }

        private static double RoundNearest(double raw, double n)
        {
            return (Math.Round(raw / n)) * n;
        }

        public void SetDateTimeVisibility(bool visible)
        {
            if (!visible)
            {
                if (!DateTimeOn)
                    return;

                // is it 0?
                DateTimeOn = false;
                Width -= 50;
                ByStarfrost.Margin = new Thickness(ByStarfrost.Margin.Left - 50, ByStarfrost.Margin.Top, ByStarfrost.Margin.Right, ByStarfrost.Margin.Bottom); // set the position
                StormIntensities.Width -= 125;
                StormIntensities_DateTime.Width = 0;
            }
            else
            {
                if (DateTimeOn)
                    return;

                DateTimeOn = true;
                Width += 50;
                ByStarfrost.Margin = new Thickness(ByStarfrost.Margin.Left + 50, ByStarfrost.Margin.Top, ByStarfrost.Margin.Right, ByStarfrost.Margin.Bottom);
                StormIntensities.Width += 125;
                StormIntensities_DateTime.Width = 125;
            }
        }

        public void AddPoint()
        {
            try
            {
                double intensity = Convert.ToDouble(EnterKt.Text);
                
                StormIntensityNode node = new StormIntensityNode { DateTime = CurrentDateTime, Intensity = intensity, ACE = 0, Total = 0 };
                CurrentDateTime = CurrentDateTime.AddHours(6);

                node.ACE = GenACE(node.Intensity); // calculate the ACE

                if (IntensityMeasure == IntensityMeasureType.Knots
                    && intensity < 34)
                {
                    node.ACE = 0;
                }
                else if (IntensityMeasure == IntensityMeasureType.Mph
                    && intensity < 39)
                {
                    node.ACE = 0;
                }

                if (!SinglePoint)
                {
                    foreach (StormIntensityNode sin in StormIntensities.Items)
                        TotalACE += sin.ACE; // add the ace of every node to each system

                    if (StormIntensities.Items.Count == 0)
                    {
                        TotalACE = node.ACE; // fixes this bug
                        node.Total = TotalACE;
                    }
                    else
                        node.Total = TotalACE + node.ACE;

                }
                // fix bug.
                else
                {
                    StormIntensities.Items.Clear();
                    TotalACE = TotalACE + node.ACE;
                    node.Total = TotalACE;
                }

                StormIntensities.Items.Add(node);

                if (!SinglePoint)
                    TotalACE = 0; // dont do this if we are in single point mode

                return;
            }
            catch (FormatException) // someone entered gibberish
            {
                MessageBox.Show("Error: You must input a number.", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}
