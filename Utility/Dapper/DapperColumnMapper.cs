#nullable disable
using Dapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Utility.Dapper
{
    /// <summary>
    /// 提供將具備 [Column] 屬性或 [DapperMapper] 標記的模型類別自動註冊至 Dapper 的功能，
    /// 使 Dapper 能正確對應資料庫欄位與模型屬性，簡化欄位映射設定流程。
    /// </summary>
    public class DapperColumnMapper
    {
        /// <summary>
        /// 自動註冊所有有 [Column] 的 model 至 Dapper，並可選擇排除類型
        /// </summary>
        /// <param name="assembly">要掃描的組件</param>
        /// <param name="allModel">
        /// 是否自動註冊所有包含 [Column] 屬性的類別：
        /// - true：註冊所有有 [Column] 屬性的類別。
        /// - false：僅註冊有 [DapperMapper] 標記的類別。
        /// </param>
        public static void Register(Assembly assembly, bool allModel = true)
        {
            var columnAttrType = typeof(ColumnAttribute);
            var dapperMapperAttrType = typeof(DapperMapperAttribute);

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t =>
                    allModel
                        ? t.GetProperties().Any(p => p.IsDefined(columnAttrType, inherit: false))
                        : t.IsDefined(dapperMapperAttrType, inherit: false)
                )
                .ToList();

            foreach (var type in types)
            {
                var mapperType = typeof(ColumnAttributeTypeMapper<>).MakeGenericType(type);
                var mapperInstance = (SqlMapper.ITypeMap)Activator.CreateInstance(mapperType);
                SqlMapper.SetTypeMap(type, mapperInstance);
            }
        }
    }
}
