using System;

namespace Utility.Dapper
{
    /// <summary>
    /// 標記類別以指定其為 Dapper 映射對象。
    /// 可用於識別需要特殊處理或自訂映射規則的類別。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DapperMapperAttribute : Attribute { }
}
