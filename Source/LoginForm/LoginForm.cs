using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using RSA = SecMail.RSA;

namespace SecMailClientGui0
{
    public partial class LoginForm : Form
    {
        public Dictionary<string, RSAParams> LocalPrivKeys;
        public Client Client = new Client("x444556.ddns.net:5000");
        public LoginForm()
        {
            InitializeComponent();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("privKeys.json"))
            {
                LocalPrivKeys = JsonConvert.DeserializeObject<Dictionary<string, RSAParams>>(File.ReadAllText("privKeys.json"));
                foreach (KeyValuePair<string, RSAParams> kvp in LocalPrivKeys)
                {
                    listBox1.Items.Add(kvp.Key);
                }
            }
            else
            {
                LocalPrivKeys = new Dictionary<string, RSAParams>();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                Form1 f1 = new Form1(listBox1.SelectedItem as string, LocalPrivKeys[listBox1.SelectedItem as string].ToRSAParameters());
                f1.Client = Client;
                f1.FormClosing += (object s, FormClosingEventArgs fcea) => { this.Close(); };
                this.Hide();
                f1.Show();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string addr = "";

            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
            {
                addr += chars[rnd.Next(0, chars.Length)];
            }
            int addrlen = 0;
            for (int i = 0; i < 16 && i < addr.Length; i++) addrlen++;
            addr = addr.Substring(0, addrlen);
            Console.WriteLine("  Addr: " + addr);
            RSAParameters newPrivKey = RSA.NewPrivateKey();

            if (Client.Ping())
            {
                List<byte> bytes = new List<byte>();
                bytes.Add((byte)Server.PacketType.UpdatePubKey);
                bytes.AddRange(Encoding.UTF8.GetBytes(addr));
                for (; bytes.Count < 17; bytes.Add(0)) ;
                bytes.AddRange(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(RSA.GetPublicKey(
                    newPrivKey))));
                byte[] resp = Client.Send(bytes.ToArray());
                if (resp.Length > 0 && resp[0] == (byte)Server.PacketType.ACK)
                {
                    Console.WriteLine("PubKey updated: " + Client.Hostname + ":" + Client.Port);
                    LocalPrivKeys.Add(addr, new RSAParams(newPrivKey));
                    listBox1.Items.Add(addr);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    Console.WriteLine("Failed (" + Client.Hostname + ":" + Client.Port +
                        "): " + (Server.PacketType)resp[0]);
                    MessageBox.Show("Failed: " + (Server.PacketType)resp[0] +
                        "\n\nTry again in a few minutes!");
                }
            }
            else
            {
                Console.WriteLine("Failed: " + Client.Hostname + ":" + Client.Port +
                    " is not available!");
                MessageBox.Show("The server is not available!\n\nTry again in a few minutes!");
            }
            File.WriteAllText("privKeys.json", JsonConvert.SerializeObject(LocalPrivKeys));
        }
    }
}
