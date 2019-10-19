using System;
using System.IO;
using System.Reflection;
using CefSharp;
using CefSharp.WinForms;
using Rhino.PlugIns;
using Rhino.UI;

namespace SampleCsCefSharpVue
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class SampleCsCefSharpVuePlugIn : Rhino.PlugIns.PlugIn
    {
        public static ChromiumWebBrowser Browser;
        public static SampleCsCefSharpInterop Interop;

        public SampleCsCefSharpVuePlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the SampleCsChromiumVuePlugIn plug-in.</summary>
        public static SampleCsCefSharpVuePlugIn Instance
        {
            get; private set;
        }

        /// <summary>
        /// The tabbed dockbar user control
        /// </summary>
        public SampleCsCefSharpVuePanel PanelUserControl { get; set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.

        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            Panels.RegisterPanel(this, typeof(SampleCsCefSharpVuePanel), "SampleCsCefSharpVue", SampleCsCefSharpVue.Properties.Resources.icon);

            if (!Cef.IsInitialized)
                InitializeCef();

            // initialise one browser instance
            InitializeCefSharp();

            Interop = new SampleCsCefSharpInterop(Browser);

            // make them talk together
            Browser.RegisterAsyncJsObject("Interop", Interop);

            Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            Rhino.RhinoDoc.AddRhinoObject += RhinoDoc_AddRhinoObject;

            return base.OnLoad(ref errorMessage);
        }

        private void RhinoDoc_AddRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
        {
            Interop.AddText(e.TheObject.ObjectType.ToString());
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            if (Browser.IsBrowserInitialized)
                Browser.ShowDevTools();

            Interop.AddText("Hello World!");
        }

        private void InitializeCefSharp()
        {
#if DEBUG
            //use localhost
            Browser = new ChromiumWebBrowser(@"http://localhost:8080/");
#else
            //use app files

            // WARNING: THIS SHOULD POINT TO YOUR WEBAPP DIST FILES

            var path = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            Debug.WriteLine(path, "SampleCsCefSharpVue");

            var indexPath = string.Format(@"{0}\app\index.html", path);

            if (!File.Exists(indexPath))
                Debug.WriteLine("SampleCsCefSharpVue: Error. The html file doesn't exists : {0}", "SampleCsCefSharpVue");

            indexPath = indexPath.Replace("\\", "/");

            Browser = new ChromiumWebBrowser(indexPath);
            index = indexPath;
#endif
            // Allow the use of local resources in the browser
            Browser.BrowserSettings = new BrowserSettings
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            Browser.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private void InitializeCef()
        {
            Cef.EnableHighDPISupport();

            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyPath = Path.GetDirectoryName(assemblyLocation);
            var pathSubprocess = Path.Combine(assemblyPath, "CefSharp.BrowserSubprocess.exe");
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            var settings = new CefSettings
            {
                LogSeverity = LogSeverity.Verbose,
                LogFile = "ceflog.txt",
                BrowserSubprocessPath = pathSubprocess,

            };

            settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1");
            settings.CefCommandLineArgs.Add("disable-web-security", "1");
            Cef.Initialize(settings);
        }
    }
}