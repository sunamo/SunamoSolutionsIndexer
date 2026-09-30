namespace SunamoDevCode._sunamo.SunamoHtml.Html;

internal class HtmlAssistant
{
    internal static string TrimInnerHtml(string value)
    {
        throw new Exception("Code without CreateHtmlDocument");
        //HtmlDocument hd = HtmlAgilityHelper.CreateHtmlDocument();
        //hd.LoadHtml(value);
        //foreach (var item in hd.DocumentNode.DescendantsAndSelf())
        //{
        //    if (item.NodeType == HtmlNodeType.Element)
        //    {
        //        item.InnerHtml = item.InnerHtml.Trim();
        //    }
        //}
        //return hd.DocumentNode.OuterHtml;
    }
    internal static string HtmlDecode(string encodedText)
    {
        return WebUtility.HtmlDecode(encodedText);
    }
}