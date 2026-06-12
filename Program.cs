// See https://aka.ms/new-console-template for more information

// Convert parks JSON to CSV (will write parks.csv)
FileReader.ConvertParksJsonToCsv();

FileReader.LoadSchoolCoordinates();

foreach (var coord in FileReader.Coord)
{
    Console.WriteLine($"{coord.Second}, {coord.Sixth}, {coord.Seventh}");
}
