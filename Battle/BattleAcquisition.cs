using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace Univ.NsBattle
{
  internal class BattleAcquisition
  {
    int exp_;
    int gold_;
    int[] itemsId_;
    int[] itemsNum_;

    const int kItemsArraySize = 8;
    public BattleAcquisition()
    {
      exp_ = 0;
      gold_ = 0;
      itemsId_ = new int[kItemsArraySize];
      itemsNum_ = new int[kItemsArraySize];
      for (int i = 0; i < kItemsArraySize; i++)
      {
        itemsId_[i] = 0;
        itemsNum_[i] = 0;
      }
    }
    public void Acquire(int exp, int gold, int item, int itemPer)
    {
      JsTrans.Assert(exp >= 0, "BattleAcquisition Acquire exp("+exp+" >= 0");
      JsTrans.Assert(gold >= 0, "BattleAcquisition Acquire gold("+gold+") >= 0");
      exp_ += exp;
      gold_ += gold;

      if (item >= 0)
      {
        if (itemPer > JsTrans.getRandomInt(100))
        {
          for (int i = 0; i < kItemsArraySize; i++)
          {
            // itemsNum_[i] == 0 は最初の登録。
            if (itemsNum_[i] == 0 || 
              itemsId_[i] == item)
            {
              itemsId_[i] = item;
              itemsNum_[i]++;
              break;
            }
          }
        }
      }
    }
    public int Exp { get { return exp_; } }
    public int Gold { get { return gold_; } }
    public bool Item(int no, out int item, out int num)
    {
      JsTrans.Assert(no >= 0 && no < kItemsArraySize, "BattleAcquisition Item no("+no+")");
      item = itemsId_[no];
      num = itemsNum_[no];
      if (itemsNum_[no] == 0) return false;
      return true;
    }
    public void AppendExpText(Queue<string> ss)
    {
      if (exp_ > 0)
      {
        ss.Enqueue("経験値" + exp_ + "かくとく！");
      }
    }
    public void AppendGoldAndItemText(Queue<string> ss)
    {
      if (exp_ > 0)
      {
        ss.Enqueue(exp_.ToString() + "ゴールド てにいれた！");
      }
      Data.ConstItem[] itemData = Data.DataSC.Items();
      for (int i = 0; i < kItemsArraySize; i++)
      {
        if (itemsNum_[i] == 0) break;

        string name = itemData[itemsId_[i]].Name;
        if (itemsNum_[i] == 1)
        {
          ss.Enqueue(name + "をてにいれた！");
        }
        else
        {
          ss.Enqueue(name + "を" + itemsNum_[i] + "個 てにいれた！");
        }
      }
    }
  }
}
