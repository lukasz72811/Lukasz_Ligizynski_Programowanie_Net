using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Program : Form
{
    private Button przyciskWybieraniaPlikow;
    private Button przyciskSzyfrowania;
    private Button przyciskOdszyfrowania;
    private TextBox poleTekstowe;
    private ComboBox OkienkaRazem;
    private OpenFileDialog otworzPlik;
    private AesManaged aes;
    private RSACryptoServiceProvider rsa;
    private byte[] rsaByte;

    public Program()
    {
        this.Text = "Szyfrowanie Plików";
        this.Size = new System.Drawing.Size(800, 600);

        przyciskWybieraniaPlikow = new Button();
        przyciskWybieraniaPlikow.Text = "Wybierz Pliki";
        przyciskWybieraniaPlikow.Location = new System.Drawing.Point(20, 20);
        przyciskWybieraniaPlikow.Click += PrzyciskWybieraniaPlikowKlikniecie;

        przyciskSzyfrowania = new Button();
        przyciskSzyfrowania.Text = "Szyfruj";
        przyciskSzyfrowania.Location = new System.Drawing.Point(20, 60);
        przyciskSzyfrowania.Click += przyciskSzyfrowaniaKlikniecie;

        przyciskOdszyfrowania = new Button();
        przyciskOdszyfrowania.Text = "Odszyfruj";
        przyciskOdszyfrowania.Location = new System.Drawing.Point(20, 100);
        przyciskOdszyfrowania.Click += przyciskOdszyfrowaniaKlikniecie;

        poleTekstowe = new TextBox();
        poleTekstowe.Location = new System.Drawing.Point(20, 140);
        poleTekstowe.Size = new System.Drawing.Size(740, 400);
        poleTekstowe.Multiline = true;
        poleTekstowe.ScrollBars = ScrollBars.Vertical;

     OkienkaRazem = new ComboBox();
     OkienkaRazem.Items.AddRange(new string[] {
            "AES 128 bit", "AES 256 bit"
        });
     OkienkaRazem.SelectedIndex = 0;
     OkienkaRazem.Location = new System.Drawing.Point(20, 180);

        otworzPlik = new OpenFileDialog();
        otworzPlik.Multiselect = true;

        this.Controls.Add(przyciskWybieraniaPlikow);
        this.Controls.Add(przyciskSzyfrowania);
        this.Controls.Add(przyciskOdszyfrowania);
        this.Controls.Add(poleTekstowe);
        this.Controls.Add(OkienkaRazem);

        rsa = new RSACryptoServiceProvider(2048);
        aes = new AesManaged();
    }

    private void PrzyciskWybieraniaPlikowKlikniecie(object sender, EventArgs e)
    {
        if (otworzPlik.ShowDialog() == DialogResult.OK)
        {
        poleTekstowe.AppendText("Wybrane pliki:" + Environment.NewLine);
        foreach (string file in otworzPlik.FileNames)
            {
                poleTekstowe.AppendText(file + Environment.NewLine);
            }
        }
    }

    private async void przyciskSzyfrowaniaKlikniecie(object sender, EventArgs e)
    {
        aes.GenerateKey();
        aes.GenerateIV();
        rsaByte = rsa.Encrypt(aes.Key, true);

        ZapiszUstawienia("config.json", aes);

        foreach (string filePath in otworzPlik.FileNames)
        {
            string encryptedFilePath = filePath + ".enc";
            await ZaszyfrujPlik(filePath, encryptedFilePath, aes);
            poleTekstowe.AppendText($"Plik zaszyfrowany: {encryptedFilePath}" +  Environment.NewLine);
        }
    }

    private async void przyciskOdszyfrowaniaKlikniecie(object sender, EventArgs e)
    {
        aes.Key = rsa.Decrypt(rsaByte, true);
        WczytajUstawienia("config.json", aes);

        foreach (string filePath in otworzPlik.FileNames)
        {
            string encryptedFilePath = filePath + ".enc";
            string decryptedFilePath = filePath + ".dec.txt";
            await OdszyfrujPlik(encryptedFilePath, decryptedFilePath, aes);
            poleTekstowe.AppendText($"Plik odszyfrowany: {decryptedFilePath}" +  Environment.NewLine);
        }
    }

    private async Task ZaszyfrujPlik(string inputFile, string outputFile, AesManaged aes)
    {
        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (CryptoStream cs = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write))
                    {
                        await fsInput.CopyToAsync(cs);
                    }
                }
            }
        }
    }

    private async Task OdszyfrujPlik(string inputFile, string outputFile, AesManaged aes)
    {
        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            using (FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                    {
                        await cs.CopyToAsync(fsDecrypted);
                    }
                }
            }
        }
    }

    private void ZapiszUstawienia(string configFilePath, AesManaged aes)
    {
        var config = new EncryptionConfig
        {
            Key = Convert.ToBase64String(aes.Key),
            IV = Convert.ToBase64String(aes.IV)
        };

        string json = JsonSerializer.Serialize(config);
        File.WriteAllText(configFilePath, json);
    }

    private void WczytajUstawienia(string configFilePath, AesManaged aes)
    {
        string json = File.ReadAllText(configFilePath);
        var config = JsonSerializer.Deserialize<EncryptionConfig>(json);

        aes.Key = Convert.FromBase64String(config.Key);
        aes.IV = Convert.FromBase64String(config.IV);
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Program());
    }
}

public class EncryptionConfig
{
    public string? Key { get; set; }
    public string? IV { get; set; }
}