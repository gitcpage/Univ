﻿using System;
using System.Linq;
using Windows.UI.Xaml; // UIElement
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml.Media.Imaging; // BitmapImage

namespace Univ.NsField
{
  internal class FieldBgEx : FieldBg
  {
    BitmapImage water_;
    int rMoveX_ = 0;
    int rMoveY_ = 0;
    int[,] what;
    Image[,] foreImages_;

    public FieldBgEx(Grid monitorBg, Data.FieldData data) : base(monitorBg, data)
    {
      water_ = UnivLib.BitmapImageFromAssets("tipf/p.png"); //"tipf/p.png");
    }

    public override void Run()
    {
      System.Text.StringBuilder sbForLog = new System.Text.StringBuilder();
      what = new int[tWholeYNum_, tWholeXNum_];
      foreImages_ = new Image[tWholeYNum_, tWholeXNum_];
      backImages_ = new Image[tWholeYNum_, tWholeXNum_];
      UnivLib.Array2DimensionInit(what);

      int[,] d = fieldData_.Data;
      for (var y = 0; y < tWholeYNum_; y++)
      { //右
        var y00 = string.Format("{0:D2}", y);
        for (var x = 0; x < tWholeXNum_; x++)
        {
          var x00 = string.Format("{0:D2}", x);
          var id = "idMapTip" + x00 + y00;
          if (d[y,x] == 0)
          {
            foreImages_[y, x] = AppendXyIndex(x, y, weeds_[0], id);
            id = "idMapTip" + x00 + y00 + "Back";
            backImages_[y, x] = UnivLib.ImageInstance(x * kTipXSize, y * kTipYSize, weeds_[1], id);
            what[y, x] = 1;
            sbForLog.Append(0);
          }
          else
          {
            foreImages_[y, x] = AppendXyIndex(x, y, water_, id);
            sbForLog.Append(1);
          }
        }
        sbForLog.Append(Environment.NewLine);
      }
    }

    public override void ChangeBg(int no)
    {
      if (no == 1)
      {
        for (var y = 0; y < tWholeYNum_; y++)
        {
          for (var x = 0; x < tWholeXNum_; x++)
          {
            if (what[y, x] == 1)
            {
              monitor_.Children.Add(backImages_[y, x]);
            }
          }
        }
      }
      else
      {
        for (var y = 0; y < tWholeYNum_; y++)
        {
          for (var x = 0; x < tWholeXNum_; x++)
          {
            if (what[y, x] == 1)
            {
              monitor_.Children.Remove(backImages_[y, x]);
            }
          }
        }
      }
    }//public override void ChangeBg(int no)
    public override bool CanMove(int dx, int dy)
    {
      if (dx != 0)
      {
        if (dx < 0)
        {
          if (-rMoveX_ + dx >= 
            kTipXSize * (tWholeXNum_ - kTipXNum - 1)) return false;
        }
        if (dx > 0)
        {
          if (-rMoveX_ <= 0) return false;
        }
      }
      if (dy != 0)
      {
        if (dy < 0)
        {
          if (-rMoveY_ + dy >=
            kTipYSize * (tWholeYNum_ - kTipYNum - 1)) return false;
        }
        if (dy > 0)
        {
          if (-rMoveY_ <= 0) return false;
        }
      }
      return true;
    }
    public override void SyncTip()
    {
      for (int y = 0; y < kTipYNum * 2; y++)
      {
        for (int x = 0; x < kTipXNum * 2; x++)
        {
          Thickness t = foreImages_[y, x].Margin;
          t.Left = x * kTipXSize + rMoveX_;
          t.Top = y * kTipYSize + rMoveY_;
          foreImages_[y, x].Margin = t;
          if (backImages_[y, x] != null)
            backImages_[y, x].Margin = t;
        }
      }
    }
    public override void MoveTip(int rtX, int rtY)
    {
      rMoveX_ = rtX * kTipXSize;
      rMoveY_ = rtY * kTipYSize;
      SyncTip();
    }
    public override void GetTipPosition(out int rtX, out int rtY)
    {
      rtX = rMoveX_ / kTipXSize;
      rtY = rMoveY_ / kTipYSize;
    }
    public override void Move(int rx, int ry)
    {
      rMoveX_ += rx;
      rMoveY_ += ry;
      for (int y = 0; y < kTipYNum*2; y++)
      {
        for (int x = 0; x < kTipXNum*2; x++)
        {
          Thickness t = foreImages_[y, x].Margin;
          t.Left += rx;
          t.Top += ry;
          foreImages_[y, x].Margin = t;
          if (backImages_[y, x] != null)
            backImages_[y, x].Margin = t;
        }
      }
    }
  }
}
