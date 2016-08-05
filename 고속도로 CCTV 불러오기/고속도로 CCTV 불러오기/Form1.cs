using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 고속도로_CCTV_불러오기
{
    public partial class Form1 : Form
    {
        int 제한 = 0;
        public Form1()
        {
            InitializeComponent();

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
                string findThisString = "<h1>";
                int strNumber;
                int strIndex = 0;
                for (strNumber = 0; strNumber < test.Length; strNumber++)
                {
                    strIndex = test[strNumber].IndexOf(findThisString);
                    if (strIndex >= 0)
                        break;
                }
                if (strNumber != 18 && test[strNumber] != "\t\t<h1><em class=\"_sectionName\"></em></h1>")
                {
                    string 고속도로이름 = test[strNumber].Remove(test[strNumber].IndexOf("<em"));
                    고속도로이름 = 고속도로이름.Remove(0, 고속도로이름.IndexOf(">") + 1);
                    string 고속도로구간 = test[strNumber].Remove(0, test[strNumber].IndexOf('\u0022' + ">"));
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
                                string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[170].Remove(0, test[170].IndexOf(":") + 3).Remove(64) + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(test[171].Remove(0, test[171].IndexOf(":") + 3).IndexOf(",") - 1);
                                수신된CCTV.Add(파싱값);
                                Console.WriteLine(파싱값);

                            }
                            //else if (test[171].Remove(0, test[170].IndexOf(":") + 3).Remove(64).Equals(@"encryptedString\"))
                            //{
                            //    string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[172].Remove(0, test[172].IndexOf(":") + 3).Remove(64) + " : " + test[173].Remove(0, test[173].IndexOf(":") + 3).Remove(test[173].Remove(0, test[173].IndexOf(":") + 3).IndexOf(",") - 1);
                            //    수신된CCTV.Add(파싱값);
                            //    Console.WriteLine(파싱값);
                            //}
                            else
                            {
                                string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(64) + " : " + test[172].Remove(0, test[172].IndexOf(":") + 3).Remove(test[172].Remove(0, test[172].IndexOf(":") + 3).IndexOf(",") - 1);
                                수신된CCTV.Add(파싱값);
                                Console.WriteLine(파싱값);
                            }
                        }

                    }
                    else
                    {

                        if (test[170] != "\tvar cctv = {")
                        {
                            string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[170].Remove(0, test[170].IndexOf(":") + 3).Remove(64) + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(test[171].Remove(0, test[171].IndexOf(":") + 3).IndexOf(",") - 1);
                            수신된CCTV.Add(파싱값);
                            Console.WriteLine(파싱값);

                        }
                        //else if (test[171].Remove(0, test[170].IndexOf(":") + 3).Contains("encryptedString"))
                        //{
                        //    string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[171].Remove(0, test[170].IndexOf(":") + 3).Remove(64) + " : " + test[172].Remove(0, test[172].IndexOf(":") + 3).Remove(test[172].Remove(0, test[172].IndexOf(":") + 3).IndexOf(",") - 1);
                        //    수신된CCTV.Add(파싱값);
                        //    Console.WriteLine(파싱값);
                        //}
                        else
                        {
                            string 파싱값 = 고속도로이름 + " : " + 고속도로구간 + " : " + test[171].Remove(0, test[171].IndexOf(":") + 3).Remove(64) + " : " + test[172].Remove(0, test[172].IndexOf(":") + 3).Remove(test[172].Remove(0, test[172].IndexOf(":") + 3).IndexOf(",") - 1);
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
                string[] row = { 입력.Split(':')[0].Trim(), 입력.Split(':')[1].Trim(), 입력.Split(':')[2].Trim(), 입력.Split(':')[3].Trim() };
                var listViewItem = new ListViewItem(row);
                Invoke(new Action(
                           delegate ()
                           {
                               listView1.Items.Add(listViewItem);
                           }));

                Console.WriteLine(입력);
            }


        }

       
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                axWindowsMediaPlayer1.URL = "http://cctvsec.ktict.co.kr/"+ item.SubItems[3].Text + "/"+ item.SubItems[2].Text;
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
    }
    
}
