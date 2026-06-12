# daily-task-logger Skill

目的：將今天完成的任務記錄到工作目錄下的 `logs[yyyy-MM-dd].md` 檔案。一天只會有一個檔案，若已存在則追加到同一檔案尾端。

格式範例：

[session name]
### Prompt[n]: [....]
[Ask /Agent/ plan] (Model): [...]
### Prompt[n]: [....]
[Ask /Agent/ plan] (Model): [...]

使用說明：
- 此 skill 搭配 `scripts/daily-task-logger.ps1` 腳本使用。
- 腳本接受一個 JSON 字串（透過 `-DataJson` 參數或標準輸入），格式如下：

{
  "sessionName": "例：上午工作回顧",
  "prompts": [
    {"prompt": "第一個 prompt 文本", "plan": "Agent 的 plan 文字", "model": "使用的模型名稱"},
    {"prompt": "第二個 prompt 文本", "plan": "Agent 的 plan 文字", "model": "使用的模型名稱"}
  ]
}

- 腳本會在執行目錄下的 `logs[yyyy-MM-dd].md` 檔案追加以下內容（若檔案不存在則建立）：

[session name]
### Prompt1: [prompt text]
[Ask /Agent/ plan] (Model): [plan text]

- 範例命令：

PowerShell (直接傳 JSON 字串)：

```powershell
$json = '{"sessionName":"晨會","prompts":[{"prompt":"整理待辦","plan":"Agent: 彙總並標註優先順序","model":"GPT-5 mini"}]}'
.\scripts\daily-task-logger.ps1 -DataJson $json
```

或從檔案讀入：

```powershell
Get-Content .\daily_entry.json -Raw | .\scripts\daily-task-logger.ps1 -
```

注意：檔名會包含中括號（例如 `logs[2026-06-12].md`），這是依照要求的樣式。