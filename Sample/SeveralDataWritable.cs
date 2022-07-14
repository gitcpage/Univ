using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class SeveralDataWritable : SeveralData
  {
    // 書き込みアクセサ
    public void name(string name) { name_ = name; }

    private SeveralDataWritable() { }
    static readonly object lockObject_ = new object();
    public static new SeveralDataWritable[] Instances(int instanceNumber)
    {
      SeveralDataWritable[] insts;
      lock (lockObject_)
      {
        JsTrans.Assert(s_instances == null,
          typeof(SeveralDataWritable).Name + " がインスタンスが既にインスタンス化されています。");
        s_num = instanceNumber;
        s_instances = insts = new SeveralDataWritable[instanceNumber];
        for (int i = 0; i < instanceNumber; i++)
          s_instances[i] = new SeveralDataWritable();
      }
      return insts;
    }
  }
}
