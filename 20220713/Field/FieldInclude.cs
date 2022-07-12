using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Media; // Stretch

namespace Univ
{
  internal class FieldInclude
  {
    protected const int kTipXSize = 50;
    protected const int kTipYSize = 40;

    protected const int kTipXNum = 16;
    protected const int kTipYNum = 15;
    protected const int kTipYTopSideLimit = 7; // 上側として扱う上限値。セリフの表示位置に使用する。
    protected const int kTipYLeftSideLimit = 8; // 左側として扱う上限値。

    protected const int kMoveXStep = 10;
    protected const int kMoveYStep = 8;
    protected const int kMoveTime = 5;

    protected Grid monitor_;

    public FieldInclude(Grid grid)
    {
      monitor_ = grid;
    }

    protected Image AppendXyIndex(int x, int y, BitmapImage bi,
      string name, string tag = "", int z = 0)
    {
      return Append(x * kTipXSize, y * kTipYSize, bi, name, tag, z);
    }
    protected Image Append(int x, int y, BitmapImage bi,
      string name, string tag = "", int z = 0)
    {
      Image image = new Image();
      image.Source = bi;
      image.Name = name;
      image.Margin = new Thickness(x, y, 0, 0);
      image.HorizontalAlignment = HorizontalAlignment.Left;
      image.VerticalAlignment = VerticalAlignment.Top;
      image.Stretch = Stretch.None;
      image.Tag = tag;
      if (z != 0)
        Canvas.SetZIndex(image, z);
      monitor_.Children.Add(image);
      return image;
    }

    protected Image AppendXyIndex(int x, int y, FieldBlock fb,
      string name, string tag = "", int z = 0)
    {
      return Append(x * kTipXSize, y * kTipYSize, fb, name, tag, z);
    }
    protected Image Append(int x, int y, FieldBlock fb,
      string name, string tag = "", int z = 0)
    {
      Image image = new Image();
      image.Source = fb.Bitmap;
      image.Name = name;
      image.Margin = new Thickness(x, y, 0, 0);
      image.HorizontalAlignment = HorizontalAlignment.Left;
      image.VerticalAlignment = VerticalAlignment.Top;
      image.Stretch = Stretch.None;
      image.Tag = tag;
      if (z != 0)
        Canvas.SetZIndex(image, z);
      monitor_.Children.Add(image);
      fb.Image = image;
      return image;
    }
  }
}
