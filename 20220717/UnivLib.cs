using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Media; // Stretch, Brush
using Windows.UI; // Color
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // Thickness

namespace Univ
{
  internal class UnivLib
  {
    static List<string> s_paths_ = new List<string>();
    static List<BitmapImage> s_bmps_ = new List<BitmapImage>();
    static public BitmapImage BitmapImageFromAssets(string path)
    {
      for (int i = 0; i < s_paths_.Count; i++)
      {
        if (s_paths_[i] == path)
        {
          return s_bmps_[i];
        }
      }
      Uri uri = new Uri("ms-appx:///Assets/" + path);
      BitmapImage img =  new BitmapImage(uri);
      s_paths_.Add(path);
      s_bmps_.Add(img);
      return img;
    }
    static public void MsgBitmapPaths()
    {
      string s = "MsgBitmapPaths() \n\n";
      foreach (string path in s_paths_)
      {
        s += path + "\n";
      }
      JsTrans.alert(s);
    }
    // TextBlock.Foreground などに設定する値。
    // 引数は Colors.Red などから選択することができる。
    static public Brush GetBrush(Color c)
    {
      return new SolidColorBrush(c);
    }
    static public Brush GetBrush(byte a, byte r, byte g, byte b)
    {
      return GetBrush(Color.FromArgb(a, r, g, b));
    }
    static public Brush GetBrush(byte r, byte g, byte b)
    {
      return GetBrush(255, r, g, b);
    }

    static public Image ImageInstance(int x, int y, BitmapImage bitmapImage,
      string name, string tag = "", int z = 0)
    {
      Image image = new Image();
      image.Source = bitmapImage;
      image.Name = name;
      image.Margin = new Thickness(x, y, 0, 0);
      image.HorizontalAlignment = HorizontalAlignment.Left;
      image.VerticalAlignment = VerticalAlignment.Top;
      image.Stretch = Stretch.None;
      image.Tag = tag;
      if (z != 0)
        Canvas.SetZIndex(image, z);
      return image;
    }
    static public Image ImageInstance(BitmapImage bitmapImage,
      string name, string tag = "", int z = 0)
    {
      return ImageInstance(0, 0, bitmapImage, name, tag, z);
    }
    static public Image ImageInstance(int x, int y, string path,
      string name, string tag = "", int z = 0)
    {
      return ImageInstance(x, y, BitmapImageFromAssets(path), name, tag, z);
    }
    static public Image ImageInstance(string path,
      string name, string tag = "", int z = 0)
    {
      return ImageInstance(0, 0, BitmapImageFromAssets(path), name, tag, z);
    }
  }
}
