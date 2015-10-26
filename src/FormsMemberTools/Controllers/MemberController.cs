using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.member;
using Umbraco.Forms.Web.Models.Backoffice;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace FormsMemberTools.Controllers
{
    [PluginController("FormsMemberTools")]
    public class MemberController : UmbracoAuthorizedJsonController  
    {

        public IEnumerable<PickerItem> GetAllMemberTypesWithAlias()
        {
            var list = new List<PickerItem>();
           
            Guid[] guids = umbraco.cms.businesslogic.CMSNode.getAllUniquesFromObjectType(new Guid("9b5416fb-e72f-45a9-a07b-5a9a2709ce43"));
            foreach (var g in guids)
            {
                
                var mt = new MemberType(g);
                var p = new PickerItem
                {
                    Id = mt.Alias,
                    Value = mt.Text
                };
                list.Add(p);
                
            }
            return list;
        }

        public IEnumerable<PickerItem> GetAllProperties(string membertypeAlias)
        {
            var list = new List<PickerItem>();



            var memtype = umbraco.cms.businesslogic.member.MemberType.GetByAlias(membertypeAlias);

            foreach(var prop in memtype.PropertyTypes)
            {
                var p = new PickerItem
                {
                    Id = prop.Alias,
                    Value = prop.Name
                };

                if(!list.Contains(p))
                    list.Add(p);
            }

            return list;
        }
 
    
    }
}