﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // UIElement

namespace Univ.NsField
{
  internal class FieldBg : FieldBgAbstract // : FieldInclude
  {
    BitmapImage[] weeds;
    //Image[,] foreImages_;
    Image[,] backImages_;

    public FieldBg(Grid monitorBg) : base(monitorBg)
    {
      weeds = new BitmapImage[2];
      weeds[0] = UnivLib.BitmapImageFromAssets("tipf/w1.png");
      weeds[1] = UnivLib.BitmapImageFromAssets("tipf/w2.png");

      //foreImages_ = new Image[tWholeYNum_, tWholeXNum_];
      backImages_ = new Image[tWholeYNum_, tWholeXNum_];
    }

    public override void Run()
    {
      for (var y = 0; y < kTipYNum; y++)
      {
        var y00 = string.Format("{0:D2}", y);
        for (var x = 0; x < kTipXNum; x++)
        {
          var x00 = string.Format("{0:D2}", x);
          var id = "idMapTip" + x00 + y00;
          AppendXyIndex(x, y, weeds[0], id);
          var id2 = "idMap2Tip" + x00 + y00;
          backImages_[y, x] = UnivLib.ImageInstance(x * kTipXSize, y * kTipYSize, weeds[1], id2, z:1);
        }
      }
    }

    public override void ChangeBg(int no)
    {
      if (no == 1)
      {
        for (var y = 0; y < kTipYNum; y++)
        {
          //var y00 = string.Format("{0:D2}", y);
          for (var x = 0; x < kTipXNum; x++)
          {
            //var x00 = string.Format("{0:D2}", x);
            //var id = "idMapTip" + x00 + y00;
            //AppendXyIndex(x, y, weeds[no], id/*, "weed2"*/, 1);
            monitor_.Children.Add(backImages_[y, x]);
          }
        }
      }
      else
      {
        for (var y = 0; y < kTipYNum; y++)
        {
          for (var x = 0; x < kTipXNum; x++)
          {
            monitor_.Children.Remove(backImages_[y, x]);
          }
        }
        /*var uiec = monitor_.Children.ToArray();
        foreach (UIElement weed in uiec)
        {
          if (weed is Image)
          {
            Image i = (Image)weed;
            if (i.Tag != null && i.Tag.ToString() == "weed2")
            {
              monitor_.Children.Remove(weed);
            }
          }
        }*/
      }
    }
  }
}
