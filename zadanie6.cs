using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Podaj nazwę pliku źródłowego:");
            string sourceFileName = Console.ReadLine();

            Console.WriteLine("Podaj nazwę pliku docelowego:");
            string targetFileName = Console.ReadLine();

            if (!File.Exists(sourceFileName))
            {
                Console.WriteLine("Plik źródłowy nie istnieje.");
                return;
            }

            using (FileStream sourceStream = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read))
            using (FileStream targetStream = new FileStream(targetFileName, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    targetStream.Write(buffer, 0, bytesRead);
                }
            }

            Console.WriteLine("Plik został skopiowany pomyślnie.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd podczas kopiowania pliku: {ex.Message}");
        }
    }
}