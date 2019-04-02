using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines.Save;
using Sitecore.Xml;
using System;
using System.Xml;

namespace Sitecore.Support.Pipelines.Save
{
    public class ParseXml: Sitecore.Pipelines.Save.ParseXml
    {
        public new void Process(SaveArgs args)
        {

            Assert.ArgumentNotNull(args, "args");
            if (args.Items == null)
            {
                base.Process(args);
            }
        }

    }
}