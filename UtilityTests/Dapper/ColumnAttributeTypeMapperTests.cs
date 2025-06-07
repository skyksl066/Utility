using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Data.SQLite;

namespace Utility.Dapper.Tests
{
    [DapperMapper]
    public class Model()
    {
        [Column("test_id")]
        public int Id { get; set; }
        [Column("test_name")]
        public string? Name { get; set; }
    }

    [TestClass()]
    public class ColumnAttributeTypeMapperTests
    {
        [TestMethod()]
        public void ColumnAttributeTypeMapperTest()
        {
            // Arrange
            DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: false);
            // 使用 in-memory SQLite
            using var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();

            // 建立資料表
            var createTableSql = @"
                CREATE TABLE test_table (
                    test_id INTEGER PRIMARY KEY,
                    test_name TEXT
                );
            ";
            connection.Execute(createTableSql);

            // 插入測試資料
            var insertSql = "INSERT INTO test_table (test_id, test_name) VALUES (@Id, @Name);";
            var testData = new Model { Id = 1, Name = "TestName" };
            connection.Execute(insertSql, testData);

            // Act
            var result = connection.QuerySingleOrDefault<Model>("SELECT * FROM test_table WHERE test_id = @Id", new { Id = 1 });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Name, result.Name);
        }
    }
}