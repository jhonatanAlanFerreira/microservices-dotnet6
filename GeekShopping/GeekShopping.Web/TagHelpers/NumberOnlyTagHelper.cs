using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GeekShopping.Web.TagHelpers
{

    [HtmlTargetElement("input", Attributes  = "number-only")]
    public class NumberOnlyTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("onkeydown", "return event.keyCode < 65 || (event.keyCode > 95 && event.keyCode < 106)");
        }
    }
}
