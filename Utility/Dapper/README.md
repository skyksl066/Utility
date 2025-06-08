# Utility.Dapper

## 簡介

`Utility.Dapper` 提供 Dapper 的自動欄位對應功能，讓資料表欄位名稱與 .NET 模型屬性名稱不一致時，依據 `[Column]` 屬性自動完成映射，簡化資料存取層開發。

---

## 如何引入

1. 於專案中加入 `Utility` 套件。
2. 在程式碼中引用命名空間：
    ```csharp
    using Utility.Dapper;
    ```

---

## 如何使用

### 自動註冊 [Column] 屬性對應

使用 `DapperColumnMapper.Register` 方法，將具備 `[Column]` 屬性的模型類別自動註冊至 Dapper。

#### 註冊所有有 [Column] 屬性的類別
```csharp
// Program.cs
DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: true);
```

#### 僅註冊有 [DapperMapper] 標記的類別
```csharp
// Program.cs
DapperColumnMapper.Register(Assembly.GetExecutingAssembly(), allModel: false);
```

#### 範例模型
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

## 效果

- 查詢結果的資料表欄位名稱會自動對應到模型屬性上 `[Column(Name)]` 的值。
- 若無 [Column] 屬性，則退回預設的屬性名稱對應（與 Dapper 預設行為相同）。

---

## 與 `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true` 差異

- `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true` 僅將底線命名（如 `user_id`）自動對應到駝峰式屬性（如 `UserId`），無法處理名稱完全不一致的情況。
- `Utility.Dapper` 的 `ColumnAttributeTypeMapper` 可依據 `[Column("xxx")]` 精確對應資料表欄位與屬性名稱，適用於欄位名稱與屬性名稱差異較大或需精確對應時。
- 若同時啟用兩者，仍以 `ColumnAttributeTypeMapper` 的對應邏輯為主。

---

## 注意事項

- 請確保模型類別與資料表欄位對應正確，避免因名稱不一致導致查詢結果為 null。
- 若有多個模型需註冊，建議傳入包含所有模型的 Assembly。
- 若專案同時有多種對應需求，可依情境選擇 `allModel` 參數。

---

## 相關連結

- [Dapper 官方文件](https://github.com/DapperLib/Dapper)
- [System.ComponentModel.DataAnnotations.Schema.ColumnAttribute](https://learn.microsoft.com/zh-tw/dotnet/api/system.componentmodel.dataannotations.schema.columnattribute)
