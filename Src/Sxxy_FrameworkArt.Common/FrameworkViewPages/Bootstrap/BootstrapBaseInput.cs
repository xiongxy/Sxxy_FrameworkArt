using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap
{
    public class BootstrapBaseInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public bool IsRequired { get; set; }
        public string Error { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
