using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Proyecto_SO
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked) //RadioButton de la longitud del nombre introducido
            {
                string mensaje = "1/" + textBox1.Text;
                // Enviamos al servidor el nombre introducido
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta por parte del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("La longitud de tu nombre es: " + mensaje);
            }
            else if (radioButton1.Checked) //RadioButton de si el nombre introducido es bonito
            {
                string mensaje = "2/" + textBox1.Text;
                // Enviamos al servidor el nombre introducido
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta por parte del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje == "SI")
                {
                    MessageBox.Show("Tu nombre es bonito :)");
                }
                else
                {
                    MessageBox.Show("Pues no, tu nombre no es bonito");
                }
            }
            else if (radioButton3.Checked) //RadioButton de si soy alto o no
            {
                string mensaje = "3/" + textBox1.Text + "/" + textBox2.Text;
                // Enviamos al servidor el nombre introducido
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta por parte del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                MessageBox.Show(mensaje);
            }
            else if (radioButton4.Checked) //RadioButton de si mi nombre es palíndromo
            {
                string mensaje = "4/" + textBox1.Text + "/" + textBox2.Text;
                // Enviamos al servidor el nombre introducido
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta por parte del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                MessageBox.Show(mensaje);
            }
            else if (radioButton5.Checked) //RadioButton de pedir el nombre en mayúsculas
            {
                string mensaje = "5/" + textBox1.Text + "/" + textBox2.Text;
                // Enviamos al servidor el nombre introducido
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta por parte del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                MessageBox.Show(mensaje);
            }
            else
            {
                MessageBox.Show("Se ha conectado con el servidor pero no hay ninguna solicitud que hacerle");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y el puerto del servidor al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.0.26");
            IPEndPoint ipep = new IPEndPoint(direc, 9040);


            //Creamos el socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep); //Intentamos conectarnos al socket
                label1.ForeColor = Color.Green;
                textBox1.ForeColor = Color.Black;
                textBox2.ForeColor = Color.Black;
                MessageBox.Show("Conectado con el servidor");
            }
            catch (SocketException ex) // Ha habido un error con la conexion
            {
                MessageBox.Show("ERROR: No se ha podido conectar con el servidor");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Mensaje de desconectar
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // FIN del servicio, nos desconectamos del servidor
            label1.ForeColor = Color.AliceBlue;
            textBox1.ForeColor = Color.Gray;
            textBox2.ForeColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
