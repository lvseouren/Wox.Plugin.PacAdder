using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace Wox.Plugin.PacAdder
{
    /// <summary>
    /// SettingControl.xaml 的交互逻辑
    /// </summary>
    public partial class SettingControl : UserControl
    {
        Setting _setting;
        public SettingControl(Setting setting)
        {
            InitializeComponent();
            this._setting = setting;
            DocPath.Text = _setting.DocPath;
        }

        private void OnSelectDirectoryClick(object sender, RoutedEventArgs e)
        {
            var fileBrowserDialog = new OpenFileDialog();
            if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                _setting.DocPath = fileBrowserDialog.FileName;
                DocPath.Text = _setting.DocPath;
                _setting.Save();
            }
        }
    }
}
