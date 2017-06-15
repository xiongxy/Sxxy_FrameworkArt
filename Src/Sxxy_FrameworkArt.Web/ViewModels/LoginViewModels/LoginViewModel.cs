using System.Data.Entity;
using System.Linq;
using Sxxy_FrameworkArt.Common;
using Sxxy_FrameworkArt.Common.SupportClasses;
using Sxxy_FrameworkArt.Models.SystemEntity;

namespace Sxxy_FrameworkArt.Web.ViewModels.LoginViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string LoginName = "用户名：";
        public string LoginPassword = "密　码：";
        public string ReturnPassword = "忘记密码？";
        public string LoginTitle = "用户登录";
        public string LoginBtn = "登录";
        public string Code { get; set; }
        public string Password { get; set; }
        //重定向
        public string Redirect { get; set; }
        /// <summary>
        /// 进行登录
        /// </summary>
        /// <param name="OutsidePris">外部传递的页面权限</param>
        /// <returns>登录用户的信息</returns>
        public LoginUserInfo DoLogin(bool IgnorePris = false)
        {
            //根据用户名和密码查询用户
            var user = Dc.Set<SystemUser>()
                .Where(x => x.Code.ToLower() == Code.ToLower() && x.Password.ToLower() == Password.ToLower() && x.IsValid == true)
                .Include(x => x.Roles)
                .SingleOrDefault();
            //如果没有找到则输出错误
            if (user == null)
            {
                ModelStateDictionarys.AddModelError("", "登录失败");
                return null;
            }

            var roleIDs = user.Roles.Select(x => x.Id).ToList();
            //查找登录用户的数据权限
            //var dpris = Dc.Set<DataPrivilege>()
            //    .Where(x => x.UserId == user.Id && x.DomainId == null)
            //    .ToList();
            //生成并返回登录用户信息
            LoginUserInfo rv = new LoginUserInfo();
            rv.Id = user.Id;
            rv.Code = user.Code;
            rv.Name = user.Name;
            rv.Roles = user.Roles;
            //rv.DataPrivileges = dpris;
            if (IgnorePris == false)
            {
                //查找登录用户的页面权限
                //var pris = Dc.Set<FunctionPrivilege>()
                //    .Where(x => x.UserId == user.Id || (x.RoleId != null && roleIDs.Contains(x.RoleId)))
                //    .ToList();
                //rv.FunctionPrivileges = pris;
            }
            return rv;
        }

        public string LoginUserValidate(string code, string pwd)
        {
            string msg = "";
            SystemUser user = Dc.Set<SystemUser>().FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            if (user == null)//用户名错误
            {
                msg = "用户名或密码错误";
            }
            else
            {
                if (user.IsValid == false)
                {
                    msg = "您的用户已被禁用";
                }
                else
                {
                    if (user.Password != pwd)//密码错误
                    {
                        msg = "用户名或密码错误";
                    }
                }
            }
            return msg;
        }
    }
}