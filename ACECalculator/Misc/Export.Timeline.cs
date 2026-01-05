using Microsoft.Win32; 
using System.IO;
using System.Windows;

namespace ACECalculator
{
    public partial class MainWindow
    {

        //DO NOT USE 
        public void Export_Timeline()
        {
            try
            {

                if (!DateTimeOn)
                {
                    MessageBox.Show("You must have date and time on to generate a timeline.", "ACE Calculator", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Title = "Save for export to Wikia timeline format",
                    Filter = "Text documents (*.txt)|.txt"
                };
                
                sfd.ShowDialog();

                if (sfd.FileName != "")
                {
                    // write the wikia timeline syntax to the file

                    if (File.Exists(sfd.FileName))
                    {
                        File.Delete(sfd.FileName); // prevent overwrite
                    }

                    using (StreamWriter sw = new StreamWriter(new FileStream(sfd.FileName, FileMode.OpenOrCreate)))
                    {
                        sw.WriteLine("<div style=\"overflow: auto; \"><timeline>");
                        sw.WriteLine("ImageSize = width:800 height:240"); // the size of the timeline
                        sw.WriteLine("PlotArea  = top:10 bottom:80 right:20 left:20"); // the plot area of the timeline
                        sw.WriteLine("Legend    = columns:3 left:30 top:58 columnwidth:270"); // the plot area of the timeline
                        sw.WriteLine("AlignBars = early"); // bar alignment

                        // section 2: definition part 2

                        sw.WriteLine("DateFormat = dd/mm/yyyy"); // date format
                        sw.WriteLine("Period     = from:01/11/2019 till:01/05/2020"); // period of the timeline
                        sw.WriteLine("TimeAxis   = orientation:horizontal"); // orientation of the timeline
                        sw.WriteLine("ScaleMinor = grid:black unit:month increment:1 start:01/11/2019"); // aaa

                        // section 3: colour definitions
                        sw.WriteLine("Colors = "); // color definitions
                        sw.WriteLine("  id:canvas value:gray(0.88)"); // background colour
                        sw.WriteLine("  id:GP     value:red"); // ???
                        sw.WriteLine("  id:TD     value:rgb(0.38,0.73,1)  legend:Tropical_Depression_=_&lt;39_mph_(0-62_km/h)"); // TD background colour
                        sw.WriteLine("  id:TS     value:rgb(0,0.98,0.96)  legend:Tropical_Storm_=_39-73_mph_(63-117 km/h)"); // TS background colour
                        sw.WriteLine("  id:C1     value:rgb(1,1,0.80)     legend:Category_1_=_74-95_mph_(119-153_km/h)"); // C1 background colour
                        sw.WriteLine("  id:C2     value:rgb(1,0.91,0.46)  legend:Category_2_=_96-110_mph_(154-177_km/h)"); // C2 background colour
                        sw.WriteLine("  id:C3     value:rgb(1,0.76,0.25)  legend:Category_3_=_111-130_mph_(178-209-km/h)"); // C3 background colour
                        sw.WriteLine("  id:C4     value:rgb(1,0.56,0.13)  legend:Category_4_=_131-155_mph_(210-249_km/h)"); // C4 background colour
                        sw.WriteLine("  id:C5     value:rgb(1,0.91,0.46)  legend:Category_2_=_96-110_mph_(154-177_km/h)"); // C5 background colour

                        // section 4: BG defs
                        sw.WriteLine("Backgroundcolors = canvas:canvas");

                        // section 5: bar data
                        sw.WriteLine("BarData ="); // bar data
                        sw.WriteLine("  barset:Hurricane");

                        // section 6: plot data
                        sw.WriteLine("  bar:Month");
                    }
                }
                else
                {
                    return;
                }
            }
            catch (IOException err)
            {
                MessageBox.Show($"An error occurred while exporting to EasyTimeline format. An IOException has occurred. We're sorry, but please restart. Thanks.\n\n{err}");
                Application.Current.Shutdown(-2);
            }
        }
    }
}
