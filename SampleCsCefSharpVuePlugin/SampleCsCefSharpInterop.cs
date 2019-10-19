using CefSharp;
using CefSharp.WinForms;
using Rhino.Geometry;
using System.Diagnostics;

namespace SampleCsCefSharpVue
{
    public class SampleCsCefSharpInterop
    {
        public ChromiumWebBrowser Browser { get; private set; }
        public SampleCsCefSharpInterop(ChromiumWebBrowser browser)
        {
            Browser = browser;
     
        }

        #region To UI (Generic)
        // from SpeckleRhino
        public void NotifyFrame(string eventType, string eventInfo)
        {

            var script = string.Format("window.EventBus.$emit('{0}', '{1}')", eventType, eventInfo);
            try
            {
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
            catch
            {
                Debug.WriteLine("For some reason, this browser was not initialised.");
            }
        }
        #endregion

        #region To UI

        public void AddText(string text)
        {
            NotifyFrame("add-text", text);
        }

        #endregion

        #region From UI

        public void doSomething(dynamic args)
        {
            var location = new Point3d(args.location[0], args.location[1], args.location[2]);
            var sphere = new Sphere(location, args.radius);
            Rhino.RhinoDoc.ActiveDoc.Objects.AddSphere(sphere);
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        #endregion



    }
}
