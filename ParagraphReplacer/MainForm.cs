using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ParagraphReplacer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFolder.Text = dlg.SelectedPath;
                }
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ParagraphReplacerOptions options = new ParagraphReplacerOptions
            {
                Directory = txtFolder.Text,
                FileFilter = txtFilter.Text,
                FindText = txtFindText.Text,
                ReplaceText = txtReplaceText.Text,
                CaseSensitive = chkCaseSensitive.Checked
            };
            Action<ParagraphReplacerOptions> action = new Action<ParagraphReplacerOptions>(Replace);
            action.BeginInvoke(options, new AsyncCallback(iar => action.EndInvoke(iar)), null);
        }

        private void Replace(ParagraphReplacerOptions options)
        {
            if (!Directory.Exists(options.Directory))
                throw new DirectoryNotFoundException("Invalid directory.");

            var files = Directory.GetFiles(options.Directory, options.FileFilter, SearchOption.AllDirectories);
            files.ToList().ForEach(x => ReplaceText(x, options));
        }

        private void ReplaceText(string file, ParagraphReplacerOptions options)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("Invalid file.", file);

            string text = File.ReadAllText(file);
            StringComparison comparison = options.CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
            int index = -1;
            while ((index = text.IndexOf(options.FindText, comparison)) != -1)
            {
                text = text.Remove(index, options.FindText.Length).Insert(index, options.ReplaceText);
            }
            File.WriteAllText(file, text);
        }
    }
}
