using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public static class FileReader
{
    private static readonly List<(string Second, string Sixth, string Seventh)> _coord = new();

    public static IReadOnlyList<(string Second, string Sixth, string Seventh)> Coord => _coord;

    public static void LoadSchoolCoordinates(string filePath = "school.csv")
    {
        _coord.Clear();

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] fields = line.Split(',');
                if (fields.Length < 7)
                {
                    Console.WriteLine($"Skipping invalid row {i + 1}: expected at least 7 fields.");
                    continue;
                }

                _coord.Add((fields[1], fields[5], fields[6]));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    public static void PrintFileContents()
    {
        const string path = @"c:\temp\data.txt";
        try
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"File not found: {path}");
                return;
            }

            string content = File.ReadAllText(path);
            Console.WriteLine(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    public static void ConvertParksJsonToCsv(string jsonPath = "TCMSV_alldesc.json", string? outPath = null)
    {
        try
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"JSON file not found: {jsonPath}");
                return;
            }

            string json = File.ReadAllText(jsonPath);
            using JsonDocument doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("data", out JsonElement data) ||
                !data.TryGetProperty("park", out JsonElement parks) ||
                parks.ValueKind != JsonValueKind.Array)
            {
                Console.WriteLine("Unexpected JSON structure: missing data.park array.");
                return;
            }

            var lines = new List<string>();
            // Header
            lines.Add("id,area,name,type,address,tel,totalcar,totalmotor,totalbike,entrance_x,entrance_y");

            foreach (var p in parks.EnumerateArray())
            {
                string id = p.TryGetProperty("id", out var v) ? v.GetString() ?? string.Empty : string.Empty;
                string area = p.TryGetProperty("area", out v) ? v.GetString() ?? string.Empty : string.Empty;
                string name = p.TryGetProperty("name", out v) ? v.GetString() ?? string.Empty : string.Empty;
                string type = p.TryGetProperty("type", out v) ? v.GetString() ?? string.Empty : string.Empty;
                string address = p.TryGetProperty("address", out v) ? v.GetString() ?? string.Empty : string.Empty;
                string tel = p.TryGetProperty("tel", out v) ? v.GetString() ?? string.Empty : string.Empty;
                string totalcar = p.TryGetProperty("totalcar", out v) ? v.GetRawText().Trim('"') : string.Empty;
                string totalmotor = p.TryGetProperty("totalmotor", out v) ? v.GetRawText().Trim('"') : string.Empty;
                string totalbike = p.TryGetProperty("totalbike", out v) ? v.GetRawText().Trim('"') : string.Empty;

                string entrX = string.Empty;
                string entrY = string.Empty;
                if (p.TryGetProperty("EntranceCoord", out var ec) && ec.TryGetProperty("EntrancecoordInfo", out var ecInfo) && ecInfo.ValueKind == JsonValueKind.Array)
                {
                    var first = ecInfo.EnumerateArray().FirstOrDefault();
                    if (first.ValueKind != JsonValueKind.Undefined)
                    {
                        if (first.TryGetProperty("Xcod", out var x)) entrX = x.GetString() ?? string.Empty;
                        if (first.TryGetProperty("Ycod", out var y)) entrY = y.GetString() ?? string.Empty;
                    }
                }

                string Escape(string s)
                {
                    if (s is null) return string.Empty;
                    return '"' + s.Replace("\"", "\"\"") + '"';
                }

                var csv = string.Join(",",
                    Escape(id), Escape(area), Escape(name), Escape(type), Escape(address), Escape(tel),
                    Escape(totalcar), Escape(totalmotor), Escape(totalbike), Escape(entrX), Escape(entrY));
                lines.Add(csv);
            }

            if (string.IsNullOrEmpty(outPath))
            {
                outPath = Path.Combine(AppContext.BaseDirectory, "parks.csv");
            }

            File.WriteAllLines(outPath, lines, Encoding.UTF8);
            Console.WriteLine($"Wrote {lines.Count - 1} park records to {outPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting JSON to CSV: {ex.Message}");
        }
    }
}
