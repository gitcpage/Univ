using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Data;

namespace Univ
{
  internal class BattleDebug : Battle
  {
    public BattleDebug(MainPage mainPage, StatusWritable[] charsWritable) : base(mainPage, charsWritable)
    {
    }
    public override void Run()
    {
      base.Run();
      atb_.AccumulateFull();
    }
  }
}
