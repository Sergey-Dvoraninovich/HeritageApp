#pragma checksum "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f5cb47f2e0a3032e4dc6d05c2839d45bee9f921c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Programs_Index), @"mvc.1.0.view", @"/Views/Programs/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f5cb47f2e0a3032e4dc6d05c2839d45bee9f921c", @"/Views/Programs/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"23ac09be4bcfaa7f9829a01d1a134874eaae1f3b", @"/Views/_ViewImports.cshtml")]
    public class Views_Programs_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<HeritageWebApplication.Models.RenovationCompany>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
  
    ViewBag.Title = "Список Программ";

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div>\r\n");
#nullable restore
#line 10 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
     foreach (var renovation in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <p><h1>");
#nullable restore
#line 12 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
          Write(renovation.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1></p>\r\n        <p>");
#nullable restore
#line 13 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
      Write(renovation.Desc);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n        <a class=\"draw-outline draw-outline--tandem\"");
            BeginWriteAttribute("href", " href=\"", 332, "\"", 373, 2);
            WriteAttributeValue("", 339, "/programs/detail?id=", 339, 20, true);
#nullable restore
#line 14 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
WriteAttributeValue("", 359, renovation.Id, 359, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Просмотр</a>\r\n");
#nullable restore
#line 15 "C:\Users\Сергей\RiderProjects\HeritageWebApplication\HeritageWebApplication\Views\Programs\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<HeritageWebApplication.Models.RenovationCompany>> Html { get; private set; }
    }
}
#pragma warning restore 1591
