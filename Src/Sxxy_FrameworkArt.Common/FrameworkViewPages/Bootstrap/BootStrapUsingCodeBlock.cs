using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap
{
    public class BootStrapSearcherPanel : IDisposable
    {
        private readonly ViewContext _viewContext;
        public BootStrapSearcherPanel(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
        public void Dispose()
        {
            _viewContext.Writer.WriteLine("</form></div></div></div>");
        }
    }
    public class BootStrapRow : IDisposable
    {
        private readonly ViewContext _viewContext;
        public BootStrapRow(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
        public void Dispose()
        {
            _viewContext.Writer.WriteLine("</div>");
        }
    }
    public class BootStrapColumn : IDisposable
    {
        private readonly ViewContext _viewContext;
        public BootStrapColumn(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
        public void Dispose()
        {
            _viewContext.Writer.WriteLine("</div>");
        }
    }
    public class BootStrapBox : IDisposable
    {
        private readonly ViewContext _viewContext;
        public BootStrapBox(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
        public void Dispose()
        {
            _viewContext.Writer.WriteLine("</div>");
        }
    }




}
