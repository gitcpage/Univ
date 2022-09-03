using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  public enum TargetMethod { 
    None=0, 
    EnemyOne=1, EnemyAll=2, EnemyOneAll=3,
    PartyOne=4, PartyAll=8, PartyOneAll=12,
    All=10, PartyDeadOne=16, PartyDeadAll=32
  }
  public enum SkillKind { Tech, Recovery, Aid, Attack }
  internal class ConstSkill
  {
    public readonly string Name;
    public readonly int Mp;
    public readonly int EffectValue;
    public readonly TargetMethod Target;

    public ConstSkill(string text)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length == 4,
        "ConstSkill text of 'ConstSkill(string text)' split tab is not 4 items.");
      this.Name = items[0];
      this.Mp = int.Parse(items[1]);
      this.EffectValue = int.Parse(items[2]);
      this.Target = (TargetMethod)int.Parse(items[3]);
    }
  }
}
