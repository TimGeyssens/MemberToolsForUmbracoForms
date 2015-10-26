using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.property;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Providers.Models;
using Umbraco.Forms.Data;
using FormsMemberTools.Models;

namespace FormsMemberTools.Providers.WorkflowTypes
{
    public class SaveAsUmbracoMember: WorkflowType
    {

        public SaveAsUmbracoMember()
        {
            this.Id = new Guid("ad639eeb-5e7a-47cd-9555-31183171956e");
            this.Name = "Save as member";
            this.Description = "Saves the form values as an umbraco member";
        }


        [Setting("Member Type", description = "Map member type",
            view = "~/App_Plugins/FormsMemberTools/SettingTypes/membermapper.html")]
        public string Fields { get; set; }

        public override Umbraco.Forms.Core.Enums.WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            var maps = JsonConvert.DeserializeObject<MemberMapper>(Fields);

            Dictionary<string, string> mappings = new Dictionary<string, string>();
          

            string nameMapping = "NodeName";
            string loginMapping = "Login";
            string emailMapping = "Email";
            string passwordMapping = "Password";

            if (!string.IsNullOrEmpty(maps.NameStaticValue))
                nameMapping = maps.NameStaticValue;
            else if (!string.IsNullOrEmpty(maps.NameField))
                nameMapping = record.RecordFields[new Guid(maps.NameField)].ValuesAsString(false);

            if (!string.IsNullOrEmpty(maps.LoginStaticValue))
                loginMapping = maps.LoginStaticValue;
            else if (!string.IsNullOrEmpty(maps.LoginField))
                loginMapping = record.RecordFields[new Guid(maps.LoginField)].ValuesAsString(false);

            if (!string.IsNullOrEmpty(maps.EmailStaticValue))
                emailMapping = maps.EmailStaticValue;
            else if (!string.IsNullOrEmpty(maps.EmailField))
                emailMapping = record.RecordFields[new Guid(maps.EmailField)].ValuesAsString(false);

            if (!string.IsNullOrEmpty(maps.PasswordStaticValue))
                passwordMapping = maps.PasswordStaticValue;
            else if (!string.IsNullOrEmpty(maps.PasswordField))
                passwordMapping = record.RecordFields[new Guid(maps.PasswordField)].ValuesAsString(false);


            foreach (var map in maps.Properties)
            {
                if (map.HasValue())
                {

                    var val = map.StaticValue;
                    if (!string.IsNullOrEmpty(map.Field))
                        val = record.RecordFields[new Guid(map.Field)].ValuesAsString(false);
                    mappings.Add(map.Alias, val);
                }
            }

            MemberType mt = MemberType.GetByAlias(maps.MemTypeAlias);

            if (mt != null)
            {
                Member m = Member.MakeNew(nameMapping, loginMapping, emailMapping, mt, new umbraco.BusinessLogic.User(0));
                m.Password = passwordMapping;

                foreach (Property p in m.getProperties)
                {

                    try
                    {

                        if (mappings.ContainsKey(p.PropertyType.Alias))
                        {
                            switch (((BaseDataType)p.PropertyType.DataTypeDefinition.DataType).DBType)
                            {
                                case DBTypes.Date:
                                    p.Value = DateTime.Parse(mappings[p.PropertyType.Alias]);
                                    break;
                                case DBTypes.Integer:
                                    string v = mappings[p.PropertyType.Alias];
                                    if (v.ToLower() == "true" || v.ToLower() == "false")
                                        p.Value = bool.Parse(v);
                                    else
                                        p.Value = int.Parse(mappings[p.PropertyType.Alias]);
                                    break;
                                default:
                                    p.Value = mappings[p.PropertyType.Alias];
                                    break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<SaveAsUmbracoMember>(ex);
                    }
                }

                m.XmlGenerate(new System.Xml.XmlDocument());
                m.Save();


                //store record id and member id

                //SettingsStorage ss = new SettingsStorage();
                //ss.InsertSetting(record.Id, "SaveAsUmbracoMemberCreatedMemberID", m.Id.ToString());
                //ss.Dispose();

            }

            return Umbraco.Forms.Core.Enums.WorkflowExecutionStatus.Completed;
        }

        public override List<Exception> ValidateSettings()
        {
            return new List<Exception>();
        }
    }
}