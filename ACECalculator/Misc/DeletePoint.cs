using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACECalculator
{
    public partial class MainWindow
    {
        private void DeleteSingleItem()
        {
            int s = 0;
            double tempACE = 0;
            bool deleted = false;

            // loop through everything
            for (int i = 0; i < StormIntensities.Items.Count; i++)
            {
                //V1.4: new version - support deleting multiple items

                if (StormIntensities.SelectedIndex == i) // if it is the same as the selected index
                {
                    s = StormIntensities.SelectedIndex;
                    StormIntensityNode sinTemp = (StormIntensityNode)StormIntensities.Items[i]; // cast...
                    tempACE = sinTemp.ACE;
                    deleted = true;
                    StormIntensities.Items.RemoveAt(i); // remove the item at the selected index. Yay.
                }

                if (deleted == true)
                {
                    if (i > s)
                    {
                        StormIntensityNode sin = (StormIntensityNode)StormIntensities.Items[i];

                        sin.Total -= tempACE;
                        sin.DateTime = sin.DateTime.AddHours(-6); // yeah
                    }
                }
            }

            StormIntensities.Items.Refresh();
        }

        private void DeleteMultipleItems(System.Collections.IList ItemsToRemove)
        {
            //v1.4 only
            double tempACE = 0;

            for (int i = 0; i < ItemsToRemove.Count - 1; i++)
            {
                StormIntensityNode SNode_ToBeDeleted = (StormIntensityNode)ItemsToRemove[i];

                StormIntensities.Items.Remove(SNode_ToBeDeleted);

                for (int j = 0; j < StormIntensities.Items.Count - 1; j++)
                {
                    if (j > i)
                    {
                        StormIntensityNode SNode_Change = (StormIntensityNode)StormIntensities.Items[j]; // the node to change
                        SNode_Change.Total -= tempACE;

                        if (SNode_Change.DateTime.Date != new DateTime(0001, 1, 1))
                        {
                            SNode_Change.DateTime = SNode_Change.DateTime.AddHours(-6); // yeah
                        }
                    }
                }
            }
        }
    }
}
