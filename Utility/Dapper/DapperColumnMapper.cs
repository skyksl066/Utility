#nullable disable
using Dapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Utility.Dapper
{
    public class DapperColumnMapper
    {
        /// <summary>
        /// 自動註冊所有有 [Column] 的 model 至 Dapper，並可選擇排除類型
        /// </summary>
        /// <param name="assembly">要掃描的組件</param>
        /// <param name="excludedTypes">要排除的型別（可選）</param>
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
