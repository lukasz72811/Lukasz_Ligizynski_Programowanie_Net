using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public class Program : Form
{
    private ComboBox comboBoxAlgorytm;
    private Button buttonGenerujKlucze;
    private Button buttonSzyfruj;
    private Button buttonOdszyfruj;
    private TextBox textBoxJawny;
    private TextBox textBoxZaszyfrowany;
    private TextBox textBoxKlucz;
    private TextBox textBoxIV;
    private Label labelCzasSzyfrowania;
    private Label labelCzasDeszyfrowania;
    private DataGridView dataGridView;

    private SymmetricAlgorithm algorytm;

    public Program()
    {
        this.Text = "Szyfrowanie";
        this.Size = new System.Drawing.Size(800, 600);

        comboBoxAlgorytm = new ComboBox();
        comboBoxAlgorytm.Items.AddRange(new string[] {
            "AES (CSP) 128 bit", "AES (CSP) 256 bit", 
            "AES Managed 128 bit", "AES Managed 256 bit", 
            "Rijndael Managed 128 bit", "Rijndael Managed 256 bit", 
            "DES 56 bit", "3DES 168 bit"
        });
        comboBoxAlgorytm.SelectedIndex = 0;
        comboBoxAlgorytm.Location = new System.Drawing.Point(20, 20);

        buttonGenerujKlucze = new Button();
        buttonGenerujKlucze.Text = "Generuj Klucze";
        buttonGenerujKlucze.Location = new System.Drawing.Point(20, 60);
        buttonGenerujKlucze.Click += ButtonGenerujKlucze_Click;

        buttonSzyfruj = new Button();
        buttonSzyfruj.Text = "Szyfruj";
        buttonSzyfruj.Location = new System.Drawing.Point(20, 100);
        buttonSzyfruj.Click += ButtonSzyfruj_Click;

        buttonOdszyfruj = new Button();
        buttonOdszyfruj.Text = "Odszyfruj";
        buttonOdszyfruj.Location = new System.Drawing.Point(20, 140);
        buttonOdszyfruj.Click += ButtonOdszyfruj_Click;

        textBoxJawny = new TextBox();
        textBoxJawny.Location = new System.Drawing.Point(150, 20);
        textBoxJawny.Size = new System.Drawing.Size(600, 20);

        textBoxZaszyfrowany = new TextBox();
        textBoxZaszyfrowany.Location = new System.Drawing.Point(150, 60);
        textBoxZaszyfrowany.Size = new System.Drawing.Size(600, 20);

        textBoxKlucz = new TextBox();
        textBoxKlucz.Location = new System.Drawing.Point(150, 100);
        textBoxKlucz.Size = new System.Drawing.Size(600, 20);

        textBoxIV = new TextBox();
        textBoxIV.Location = new System.Drawing.Point(150, 140);
        textBoxIV.Size = new System.Drawing.Size(600, 20);

        labelCzasSzyfrowania = new Label();
        labelCzasSzyfrowania.Location = new System.Drawing.Point(20, 180);
        labelCzasSzyfrowania.Size = new System.Drawing.Size(600, 20);

        labelCzasDeszyfrowania = new Label();
        labelCzasDeszyfrowania.Location = new System.Drawing.Point(20, 220);
        labelCzasDeszyfrowania.Size = new System.Drawing.Size(600, 20);

        dataGridView = new DataGridView();
        dataGridView.Location = new System.Drawing.Point(20, 260);
        dataGridView.Size = new System.Drawing.Size(750, 300);
        dataGridView.ColumnCount = 3;
        dataGridView.Columns[0].Name = "Algorytm";
        dataGridView.Columns[1].Name = "Czas (s/blok)";
        dataGridView.Columns[2].Name = "Bajty/sek (RAM)";

        this.Controls.Add(comboBoxAlgorytm);
        this.Controls.Add(buttonGenerujKlucze);
        this.Controls.Add(buttonSzyfruj);
        this.Controls.Add(buttonOdszyfruj);
        this.Controls.Add(textBoxJawny);
        this.Controls.Add(textBoxZaszyfrowany);
        this.Controls.Add(textBoxKlucz);
        this.Controls.Add(textBoxIV);
        this.Controls.Add(labelCzasSzyfrowania);
        this.Controls.Add(labelCzasDeszyfrowania);
        this.Controls.Add(dataGridView);
    }

    private void ButtonGenerujKlucze_Click(object sender, EventArgs e)
    {
        switch (comboBoxAlgorytm.SelectedItem.ToString())
        {
            case "AES (CSP) 128 bit":
                algorytm = new AesCryptoServiceProvider() { KeySize = 128 };
                break;
            case "AES (CSP) 256 bit":
                algorytm = new AesCryptoServiceProvider() { KeySize = 256 };
                break;
            case "AES Managed 128 bit":
                algorytm = new AesManaged() { KeySize = 128 };
                break;
            case "AES Managed 256 bit":
                algorytm = new AesManaged() { KeySize = 256 };
                break;
            case "Rijndael Managed 128 bit":
                algorytm = new RijndaelManaged() { KeySize = 128 };
                break;
            case "Rijndael Managed 256 bit":
                algorytm = new RijndaelManaged() { KeySize = 256 };
                break;
            case "DES 56 bit":
                algorytm = new DESCryptoServiceProvider();
                break;
            case "3DES 168 bit":
                algorytm = new TripleDESCryptoServiceProvider();
                break;
            default:
                throw new Exception("Nieznany algorytm");
        }
        algorytm.GenerateKey();
        algorytm.GenerateIV();
        textBoxKlucz.Text = BitConverter.ToString(algorytm.Key).Replace("-", "");
        textBoxIV.Text = BitConverter.ToString(algorytm.IV).Replace("-", "");
    }

    private void ButtonSzyfruj_Click(object sender, EventArgs e)
    {
        byte[] plainText = Encoding.ASCII.GetBytes(textBoxJawny.Text);
        byte[] encrypted;

        Stopwatch sw = Stopwatch.StartNew();
        using (ICryptoTransform encryptor = algorytm.CreateEncryptor(algorytm.Key, algorytm.IV))
        {
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainText, 0, plainText.Length);
                    csEncrypt.FlushFinalBlock();
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        sw.Stop();
        labelCzasSzyfrowania.Text = $"Czas szyfrowania: {sw.Elapsed.TotalMilliseconds} ms";
        textBoxZaszyfrowany.Text = BitConverter.ToString(encrypted).Replace("-", "");

        double bytesPerSecondRAM = plainText.Length / sw.Elapsed.TotalSeconds;
        dataGridView.Rows.Add(comboBoxAlgorytm.SelectedItem.ToString(), sw.Elapsed.TotalSeconds, bytesPerSecondRAM);
    }

    private void ButtonOdszyfruj_Click(object sender, EventArgs e)
    {
        byte[] cipherText = StringToByteArray(textBoxZaszyfrowany.Text);
        byte[] decrypted = new byte[cipherText.Length];

        Stopwatch sw = Stopwatch.StartNew();
        using (ICryptoTransform decryptor = algorytm.CreateDecryptor(algorytm.Key, algorytm.IV))
        {
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    csDecrypt.Read(decrypted, 0, decrypted.Length);
                }
            }
        }
        sw.Stop();
        labelCzasDeszyfrowania.Text = $"Czas deszyfrowania: {sw.Elapsed.TotalMilliseconds} ms";
        textBoxJawny.Text = Encoding.ASCII.GetString(decrypted).TrimEnd('\0');
    }

    private byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Program());
    }
}
