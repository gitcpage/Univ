using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // UIElement

namespace Univ
{
  internal class FieldBg : FieldCommon
  {
    BitmapImage[] weeds;

    public FieldBg()
    {
      weeds = new BitmapImage[2];
      weeds[0] = UnivLib.BitmapImageFromAssets("tipf/w1.png");
      weeds[1] = UnivLib.BitmapImageFromAssets("tipf/w2.png");
    }

    public void Run()
    {
      for (var y = 0; y < kTipYNum; y++)
      {
        var y00 = string.Format("{0:D2}", y);
        for (var x = 0; x < kTipXNum; x++)
        {
          var x00 = string.Format("{0:D2}", x);
          var id = "idMapTip" + x00 + y00;
          AppendXyIndex(x, y, weeds[0], id, "weed1");
        }
      }
    }

    public void ChangeBg(int no)
    {
      if (no == 1)
      {
        for (var y = 0; y < kTipYNum; y++)
        {
          var y00 = string.Format("{0:D2}", y);
          for (var x = 0; x < kTipXNum; x++)
          {
            var x00 = string.Format("{0:D2}", x);
            var id = "idMapTip" + x00 + y00;
            AppendXyIndex(x, y, weeds[no], id, "weed2", 1);
          }
        }
      }
      else
      {
        var uiec = s_monitor_.Children.ToArray();
        foreach (UIElement weed in uiec)
        {
          if (weed is Image)
          {
            Image i = (Image)weed;
            if (i.Tag.ToString() == "weed2")
            {
              s_monitor_.Children.Remove(weed);
            }
          }
        }
      }
    }
  }
}
