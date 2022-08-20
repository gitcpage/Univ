using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.NsBattle
{
  internal class BattleCommandInfo
  {
    public int from;
    public int to;
    public BattleCommandInfo(int from, int to)
    {
      this.from = from;
      this.to = to;
    }
  }
}
