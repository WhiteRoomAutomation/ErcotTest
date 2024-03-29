﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Configuration;
using HtmlAgilityPack;


namespace ErcotTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*var client = new System.Net.WebClient();

            Console.WriteLine (client.DownloadString("http://www.ercot.com/content/cdr/html/real_time_spp"));

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = (SettingsSection)config.GetSection("system.net/settings");
            settings.HttpWebRequest.UseUnsafeHeaderParsing = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("system.net/settings");*/

            var url = "http://www.ercot.com/content/cdr/html/current_np6788";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            foreach (var tNode in doc.DocumentNode.SelectNodes("//table"))
            {
                DataTable dt1 = new DataTable();
                Console.WriteLine(tNode.Name);


                try
                {               

                    //foreach (var hNode in tNode.SelectNodes("//th"))
                    foreach (var hNode in tNode.SelectNodes("//td[@class='headerValueClass']"))
                    {
                        //Console.WriteLine(hNode.InnerText);
                        dt1.Columns.Add(hNode.InnerText, typeof(string));
                    }
                    var Rows = tNode.Descendants("tr")
                        .Where(tr => tr.Elements("td").Count() > 1)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).Where(td => td.Length > 0).ToList())
                        .ToList();
                    //Console.WriteLine(Rows.Count);
                    foreach (List<string> row in Rows)
                    {
                        dt1.Rows.Add(row.ToArray());
                    }
                }
                catch (Exception ex)
                {

                }

                var selRows = dt1.Select("[Settlement Point] = 'HB_SOUTH'");
                if (selRows.Length > 0)
                {
                    Console.WriteLine(selRows[0][3]);
                }


                

            }
        }

        


    }
}
