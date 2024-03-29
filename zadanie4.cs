using System;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = "tekst.txt";

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    char[] charArray = line.ToCharArray();
                    Array.Reverse(charArray);
                    Console.WriteLine(new string(charArray));
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Plik '{filePath}' nie został znaleziony.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }
}
