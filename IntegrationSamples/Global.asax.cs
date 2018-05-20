using IntegrationSamples.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IntegrationSamples
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private string con = AppSettings.ConnectionString;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //here in Application Start we will start Sql Dependency
            //SqlDependency.Start(con);
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    NotificationComponent NC = new NotificationComponent();
        //    var currentTime = DateTime.Now;
        //    HttpContext.Current.Session["LastUpdated"] = currentTime;
        //    NC.RegisterNotification(currentTime);
        //}


        protected void Application_End()
        {
            //here we will stop Sql Dependency
            //SqlDependency.Stop(con);
        }
    }
}
