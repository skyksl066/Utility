# Utility.Dapper

## ²��

`Utility.Dapper` ���� Dapper ���۰��������\��A����ƪ����W�ٻP .NET �ҫ��ݩʦW�٤��@�P�ɡA�̾� `[Column]` �ݩʦ۰ʧ����M�g�A²�Ƹ�Ʀs���h�}�o�C

---

## �p��ޤJ

1. ��M�פ��[�J `Utility` �M��C
2. �b�{���X���ޥΩR�W�Ŷ��G
    ```csharp
    using Utility.Dapper;
    ```

---

## �p��ϥ�

### �۰ʵ��U [Column] �ݩʹ���

�ϥ� `DapperColumnMapper.Register` ��k�A�N��� `[Column]` �ݩʪ��ҫ����O�۰ʵ��U�� Dapper�C

#### ���U�Ҧ��� [Column] �ݩʪ����O
```csharp
// Program.cs
DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: true);
```

#### �ȵ��U�� [DapperMapper] �аO�����O
```csharp
// Program.cs
DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: false);
```

#### �d�Ҽҫ�
```csharp
// Model.cs
[DapperMapper]
public class Model()
{
    [Column("test_id")]
    public int Id { get; set; }
    [Column("test_name")]
    public string? Name { get; set; }
}
```

---

## �ĪG

- �d�ߵ��G����ƪ����W�ٷ|�۰ʹ�����ҫ��ݩʤW `[Column(Name)]` ���ȡC
- �Y�L [Column] �ݩʡA�h�h�^�w�]���ݩʦW�ٹ����]�P Dapper �w�]�欰�ۦP�^�C

---

## �P `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true` �t��

- `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true` �ȱN���u�R�W�]�p `user_id`�^�۰ʹ�����m�p���ݩʡ]�p `UserId`�^�A�L�k�B�z�W�٧������@�P�����p�C
- `Utility.Dapper` �� `ColumnAttributeTypeMapper` �i�̾� `[Column("xxx")]` ��T������ƪ����P�ݩʦW�١A�A�Ω����W�ٻP�ݩʦW�ٮt�����j�λݺ�T�����ɡC
- �Y�P�ɱҥΨ�̡A���H `ColumnAttributeTypeMapper` �������޿謰�D�C

---

## �`�N�ƶ�

- �нT�O�ҫ����O�P��ƪ����������T�A�קK�]�W�٤��@�P�ɭP�d�ߵ��G�� null�C
- �Y���h�Ӽҫ��ݵ��U�A��ĳ�ǤJ�]�t�Ҧ��ҫ��� Assembly�C
- �Y�M�צP�ɦ��h�ع����ݨD�A�i�̱��ҿ�� `allModel` �ѼơC

---

## �����s��

- [Dapper �x����](https://github.com/DapperLib/Dapper)
- [System.ComponentModel.DataAnnotations.Schema.ColumnAttribute](https://learn.microsoft.com/zh-tw/dotnet/api/system.componentmodel.dataannotations.schema.columnattribute)
