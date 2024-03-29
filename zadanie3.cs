using System;
using System.IO;
using System.Text;

public class Program
{
    public static void Main()
    {
        string sciezkaDoPliku = "tekst.txt";

        if (!File.Exists(sciezkaDoPliku))
        {
            Console.WriteLine("Plik nie istnieje.");
            return;
        }

        using (FileStream fs = new FileStream(sciezkaDoPliku, FileMode.Open, FileAccess.Read))
        {
            byte[] bufor = new byte[1024];
            StringBuilder sb = new StringBuilder();

            int odczytaneBajty;
            while ((odczytaneBajty = fs.Read(bufor, 0, bufor.Length)) > 0)
            {
                sb.Append(Encoding.UTF8.GetString(bufor, 0, odczytaneBajty));
            }

            Console.WriteLine("Zawartość pliku:");
            Console.WriteLine(sb.ToString());
        }
    }
}
