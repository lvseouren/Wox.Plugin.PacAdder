using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;
using Control = System.Windows.Controls.Control;

namespace Wox.Plugin.PacAdder
{
    public class Main : IPlugin, ISettingProvider
    {
        private PluginInitContext _context;
        Setting _setting;
        public void Init(PluginInitContext context) 
        {
            _context = context;
            _setting = Setting.Load(context);
        }
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            string ROOT = _setting.DocPath;

            if (_setting.DocPath == null || _setting.DocPath.Trim().Length == 0)
            {
                results.Add(new Result()
                {
                    Title = "请先设置文档路径，再使用插件",
                    SubTitle = "点击进入设置界面",
                    IcoPath = "Images\\error.png",
                    Action = e => {
                        _context.API.OpenSettingDialog();
                        return true;
                    }
                });
                return results;
            }
            if (!File.Exists(_setting.DocPath))
            {
                results.Add(new Result()
                {
                    Title = "文档不存在",
                    SubTitle = "点击设置文档路径",
                    IcoPath = "Images\\error.png",
                    Action = e => {
                        _context.API.OpenSettingDialog();
                        return true;
                    }
                });
                return results;
            }

            return new List<Result>
            {
                new Result
                {
                    Title = $"往PAC中添加地址： {query.Search}",
                    IcoPath = "pac.png",
                    Action = _ =>
                    {
                        CreateEntry("var rules = [", query.Search);
                        return true;
                    }
                }
            };
        }

        public void CreateEntry(string endTag, string lineToAdd)
        {
            //var fileName = @"D:\Program Files\v2rayN-Core\v2rayN-Core\pac.txt";
            var fileName = _setting.DocPath;
            lineToAdd = String.Format("  \"||{0}\",", lineToAdd);
            
            var txtLines = File.ReadAllLines(fileName).ToList();   //Fill a list with the lines from the txt file.
            
            if (txtLines.IndexOf(lineToAdd) >= 0)//check if lineToAdd exist already
            {
                MessageBox.Show("地址已存在");
                return;
            }

            txtLines.Insert(txtLines.IndexOf(endTag)+1, lineToAdd);  //Insert the line you want to add last under the tag 'item1'.
            File.WriteAllLines(fileName, txtLines);                //Add the lines including the new one.
            MessageBox.Show("地址添加成功");
        }

        public Control CreateSettingPanel()
        {
            return new SettingControl(_setting);
        }
    }
}