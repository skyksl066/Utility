#nullable disable
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utility.Dapper
{
    /// <summary>
    /// Source: https://gist.github.com/kalebpederson/5460509
    /// Author: Kaleb Pederson
    /// 提供多個 Dapper 型別對應器（ITypeMap）的備援機制，
    /// 依序嘗試每個對應器來解析建構函式、成員或參數的對應關係。
    /// 當前一個對應器無法對應時，會自動嘗試下一個，直到找到合適的對應或全部失敗為止。
    /// </summary>
    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        /// <summary>
        /// 建立 FallbackTypeMapper 實例，依序使用多個 ITypeMap 進行型別對應。
        /// </summary>
        /// <param name="mappers">要依序嘗試的 Dapper 型別對應器集合。</param>
        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }

        /// <summary>
        /// 依序嘗試所有型別對應器，尋找符合指定參數名稱與型別的建構函式。
        /// </summary>
        /// <param name="names">建構函式參數名稱陣列。</param>
        /// <param name="types">建構函式參數型別陣列。</param>
        /// <returns>找到的建構函式，若無則回傳 null。</returns>
        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// 依序嘗試所有型別對應器，取得指定建構函式參數與資料表欄位的對應成員。
        /// </summary>
        /// <param name="constructor">建構函式資訊。</param>
        /// <param name="columnName">資料表欄位名稱。</param>
        /// <returns>對應的成員資訊，若無則回傳 null。</returns>
        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// 依序嘗試所有型別對應器，取得指定資料表欄位名稱對應的成員。
        /// </summary>
        /// <param name="columnName">資料表欄位名稱。</param>
        /// <returns>對應的成員資訊，若無則回傳 null。</returns>
        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// 依序嘗試所有型別對應器，尋找明確標記為 [ExplicitConstructor] 的建構函式。
        /// </summary>
        /// <returns>找到的明確建構函式，若無則回傳 null。</returns>
        public ConstructorInfo FindExplicitConstructor()
        {
            return _mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);
        }
    }
}
