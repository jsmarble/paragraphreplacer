using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParagraphReplacer
{
    public class ParagraphReplacerOptions
    {
        public string Directory { get; set; }
        public string FileFilter { get; set; }
        public string FindText { get; set; }
        public string ReplaceText { get; set; }
        public bool CaseSensitive { get; set; }
    }
}
