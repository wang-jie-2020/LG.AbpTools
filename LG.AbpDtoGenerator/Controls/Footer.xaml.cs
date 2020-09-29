using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LG.AbpDtoGenerator.View.Controls
{
    public partial class Footer : UserControl
    {
        public Footer()
        {
            this.InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
