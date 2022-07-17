using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class CharsWritable : Chars
  {
    // 書き込みアクセサ
    public void name(string name) { name_ = name; }
    public void hp(int hp) { hp_ = hp; }

    private CharsWritable() { }

    static readonly object lockObject_ = new object();
    public static new CharsWritable[] Instances(int instanceNumber)
    {
      CharsWritable[] insts;
      lock (lockObject_)
      {
        JsTrans.Assert(s_instances == null,
          typeof(SeveralDataWritable).Name + " がインスタンスが既にインスタンス化されています。");
        s_num = instanceNumber;
        s_instances = insts = new CharsWritable[instanceNumber];
        for (int i = 0; i < instanceNumber; i++)
          s_instances[i] = new CharsWritable();
      }
      return insts;
    }
  }
}
