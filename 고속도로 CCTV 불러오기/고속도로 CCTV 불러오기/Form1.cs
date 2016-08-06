using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
namespace 고속도로_CCTV_불러오기
{
    public partial class Form1 : Form
    {
      

        int 제한 = 0;
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
        }

        void CCTV수신()
        {

            List<string> 수신된CCTV = new List<string>();
            List<string> 수신된CCTVID = new List<string>();
            for (int i = 10; 수신된CCTV.Count != 제한; i++)
            {
                수신된CCTV = 수신된CCTV.Distinct().ToList();
                string strUri = "https://m.map.naver.com/traffic/cctvPlayer.nhn?cctvGroupId=17&channel=" + i + "&seq=2#";

                /////////////////////////////////////////////////////////////////////////////////////
                /* POST */
                // HttpWebRequest 객체 생성, 설정
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUri);
                request.Method = "POST";    // 기본값 "GET"
                request.ContentType = "application/x-www-form-urlencoded";
                // 요청, 응답 받기
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 응답 Stream 읽기
                Stream stReadData = response.GetResponseStream();
                StreamReader srReadData = new StreamReader(stReadData, Encoding.UTF8);

                // 응답 Stream -> 응답 String 변환
                string strResult = srReadData.ReadToEnd();
                string[] test = strResult.Split('\n');

                if (test.Length != 18 && test[111] != "\t\t<h1><em class=\"_sectionName\"></em></h1>")
                {
                    string 고속도로이름 = test[111].Remove(test[111].IndexOf("<em"));
                    고속도로이름 = 고속도로이름.Remove(0, 고속도로이름.IndexOf(">") + 1);
                    string 고속도로구간 = test[111].Remove(0, test[111].IndexOf('\u0022' + ">"));
                    고속도로구간 = 고속도로구간.Remove(0, 2);
                    고속도로구간 = 고속도로구간.Remove(고속도로구간.IndexOf("<"));
                    Console.WriteLine(strUri);
                    if (수신된CCTV.Count != 0)
                    {
                        bool 있다 = false;
                        for (int ii = 0; ii != 수신된CCTV.Count; ii++)
                        {
                            if (수신된CCTV[ii].Split(':')[0].Trim() + " : " + 수신된CCTV[ii].Split(':')[1].Trim() == 고속도로이름.Trim() + " : " + 고속도로구간.Trim())
                            {
                                있다 = true;
                                수신된CCTV.RemoveAt(ii);
                                ii--;
                            }
                            else
                            {
                                있다 = false;
                            }
                        }
                        if (!있다)
                        {

                            있다 = false;
                            if (test[170] != "\tvar cctv = {")
                            {
                                string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[170].Remove(0, test[170].IndexOf(":") + 3).Remove(64) + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(test[171].Remove(0, test[171].IndexOf(":") + 3).IndexOf(",") - 1) + " : " + test[173].Remove(0, test[173].IndexOf(":") + 2).Replace(",","")  + " : " + test[172].Remove(0, test[172].IndexOf(":") + 2).Replace(",", "");
                                수신된CCTV.Add(파싱값);
                            
                                Console.WriteLine(파싱값);

                            }
                           
                            else
                            {
                                string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(64) + " : " + test[172].Remove(0, test[172].IndexOf(":") + 3).Remove(test[172].Remove(0, test[172].IndexOf(":") + 3).IndexOf(",") - 1) + " : " + test[174].Remove(0, test[173].IndexOf(":") + 2).Replace(",", "") + " : " + test[173].Remove(0, test[174].IndexOf(":") + 2).Replace(",", "");
                                수신된CCTV.Add(파싱값);
                                Console.WriteLine(파싱값);
                            }
                        }

                    }
                    else
                    {
                        if (test[170] != "\tvar cctv = {")
                        {
                            string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[170].Remove(0, test[170].IndexOf(":") + 3).Remove(64) + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(test[171].Remove(0, test[171].IndexOf(":") + 3).IndexOf(",") - 1) + " : " + test[173].Remove(0, test[173].IndexOf(":") + 2).Replace(",", "") + " : " + test[172].Remove(0, test[172].IndexOf(":") + 2).Replace(",", "");
                            수신된CCTV.Add(파싱값);

                            Console.WriteLine(파싱값);

                        }

                        else
                        {
                            string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(64) + " : " + test[172].Remove(0, test[172].IndexOf(":") + 3).Remove(test[172].Remove(0, test[172].IndexOf(":") + 3).IndexOf(",") - 1) + " : " + test[174].Remove(0, test[173].IndexOf(":") + 2).Replace(",", "") + " : " + test[173].Remove(0, test[174].IndexOf(":") + 2).Replace(",", "");
                            수신된CCTV.Add(파싱값);
                            Console.WriteLine(파싱값);
                        }
                    }

                    Invoke(new Action(
                           delegate ()
                           {
                               progressBar1.Value = Convert.ToInt32(Convert.ToSingle(수신된CCTV.Count) / Convert.ToSingle(제한) * 100);
                               label1.Text = 수신된CCTV.Count + " / " + 제한;
                           }));

                    고속도로이름 = null;
                    고속도로구간 = null;


                }
            }
            Invoke(new Action(
                           delegate ()
                           {
                               listView1.Items.Clear();
                           }));
            foreach (string 입력 in 수신된CCTV)
            {
                string[] row = { 입력.Split(':')[0].Trim(), 입력.Split(':')[1].Trim(), 입력.Split(':')[2].Trim(), 입력.Split(':')[3].Trim(), 입력.Split(':')[4].Trim(), 입력.Split(':')[5].Trim() };
                var listViewItem = new ListViewItem(row);
                Invoke(new Action(
                           delegate ()
                           {
                               listView1.Items.Add(listViewItem);
                           }));

                Console.WriteLine(입력);
            }


        }

        public ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("about:blank");
            panel1.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                browser.Load("http://maps.google.com/maps?z=7&t=m&q=loc:" + item.SubItems[4].Text.Trim() + "+" + item.SubItems[5].Text.Trim());
                axWindowsMediaPlayer1.URL = "http://cctvsec.ktict.co.kr/" + item.SubItems[3].Text + "/" + item.SubItems[2].Text;
            }

        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 1)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                제한 = Convert.ToInt32(textBox1.Text);
                Thread thread = new Thread(new ThreadStart(CCTV수신));
                thread.Start();
            }
            else
            {
                MessageBox.Show("제한값좀입력좀....");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool 있네 = false;

            List<ListViewItem> temp = new List<ListViewItem>();
            foreach (ListViewItem itemRow in this.listView1.Items)
            {
                for (int i = 0; i < itemRow.SubItems.Count; i++)
                {
                    if (itemRow.SubItems[0].ToString().Contains(textBox2.Text))
                    {
                        temp.Add(itemRow);
                        있네 = true;
                    }
                }
            }
            if (있네)
            {
                temp = temp.Distinct().ToList();
                listView1.Items.Clear();
                for (int i = 0; i != temp.Count; i++)
                {
                    listView1.Items.Add(temp[i]);
                }
            }
        }
    }
}
