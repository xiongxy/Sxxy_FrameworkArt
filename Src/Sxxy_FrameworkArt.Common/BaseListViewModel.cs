using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common
{
    public interface IBaseListViewModel<out TModel, out TSearch>
    {
    }
    public class BaseListViewModel<TModel, TSearch> : BaseViewModel, IBaseListViewModel<TModel, TSearch>
    {
    }
}
