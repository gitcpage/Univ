using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage

namespace Univ.NsField
{
  internal class FieldBg : FieldInclude
  {
    public int tWholeXNum_ { get; private set; } = kTipXNum;
    public int tWholeYNum_ { get; private set; } = kTipYNum;
    protected Data.FieldData fieldData_;
    protected BitmapImage[] weeds;
    //Image[,] foreImages_;
    protected Image[,] backImages_;

    public FieldBg(Grid grid, Data.FieldData data) : base(grid)
    {
      fieldData_ = data;
      tWholeXNum_ = fieldData_.Width;
      tWholeYNum_ = fieldData_.Height;

      weeds = new BitmapImage[2];
      weeds[0] = UnivLib.BitmapImageFromAssets("tipf/w1.png");
      weeds[1] = UnivLib.BitmapImageFromAssets("tipf/w2.png");

      //foreImages_ = new Image[tWholeYNum_, tWholeXNum_];
      backImages_ = new Image[tWholeYNum_, tWholeXNum_];
    }
    public virtual void Run()
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
          backImages_[y, x] = UnivLib.ImageInstance(x * kTipXSize, y * kTipYSize, weeds[1], id2, z: 1);
        }
      }
    }
    public virtual void ChangeBg(int no)
    {
      if (no == 1)
      {
        for (var y = 0; y < kTipYNum; y++)
        {
          for (var x = 0; x < kTipXNum; x++)
          {
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
      }
    }

    public virtual bool CanMove(int dx, int dy)
    {
      return false;
    }
    public virtual void SyncTip()
    {
      JsTrans.Assert("FieldBg.cs SyncTip このメソッドは使用しない");
    }
    public virtual void MoveTip(int tX, int tY)
    {
      JsTrans.Assert(tX==0 && tY==0 , "FieldBg.cs MoveTip このメソッドは使用しない(tX==0 && tY==0はOK)");
    }
    public virtual void GetTipPosition(out int tX, out int tY)
    {
      tX = 0;
      tY = 0;
    }
    public virtual void Move(int rx, int ry)
    {
      JsTrans.Assert("FieldBg.cs Move このメソッドは使用しない");
    }
  }
}
