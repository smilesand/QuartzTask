using AnalyzeTask.Tool.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeTask.DataSource
{
    /// <summary>
    /// 获取需要同步的数据
    /// </summary>
    public class NeedSynData : IDisposable
    {
        /// <summary>
        /// 需要同步的时间集合
        /// </summary>
        public DbContext dbContext = new DbContext();

        public void GetNeedSynData()
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
