using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Yêu cầu:
//-TextBox Kết quả không cho phép người dùng nhập dữ liệu.
//- Khi người dùng nhấn nút Giải PT (Hoặc nhấn Alt+G), chương trình sẽ kiểm tra xem người dùng đã nhập đủ a, b, c chưa; nếu chưa nhập bắt người dùng phải nhập. Nếu đã nhập đủ dữ liệu kiểm tra xem a, b, c đã nhập có phải số không. Nếu là số thì giải phương trình bậc 2 với a, b, c đã nhập và đưa kết quả vào TextBox kết quả.
//- Khi người dùng nhấn nút Làm lại (Hoặc nhấn Alt +L), Form sẽ trở về trạng thái ban đầu (Các TextBox bị xóa trống, con trỏ đang ở TextBox Nhập a)
//- Khi người dùng nhấn nút Thoát (hoặc nhấn Alt + T) chương trình sẽ đưa ra thông báo “Ban cố muốn thoát không?” với 2 nút Yes/No, nếu chọn Yes thì thoát.


namespace Quadratic_Equation_Calculator
{
    public partial class Form1 : Form
    {
        private float zoomFactor = 1.0f;
        private const float zoomStep = 0.1f;
        private double a;
        private double b;
        private double c;
        private bool check1 = false;
        private bool checking = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void calc_Click(object sender, EventArgs e)
        {
            string txtA = inputa.Text;
            string txtB = inputb.Text;
            string txtC = inputc.Text;
            if (txtA == "")
            {
                MessageBox.Show("Bạn chưa nhập a", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtB == "")
            {
                MessageBox.Show("Bạn chưa nhập b", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtC == "")
            {
                MessageBox.Show("Bạn chưa nhập c", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    a = double.Parse(txtA);
                    b = double.Parse(txtB);
                    c = double.Parse(txtC);
                    if (a == 0)
                    {
                        if (b == 0)
                        {
                            if (c == 0)
                            {
                                result.Text = "Phương trình có vô số nghiệm";
                            }
                            else
                            {
                                result.Text = "Phương trình vô nghiệm";
                            }
                        }
                        else
                        {
                            result.Text = "Phương trình có nghiệm duy nhất: x = " + (-c / b);
                        }
                    }
                    else
                    {
                        double delta = b * b - 4 * a * c;
                        if (delta < 0)
                        {
                            result.Text = "Phương trình vô nghiệm";
                        }
                        else if (delta == 0)
                        {
                            result.Text = "Phương trình có nghiệm kép: x1 = x2 = " + (-b / (2 * a));
                        }
                        else
                        {
                            double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
                            double x2 = (-b - Math.Sqrt(delta)) / (2 * a);
                            result.Text = "Phương trình có 2 nghiệm phân biệt: x1 = " + x1 + ", x2 = " + x2;
                            checking = true;
                        }
                    }
                    check1 = true;
                    pictureBox2.Invalidate();
                }
                catch (Exception)
                {
                    MessageBox.Show("a, b, c phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void reset_Click_1(object sender, EventArgs e)
        {
            inputa.Text = "";
            inputb.Text = "";
            inputc.Text = "";
            result.Text = "";
            checking = false;
            check1 = false;
            pictureBox2.Invalidate();
        }

        private void exitbutton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();        
        }

        // Lắng nghe keyboard event

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.G))
            {
                calc_Click(null, null);
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.L))
            {
                reset_Click_1(null, null);
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.T))
            {
                exitbutton_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/hoangmanhkhiem";
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở liên kết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Blue, 2);
            Pen derivativePen = new Pen(Color.Red, 2);
            Font font = new Font("Arial", 8);

            // Vẽ trục X và Y
            g.DrawLine(Pens.Black, pictureBox1.Width / 2, 0, pictureBox1.Width / 2, pictureBox1.Height);
            g.DrawLine(Pens.Black, 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2);

            for (int i = 1; i < pictureBox1.Width / (20 * zoomFactor); i++)
            {
                int x = pictureBox1.Width / 2 + (int)(i * 20 * zoomFactor);
                int y = pictureBox1.Height / 2;
                g.DrawLine(pen, x, y - 5, x, y + 5);
                g.DrawString((i).ToString(), font, Brushes.Black, x + 2, y + 5);
                g.DrawLine(pen, pictureBox1.Width / 2 - (int)(i * 20 * zoomFactor), y - 5, pictureBox1.Width / 2 - (int)(i * 20 * zoomFactor), y + 5);
                g.DrawString((-i).ToString(), font, Brushes.Black, pictureBox1.Width / 2 - (int)(i * 20 * zoomFactor) + 2, y + 5);
            }

            for (int i = 1; i < pictureBox1.Height / (20 * zoomFactor); i++)
            {
                int x = pictureBox1.Width / 2;
                int y = pictureBox1.Height / 2 + (int)(i * 20 * zoomFactor);
                g.DrawLine(pen, x - 5, y, x + 5, y);
                g.DrawString((-i).ToString(), font, Brushes.Black, x - 20, y + 2);
                g.DrawLine(pen, x - 5, pictureBox1.Height / 2 - (int)(i * 20 * zoomFactor), x + 5, pictureBox1.Height / 2 - (int)(i * 20 * zoomFactor));
                g.DrawString((i).ToString(), font, Brushes.Black, x - 20, pictureBox1.Height / 2 - (int)(i * 20 * zoomFactor) + 2);
            }

            PointF[] quadraticPoints = new PointF[pictureBox1.Width];
            for (int x = 0; x < pictureBox1.Width; x++)
            {
                double xValue = (x - pictureBox1.Width / 2) / (20.0 * zoomFactor); // Chuyển đổi từ pixel thành giá trị x
                double yValue = a * xValue * xValue + b * xValue + c;
                int y = pictureBox1.Height / 2 - (int)(yValue * 20 * zoomFactor); // Chuyển đổi từ giá trị y thành pixel
                quadraticPoints[x] = new PointF(x, y);
                Console.WriteLine($"x = {xValue}, y = {yValue}"); // Kiểm tra giá trị
            }

            if (check1 == true)
            {
                g.DrawCurve(pen, quadraticPoints);

                double x0 = -b / (2 * a);
                double y0 = a * x0 * x0 + b * x0 + c;
                float x0Pixel = (float)(pictureBox1.Width / 2 + x0 * 20 * zoomFactor);
                float y0Pixel = (float)(pictureBox1.Height / 2 - y0 * 20 * zoomFactor);
                g.FillEllipse(Brushes.Red, x0Pixel - 2, y0Pixel - 2, 4, 4);

                double x1 = 0;
                double y1 = a * x1 * x1 + b * x1 + c;
                float x1Pixel = (float)(pictureBox1.Width / 2 + x1 * 20 * zoomFactor);
                float y1Pixel = (float)(pictureBox1.Height / 2 - y1 * 20 * zoomFactor);
                g.FillEllipse(Brushes.Red, x1Pixel - 2, y1Pixel - 2, 4, 4);
            }
            

            if(checking == true)
            {
                double x_1 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double x_2 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double y_1 = a * x_1 * x_1 + b * x_1 + c;
                double y_2 = a * x_2 * x_2 + b * x_2 + c;
                float x_1Pixel = (float)(pictureBox1.Width / 2 + x_1 * 20 * zoomFactor);
                float y_1Pixel = (float)(pictureBox1.Height / 2 - y_1 * 20 * zoomFactor);
                float x_2Pixel = (float)(pictureBox1.Width / 2 + x_2 * 20 * zoomFactor);
                float y_2Pixel = (float)(pictureBox1.Height / 2 - y_2 * 20 * zoomFactor);
                g.FillEllipse(Brushes.Red, x_1Pixel - 2, y_1Pixel - 2, 4, 4);
                g.FillEllipse(Brushes.Red, x_2Pixel - 2, y_2Pixel - 2, 4, 4);
            }

            pen.Dispose();
            derivativePen.Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            zoomFactor += zoomStep;
            pictureBox2.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zoomFactor > zoomStep)
            {
                zoomFactor -= zoomStep;
                pictureBox2.Invalidate();
            }
        }
    }
}
