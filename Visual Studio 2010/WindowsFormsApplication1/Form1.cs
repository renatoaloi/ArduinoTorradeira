using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int contaDisparo = 0;
        System.Drawing.Bitmap bmp;

        public Form1()
        {
            InitializeComponent();

            this.bmp = new Bitmap(320, 240);

            resetBmp();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = (Image)this.bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.textBox2.Text) <= Convert.ToInt32(this.textBox3.Text))
            {
                this.textBox1.Text = this.textBox2.Text + "\r\n" + this.textBox1.Text;

                disparo(Convert.ToInt32(this.textBox2.Text));
            }

            this.textBox2.Text = "";
            this.textBox2.Focus();
        }

        void disparo(int valor)
        {
            int fator = 0;
            if (contaDisparo + 5 >= 320)
            {
                resetBmp();
                this.pictureBox1.Image = (Image)this.bmp;
                this.contaDisparo = 0;
            }

            for (int i = contaDisparo; i < contaDisparo + 5; i++)
            {
                fator = (int)(240.0 * ((float)valor / (float)Convert.ToDecimal(this.textBox3.Text)));
                for (int j = 239; j >= 240 - fator; j--)
                {
                    this.bmp.SetPixel(i, j, Color.FromArgb(255, 0, 0, 255));
                }
            }
            this.pictureBox1.Image = (Image)this.bmp;
            this.contaDisparo += 5;
        }

        void resetBmp()
        {
            for (int i = 0; i < 320; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    this.bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetBmp();
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.pictureBox1.Image = (Image)this.bmp;

            this.contaDisparo=0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.button3.Text != "Fechar COM")
            {
                serialPort1.PortName = this.textBox4.Text;
                serialPort1.Open();
                this.button3.Text = "Fechar COM";
            }
            else
            {
                serialPort1.Close();
                this.button3.Text = "Receb. Serial";
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
            atualizaTextoSerial(serialPort1.ReadLine());
        }

        void atualizaTextoSerial(string texto)
        {
            if (texto.Split(':').Count() > 1)
            {
                string valorTexto = texto.Split(':')[1].ToString();
                decimal valorFloat = Convert.ToDecimal(valorTexto);
                int valorInt = (int)Math.Ceiling(valorFloat);
                valorTexto = valorInt.ToString();

                if (this.label4.InvokeRequired)
                {
                    this.textBox2.Invoke((MethodInvoker)delegate { this.textBox2.Text = valorTexto; });
                    this.label4.Invoke((MethodInvoker)delegate { this.label4.Text = texto; });
                    this.button1.Invoke((MethodInvoker)delegate { this.button1_Click(null, null); });
                }
                else
                {
                    this.textBox2.Text = valorTexto;
                    this.label4.Text = texto;
                    this.button1_Click(null, null);
                }
            }
        }
    }
}
