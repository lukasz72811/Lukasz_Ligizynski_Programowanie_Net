using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Wybierz opcję:");
        Console.WriteLine("1. Zapisz dane do pliku binarnego");
        Console.WriteLine("2. Odczytaj dane z pliku binarnego");
        Console.Write("Twój wybór: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ZapiszDane();
                break;
            case "2":
                OdczytajDane();
                break;
            default:
                Console.WriteLine("Niepoprawny wybór.");
                break;
        }
    }

    static void ZapiszDane()
    {
        try
        {
            Console.WriteLine("Podaj imię:");
            string imie = Console.ReadLine();

            Console.WriteLine("Podaj wiek:");
            int wiek = int.Parse(Console.ReadLine());

            Console.WriteLine("Podaj adres:");
            string adres = Console.ReadLine();

            using (FileStream fs = new FileStream("dane.bin", FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(imie);
                writer.Write(wiek);
                writer.Write(adres);
            }

            Console.WriteLine("Dane zostały zapisane do pliku.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd podczas zapisywania danych: {ex.Message}");
        }
    }

    static void OdczytajDane()
    {
        try
        {
            using (FileStream fs = new FileStream("dane.bin", FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                string imie = reader.ReadString();
                int wiek = reader.ReadInt32();
                string adres = reader.ReadString();

                Console.WriteLine($"Imię: {imie}, Wiek: {wiek}, Adres: {adres}");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Plik z danymi nie istnieje.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd podczas odczytywania danych: {ex.Message}");
        }
    }
}
