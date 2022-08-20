using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Data;

namespace Univ.NsBattle
{
  internal class UnitInfo
  {
    public int hp;
    public int mp;
    public UnitInfo(int hp, int mp)
    {
      this.hp = hp;
      this.mp = mp;
    }
  }
  internal class BattleData
  {
    Status[] chars_;
    StatusWritable[] friendsWritable_;
    UnitInfo[] charsInfo_;

    ConstStatusMons[] monsStatuses_;
    ConstMonsArrangement[] monsArrangements_;
    UnitInfo[] monsInfo_;

    public BattleData(SecurityToken stFriends, int monsGroupId)
    {
      chars_ = Status.Instances;
      friendsWritable_ = DataSC.FriendsWritable(stFriends);//charsWritable;
      charsInfo_ = new UnitInfo[5];
      for (int i = 0; i < 5; i++)
      {
        charsInfo_[i] = new UnitInfo(chars_[i].NowHp(), chars_[i].NowMp());
      }

      Loader ldr = Loader.Instance; // ローダー
      ConstMonsGroups[] monsGroups = ldr.monsGroupsData; //グループ一覧
      ConstStatusMons[] monss = ldr.monsData; //モンス一覧
      ConstMonsArrangement[] monsGroup = monsGroups[monsGroupId].group; //グループ

      monsStatuses_ = new ConstStatusMons[monsGroup.Length];
      monsArrangements_ = new ConstMonsArrangement[monsGroup.Length];
      monsInfo_ = new UnitInfo[monsGroup.Length];
      for (int i = 0; i < monsGroup.Length; i++)
      {
        monsArrangements_[i] = monsGroup[i];
        monsStatuses_[i] = monss[monsArrangements_[i].id];
        monsInfo_[i] = new UnitInfo(monsStatuses_[i].hp(), monsStatuses_[i].mp());
      }
    }

    // △△△味方△△△
    public Status Friend(int idx)
    {
      return chars_[idx];
    }
    public StatusWritable FriendsWritable(int idx)
    {
      return friendsWritable_[idx];
    }
    public StatusWritable[] FriendsWritable()
    {
      return friendsWritable_;
    }
    public UnitInfo FriendInfo(int idx)
    {
      return charsInfo_[idx];
    }
    // ▽▽▽味方▽▽▽

    // △△△敵△△△
    public int MonsterNumber()
    {
      return monsStatuses_.Length;
    }
    public ConstStatusMons Monster(int idx)
    {
      return monsStatuses_[idx];
    }
    public ConstMonsArrangement MonsArrangement(int idx)
    {
      return monsArrangements_[idx];
    }
    public UnitInfo MonsInfo(int idx)
    {
      return monsInfo_[idx];
    }
    public int MonsFirstAlive()
    {
      for (int i = 0; i < monsInfo_.Length; i++)
      {
        if (monsInfo_[i].hp > 0) return i;
      }
      return -1;
    }
    // ▽▽▽敵▽▽▽
  }
}
