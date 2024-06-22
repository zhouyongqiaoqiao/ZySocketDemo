using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contract
{
    internal class QueryBeforeLoginContract
    {
        public QueryBeforeLoginContract() { }
        public QueryBeforeLoginContract(int queryType, string queryData)
        {
            this.QueryType = queryType;
            this.QueryData = queryData;
        }

        public int QueryType { get; set; }
        public string QueryData { get; set; }
    }
}
