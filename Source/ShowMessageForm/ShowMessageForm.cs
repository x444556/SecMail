using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecMailClientGui0
{
    public partial class ShowMessageForm : Form
    {
        private int LastHeight, LastWidth;

        private string From, To;
        private JsonMessage JsonMsg;
        public ShowMessageForm(string from, string to, JsonMessage jsmsg)
        {
            From = from;
            To = to;
            JsonMsg = jsmsg;

            InitializeComponent();
        }
        public static string Storage(ulong bytes)
        {
            if (bytes >= 1000000000000) return Math.Round(bytes / 1000000000000.0, 2).ToString("0.00") + " TB";
            else if (bytes >= 1000000000) return Math.Round(bytes / 1000000000.0, 2).ToString("0.00") + " GB";
            else if (bytes >= 1000000) return Math.Round(bytes / 1000000.0, 2).ToString("0.00") + " MB";
            else if (bytes >= 1000) return Math.Round(bytes / 1000.0, 2).ToString("0.00") + " KB";
            else return bytes + "  B";
        }
        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        private void ShowMessageForm_ResizeBegin(object sender, EventArgs e)
        {
            LastHeight = Size.Height;
            LastWidth = Size.Width;
        }
        private void ShowMessageForm_Load(object sender, EventArgs e)
        {
            label3.Text = "From:   " + From + " / \"" + JsonMsg.From + "\"";
            label1.Text = "To     :   " + To + " / \"" + JsonMsg.To + "\"";
            label2.Text = JsonMsg.DateTime.Day.ToString().PadLeft(2, '0') + "." + JsonMsg.DateTime.Month.ToString().PadLeft(2, '0') + 
                "." + JsonMsg.DateTime.Year + " " + JsonMsg.DateTime.Hour.ToString().PadLeft(2, '0') + ":" + 
                JsonMsg.DateTime.Minute.ToString().PadLeft(2, '0');
            label5.Text = "\"" + JsonMsg.Subject + "\"";

            foreach(MessageAttachment attachment in JsonMsg.Attachments)
            {
                if(attachment.Type == MessageAttachment.AttachmentType.Image)
                {
                    Image img = Image.FromStream(new MemoryStream(attachment.Data, 0, attachment.Data.Length));
                    img = resizeImage(img, new Size(64, 64));
                    dataGridView1.Rows.Add(attachment.Name, attachment.Type, Storage((ulong)attachment.Data.Length), img);
                }
                else if ((attachment.Type == MessageAttachment.AttachmentType.Code || attachment.Type == 
                    MessageAttachment.AttachmentType.Script || attachment.Type == MessageAttachment.AttachmentType.Text) && 
                    File.Exists("Document_16x.png"))
                {
                    dataGridView1.Rows.Add(attachment.Name, attachment.Type, Storage((ulong)attachment.Data.Length), 
                        resizeImage(Image.FromFile("Document_16x"), new Size(64, 64)));
                }
                else
                {
                    dataGridView1.Rows.Add(attachment.Name, attachment.Type, Storage((ulong)attachment.Data.Length));
                }
            }

            textBox4.Text = Encoding.UTF8.GetString(JsonMsg.Content);
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = JsonMsg.Attachments[dataGridView1.SelectedRows[0].Index].Name;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, JsonMsg.Attachments[dataGridView1.SelectedRows[0].Index].Data);
            }
        }
        private void ShowMessageForm_ResizeEnd(object sender, EventArgs e)
        {
            double heightMul = Size.Height / (double)LastHeight;
            double widthMul = Size.Width / (double)LastWidth;
            ResizeChildren(this, heightMul, widthMul);
        }
        private void ResizeChildren(Control c, double heightMul, double widthMul)
        {
            foreach (Control cont in c.Controls)
            {
                cont.Size = new Size((int)(cont.Size.Width * widthMul), (int)(cont.Size.Height * heightMul));
                cont.Location = new Point((int)(cont.Location.X * widthMul), (int)(cont.Location.Y * heightMul));
                if (cont.GetType() == typeof(Label))
                {
                    ((Label)cont).Font = new Font(((Label)cont).Font.Name, (int)(((Label)cont).Font.Size * heightMul),
                        ((Label)cont).Font.Style);
                }
                else if (cont.GetType() == typeof(TextBox))
                {
                    ((TextBox)cont).Font = new Font(((TextBox)cont).Font.Name, (int)(((TextBox)cont).Font.Size *
                        heightMul), ((TextBox)cont).Font.Style);
                }
                else if (cont.GetType() == typeof(Button))
                {
                    ((Button)cont).Font = new Font(((Button)cont).Font.Name, (int)(((Button)cont).Font.Size *
                        heightMul), ((Button)cont).Font.Style);
                }
                else if (cont.GetType() == typeof(DataGridView))
                {
                    ((DataGridView)cont).Font = new Font(((DataGridView)cont).Font.Name, (int)(((DataGridView)cont).Font.Size *
                        heightMul), ((DataGridView)cont).Font.Style);
                    /*((DataGridView)cont).RowsDefaultCellStyle.Font = new Font(((DataGridView)cont).RowsDefaultCellStyle.Font.Name, 
                        (int)(((DataGridView)cont).RowsDefaultCellStyle.Font.Size * heightMul), 
                        ((DataGridView)cont).RowsDefaultCellStyle.Font.Style);*/
                    foreach (DataGridViewRow row in ((DataGridView)cont).Rows)
                    {
                        //row.DefaultCellStyle = ((DataGridView)cont).RowsDefaultCellStyle;
                        row.Height = (int)(row.Height * heightMul);
                    }
                    foreach (DataGridViewColumn col in ((DataGridView)cont).Columns)
                    {
                        col.Width = (int)(col.Width * widthMul);
                    }
                }
                else if (cont.GetType() == typeof(ListBox))
                {
                    ((ListBox)cont).Font = new Font(((ListBox)cont).Font.Name, (int)(((ListBox)cont).Font.Size *
                        heightMul), ((ListBox)cont).Font.Style);
                }
                else if (cont.GetType() == typeof(NumericUpDown))
                {
                    ((NumericUpDown)cont).Font = new Font(((NumericUpDown)cont).Font.Name, (int)(((NumericUpDown)cont).Font.Size *
                        heightMul), ((NumericUpDown)cont).Font.Style);
                }
                cont.Refresh();
                ResizeChildren(cont, heightMul, widthMul);
            }
        }
    }
}
