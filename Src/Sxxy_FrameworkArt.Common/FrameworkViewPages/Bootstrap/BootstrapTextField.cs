﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Common.FrameworkViewPages.Bootstrap
{
    public class BootstrapTextField
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string LableText { get; set; }
        /// <summary>
        /// Input标签类型
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// 描述, Input 属性placeholder
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// input标签Id标识
        /// </summary>
        public string InputId { get; set; }
        /// <summary>
        /// input标签Name
        /// </summary>
        public string InputName { get; set; }
        /// <summary>
        /// input标签 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// input标签Value值
        /// </summary>
        public string Value { get; set; }
    }
}
