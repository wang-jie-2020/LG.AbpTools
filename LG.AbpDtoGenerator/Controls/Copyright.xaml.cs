using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LG.AbpDtoGenerator.View.Controls
{
    public partial class Copyright : UserControl
    {
        public Copyright()
        {
            this.InitializeComponent();

            this.txtVersion.Text = "3.2.2";
            this.txtTime.Text = AppConsts.StartAppTime;
            this.txtDateYear.Text = DateTime.Now.Year.ToString();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
