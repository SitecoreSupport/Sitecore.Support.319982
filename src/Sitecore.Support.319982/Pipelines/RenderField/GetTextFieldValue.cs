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
			if (IsPreviewPageMode() || IsNormalPageMode())
			{
				DecodeFieldValue(args);
			}
			else
			{
				EncodeFieldValue(args);
			}
			string fieldTypeKey = args.FieldTypeKey;
			if (fieldTypeKey.Equals("text", StringComparison.InvariantCulture) || fieldTypeKey.Equals("single-line text", StringComparison.InvariantCulture))
			{
				args.WebEditParameters.Add("prevent-line-break", "true");
			}
		}

		protected virtual bool IsPreviewPageMode()
		{
			return Context.PageMode.IsPreview;
		}

		protected virtual bool IsNormalPageMode()
		{
			return Context.PageMode.IsNormal;
		}

		protected virtual void EncodeFieldValue(RenderFieldArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			if (Settings.Rendering.HtmlEncodedFieldTypes.Contains(args.FieldTypeKey) || args.FieldTypeKey.Contains("single-line text") || args.FieldTypeKey.Contains("multi-line text"))
			{
				args.Result.FirstPart = HttpUtility.HtmlEncode(args.Result.FirstPart);
			}
		}

		protected virtual void DecodeFieldValue(RenderFieldArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			if (Settings.Rendering.HtmlEncodedFieldTypes.Contains(args.FieldTypeKey) || args.FieldTypeKey.Contains("single-line text") || args.FieldTypeKey.Contains("multi-line text"))
			{
				args.Result.FirstPart = HttpUtility.HtmlDecode(args.Result.FirstPart);
			}
		}
	}
}