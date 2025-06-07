#nullable disable
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Utility.Dapper
{
    /// <summary>
    /// 使用指定的 <see cref="ColumnAttribute"/> 的 Name 屬性值，來決定查詢結果中欄位名稱與對應成員之間的關聯。
    /// 如果沒有欄位對應，則所有成員會以預設方式進行對應。
    /// </summary>
    /// <typeparam name="T">此對應器所套用的物件型別。</typeparam>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                {
                new CustomPropertyTypeMap(
                    typeof(T),
                    (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                            prop.GetCustomAttributes(false)
                                .OfType<ColumnAttribute>()
                                .Any(attr => attr.Name == columnName)
                            )
                    ),
                new DefaultTypeMap(typeof(T))
                })
        {
        }
    }
}