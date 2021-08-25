using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleanCommentSource
{
    public partial class Form1 : Form
    {
        private string[] _oldLines;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select Photos";

            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                try
                {
                    string fPath = "";

                    foreach (String file in openFileDialog1.FileNames)
                    {
                       listBox1.Items.Add(file);
                    }

                    foreach (var item in listBox1.Items)
                    {
                        fPath = item.ToString();
                        _oldLines = await File.ReadAllLinesAsync(fPath);

                        await ClearComment1(fPath);
                        //await ClearComment2(fPath);
                        //await ClearComment3(fPath);
                        await AddHeader(fPath);
                    }
                    //foreach (var item in listBox1.Items)
                    //{
                    //    fPath = item.ToString();
                    //    await ClearComment2(fPath);
                    //    //await AddHeader(fPath);
                    //}
                    MessageBox.Show("Done rồi nha!");
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private async Task ClearComment1(string fPath)
        {
            try
            {
                List<String> newLines = new List<String>();
                var lineComments = @"\/\/[^\s\/]";
                foreach (String line in _oldLines)
                {
                    if (line.StartsWith('/'))
                    {
                        // do nothing
                    } 
                    else if(line.Contains("//    "))
                    {
                        // do nothing
                    }
                    else if(Regex.IsMatch(line, lineComments) && !line.Contains("///"))
                    {
                        // do nothing
                    }
                    else if (line.Contains("ShopWorksNeo"))
                    {
                        // do nothing
                    }
                    else
                    {
                        newLines.Add(line);
                    }
                }
                await File.WriteAllLinesAsync(fPath, newLines);

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi định mệnh!");
                throw;
            }
            

        }

        private async Task AddHeader(string fpath)
        {
            string content = await File.ReadAllTextAsync("header.txt");
            string oldContent = await File.ReadAllTextAsync(fpath);
            content = content + "\n" + oldContent;

            await File.WriteAllTextAsync(fpath, content);
        }
    }
}
