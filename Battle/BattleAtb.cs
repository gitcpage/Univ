using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.NsBattle
{
  internal class BattleAtb
  {
    public enum State
    {
      None, Command, TargetSelect, Doing, Done
    }

    public const int kBarFullValue = 1000;
    int[] barsValue_ = null; // kBarFullValue以上の値をになることもある。

    BattleUI ui_;
    private State state_;
    public State state { 
      get { return state_; }
      set { state_ = value; }
    }

    public BattleAtb(BattleUI ui)
    {
      if (barsValue_ == null)
        barsValue_ = new int[5];

      ui_ = ui;
      state = State.None;
    }

    public void Initialize()
    {
      for (int i = 0; i < barsValue_.Length; i++)
      {
        barsValue_[i] = JsTrans.getRandomInt(200);
      }
    }

    public int GetVarValue(int charId)
    {
      return barsValue_[charId];
    }
    public void SetVarValue(int charId, int value)
    {
      barsValue_[charId] = value;
      ui_.SetBarValue(charId, barsValue_[charId]);
    }
    public bool AddVarValue(int charId, int value)
    {
      barsValue_[charId] += value;
      ui_.SetBarValue(charId, barsValue_[charId]);
      return barsValue_[charId] >= kBarFullValue;
    }
    public int BarFullId()
    {
      int charId = -1;
      int max = 0;
      for (int i = 0; i < 5; i++)
      {
        int v = GetVarValue(i);
        if (v >= 1000)
        {
          if (v > max)
          {
            charId = i;
            max = v;
          }
        }
      }
      return charId;
    }
    public void Accumulate()
    {
      Data.Status[] sts = Data.Status.Instances;
      for (int i = 0; i < 5; i++)
      {
        AddVarValue(i, sts[i].spd());//5);
      }
    }
    public void AccumulateFull()
    {
      Data.Status[] sts = Data.Status.Instances;
      bool doContinue = true;
      while (doContinue)
      {
        for (int i = 0; i < 5; i++)
        {
          if (AddVarValue(i, sts[i].spd()))
            doContinue = false;
        }
      }
    }
  }
}
