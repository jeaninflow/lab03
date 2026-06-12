# Copilot 使用說明

此專案為單一 .NET 10 Console 應用程式。

## 核心資訊
- 主要專案檔案：`MyApp.csproj`
- 主要程式碼：`Program.cs`
- 目標框架：`net10.0`
- 專案類型：`.NET SDK` style console app
- 已啟用隱含 using 與可為空參考檢查

## 建置與執行
- 建置：`dotnet build`
- 執行：`dotnet run`

## 開發注意事項
- 不要手動編輯 `bin/` 或 `obj/` 內的產生檔案。
- 目前專案沒有測試專案；如需新增測試，請建立獨立的測試專案並加入解決方案。
- 若要擴充專案，請在新增程式碼時保持資料夾結構簡單明確。

## 供 Copilot 參考
- 這是一個非常小型、單一專案，可直接在 `Program.cs` 新增功能。
- 若要修改專案設定，請先編輯 `MyApp.csproj`。
- 如果需要更多專案上下文，請先檢查 `.csproj` 的 TargetFramework 與架構設定。