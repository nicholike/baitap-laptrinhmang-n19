using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawLuckyWheel
{
    public partial class Form3 : Form
    {
        private readonly HttpClient _httpClient;
        public Form3(List<string> history)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            this.TransparencyKey = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;
           
            this.listView1.Columns.Add("Code");
            this.listView1.Columns.Add("Tên");
            this.listView1.Columns.Add("Số điện thoại");
            this.listView1.Columns.Add("Giải thưởng");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            string url = "https://localhost:7284/api/info";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                List<Info> items = JsonConvert.DeserializeObject<List<Info>>(responseData);
                this.listView1.Items.Clear();
                foreach (var item in items)
                {
                    this.listView1.Items.Add(new ListViewItem(new string[] { item.Code.ToString(), item.Name, item.PhoneNumber, item.Prize }));
                }
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
    }
}
