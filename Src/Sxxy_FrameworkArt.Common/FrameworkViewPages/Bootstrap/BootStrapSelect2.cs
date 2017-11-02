using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap
{
    public class BootStrapSelect2
    {
        public List<SelectListItem> AllItems { get; set; }
        public string SelectedItem { get; set; }
        public string JsonDataBySelectItem { get; set; }
        public string JsonDataBySimpleSelectItem { get; set; }
        public string JsonDataBySelect2 { get; set; }
    }

    public class BootStrapSelect2Item
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
