param(
    [Parameter(Mandatory=$false, Position=0, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]
    [string]$DataJson
)

# 讀取 JSON：若沒有透過參數提供，則從標準輸入讀取
if (-not $DataJson) {
    try {
        $stdin = [Console]::In.ReadToEnd()
        if ($stdin) { $DataJson = $stdin }
    } catch {
        # no stdin
    }
}

if (-not $DataJson) {
    Write-Error "請提供 JSON 字串 (參數 -DataJson) 或將 JSON 透過標準輸入傳入。"
    exit 1
}

# 解析 JSON
try {
    $data = $DataJson | ConvertFrom-Json -ErrorAction Stop
} catch {
    Write-Error "解析 JSON 失敗：$($_.Exception.Message)"
    exit 1
}

$sessionName = $data.sessionName
if (-not $sessionName) { $sessionName = "(無 session name)" }
$prompts = $data.prompts
if (-not $prompts) { $prompts = @() }

# 建立 logs 檔案路徑（檔名包含中括號，格式 logs[yyyy-MM-dd].md）
$date = Get-Date -Format "yyyy-MM-dd"
$logsDir = Join-Path (Get-Location).Path ""
$fileName = "logs[$date].md"
$filePath = Join-Path $logsDir $fileName

# 準備要寫入的內容
$contentLines = @()
$contentLines += "[$sessionName]"

$index = 1
foreach ($p in $prompts) {
    $promptText = $p.prompt -replace "\r?\n"," "
    $planText = $p.plan -replace "\r?\n"," "
    $modelText = $p.model
    $contentLines += "### Prompt$index: $promptText"
    if ($planText -or $modelText) {
        $line = "[Ask /Agent/ plan] (Model): $planText"
        if ($modelText) { $line += " ($modelText)" }
        $contentLines += $line
    }
    $contentLines += ""
    $index++
}

# 追加到檔案（建立 logs 檔案若不存在）
try {
    if (-not (Test-Path $filePath)) {
        $contentLines | Out-File -FilePath $filePath -Encoding UTF8 -Force
    } else {
        $contentLines | Out-File -FilePath $filePath -Encoding UTF8 -Append
    }
    Write-Output "已寫入 $filePath"
} catch {
    Write-Error "寫入檔案失敗：$($_.Exception.Message)"
    exit 1
}