using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{
    public class ChangePara : ExpressionVisitor
    {
        private ParameterExpression _pe;

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="expression">要修改的表达式</param>
        /// <param name="pe">新的参数</param>
        /// <returns>修改后的表达式</returns>
        public Expression Change(Expression expression, ParameterExpression pe)
        {
            _pe = pe;
            return Visit(expression);
        }

        /// <summary>
        /// 检查所有参数类型的表达式
        /// </summary>
        /// <param name="node">表达式节点</param>
        /// <returns>新的参数类型</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _pe;
        }

        /// <summary>
        /// 检查所有成员访问类型的表达式
        /// </summary>
        /// <param name="node">表达式节点</param>
        /// <returns>修改后的表达式</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression.NodeType == ExpressionType.Parameter)
            {
                var rv = Expression.MakeMemberAccess(_pe, node.Member);
                return rv;
            }
            else
            {
                return base.VisitMember(node);
            }
        }
    }
}
