using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ZySocketCore.Core.DynamicGroup
{
    //
    // 摘要:
    //     群组的信息。
    public class GroupInfo
    {
        public GroupInfo() { }
        public GroupInfo(string id, List<string> members)
        {
            ID = id;
            Members.AddRange(members);
        }

        //
        // 摘要:
        //     组ID。
        public string ID { get; set; }

        /// <summary>
        /// 附加备注信息。如要修改，请调用IDynamicGroupOutter或IDynamicGroupController的SetGroupTag方法，修改后，Tag将自动在C/S间同步。
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 组成员列表（只读）。
        /// </summary>
        public List<string> Members { get;set; }=new List<string>() { };

        /// <summary>
        /// 本地附加信息（只在当前进程使用，不会在C/S间同步）。
        /// </summary>        
        [JsonIgnore]
        public object LocalTag { get; set; }

        /// <summary>
        /// 是否自动退出组（只在当前进程使用，不会在C/S间同步）。
        /// </summary>
        [JsonIgnore]
        internal bool QuitGroupWhenOffline { get; set; } = false;

        public void SetTag(string tag)
        {
            Tag = tag;
        }

        public void AddMembers(List<string> members)
        {
            foreach (var item in members)
            {
                this.AddMember(item);
            }
        }

        public bool AddMember(string member)
        {
            if (Members.Contains(member)) return false;
            Members.Add(member);
            return true;
        }

        public void RemoveMembers(List<string> members)
        {
            foreach (var item in members)
            {
                RemoveMember(item);
            }
        }

        public bool RemoveMember(string member)
        {
            if (Members != null && Members.Contains(member))
            {
                Members.Remove(member);
                return true;
            }
            return false;
        }

        public void SetGroupActionType(bool isAutoQuit)
        {
            this.QuitGroupWhenOffline = isAutoQuit;
        }

        public override string ToString()
        {
            return $"ID:{ID},Members:{String.Join(",",Members)},Tag:{Tag}";
        }
    }
}
