using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI.Xaml.Media.Animation; // ObjectAnimationUsingKeyFrames
using Windows.System; //VirtualKey

namespace Univ
{
  internal partial class Field : FieldCommon
  {
    //BitmapImage bmpObj_;
    //Image imgObj_;
    FieldBlock obj_;
    int boxPos_ = 0;
    FieldBlock player_;
    FieldBg bg_;

    public Field(FrameTimer frameTimer, Grid monitor)
    {
      s_frameTimer_ = frameTimer;
      s_monitor_ = monitor;

      bg_ = new FieldBg();
      //bmpObj_ = UnivLib.BitmapImageFromAssets("char/t5040walkt.png");
      obj_ = new FieldBlock("char/t5040walkt.png");
      player_ = new FieldBlock("char/char1p", FieldBlock.DirectionSlot.DownUp);
    }
    public void Run()
    {
      bg_.Run();
      //imgObj_ = AppendXyIndex(0, 1, bmpObj_, "imgObj", "obj", 2);
      AppendXyIndex(0, 1, obj_, "imgObj", "obj", 2);
      player_.Image = AppendXyIndex(0, 0, player_.Bitmap, "imgBox", "box", 2);
      s_frameTimer_.setTimeOut(FrameOne, 200);
    }
    public void FrameOne(object sender, object e)
    {
      if (s_frameTimer_.FrameCount % 10 == 0)
      {
        bg_.ChangeBg(0);
      }
      else if(s_frameTimer_.FrameCount % 5 == 0)
      {
        bg_.ChangeBg(1);
      }

      //右に延々動くやつ
      obj_.XIndex = s_frameTimer_.FrameCount % kTipXNum;
      /*int x = s_frameTimer_.FrameCount % kTipXNum;
      Thickness t = imgObj_.Margin;
      t.Left = x * kTipXSize; ;
      imgObj_.Margin = t;*/

      //上下に動かせるやつ
      if (s_frameTimer_.IsKeyDown(VirtualKey.Up))
      { // ↑
        if (boxPos_ > 0)
        {
          boxPos_ -= 1;
          player_.Y = boxPos_ * kTipYSize;
          Uri uri = new Uri("ms-appx:///Assets/char/char1p20.png");
          BitmapImage bitmapImage = new BitmapImage(uri);
          player_.Image.Source = bitmapImage;        }
      }
      else if (s_frameTimer_.IsKeyDown(VirtualKey.Down))
      { // ↓
        if (boxPos_ < kTipYNum - 1)
        {
          boxPos_ += 1;
          player_.Y = boxPos_ * kTipYSize;
        }
      }
    }
  }
}
