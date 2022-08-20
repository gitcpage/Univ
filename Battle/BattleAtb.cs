using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.NsBattle
{
  //NowDo: ATBはコマンドとリンクさせる。

  //ATBゲージをためて、コマンドを表示するまでの手順は以下の通りである。
  // step1 Accumulate()内のAddVarValue()によってゲージをためる。
  // step2 ゲージがkGaugeFullValue以上になったら、GaugeFullId()でキャラクタIDを返す。
  //その後、Battle.csにおいて、stateをCommandにし、コマンドを表示する。
  internal class BattleAtb : BattleInclude
  {

    int[] gaugesValue_ = null; // kGaugeFullValue以上の値をになることもある。

    BattleUI ui_;

    public BattleAtb(BattleUI ui)
    {
      if (gaugesValue_ == null)
        gaugesValue_ = new int[kFriendMax];

      ui_ = ui;
    }

    public void Initialize()
    {
      for (int i = 0; i < gaugesValue_.Length; i++)
      {
        gaugesValue_[i] = JsTrans.getRandomInt(200);
      }
    }

    public int GetGaugeValue(int charId)
    {
      int v = gaugesValue_[charId];
      return v == kGaugeInAction ? 0 : v;
    }
    public void SetGaugeValueZero(int charId)
    {
      gaugesValue_[charId] = 0;
      ui_.SetGaugeValue(charId, gaugesValue_[charId]);
    }
    public void SetGaugeValueAction(int charId)
    {
      gaugesValue_[charId] = kGaugeInAction;
    }
    // 戻り値：ゲージがたまったら true を返す。
    public bool AddVarValue(int charId, int value)
    {
      if (gaugesValue_[charId] == kGaugeInAction) return false;
      gaugesValue_[charId] += value;
      ui_.SetGaugeValue(charId, gaugesValue_[charId]);
      return gaugesValue_[charId] >= kGaugeFullValue;
    }
    // 戻り値：ATBがたまった、最も大きいATBを持つキャラクターのIDを返す。
    //         ATBがたまったキャラクターがいなかった場合は-1を返す。
    public int GaugeFullId()
    {
      int charId = -1;
      int max = kGaugeFullValue-1;
      for (int i = 0; i < kFriendMax; i++)
      {
        int v = GetGaugeValue(i);
        if (v > max)
        { //step2 ゲージをたまった場合の処理
          charId = i;
          max = v;
        }
      }
      return charId;
    }
    public void Accumulate()
    {
      Data.Status[] sts = Data.Status.Instances;
      for (int i = 0; i < 5; i++)
      {
        if (gaugesValue_[i] > kGaugeFullValue * 10 &&
          gaugesValue_[i] < kGaugeInAction)
          return; //ゲージがたまりすぎなキャラがいる場合、ゲージためを省略する。
      }
      for (int i = 0; i < 5; i++)
      {
        AddVarValue(i, sts[i].spd()/2);//5); //step1 ゲージをためる
      }
    }
    public void AccumulateFull()
    {
      Data.Status[] sts = Data.Status.Instances;
      bool doContinue = true;
      while (doContinue)
      {
        for (int i = 0; i < kFriendMax; i++)
        {
          if (AddVarValue(i, sts[i].spd()))
            doContinue = false;
        }
      }
    }
  }
}
