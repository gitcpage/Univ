using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Univ.NsField
{
  internal abstract class FieldBgAbstract : FieldInclude
  {
    public int tWholeXNum_ { get; protected set; } = kTipXNum;
    public int tWholeYNum_ { get; protected set; } = kTipYNum;
    public FieldBgAbstract(Grid grid) : base(grid)
    {
    }
    public abstract void Run();
    public abstract void ChangeBg(int no);

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
