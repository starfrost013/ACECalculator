using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACECalculator
{
    partial class MainWindow
    {
        public double GenACE(double intensity, int mode)
        {
            double ace;
            switch (mode)
            {
                case 0:

                    ace = Math.Pow(intensity, 2) / 10000;
                    return ace;
                case 1:
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
            if (visible == false)
            {
                if (this.DateTimeOn == false)
                {
                    return;
                }
                // is it 0?
                this.DateTimeOn = false;
                this.Width -= 50;
                ByCosmoForHHW.Margin = new Thickness(ByCosmoForHHW.Margin.Left - 50, ByCosmoForHHW.Margin.Top, ByCosmoForHHW.Margin.Right, ByCosmoForHHW.Margin.Bottom); // set the position
                StormIntensities.Width -= 125;
                StormIntensities_DateTime.Width = 0;
            }
            else
            {
                if (this.DateTimeOn == true) // if its the same, prevent repeats.
                {
                    return;
                }
                this.DateTimeOn = true;
                this.Width += 50;
                ByCosmoForHHW.Margin = new Thickness(ByCosmoForHHW.Margin.Left + 50, ByCosmoForHHW.Margin.Top, ByCosmoForHHW.Margin.Right, ByCosmoForHHW.Margin.Bottom);
                StormIntensities.Width += 125;
                StormIntensities_DateTime.Width = 125;
            }
        }

        public void AddPoint()
        {
            try
            {
                double intensity = Convert.ToDouble(EnterKt.Text);
                double ace = 0;
                double t = 0;

                
                StormIntensityNode node = new StormIntensityNode { DateTime = CurrentDateTime, Intensity = intensity, ACE = ace, Total = t };
                CurrentDateTime = CurrentDateTime.AddHours(6);

                if (IntensityMeasure == 0)
                {
                    node.ACE = GenACE(node.Intensity, 0); // calculate the ACE

                    if (intensity < 34)
                    {
                        node.ACE = 0;
                    }
                }
                else
                {

                    node.ACE = GenACE(node.Intensity, 1); // convert to knots 

                    if (intensity < 39)
                    {
                        node.ACE = 0;
                    }

                }
                if (SinglePoint == 0)
                {
                    foreach (StormIntensityNode sin in StormIntensities.Items)
                    {
                        TotalACE = TotalACE + sin.ACE; // add the ace of every node to each system
                    }

                    if (StormIntensities.Items.Count == 0)
                    {
                        TotalACE = node.ACE; // fixes this bug
                        node.Total = TotalACE;
                    }
                    else
                    {
                        node.Total = TotalACE + node.ACE;
                    }

                }
                // fix bug.
                else
                {
                    StormIntensities.Items.Clear();
                    TotalACE = TotalACE + node.ACE;
                    node.Total = TotalACE;
                }

                StormIntensities.Items.Add(node);


                if (SinglePoint == 0)
                {
                    TotalACE = 0; // dont do this if we are in single point mode
                }
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
