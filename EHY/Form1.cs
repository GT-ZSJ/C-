using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using ZXing.QrCode;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ZXing.QrCode.Internal;

namespace EHY
{
    public partial class Form1 : Form
    {
        EncodingOptions options = null;
        BarcodeWriter writer = null;
        BarcodeReader reader = null;
        public Form1()
        {
            InitializeComponent();
            options = new EncodingOptions
            {
                //DisableECI = true,
                //CharacterSet = "UTF-8",
                Width = pictureBox1.Width,
                Height = pictureBox1.Height
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.ITF;
            writer.Options = options;

            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = pictureBox1.Width,
                Height = pictureBox1.Height
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            reader = new BarcodeReader();

        }
        string opFilePath = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("条形码");
            comboBox1.Items.Add("二维码");
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Add("条形/二维码识别");
            comboBox2.Items.Add("条形/二维码生成");
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "条形/二维码生成")
            {
                if (comboBox1.Text == "条形码")
                {
                    // 获取文本框中的内容
                    string inputText = textBox1.Text;

                    // 判断是否包含数字之外的字符
                    if (!Regex.IsMatch(inputText, @"^[0-9]+$"))
                    {
                        // 弹出窗口警告
                        MessageBox.Show("输入内容必须为数字，请重新输入！");

                        // 清空文本框中的内容
                        textBox1.Clear();
                        return;
                    }//判断条形码内是否为数字。
                    int a = 2;
                    int b = textBox1.Text.Length;
                    b = b % a;
                    if (b == 1)
                    {
                        MessageBox.Show("请输入复数");
                        return;
                    }//判断条形码内是否为单数数字。
                    writer.Format = BarcodeFormat.ITF;
                }
                if (comboBox1.Text == "二维码") { writer.Format = BarcodeFormat.QR_CODE; }
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("输入内容不能为空");
                    return;
                }

                Bitmap bitmap = writer.Write(textBox1.Text);
                pictureBox1.Image = bitmap;
            }
            if (comboBox2.Text == "条形/二维码识别")
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
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

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
                saveFileDialog1.Filter = "BMP Files(*.bmp)|*.bmp";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName);
                }
                return;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "条形/二维码生成")
            {
                comboBox1.Visible = true; // 显示comboBox2
                pictureBox1.Image = null;
                textBox1.Text = null;
                return;
            }
            if (comboBox2.SelectedItem.ToString() == "条形/二维码识别")
            {
                comboBox1.Visible = false; // 隐藏comboBox2
                pictureBox1.Image = null;
                textBox1.Text = null;
                return;
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
