using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Data;
using Univ.NsBattle;

namespace Univ
{
  internal class BattleDebug : Battle
  {
    public BattleDebug(MainPage mainPage, Data.SecurityToken stFriends, int monsGroupId)
      : base(mainPage, stFriends, monsGroupId)
    {
    }
    public override void Run()
    {
      base.Run();
      atb_.AccumulateFull();
      for (int i = 0; i < data_.MonsterNumber(); i++)
      {
        UnitInfo info = data_.MonsInfo(i);
        info.hp = 1;
      }
    }
  }
}
