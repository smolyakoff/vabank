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

            return bundle.Render("~/styles_#.css");
        }

        public virtual string Scripts()
        {
            var bundle = Bundle.JavaScript();

            bundle.Add(BowerPath("angular/angular.js"));

            bundle.Add(ApplicationPath("vabank.js"));
            return bundle.Render("~/app_#.js");
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