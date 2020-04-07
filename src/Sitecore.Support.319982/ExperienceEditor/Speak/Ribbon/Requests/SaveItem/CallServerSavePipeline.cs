using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.Pipelines;
using Sitecore.Pipelines.Save;
using Sitecore.ExperienceEditor.Switchers;
using Sitecore.Caching;
using Sitecore.Globalization;
using Sitecore.Configuration;
using System.Web;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
    public class CallServerSavePipeline : PipelineProcessorRequest<PageContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            PipelineProcessorResponseValue value2 = new PipelineProcessorResponseValue();
            Pipeline pipeline = PipelineFactory.GetPipeline("saveUI");
            pipeline.ID = ShortID.Encode(ID.NewID);
            SaveArgs saveArgs = base.RequestContext.GetSaveArgs();
            if (!Settings.Rendering.HtmlEncodedFieldTypes.Contains("single-line text"))
            {
                saveArgs = this.FixValues(base.RequestContext.Item.Database, base.RequestContext.FieldValues, saveArgs);
            }
            using (new ClientDatabaseSwitcher(base.RequestContext.Item.Database))
            {
                pipeline.Start(saveArgs);
                CacheManager.GetItemCache(base.RequestContext.Item.Database).Clear();
                value2.AbortMessage = Translate.Text(saveArgs.Error);
                return value2;
            }
        }
        private SaveArgs FixValues(Database database, Dictionary<string, string> dictionaryForm, SaveArgs saveArgs)
        {
            string[] array = dictionaryForm.Keys.ToArray<string>();
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (text.StartsWith("fld_", StringComparison.InvariantCulture) || text.StartsWith("flds_", StringComparison.InvariantCulture))
                {
                    string text2 = text;
                    int num = text2.IndexOf('$');
                    if (num >= 0)
                    {
                        text2 = StringUtil.Left(text2, num);
                    }
                    string[] array3 = text2.Split(new char[]
                    {
                '_'
                    });
                    ID iD = ShortID.DecodeID(array3[1]);
                    ID iD2 = ShortID.DecodeID(array3[2]);
                    Item item = database.GetItem(iD);
                    if (item != null)
                    {
                        Field field = item.Fields[iD2];
                        string typeKey = field.TypeKey;
                        if (typeKey != null && typeKey.Equals("single-line text", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (dictionaryForm[text] == HttpUtility.HtmlDecode(dictionaryForm[text]))
                            {
                                saveArgs.Items.Where(s => s.ID == item.ID).FirstOrDefault().Fields.Where(ab => ab.ID == iD2).ToList().ForEach(field1 => field1.Value = dictionaryForm[text]);
                            }
                        }
                    }
                }
            }
            return saveArgs;
        }
    }
}