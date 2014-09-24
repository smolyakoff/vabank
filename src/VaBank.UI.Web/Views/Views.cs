using RazorGenerator.Templating;
using SquishIt.Framework;

namespace VaBank.UI.Web.Views
{
    //Just dummy class to make resharper and visual studio happy
    public partial class Index : IndexBase
    {
         
    }

    public class IndexBase : RazorTemplateBase
    {
        public virtual string Styles()
        {
            var bundle = Bundle.Css();

            bundle
                .Add(BowerPath("angular-datepicker/dist/index.css"))
                .Add(BowerPath("isteven-angular-multiselect/angular-multi-select.css"))
                .Add(BowerPath("angular-loading-bar/build/loading-bar.css"))
                .Add(ApplicationPath("styles/vabank.scss"));

            return bundle.Render("~/Client/styles_#.css");
        }

        public virtual string Scripts()
        {
            var bundle = Bundle.JavaScript();

            bundle
                .Add(BowerPath("moment/moment.js"))
                .Add(BowerPath("underscore/underscore.js"))
                .Add(BowerPath("angular/angular.js"))
                .Add(BowerPath("angular-resource/angular-resource.js"))
                .Add(BowerPath("angular-ui-router/release/angular-ui-router.js"))
                .Add(BowerPath("angular-bootstrap/ui-bootstrap.js"))
                .Add(BowerPath("angular-bootstrap/ui-bootstrap-tpls.js"))
                .Add(BowerPath("angular-loading-bar/build/loading-bar.js"))
                .Add(BowerPath("angular-datepicker/dist/index.js"))
                .Add(BowerPath("isteven-angular-multiselect/angular-multi-select.js"))
                .Add(BowerPath("angular-smart-table/dist/smart-table.debug.js"));

            bundle
                .Add(ApplicationPath("modules/vabank-ui/vabank-ui.js"))
                .AddDirectory(ApplicationPath("modules/vabank-ui"))
                .Add(ApplicationPath("vabank.js"))
                .AddDirectory(ApplicationPath("config"))
                .AddDirectory(ApplicationPath("areas/global"))
                .AddDirectory(ApplicationPath("areas/admin/config"))
                .AddDirectory(ApplicationPath("areas/admin/scheduler"))
                .AddDirectory(ApplicationPath("areas/admin/system-log"));

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