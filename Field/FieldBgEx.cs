using System;
using System.Linq;
using Windows.UI.Xaml; // UIElement
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml.Media.Imaging; // BitmapImage

namespace Univ.NsField
{
  internal class FieldBgEx : FieldBg
  {
    BitmapImage[] waters_ = new BitmapImage[16];
    BitmapImage forest_, forestb_, foresttb_, forestt_;
    BitmapImage mountain_, mountainr_, mountainlr_, mountainl_;
    int rMoveX_ = 0;
    int rMoveY_ = 0;
    int[,] what;
    Image[,] foreImages_;

    public FieldBgEx(Grid monitorBg, Data.FieldData data) : base(monitorBg, data)
    {
      for (int i = 0 ; i < 16; i++)
      {
        waters_[i] = UnivLib.BitmapImageFromAssets("tipf/p"+Convert.ToString(i, 2).PadLeft(4, '0')+".png");
      }

      forest_ = UnivLib.BitmapImageFromAssets("tipf/forest.png");
      forestb_ = UnivLib.BitmapImageFromAssets("tipf/forestb.png");
      foresttb_ = UnivLib.BitmapImageFromAssets("tipf/foresttb.png");
      forestt_ = UnivLib.BitmapImageFromAssets("tipf/forestt.png");

      mountain_ = UnivLib.BitmapImageFromAssets("tipf/mountain.png");
      mountainr_ = UnivLib.BitmapImageFromAssets("tipf/mountainr.png");
      mountainlr_ = UnivLib.BitmapImageFromAssets("tipf/mountainlr.png");
      mountainl_ = UnivLib.BitmapImageFromAssets("tipf/mountainl.png");
    }

    public override void Run()
    {
      //System.Text.StringBuilder sbForLog = new System.Text.StringBuilder();
      what = new int[tWholeYNum_, tWholeXNum_];
      foreImages_ = new Image[tWholeYNum_, tWholeXNum_];
      backImages_ = new Image[tWholeYNum_, tWholeXNum_];
      UnivLib.Array2DimensionInit(what);

      int[,] d = fieldData_.Data;
      bool isL, isR, isT, isB;
      BitmapImage bi;
      int b4;
      for (var y = 0; y < tWholeYNum_; y++)
      { //右
        var y00 = string.Format("{0:D2}", y);
        for (var x = 0; x < tWholeXNum_; x++)
        {
          string x00 = string.Format("{0:D2}", x);
          string id = "idMapTip" + x00 + y00;
          switch (d[y,x])
          {
            case 0:
              b4 = (x > 0 && d[y, x - 1]==0) ? 0 : 0x8;
              b4 |= (y > 0 && d[y - 1, x] == 0) ? 0 : 0x4;
              b4 |= (x < tWholeXNum_ - 1 && d[y, x + 1] == 0) ? 0 : 0x2;
              b4 |= (y < tWholeYNum_ - 1 && d[y + 1, x] == 0) ? 0 : 0x1;
              foreImages_[y, x] = AppendXyIndex(x, y, waters_[b4], id);
              break;
            case 1:
              foreImages_[y, x] = AppendXyIndex(x, y, weeds_[0], id);
              id = "idMapTip" + x00 + y00 + "Back";
              backImages_[y, x] = UnivLib.ImageInstance(x * kTipXSize, y * kTipYSize, weeds_[1], id);
              what[y, x] = 1;
              break;
            case 2:
              isT = y > 0 && d[y-1, x] == 2;
              isB = y < tWholeYNum_ - 1 && d[y+1, x] == 2;
              {
                if (isT && isB) bi = foresttb_;
                else if (isT) bi = forestt_;
                else if (isB) bi = forestb_;
                else bi = forest_;
              }
              foreImages_[y, x] = AppendXyIndex(x, y, bi, id);
              break;
            case 3:
              isL = x > 0 && d[y, x - 1] == 3;
              isR = x < tWholeXNum_-1 && d[y, x + 1] == 3;
              {
                if (isL && isR) bi = mountainlr_;
                else if (isL) bi = mountainl_;
                else if (isR) bi = mountainr_;
                else bi = mountain_;
              }
              foreImages_[y, x] = AppendXyIndex(x, y, bi, id);
              break;
            default:
              JsTrans.Assert("FieldBgEx.cs Run()");
              break;
          }
        }
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
      for (int y = 0; y < tWholeYNum_/*kTipYNum * 2*/; y++)
      {
        for (int x = 0; x < tWholeXNum_/*kTipXNum * 2*/; x++)
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
      for (int y = 0; y < tWholeYNum_/*kTipYNum*2*/; y++)
      {
        for (int x = 0; x < tWholeXNum_/*kTipXNum*2*/; x++)
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
