
# 📚 EsunLibrarySystem - 線上圖書借閱系統 (.NET 6)

此專案為「玉山銀行後端工程師實作題 B」的完整實作，使用 C# / ASP.NET Core 6 MVC 搭配 MySQL 8.4 LTS，實現一套具備註冊、登入、借書、還書等完整流程的線上圖書借閱系統。

---

## 📌 系統功能

- 📱 使用者註冊（手機號碼唯一、密碼加鹽雜湊）
- 🔐 Cookie-based 登入與身份驗證（支援 [Authorize]）
- 📘 借書功能（僅能借出可用庫存，使用交易 Transaction）
- 📗 還書功能（更新狀態與還書時間）
- ✅ 使用 Razor + Bootstrap 製作簡潔響應式 UI
- ⚙️ 使用 Stored Procedure 存取資料庫，防止 SQL Injection

---

## 🗂️ 專案結構

```plaintext
EsunLibrarySystem/
├── Controllers/
├── Models/
├── Services/
├── Repositories/
├── Views/
│   ├── Account/
│   └── Book/
├── DB/
│   ├── ddl.sql
│   └── dml.sql
├── Program.cs
└── appsettings.json
```

---

## ⚙️ 技術與架構

- **後端語言**：C# (.NET 6.0)
- **前端技術**：Razor Pages + Bootstrap 5
- **資料庫**：MySQL 8.4 (LTS)
- **架構模式**：MVC 三層式架構
- **認證機制**：ASP.NET Core Cookie-based Authentication

---

## 🧪 建置與執行方式

1. 安裝 MySQL 8.4 並建立資料庫
2. 匯入 `DB/ddl.sql` 建立資料表，匯入 `DB/dml.sql` 建立測試資料 ( 建議使用 MySQL Workbench 8.0 CE )
3. 設定 `appsettings.json` 的資料庫連線字串如下（請自行修改密碼）：

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LibraryDB;User Id=root;Password=yourpassword;"
}
```

⚠️ 請將 `"yourpassword"` 替換為你實際使用的密碼

4. 執行專案並進入 `https://localhost:xxxx/`

---

## 🔒 認證與安全性設計

- 採用 Cookie 認證 + `[Authorize]` 屬性控管
- 全部使用參數化查詢防止 SQL Injection
- Razor 自動 HTML Encoding 防範 XSS
- 借書與還書皆包 Transaction 確保一致性

---

## 📝 Stored Procedures 一覽

- `sp_RegisterUser`
- `sp_GetUserAuthInfo`
- `sp_GetAvailableBooks`
- `sp_BorrowBook`
- `sp_GetBorrowedBooksByUser`
- `sp_ReturnBook`
- `sp_UpdateLastLoginTime`

---

## 🙋 作者聲明

此專案為個人原創開發，僅用於面試與實作作業展示用途，請勿擅自轉貼或拷貝。
