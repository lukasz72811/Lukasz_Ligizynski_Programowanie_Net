using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Kalkulator
{
    public class Form1 : Form
    {
        private TextBox ekran;
        private Button[] cyfry;
        private Button dodaj, odejmij, mnóż, dziel, równość, czyść;
        private string operacja;
        private double liczba1, liczba2, wynik;

        public Form1()
        {
            StartProgramu();
        }

        private void StartProgramu()
        {
            this.ekran = new TextBox();
            this.cyfry = new Button[10];
            for (int i = 0; i < 10; i++)
            {
                this.cyfry[i] = new Button();
                this.cyfry[i].Text = i.ToString();
                this.cyfry[i].Click += new EventHandler(this.Cyfra_Click);
            }

            this.dodaj = new Button();
            this.odejmij = new Button();
            this.mnóż = new Button();
            this.dziel = new Button();
            this.równość = new Button();
            this.czyść = new Button();

            this.dodaj.Text = "+";
            this.odejmij.Text = "-";
            this.mnóż.Text = "*";
            this.dziel.Text = "/";
            this.równość.Text = "=";
            this.czyść.Text = "C";

            this.dodaj.Click += new EventHandler(this.Operator_Click);
            this.odejmij.Click += new EventHandler(this.Operator_Click);
            this.mnóż.Click += new EventHandler(this.Operator_Click);
            this.dziel.Click += new EventHandler(this.Operator_Click);
            this.równość.Click += new EventHandler(this.równość_Click);
            this.czyść.Click += new EventHandler(this.czyść_Click);

            this.ekran.Location = new System.Drawing.Point(12, 12);
            this.ekran.Size = new System.Drawing.Size(210, 22);
            this.ekran.ReadOnly = true;
            this.ekran.TextAlign = HorizontalAlignment.Right;

            int left = 12, top = 50;
            for (int i = 1; i < 10; i++)
            {
                this.cyfry[i].Size = new System.Drawing.Size(50, 50);
                this.cyfry[i].Location = new System.Drawing.Point(left, top);
                left += 60;
                if (i % 3 == 0)
                {
                    left = 12;
                    top += 60;
                }
                this.Controls.Add(this.cyfry[i]);
            }
            this.cyfry[0].Size = new System.Drawing.Size(110, 50);
            this.cyfry[0].Location = new System.Drawing.Point(12, top + 60);
            this.Controls.Add(this.cyfry[0]);

            this.dodaj.Size = new System.Drawing.Size(50, 50);
            this.dodaj.Location = new System.Drawing.Point(192, 50);
            this.odejmij.Size = new System.Drawing.Size(50, 50);
            this.odejmij.Location = new System.Drawing.Point(192, 110);
            this.mnóż.Size = new System.Drawing.Size(50, 50);
            this.mnóż.Location = new System.Drawing.Point(192, 170);
            this.dziel.Size = new System.Drawing.Size(50, 50);
            this.dziel.Location = new System.Drawing.Point(192, 230);
            this.równość.Size = new System.Drawing.Size(50, 110);
            this.równość.Location = new System.Drawing.Point(192, 290);
            this.czyść.Size = new System.Drawing.Size(50, 50);
            this.czyść.Location = new System.Drawing.Point(132, 290);

            this.Controls.Add(this.dodaj);
            this.Controls.Add(this.odejmij);
            this.Controls.Add(this.mnóż);
            this.Controls.Add(this.dziel);
            this.Controls.Add(this.równość);
            this.Controls.Add(this.czyść);
            this.Controls.Add(this.ekran);

            this.ClientSize = new System.Drawing.Size(264, 400);
            this.Name = "Form1";
            this.Text = "Kalkulator";
        }

        private void Cyfra_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (ekran.Text == "0")
                ekran.Text = button.Text;
            else
                ekran.Text += button.Text;
        }

        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            liczba1 = double.Parse(ekran.Text);
            operacja = button.Text;
            ekran.Text = "0";
        }

        private void równość_Click(object sender, EventArgs e)
        {
            liczba2 = double.Parse(ekran.Text);
            switch (operacja)
            {
                case "+":
                    wynik = liczba1 + liczba2;
                    break;
                case "-":
                    wynik = liczba1 - liczba2;
                    break;
                case "*":
                    wynik = liczba1 * liczba2;
                    break;
                case "/":
                    if (liczba2 != 0)
                        wynik = liczba1 / liczba2;
                    else
                    {
                        MessageBox.Show("Nie można dzielić przez zero.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
            }
            ekran.Text = wynik.ToString();
        }

        private void czyść_Click(object sender, EventArgs e)
        {
            ekran.Text = "0";
            liczba1 = 0;
            liczba2 = 0;
            wynik = 0;
            operacja = string.Empty;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
