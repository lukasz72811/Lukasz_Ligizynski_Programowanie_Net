using System;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class Form1 : Form
    {
        private TextBox okienko1;
        private TextBox okienko2;
        private TextBox okienko3;
        private Button przycisk1;
        private Label label1;
        private Label label2;
        private Label label3;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.okienko1 = new TextBox();
            this.okienko2 = new TextBox();
            this.okienko3 = new TextBox();
            this.przycisk1 = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.SuspendLayout();

            this.okienko1.Location = new System.Drawing.Point(120, 30);
            this.okienko1.Name = "okienko1";
            this.okienko1.Size = new System.Drawing.Size(100, 22);

            this.okienko2.Location = new System.Drawing.Point(120, 70);
            this.okienko2.Name = "okienko2";
            this.okienko2.Size = new System.Drawing.Size(100, 22);
            // 
            // okienko3
            // 
            this.okienko3.Location = new System.Drawing.Point(120, 150);
            this.okienko3.Name = "okienko3";
            this.okienko3.ReadOnly = true;
            this.okienko3.Size = new System.Drawing.Size(100, 22);

            this.przycisk1.Location = new System.Drawing.Point(120, 110);
            this.przycisk1.Name = "przycisk1";
            this.przycisk1.Size = new System.Drawing.Size(100, 30);
            this.przycisk1.Text = "Podziel";
            this.przycisk1.UseVisualStyleBackColor = true;
            this.przycisk1.Click += new EventHandler(this.przycisk1_kliknięcie);

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.Text = "Dzielna:";

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 16);
            this.label2.Text = "Dzielnik:";

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.Text = "Wynik:";

            this.ClientSize = new System.Drawing.Size(284, 201);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.przycisk1);
            this.Controls.Add(this.okienko3);
            this.Controls.Add(this.okienko2);
            this.Controls.Add(this.okienko1);
            this.Name = "Form1";
            this.Text = "Kalkulator";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void przycisk1_kliknięcie(object sender, EventArgs e)
        {
            try
            {
                double dzielna = double.Parse(okienko1.Text);
                double dzielinik = double.Parse(okienko2.Text);
                double wynik = dzielna / dzielinik;
                okienko3.Text = wynik.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Proszę wprowadzić poprawne liczby.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                okienko3.Text = "Błąd";
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Nie można dzielić przez zero.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                okienko3.Text = "Błąd";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił nieoczekiwany błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                okienko3.Text = "Błąd";
            }
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
