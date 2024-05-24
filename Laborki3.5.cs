using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class Program : Form
{
    [DllImport("gdi32.dll")]
    static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

    [DllImport("gdi32.dll")]
    static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

    [DllImport("gdi32.dll")]
    static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

    [DllImport("gdi32.dll")]
    static extern bool LineTo(IntPtr hdc, int x, int y);

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    static void Main()
    {
        Application.Run(new Program());
    }

    protected override void Narysuj(PaintEventArgs e)
    {
        base.Narysuj(e);
        IntPtr hdc = GetDC(this.Handle);

        NarysujSinusa(hdc);

        ReleaseDC(this.Handle, hdc);
    }

    void NarysujSinusa(IntPtr hdc)
    {
        IntPtr pen = CreatePen(0, 2, 0x000000FF);
        IntPtr oldPen = SelectObject(hdc, pen);

        int width = this.ClientSize.Width;
        int height = this.ClientSize.Height;
        int centerX = width / 2;
        int centerY = height / 2;
        double scale = 50.0;

        MoveToEx(hdc, 0, centerY, IntPtr.Zero);
        for (int x = 0; x < width; x++)
        {
            double y = Math.Sin((x - centerX) / scale) * scale;
            LineTo(hdc, x, centerY - (int)y);
        }

        SelectObject(hdc, oldPen);
    }

    public Program()
    {
        this.Text = "Wykres Sinusoidy";
        this.Width = 800;
        this.Height = 600;
    }
}
