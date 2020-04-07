namespace Sitecore.Support.Pipelines.RenderField
{
	using Sitecore.Configuration;
	using Sitecore.Diagnostics;
	using Sitecore.Pipelines.RenderField;
	using System;
	using System.Web;

	public class GetTextFieldValue
    {
		public void Process(RenderFieldArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			EncodeFieldValue(args);
			string fieldTypeKey = args.FieldTypeKey;
			if (fieldTypeKey.Equals("text", StringComparison.InvariantCulture) || fieldTypeKey.Equals("single-line text", StringComparison.InvariantCulture))
			{
				args.WebEditParameters.Add("prevent-line-break", "true");
			}
		}

		protected virtual void EncodeFieldValue(RenderFieldArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			if (Settings.Rendering.HtmlEncodedFieldTypes.Contains(args.FieldTypeKey))
			{
				args.Result.FirstPart = HttpUtility.HtmlEncode(args.Result.FirstPart);
			}
		}
	}
}