﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Net.Http;
using Newtonsoft.Json;

namespace DrawLuckyWheel
{
    public partial class Form1 : Form
    {
        bool wheelIsMoved;
        float wheelTimes;
        Timer wheelTimer;
        LuckyCirlce koloFortuny;
        List<string> history;
        private readonly HttpClient _httpClient;
        private List<CodeData> codes;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            wheelTimer = new Timer();
            wheelTimer.Interval = 30; // speed 
            wheelTimer.Tick += wheelTimer_Tick;
            koloFortuny = new LuckyCirlce();
            history = new List<string>();
            _httpClient = new HttpClient();
        }
        public class LuckyCirlce
        {
            public Bitmap obrazek;
            public Bitmap tempObrazek;
            public float kat;
            public int[] wartosciStanu;
            public int stan;

            public LuckyCirlce()
            {
                tempObrazek = new Bitmap(Properties.Resources.lucky_wheel);
                obrazek = new Bitmap(Properties.Resources.lucky_wheel);
                wartosciStanu = new int[] { 12, 11, 10, 09, 08, 07, 06, 05, 04, 03, 02, 01 };
                kat = 0.0f;
            }

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string url = "https://localhost:7284/api/code";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                List<CodeData> items = JsonConvert.DeserializeObject<List<CodeData>>(responseData);

                this.codes = items;
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }

        public static Bitmap RotateImage(Image image, float angle)
        {
            return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2), angle);
        }

        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");


            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics g = Graphics.FromImage(rotatedBmp);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void RotateImage(PictureBox pb, Image img, float angle)
        {
            if (img == null || pb.Image == null)
                return;

            Image oldImage = pb.Image;
            pb.Image = RotateImage(img, angle);
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }
        private async void wheelTimer_Tick(object sender, EventArgs e)
        {
            if(wheelTimes == 0 )
            {
                wheelTimer.Stop();
                history.Add(Convert.ToString(koloFortuny.wartosciStanu[koloFortuny.stan]));

                string url = "https://localhost:7284/api/info";

                try
                {
                    var postData = new
                    {
                        Code = int.Parse(textBox1.Text),
                        Name = textBox2.Text,
                        PhoneNumber = textBox3.Text,
                        Prize = Convert.ToString(koloFortuny.wartosciStanu[koloFortuny.stan])
                    };

                    string json = JsonConvert.SerializeObject(postData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();

                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show($"Request error: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}");
                }



                Form2 tempForm = new Form2(Convert.ToString(koloFortuny.wartosciStanu[koloFortuny.stan]));
                this.AddOwnedForm(tempForm);
                tempForm.Show();

            }

            if (wheelIsMoved && wheelTimes > 0)
            {
                koloFortuny.kat += wheelTimes / 10;
                koloFortuny.kat = koloFortuny.kat % 360;
                RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                wheelTimes--;
            }

            koloFortuny.stan = Convert.ToInt32(Math.Ceiling(koloFortuny.kat / 30));

            if (koloFortuny.stan == 0)
            {
                koloFortuny.stan = 0;
            }
            else
            {
                koloFortuny.stan -= 1;
            }

            label1.Text = Convert.ToString(koloFortuny.wartosciStanu[koloFortuny.stan]);


        }
    
        private void btnPlay_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mã");
                return;
            }

            if (codes.Where(value => value.Code == int.Parse(textBox1.Text)).ToList().Count <= 0)
            {
                MessageBox.Show("Mã không đúng");
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên");
                return;
            }

            if (textBox3.Text == "")
            {
                MessageBox.Show("Vui lòng nhập SĐT");
                return;
            }

            wheelIsMoved = true;
            Random rand = new Random();
            wheelTimes = rand.Next(150, 200);    

            wheelTimer.Start();
            SoundPlayer ss = new SoundPlayer("audio1.wav");
            ss.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this.history);
            form3.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
