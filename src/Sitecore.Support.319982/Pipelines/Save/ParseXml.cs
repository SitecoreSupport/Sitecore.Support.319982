using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines.Save;
using Sitecore.Xml;
using System;
using System.Xml;

namespace Sitecore.Support.Pipelines.Save
{
    public class ParseXml
    {
        public void Process(SaveArgs args)
        {

            Assert.ArgumentNotNull(args, "args");
            if (args.Items == null)
            {
                XmlDocument xml = args.Xml;
                Assert.IsNotNull(xml, "Missing XML for saving item");
                XmlDocument document2 = new XmlDocument();
                System.Xml.XmlNode node = XmlUtil.AddElement("sitecore", document2);
                XmlNodeList list = xml.SelectNodes("/sitecore/field");
                Assert.IsNotNull(list, "/sitecore/field");
                foreach (System.Xml.XmlNode node2 in list)
                {
                    string attribute = XmlUtil.GetAttribute("itemid", node2);
                    string str2 = XmlUtil.GetAttribute("language", node2);
                    string str3 = XmlUtil.GetAttribute("version", node2);
                    string str4 = XmlUtil.GetAttribute("itemrevision", node2);
                    string str5 = XmlUtil.GetAttribute("fieldid", node2);
                    string childValue = XmlUtil.GetChildValue("value", node2);
                    string[] textArray1 = new string[] { "/sitecore/item[@itemid='", attribute, "' and @language='", str2, "' and @version='", str3, "']" };
                    System.Xml.XmlNode node3 = document2.SelectSingleNode(string.Concat(textArray1));
                    if (node3 == null)
                    {
                        node3 = XmlUtil.AddElement("item", node);
                        XmlUtil.SetAttribute("itemid", attribute, node3);
                        XmlUtil.SetAttribute("language", str2, node3);
                        XmlUtil.SetAttribute("version", str3, node3);
                        XmlUtil.SetAttribute("itemrevision", str4, node3);
                    }
                    System.Xml.XmlNode node4 = XmlUtil.AddElement("field", node3);
                    XmlUtil.SetAttribute("fieldid", str5, node4);
                    XmlUtil.SetValue(childValue, node4);
                }
                XmlNodeList list2 = document2.SelectNodes("/sitecore/item");
                Assert.IsNotNull(list2, "/sitecore/item");
                SaveArgs.SaveItem[] itemArray = new SaveArgs.SaveItem[list2.Count];
                int index = 0;
                while (index < list2.Count)
                {
                    System.Xml.XmlNode node5 = list2[index];
                    XmlNodeList list3 = node5.SelectNodes("field");
                    Assert.IsNotNull(list3, "field");
                    SaveArgs.SaveItem item = new SaveArgs.SaveItem
                    {
                        ID = ID.Parse(XmlUtil.GetAttribute("itemid", node5)),
                        Version = Sitecore.Data.Version.Parse(XmlUtil.GetAttribute("version", node5)),
                        Language = Language.Parse(XmlUtil.GetAttribute("language", node5)),
                        Revision = XmlUtil.GetAttribute("itemrevision", node5),
                        Fields = new SaveArgs.SaveField[list3.Count]
                    };
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= list3.Count)
                        {
                            itemArray[index] = item;
                            index++;
                            break;
                        }
                        System.Xml.XmlNode node6 = list3[num2];
                        SaveArgs.SaveField field1 = new SaveArgs.SaveField();
                        field1.ID = new ID(XmlUtil.GetAttribute("fieldid", node6));
                        field1.Value = XmlUtil.GetValue(node6);
                        item.Fields[num2] = field1;
                        num2++;
                    }
                }
                args.Items = itemArray;
            }
        }

    }
}