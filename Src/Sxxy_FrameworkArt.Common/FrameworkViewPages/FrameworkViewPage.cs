using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages
{
    public abstract class FrameworkViewPage : WebViewPage
    {
        public BootstrapHtmlHelper Bootstrap { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Bootstrap = new BootstrapHtmlHelper { InnerHelper = Html };
        }
    }

    public abstract class FrameworkViewPage<TModel> : WebViewPage<TModel>
    {
        public BootstrapHtmlHelper<TModel> Bootstrap { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Bootstrap = new BootstrapHtmlHelper<TModel> { InnerHelper = Html };
        }
    }

    /// <summary>
    /// 自定义HtmlHelper
    /// </summary>
    public class BootstrapHtmlHelper
    {
        public HtmlHelper<object> InnerHelper { get; set; }
    }
    /// <summary>
    /// 自定义HtmlHelper的泛型模式
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class BootstrapHtmlHelper<TModel> : BootstrapHtmlHelper
    {
        public new HtmlHelper<TModel> InnerHelper { get; set; }
    }
}

