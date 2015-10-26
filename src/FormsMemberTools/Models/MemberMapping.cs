using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FormsMemberTools.Models
{
    [DataContract(Name = "memberMapping")]
    public class MemberMapping
    {
        [DataMember(Name = "id")]
        public string Alias { get; set; }
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "staticValue")]
        public string StaticValue { get; set; }

        public bool HasValue()
        {
            return !string.IsNullOrEmpty(Field) || !string.IsNullOrEmpty(StaticValue);
        }
    }
}