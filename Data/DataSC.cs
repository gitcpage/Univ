using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class DataSC
  {
    static public ConstItem[] Items()
    {
      Loader loader = Loader.Instance;
      return loader.itemData;
    }
    static public Bag Bag()
    {
      return Data.Bag.Instance;
    }
    static public Status[] Friends()
    {
      return Status.Instances;
    }
    static public StatusWritable[] FriendsWritable(SecurityToken stFriends)
    {
      Loader loader = Loader.Instance;
      return loader.Friends(stFriends);
    }
    static public Basic Basic()
    {
      return Data.Basic.Instance;
    }
    static public ConstStatus[] Equips(EquipCategory equipCategory)
    {
      Loader loader = Loader.Instance;
      return loader.EquipArray(equipCategory);
    }
    static public ConstStatus[] Equips(int equipCategory)
    {
      return Equips((EquipCategory)equipCategory);
    }
  }
}
