using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Utility.Dapper.Tests
{
    [DapperMapper]
    public class TestModel
    {
        [Column("col_id")]
        public int Id { get; set; }
        [Column("col_name")]
        public string? Name { get; set; }
    }

    public class TestModel1
    {
        [Column("col_id")]
        public int Id { get; set; }
        [Column("col_name")]
        public string? Name { get; set; }
    }

    [TestClass()]
    public class DapperColumnMapperTests
    {
        [TestMethod()]
        public void RegisterTest_AllModelFalse()
        {
            // Arrange
            SqlMapper.PurgeQueryCache();
            DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: false);

            // Assert
            var typeMap1 = SqlMapper.GetTypeMap(typeof(TestModel));
            Assert.IsNotNull(typeMap1);
            Assert.IsInstanceOfType(typeMap1, typeof(ColumnAttributeTypeMapper<TestModel>));

            var typeMap2 = SqlMapper.GetTypeMap(typeof(TestModel1));
            Assert.IsNotNull(typeMap2);
            Assert.IsInstanceOfType(typeMap2, typeof(DefaultTypeMap));
        }

        [TestMethod()]
        public void RegisterTest_AllModelTrue()
        {
            // Arrange
            SqlMapper.PurgeQueryCache();
            DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: true);

            // Assert
            var typeMap1 = SqlMapper.GetTypeMap(typeof(TestModel));
            Assert.IsNotNull(typeMap1);
            Assert.IsInstanceOfType(typeMap1, typeof(ColumnAttributeTypeMapper<TestModel>));

            var typeMap2 = SqlMapper.GetTypeMap(typeof(TestModel1));
            Assert.IsNotNull(typeMap2); // 不論有無 DapperMapper 都應註冊
            Assert.IsInstanceOfType(typeMap2, typeof(ColumnAttributeTypeMapper<TestModel1>));
        }
    }
}