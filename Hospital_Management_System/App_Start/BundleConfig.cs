using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;


namespace Hospital_Management_System
{
    public class BundleConfig
    {


        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.UseCdn = true;
            //var jqueryCdnPath = ".https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css";



            StyleBundle myCssBundle = new StyleBundle("~/Content/MyCss");

            myCssBundle.Include("~/CSS/Styles.css", "~/Content/bootstrap.min.css",
                                "~/Content/DataTables/css/jquery.dataTables.min.css",
                                "~/Content/Site.css",
                                "~/Content/bootstrap-theme.min.css",
                                "~/Content/toastr.min.css", 
                                "~/Content/DataTables/css/responsive.dataTables.min.css");

            ScriptBundle myScriptBundle = new ScriptBundle("~/Scripts/MyScript");

            myScriptBundle.Include("~/Scripts/jquery-1.11.1.min.js",
                                    "~/Scripts/bootstrap.min.js",
                                   "~/Scripts/jquery-3.5.1.min.js",
                                    "~/Scripts/DataTables/jquery.dataTables.min.js",
                                    "~/Scripts/jquery-1.9.1.min.js",
                                    "~/Scripts/toastr.min.js",
                                    "~/Scripts/jszip.min.js",
                                    "~/Scripts/DataTables/buttons.html5.min.js",
                                    "~/Scripts/DataTables/buttons.print.min.js",
                                    "~/Scripts/DataTables/dataTables.buttons.min.js",
                                    "~/Scripts/DataTables/dataTables.responsive.min.js");

            //bundles.Add(new ScriptBundle("~/bundles/jquery",jqueryCdnPath).Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(myCssBundle);
            bundles.Add(myScriptBundle);

            BundleTable.EnableOptimizations = true;

            //bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
            //          "~/Scripts/DataTables/jquery.dataTables.min.js",
            //          "~/Scripts/DataTables/dataTables.buttons.min.js",
            //          "~/Scripts/DataTables/buttons.html5.js"

            //   ));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Scripts/jquery-1.11.1.min.js"

            //            ));
            //bundles.Add(new ScriptBundle("~/bundles/jszip").Include(
            //            "~/Scripts/jszip.min.js"
            //     ));
            //bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
            //      "~/Scripts/toastr.min.js"
            //    ));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //       "~/Scripts/bootstrap.js",
            //       "~/Scripts/bootstrap.min.js"
            //       ));


            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css",

            //          "~/Content/bootstrap.min.css",
            //          "~/Content/DataTables/css/jquery.dataTables.min.css",
            //          "~/Content/toastr.min.css"
            //          ));


        }
    }
}