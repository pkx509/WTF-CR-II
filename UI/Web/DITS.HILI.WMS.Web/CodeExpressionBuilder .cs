using DITS.HILI.WMS.ClientService;
using DITS.HILI.WMS.MasterModel.Core;
using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;

namespace DITS.HILI.WMS.Web
{
    [ExpressionPrefix("Resource")]
    public class ExpressionBuilderResource : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry,
           object parsedData, ExpressionBuilderContext context)
        {
            CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());

            CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

            string evaluationMethod = "GetResource";

            return new CodeMethodInvokeExpression(thisType, evaluationMethod, new CodeExpression[] { expression });
        }
        public static string GetResource(string key)
        {
            Core.Domain.ApiResponseMessage resp = WMSProperty.GetResource(key).Result;
            if (resp.IsSuccess)
            {
                CustomResource c = resp.Get<CustomResource>();
                return c.ResourceValue;
            }
            return "";
        }
    }

    [ExpressionPrefix("Message")]
    public class ExpressionBuilderMessage : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry,
           object parsedData, ExpressionBuilderContext context)
        {
            CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());

            CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

            string evaluationMethod = "GetMessage";

            return new CodeMethodInvokeExpression(thisType, evaluationMethod, new CodeExpression[] { expression });
        }
        public static string GetMessage(string key)
        {
            Core.Domain.ApiResponseMessage resp = WMSProperty.GetMessage(key).Result;
            if (resp.IsSuccess)
            {
                CustomMessage c = resp.Get<CustomMessage>();
                return c.MessageValue;
            }
            return "";
        }
    }

    [ExpressionPrefix("MessageTitle")]
    public class ExpressionBuilderMessageTitle : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry,
           object parsedData, ExpressionBuilderContext context)
        {
            CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());

            CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

            string evaluationMethod = "GetMessage";

            return new CodeMethodInvokeExpression(thisType, evaluationMethod, new CodeExpression[] { expression });
        }
        public static string GetMessage(string key)
        {
            Core.Domain.ApiResponseMessage resp = WMSProperty.GetMessage(key).Result;
            if (resp.IsSuccess)
            {
                CustomMessage c = resp.Get<CustomMessage>();
                return c.MessageTitle;
            }
            return "";
        }
    }
}