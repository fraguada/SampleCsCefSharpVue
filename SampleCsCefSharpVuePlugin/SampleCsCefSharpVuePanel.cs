using System.Windows.Forms;

namespace SampleCsCefSharpVue
{
    /// <summary>
    /// This is the user control that is buried in the tabbed, docking panel.
    /// </summary>
    [System.Runtime.InteropServices.Guid("1CD7B8FF-8A6E-4741-9EF8-62A240337CDB")]
    public partial class SampleCsCefSharpVuePanel : UserControl
    {

        /// <summary>
        /// Returns the ID of this panel.
        /// </summary>
        public static System.Guid PanelId => typeof(SampleCsCefSharpVuePanel).GUID;

        /// <summary>
        /// Public constructor
        /// </summary>
        public SampleCsCefSharpVuePanel()
        {
            InitializeComponent();

            this.Controls.Add(SampleCsCefSharpVuePlugIn.Browser);

            // Set the user control property on our plug-in
            SampleCsCefSharpVuePlugIn.Instance.PanelUserControl = this;
        }
    }
}
