using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // Thickness

namespace Univ
{
  internal class FieldBlock : FieldCommon
  {
    public enum DirectionSlot { None, Down, DownUp }

    BitmapImage bmp_;
    Image img_;
    public FieldBlock(string path)
    {
      bmp_ = UnivLib.BitmapImageFromAssets(path);
    }
    public FieldBlock(string pathbase, DirectionSlot ds)
    {
      bmp_ = UnivLib.BitmapImageFromAssets(pathbase + "00.png");
    }
    public BitmapImage Bitmap
    {
      get { return bmp_; }
    }
    public Image Image
    {
      get { return img_; }
      set { img_ = value; }
    }

    public void SetXY(int x, int y)
    {
      Thickness t = img_.Margin;
      t.Left = x;
      t.Top = y;
      img_.Margin = t;
    }
    public void SetXYIndex(int x, int y)
    {
      SetXY(x * kTipXSize, y * kTipYSize);
    }
    public int X
    {
      get { return (int)img_.Margin.Left; }
      set {
        Thickness t = img_.Margin;
        t.Left = value;
        img_.Margin = t;
      }
    }
    public int XIndex
    {
      set
      {
        Thickness t = img_.Margin;
        t.Left = value * kTipXSize;
        img_.Margin = t;
      }
    }
    public int Y
    {
      get { return (int)img_.Margin.Top; }
      set
      {
        Thickness t = img_.Margin;
        t.Top = value;
        img_.Margin = t;
      }
    }
    public int YIndex
    {
      set
      {
        Thickness t = img_.Margin;
        t.Top = value * kTipYSize;
        img_.Margin = t;
      }
    }
  }
}
