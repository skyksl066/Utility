#nullable disable
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Utility.Dapper
{
    /// <summary>
    /// Source: https://gist.github.com/kalebpederson/5460509
    /// Author: Kaleb Pederson
    /// 使用指定的 <see cref="ColumnAttribute"/> 的 Name 屬性值，來決定查詢結果中欄位名稱與對應成員之間的關聯。
    /// 如果沒有欄位對應，則所有成員會以預設方式進行對應。
    /// </summary>
    /// <typeparam name="T">此對應器所套用的物件型別。</typeparam>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        /// <summary>
        /// 建立一個以 [Column] 屬性 Name 值為主的 Dapper 型別對應器。
        /// 會優先將查詢結果的欄位名稱與屬性上 [Column(Name)] 的值進行對應，
        /// 若找不到對應則退回預設的屬性名稱對應行為。
        /// 適用於資料表欄位名稱與模型屬性名稱不一致時的自動映射。
        /// </summary>
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