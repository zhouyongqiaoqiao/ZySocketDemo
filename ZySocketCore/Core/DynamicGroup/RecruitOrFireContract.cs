using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.DynamicGroup
{
    internal class RecruitOrFireContract
    {
        public RecruitOrFireContract()
        { }

        public RecruitOrFireContract(string groupID, List<string> members, string operatorID)
        {
            this.GroupID = groupID;
            this.Members = members;
            this.OperatorID = operatorID;
        }

        public String GroupID { get; set; }="";

        public List<String> Members { get; set; }=new List<String>();

        public String OperatorID { get; set; } = "";
    }
}
