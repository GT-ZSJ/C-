using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace guyanqin
{
    public partial class Form1 : Form
    {
        EncodingOptions options = null;
        BarcodeWriter writer = null;
        BarcodeReader reader = null;
        public Form1()
        {
            InitializeComponent();
            reader = new BarcodeReader();
        }
        string opFilePath = "";
        public bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            pictureBox1.Image = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            options = new EncodingOptions
            {
                //条形码
                Width = pictureBox1.Width,
                Height = pictureBox1.Height,


            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.ITF;
            writer.Options = options;
            options = new QrCodeEncodingOptions
            {
                //二维码
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = pictureBox1.Width,
                Height = pictureBox1.Height

            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            if (comboBox1.Text == "条形码")
            {
                int a = 2;
                int b = textBox1.Text.Length;
                b = b % a;
                if (b == 1)
                {
                    MessageBox.Show("请输入偶数个字符！");
                    return;
                }
                else if (HasChinese(textBox1.Text))
                {
                    MessageBox.Show("请输入数字，不能带有中文符！");
                }
                else
                {
                    writer.Format = BarcodeFormat.ITF;
                }
            }
            else if (comboBox1.Text == "二维码")
            {
                writer.Format = BarcodeFormat.QR_CODE;
            }
            Bitmap bitmap = writer.Write(textBox1.Text);
            pictureBox1.Image = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//openFileDialog是窗体作为模式窗体打开
                                                                //ShowDialog()是个函数，同时也是一个动作
            {
                opFilePath = openFileDialog1.FileName;
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                pictureBox1.Load(opFilePath);
                Result result = reader.Decode((Bitmap)pictureBox1.Image);
                //通过reader 解码
                textBox1.Text = result.Text;//显示解析结果
                return;
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG Files(*.png)| *.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("条形码");
            comboBox1.Items.Add("二维码");
            comboBox1.SelectedIndex = 0;
        }
    }
}
