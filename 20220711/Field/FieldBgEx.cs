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
  internal class FieldBgEx : FieldCommon
  {
    BitmapImage[] weeds_;
    BitmapImage water_;
    int moveX_ = 0;
    int moveY_ = 0;
    int wholeTipXNum_;
    int wholeTipYNum_;

    public FieldBgEx()
    {
      weeds_ = new BitmapImage[2];
      weeds_[0] = UnivLib.BitmapImageFromAssets("tipf/w1.png");
      weeds_[1] = UnivLib.BitmapImageFromAssets("tipf/w2.png");

      water_ = UnivLib.BitmapImageFromAssets("tipf/p.png"); //"tipf/p.png");
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
          AppendXyIndex(x, y, weeds_[0], id, "bg_weed1");
        }
      }
      for (var y = kTipYNum; y < kTipYNum*2; y++)
      {
        var y00 = string.Format("{0:D2}", y);
        for (var x = 0; x < kTipXNum; x++)
        {
          var x00 = string.Format("{0:D2}", x);
          var id = "idMapTip" + x00 + y00;
          AppendXyIndex(x, y, water_, id, "bg_water");
        }
      }
      wholeTipXNum_ = kTipXNum;
      wholeTipYNum_ = kTipYNum*2;
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
            Append(x * kTipXSize + moveX_, y * kTipYSize + moveY_, weeds_[no], id, "bg_weed2", 1);
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
            if (i.Tag != null && i.Tag.ToString() == "bg_weed2")
            {
              s_monitor_.Children.Remove(weed);
            }
          }
        }
      }
    }//public void ChangeBg(int no)
    /*public void MoveX(int delta)
    {
      moveX_ += delta;
      foreach (UIElement weed in s_monitor_.Children)
      {
        if (weed is Image)
        {
          Image i = (Image)weed;
          if (i.Tag != null && i.Tag.ToString().StartsWith("weed"))
          {
            Thickness t = i.Margin;
            t.Left += delta;
            i.Margin = t;
          }
        }
      }
    }*/
    public bool CanMove(int dx, int dy)
    {
      JsTrans.console_log("");
      if (dx != 0)
      {
        if (dx < 0)
        {
          if (-moveX_ + dx >= 
            kTipXSize * (wholeTipXNum_ - kTipXNum - 1)) return false;
        }
        if (dx > 0)
        {
          if (-moveX_ <= 0) return false;
        }
      }
      if (dy != 0)
      {
        if (dy < 0)
        {
          if (-moveY_ + dy >= kTipYSize * (wholeTipYNum_ - kTipYNum - 1)) return false;
        }
        if (dy > 0)
        {
          JsTrans.console_log(-moveY_ + " < " + 0);
          if (-moveY_ <= 0) return false;
        }
      }
      return true;
    }
    public void Move(int x, int y)
    {
      moveX_ += x;
      moveY_ += y;
      foreach (UIElement weed in s_monitor_.Children)
      {
        if (weed is Image)
        {
          Image i = (Image)weed;
          if (i.Tag != null)
          {
            string tag = i.Tag.ToString();
            if (tag.StartsWith("bg_"))
            {
              Thickness t = i.Margin;
              t.Left += x;
              t.Top += y;
              i.Margin = t;
            }
          }
        }
      }
    }
  }
}
