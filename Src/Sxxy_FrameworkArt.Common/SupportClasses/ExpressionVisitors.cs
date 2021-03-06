﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sxxy_FrameworkArt.Common.Helpers;

namespace Sxxy_FrameworkArt.Common.SupportClasses
{
    /// <summary>
    /// 分析x==y这种类型的表达式，并储存在Dictionary中
    /// </summary>
    public class SetValuesParser : ExpressionVisitor
    {
        private Dictionary<string, object> _rv = new Dictionary<string, object>();

        /// <summary>
        /// 开始分析表达式
        /// </summary>
        /// <param name="expression">源表达式</param>
        /// <returns>将x==y这种表达式变为Dictionary<x,y>并返回</returns>
        public Dictionary<string, object> Parse(Expression expression)
        {
            Visit(expression);
            return _rv;
        }

        /// <summary>
        /// 处理所有二进制类型的表达式
        /// </summary>
        /// <param name="node">当前表达式节点</param>
        /// <returns>修改后的表达式</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            //如果表达式是x==y的类型，则获取y的值，并使用x最为key，y的值作为value添加到Dictionary中保存
            if (node.NodeType == ExpressionType.Equal)
            {
                var pi = PropertyHelper.GetPropertyName(node.Left);
                if (!_rv.ContainsKey(pi))
                {
                    _rv.Add(pi, Expression.Lambda(node.Right).Compile().DynamicInvoke());
                }
            }
            return base.VisitBinary(node);
        }
    }
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
