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
using Windows.UI.Text; // FontWeight
using Windows.Foundation; // Size, Rect

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
      string name, int z = 0/*, string tag = ""*/)
    {
      Image image = new Image();
      image.Source = bitmapImage;
      image.Name = name;
      image.Margin = new Thickness(x, y, 0, 0);
      image.HorizontalAlignment = HorizontalAlignment.Left;
      image.VerticalAlignment = VerticalAlignment.Top;
      image.Stretch = Stretch.None;
      //image.Tag = tag;
      if (z != 0)
        Canvas.SetZIndex(image, z);
      return image;
    }
    static public Image ImageInstance(BitmapImage bitmapImage,
      string name, int z = 0/*, string tag = ""*/)
    {
      return ImageInstance(0, 0, bitmapImage, name, z/*, tag*/);
    }
    static public Image ImageInstance(int x, int y, string path,
      string name = "", int z = 0/*, string tag = ""*/)
    {
      return ImageInstance(x, y, BitmapImageFromAssets(path), name, z/*, tag*/);
    }
    static public Image ImageInstance(string path,
      string name, int z = 0/*, string tag = ""*/)
    {
      return ImageInstance(0, 0, BitmapImageFromAssets(path), name, z/*, tag*/);
    }
    static public FontWeight FontWeightBold(bool b = true)
    {
      FontWeight fw = new FontWeight();
      fw.Weight = (ushort)(b ? 700 : 400);
      return fw;
    }
    // アンダーラインを消すためのコピーに使用する。
    static public TextBlock GetTextBlock(TextBlock from)
    {
      TextBlock txt = new TextBlock();
      Thickness t = from.Margin;
      txt.Margin = new Thickness(t.Left, t.Top, t.Right, t.Bottom);
      txt.HorizontalAlignment = from.HorizontalAlignment;
      txt.VerticalAlignment = from.VerticalAlignment;
      t = from.Padding;
      txt.Padding = new Thickness(t.Left, t.Top, t.Right, t.Bottom);
      txt.FontSize = from.FontSize;
      txt.Foreground = from.Foreground;
      txt.Text = from.Text;
      txt.FontFamily = from.FontFamily;
      txt.FontWeight = from.FontWeight;
      return txt;
    }
    // TextDecorations.None; はバグで効かないので、回避策として使う関数。
    // アンダーラインを引かないのであれば、TextBlock() のみでよい。
    static public Border WrapBorder(TextBlock tb, Panel parent)
    {
      Border bdr = new Border();
      bdr.Child = tb;
      bdr.PointerEntered += (Object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) =>
      {
        Border bdrTmp = sender as Border;
        TextBlock o = bdrTmp.Child as TextBlock;
        o.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
      };
      bdr.PointerExited += (Object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) =>
      {
        Border bdrTmp = sender as Border;
        TextBlock old = bdrTmp.Child as TextBlock;
        bdr.Child = UnivLib.GetTextBlock(old);
      };
      if (parent != null) parent.Children.Add(bdr);
      return bdr;
    }
    // TextDecorations.Noneバグ回避に加えて、ワイドなTappedイベント検出をおこなう
    static public Border WrapBorder(TextBlock tb, Panel parent, int width, int x = 0, int y = 0)
    {
      Border bdr = WrapBorder(tb, parent);
      bdr.Margin = new Thickness(x, y, 0, 0);
      bdr.Width = width;
      bdr.Background = UnivLib.GetBrush(0, 128, 128, 128);//Tappedイベント検出のため
      bdr.HorizontalAlignment = HorizontalAlignment.Left;
      bdr.VerticalAlignment = VerticalAlignment.Top;
      return bdr;
    }

    static public void MeasureWidth(TextBlock[] inTbs, StackPanel container)
    {
      Size size = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
      Rect rect = new Rect(0, 0, container.Width, 500);
      double sum = 0;
      foreach (TextBlock tb in inTbs)
      {
        tb.Measure(size);
        tb.Arrange(rect);
        sum += tb.ActualWidth;
      }
      double padding = (container.Width - sum) / 2 - 5;
      Thickness t = container.Padding;
      container.Padding = new Thickness(padding, t.Top, padding, t.Bottom);
    }
    static public void MeasureWidth(TextBlock inTb, Grid container)
    {
      Size size = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
      Rect rect = new Rect(0, 0, container.Width, 500);
      inTb.Measure(size);
      inTb.Arrange(rect);
      double padding = (container.Width - inTb.ActualWidth) / 2 - 5;
      Thickness t = container.Padding;
      container.Padding = new Thickness(padding, t.Top, padding, t.Bottom);
    }
    static public void Array2DimensionInit(int[,] array, int value = 0)
    {
      Buffer.BlockCopy(Enumerable.Repeat(value, array.Length).ToArray(), 0, array, 0, array.Length * sizeof(int));
    }
  }
}
