#pragma checksum "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9689c8ed4477a1d72e255015bafee2357f013606"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Generate_Results), @"mvc.1.0.view", @"/Views/Generate/Results.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\_ViewImports.cshtml"
using SiteMapGeneratorTool;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\_ViewImports.cshtml"
using SiteMapGeneratorTool.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9689c8ed4477a1d72e255015bafee2357f013606", @"/Views/Generate/Results.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ba023cbc3fdb82444f344804c473b9d3e3be4ee6", @"/Views/_ViewImports.cshtml")]
    public class Views_Generate_Results : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
  
    ViewData["Title"] = "Generate Response";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
 if (ViewBag.Message.Valid)
{
    if (ViewBag.Message.Complete)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h1>Request complete for ");
#nullable restore
#line 9 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
                            Write(ViewBag.Message.Guid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n        <br />\r\n        <a");
            BeginWriteAttribute("href", " href=\"", 216, "\"", 288, 2);
            WriteAttributeValue("", 223, "../api/webcrawler/response/information?guid=", 223, 44, true);
#nullable restore
#line 11 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
WriteAttributeValue("", 267, ViewBag.Message.Guid, 267, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">information.json</a>\r\n        <br />\r\n        <a");
            BeginWriteAttribute("href", " href=\"", 338, "\"", 406, 2);
            WriteAttributeValue("", 345, "../api/webcrawler/response/sitemap?guid=", 345, 40, true);
#nullable restore
#line 13 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
WriteAttributeValue("", 385, ViewBag.Message.Guid, 385, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">sitemap.xml</a>\r\n        <br />\r\n        <a");
            BeginWriteAttribute("href", " href=\"", 451, "\"", 517, 2);
            WriteAttributeValue("", 458, "../api/webcrawler/response/graph?guid=", 458, 38, true);
#nullable restore
#line 15 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
WriteAttributeValue("", 496, ViewBag.Message.Guid, 496, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">graph.dgml</a>\r\n");
#nullable restore
#line 16 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h1>Request incomplete, please come back later.</h1>\r\n");
#nullable restore
#line 20 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
    }
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>Invalid GUID.</h1>\r\n");
#nullable restore
#line 25 "C:\Users\Tom\Google Drive (sc18tjo@leeds.ac.uk)\Year 3\C3931 Individual Project\6. Code\SiteMapGeneratorTool\SiteMapGeneratorTool\SiteMapGeneratorTool\Views\Generate\Results.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
