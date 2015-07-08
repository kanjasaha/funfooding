using System.Web.Mvc;
using MobileViewEngines.MVC3;
 
[assembly: WebActivator.PreApplicationStartMethod(typeof(ConsoleApp.MobileViewEngines), "Start")]
namespace ConsoleApp {
    public static class MobileViewEngines{
        public static void Start() 
        {
            ViewEngines.Engines.Insert(0, new MobileCapableRazorViewEngine());
        }
    }
}