using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        public virtual string ServerInfo()
        {
            var info = new ServerInfo();
            var infoJson = JsonConvert.SerializeObject(info, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return string.Format("<script id=\"server-info\" type=\"text/json\">{0}</script>", infoJson);
        }

        public virtual string Styles()
        {
            var bundle = Bundle.Css();

            bundle
                .Add(BowerPath("angular-bootstrap-datetimepicker/src/css/datetimepicker.css"))
                .Add(BowerPath("isteven-angular-multiselect/angular-multi-select.css"))
                .Add(BowerPath("angular-loading-bar/build/loading-bar.css"))
                .Add(BowerPath("angular-toastr/dist/angular-toastr.css"))
                .Add(ApplicationPath("styles/vabank.scss"));

            return bundle.Render("~/Client/styles_#.css");
        }

        public virtual string Scripts()
        {
            var bundle = Bundle.JavaScript();

            bundle
                .Add(BowerPath("moment/moment.js"))
                .Add(BowerPath("underscore/underscore.js"))
                .Add(BowerPath("js-schema/js-schema.debug.js"))
                .Add(BowerPath("spin.js/spin.js"))
                .Add(BowerPath("angular/angular.js"))
                .Add(BowerPath("angular-resource/angular-resource.js"))
                .Add(BowerPath("angular-local-storage/dist/angular-local-storage.js"))
                .Add(BowerPath("angular-ui-router/release/angular-ui-router.js"))
                .Add(BowerPath("angular-bootstrap/ui-bootstrap.js"))
                .Add(BowerPath("angular-bootstrap/ui-bootstrap-tpls.js"))
                .Add(BowerPath("angular-loading-bar/build/loading-bar.js"))
                .Add(BowerPath("angular-promise-tracker/promise-tracker.js"))
                .Add(BowerPath("angular-spinner/angular-spinner.js"))
                .Add(BowerPath("angular-toastr/dist/angular-toastr.js"))
                .Add(BowerPath("angular-bootstrap-datetimepicker/src/js/datetimepicker.js"))
                .Add(BowerPath("angular-date-time-input/src/dateTimeInput.js"))
                .Add(BowerPath("isteven-angular-multiselect/angular-multi-select.js"))
                .Add(BowerPath("angular-smart-table/dist/smart-table.debug.js"))
                .Add(BowerPath("angular-form-for/dist/form-for.js"))
                .Add(BowerPath("angular-form-for/dist/form-for.bootstrap-templates.js"));

            bundle
                .Add(ApplicationPath("modules/vabank-ui/vabank-ui.js"))
                .AddDirectory(ApplicationPath("modules/vabank-ui/config"))
                .AddDirectory(ApplicationPath("modules/vabank-ui"))
                .Add(ApplicationPath("vabank.js"))
                .AddDirectory(ApplicationPath("areas/global/config"))
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