using System;

public class Samochod
{
    public string marka { get; set; }
    public string model { get; set; }
    public int iloscDrzwi { get; set; }
    public double pojemnoscSilnika { get; set; }
    public double srednieSpalanie { get; set; }
    public static int iloscSamochodow { get; private set; }

    public Samochod()
    {
        marka = "nieznana";
        model = "nieznany";
        iloscDrzwi = 0;
        pojemnoscSilnika = 0.0;
        srednieSpalanie = 0.0;
        iloscSamochodow++;
    }

    public Samochod(string marka_, string model_, int iloscDrzwi_, double pojemnoscSilnika_, double srednieSpalanie_)
    {
        marka = marka_;
        model = model_;
        iloscDrzwi = iloscDrzwi_;
        pojemnoscSilnika = pojemnoscSilnika_;
        srednieSpalanie = srednieSpalanie_;
        iloscSamochodow++;
    }

    public double ObliczSpalanie(double dlugoscTrasy)
    {
        return (srednieSpalanie * dlugoscTrasy) / 100.0;
    }

    public double ObliczKosztPrzejazdu(double dlugoscTrasy, double cenaPaliwa)
    {
        double spalanie = ObliczSpalanie(dlugoscTrasy);
        return spalanie * cenaPaliwa;
    }

    public void WypiszInfo()
    {
        Console.WriteLine($"Marka: {marka}");
        Console.WriteLine($"Model: {model}");
        Console.WriteLine($"Ilość drzwi: {iloscDrzwi}");
        Console.WriteLine($"Pojemność silnika: {pojemnoscSilnika}");
        Console.WriteLine($"Średnie spalanie: {srednieSpalanie}");
    }

    public static void WypiszIloscSamochodow()
    {
        Console.WriteLine($"Liczba samochodów: {iloscSamochodow}");
    }
}

public class Garaz
{
    private string adres { get; set; }
    private int pojemnosc { get; set; }
    private int liczbaSamochodow { get; set; }
    private Samochod[] samochody;

    public Garaz(string adres_, int pojemnosc_)
    {
        adres = adres_;
        pojemnosc = pojemnosc_;
        liczbaSamochodow = 0;
        samochody = new Samochod[pojemnosc];
    }

    public void WprowadzSamochod(Samochod samochod)
    {
        if (liczbaSamochodow < pojemnosc)
        {
            samochody[liczbaSamochodow] = samochod;
            liczbaSamochodow++;
        }
        else
        {
            Console.WriteLine("Garaz jest pelny!");
        }
    }

    public Samochod WyprowadzSamochod()
    {
        if (liczbaSamochodow > 0)
        {
            Samochod ostatniSamochod = samochody[liczbaSamochodow - 1];
            samochody[liczbaSamochodow - 1] = null;
            liczbaSamochodow--;
            return ostatniSamochod;
        }
        else
        {
            Console.WriteLine("Garaz jest pusty!");
            return null;
        }
    }

    public void WypiszInfo()
    {
        Console.WriteLine($"Adres: {adres}");
        Console.WriteLine($"Pojemność: {pojemnosc}");
        Console.WriteLine($"Liczba garażowanych samochodów: {liczbaSamochodow}");
        Console.WriteLine("Informacje o garażowanych samochodach:");
        for (int i = 0; i < liczbaSamochodow; i++)
        {
            Console.WriteLine($"--- Samochod {i + 1} ---");
            samochody[i].WypiszInfo();
        }
    }
}

public class Program
{
    public static void Main()
    {
        Samochod s1 = new Samochod();
        s1.WypiszInfo();

        Console.WriteLine();

        Samochod s2 = new Samochod("Fiat", "126p", 2, 650, 6.0);
        s2.WypiszInfo();

        Console.WriteLine();

        Samochod s3 = new Samochod("Syrena", "105", 2, 800, 7.6);
        s3.WypiszInfo();

        Console.WriteLine();

        Garaz g = new Garaz("ul. Testowa 123", 3);
        g.WprowadzSamochod(s1);
        g.WprowadzSamochod(s2);
        g.WprowadzSamochod(s3);

        Console.WriteLine();

        g.WypiszInfo();

        Console.WriteLine();

        Samochod.WypiszIloscSamochodow();
    }
}
