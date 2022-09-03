using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // Thickness

namespace Univ.NsField
{
  internal class FieldBlock : FieldInclude
  {
    public enum DirectionSlot { None, Down, DownUp }

    BitmapImage bmp_;
    Image img_;
    int blockX_ = 0;
    int blockY_ = 0;
    public int BlockX { get { return blockX_; } }
    public int BlockY { get { return blockY_; } }

    public FieldBlock(string path, Grid monitoBg) : base(monitoBg)
    {
      bmp_ = UnivLib.BitmapImageFromAssets(path);
    }
    public FieldBlock(string pathbase, DirectionSlot ds, Grid monitoBg) : base(monitoBg)
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

    /*public string src
    {
      set {
        BitmapImage bitmapImage = UnivLib.BitmapImageFromAssets(value);
        Image.Source = bitmapImage;
      }
    }*/

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
    public int GetX()
    {
      return (int)img_.Margin.Left;
    }
    public int GetCenterX()
    {
      return (int)img_.Margin.Left + kTipXSize / 2;
    }
    public void SetX(int x)
    {
      Thickness t = img_.Margin;
      t.Left = x;
      img_.Margin = t;
    }
    public void SetXIndex(int xIndex)
    {
      Thickness t = img_.Margin;
      t.Left = xIndex * kTipXSize;
      img_.Margin = t;
    }
    public int GetY()
    {
      return (int)img_.Margin.Top;
    }
    public int GetCenterY()
    {
      return (int)img_.Margin.Top + kTipYSize / 2;
    }
    public void SetY(int y)
    {
      Thickness t = img_.Margin;
      t.Top = y;
      img_.Margin = t;
    }
    public void SetYIndex(int yIndex)
    {
      Thickness t = img_.Margin;
      t.Top = yIndex * kTipYSize;
      img_.Margin = t;
    }
    //public void UpdateWalktipImg(tagId, imageId, x, y, step = 0)
    /*public void GetBlockPosition(out int blockX, out int blockY)
    {
      blockX = this.blockX_;
      blockY = this.blockY_;
    }*/
    public void SetBlockPosition(int blockX, int blockY)
    {
      this.blockX_ = blockX;
      this.blockY_ = blockY;
      BlockSync();
    }
    void BlockSync()
    {
      img_.Margin = new Thickness(blockX_ * kTipXSize, blockY_ * kTipYSize, 0, 0);
    }
  }
}
