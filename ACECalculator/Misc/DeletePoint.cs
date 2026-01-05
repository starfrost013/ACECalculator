using System;

namespace ACECalculator
{
    public partial class MainWindow
    {
        private void DeleteSelectedItems()
        {
            Int32 numRemoved = 0;
            Int32 lastDeletedItem = 0;

            double tempACE = 0;

            // loop through everything
            for (int i = 0; i < StormIntensities.SelectedItems.Count; i++)
            {
                //V1.4: new version - support deleting multiple items

                StormIntensityNode sinTemp = (StormIntensityNode)StormIntensities.SelectedItems[i]; // cast...
                Int32 index = StormIntensities.Items.IndexOf(sinTemp);

                if (index > lastDeletedItem)
                    lastDeletedItem = index;

                tempACE += sinTemp.ACE;
                StormIntensities.Items.Remove(sinTemp); // remove the item at the selected index. Yay.
                i--;
                numRemoved++;
            }

            for (int i = 0; i < StormIntensities.Items.Count; i++)
            {
                StormIntensityNode sin = (StormIntensityNode)StormIntensities.Items[i];

                if (i > lastDeletedItem)
                {
                    sin.Total -= tempACE;
                    sin.DateTime = sin.DateTime.AddHours((-6 * numRemoved)); // yeah
                }

            }

            StormIntensities.Items.Refresh();
        }
    }
}
