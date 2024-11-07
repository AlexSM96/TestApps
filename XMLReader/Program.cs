using XMLReader.Readers;
using XMLReader.Readers.XmlReaderExrensions;

while (true)
{
    Console.WriteLine("Введите путь до файла: ");
    string? filePath = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(filePath)) continue;
    if(!File.Exists(filePath))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Файл не найден");
        Console.ResetColor();
        continue;
    }

    var xmlFileReader = new XmlFileReader(filePath);
    var xmlElements = xmlFileReader.GetAllElements()?.ToList();
    if(xmlElements is null || !xmlElements.Any())
    {
        Console.WriteLine("Элементы не найдены");
        continue;
    }

    Console.WriteLine($"Кол-во элементов: " + xmlElements.Count());
    var sum = xmlElements.GetIntSum("id");
    Console.WriteLine("Сумма: " + sum);
    Console.WriteLine("Список элементов: ");
    foreach (var element in xmlElements)
    {
        Console.WriteLine($"Элемент: {element?.Name} Значение: {element?.Value}");
    }

    Console.ReadKey();
}
