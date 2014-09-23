using RazorGenerator.Templating;
using SquishIt.Framework;

namespace VaBank.UI.Web.Views
{
    //Just dummy classes to make resharper and visual studio happy
    public partial class Index : IndexBase
    {
         
    }

    public class IndexBase : RazorTemplateBase
    {
        public virtual string Styles()
        {
            var bundle = Bundle.Css();

            bundle.Add(ApplicationPath("vabank.scss"));

            return bundle.Render("~/Client/styles_#.css");
        }

        public virtual string Scripts()
        {
            var bundle = Bundle.JavaScript();

            bundle.Add(BowerPath("angular/angular.js"))
                .Add(BowerPath("angular-resource/angular-resource.js"))
                .Add(BowerPath("angular-ui-router/release/angular-ui-router.js"))
                .Add(BowerPath("angular-bootstrap/ui-bootstrap.js"))
                .Add(BowerPath("angular-bootstrap/ui-bootstrap-tpls.js"));

            bundle.Add(ApplicationPath("vabank.js"))
                .AddDirectory(ApplicationPath("config"))
                .AddDirectory(ApplicationPath("areas/admin/config"));

            return bundle.Render("~/Client/app_#.js");
        }

        private static string BowerPath(string relativePath)
        {
            return string.Format("/Client/bower_components/{0}", relativePath);
        }

        private static string ApplicationPath(string relativePath)
        {
            return string.Format("/Client/app/{0}", relativePath);
        }
    }
}