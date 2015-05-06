using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBoxBaud.Text = comboBoxBaud.Items[0].ToString();
            comboBoxPort.Text = comboBoxPort.Items[0].ToString();
            comboBoxSetcard.Text = comboBoxSetcard.Items[0].ToString();
        }

        public int icdev;
        public Int16 st;

        private int StringHex(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > 'f')
                {
                    MessageBox.Show("Please enter 0 ~ 9, a ~ f between the data!");
                    return 1;
                }
            }
            if (str.Length != 6)
            {
                MessageBox.Show("Please input 6 data!");
                return 2;
            }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strBand = comboBoxBaud.SelectedItem.ToString();
            if (comboBoxPort.SelectedIndex == 0)
                icdev = Program.ic_usbinit();
            else
                icdev = Program.ic_init((Int16)(comboBoxPort.SelectedIndex - 1), Convert.ToInt32(strBand));
            if (icdev > 0)
            {
                byte[] ver = new byte[20];
                st = Program.srd_ver(icdev, 18, ver);
                if (st == 0)
                {
                    Program.dv_beep(icdev, 30);
                    listBox1.Items.Add("ic_init ok!");
                    listBox1.Items.Add(System.Text.Encoding.Default.GetString(ver));
                }
                else
                {
                    listBox1.Items.Add("ic_init ok!");
                    listBox1.Items.Add("srd_ver error! " + st.ToString());
                }
            }
            else
                listBox1.Items.Add("ic_init error!");
        }

        private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPort.SelectedIndex == 0)
                comboBoxBaud.Enabled = false;
            else
                comboBoxBaud.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.ic_exit(icdev);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            st = Program.chk_4442(icdev);
            if (st != 0)
                listBox1.Items.Add("please input the SLE4442 card!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Int16 count=-1;
            st=Program.rsct_4442(icdev, ref count);
            if (st == 0)
                listBox1.Items.Add("错误计数为： " + ((int)count).ToString());
            else
                listBox1.Items.Add("读错误计数失败！");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] keyHex=new byte[4];
            string wkey = textBoxKey.Text;
            if (StringHex(wkey) != 0)
                return;
            byte[] key=Encoding.Default.GetBytes(wkey);
            Program.asc_hex(key, keyHex, key.Length);
            st = Program.csc_4442(icdev, 3, keyHex);
            if (st != 0)
                listBox1.Items.Add("验证密码错误！");
            else
                listBox1.Items.Add("验证密码成功！");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] keyHex = new byte[4];
            string wkey = textBoxKey.Text;
            if (StringHex(wkey) != 0)
                return;
            byte[] key = Encoding.Default.GetBytes(wkey);
            Program.asc_hex(key, keyHex, key.Length);
            st = Program.wsc_4442(icdev, 3, keyHex);
            if (st != 0)
                listBox1.Items.Add("更改密码错误！");
            else
                listBox1.Items.Add("更改密码成功！");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            byte[] wdata = Encoding.Default.GetBytes(textBoxData.Text);
            st = Program.swr_4442(icdev, (Int16)32, (Int16)wdata.Length, wdata);
            if (st != 0)
                listBox1.Items.Add("写数据错误！");
            else
                listBox1.Items.Add("写数据成功！");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            byte[] rdata = new byte[256];
            st = Program.srd_4442(icdev, (Int16)32, (Int16)224, rdata);
            if (st != 0)
                listBox1.Items.Add("读数据错误！");
            else
                listBox1.Items.Add(Encoding.Default.GetString(rdata));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            byte[] receive_data = new byte[100];
            Int16 lenth = -1;
            st = Program.sam_slt_reset(icdev, 0, ref lenth, receive_data);
            if (st == 0)
            {
                byte[] rec_data = new byte[100];
                Program.hex_asc(receive_data, rec_data, lenth);
                listBox1.Items.Add("sam_slt_reset ok!   " + Encoding.Default.GetString(rec_data));
            }
            else
            {
                listBox1.Items.Add("sam_slt_reset error!  " + st.ToString());
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            byte[] return_data = new byte[500];
            byte[] send_cmd = new byte[255];
          
            Int16 rLen = -1;
            string cmdData;
            if (textBoxCmd.Text.Length<32)
            {
                cmdData = "00400" + Convert.ToString(textBoxCmd.Text.Length/2,16) + textBoxCmd.Text;
            } 
            else
            {
                cmdData = "0040" + Convert.ToString(textBoxCmd.Text.Length/2,16) + textBoxCmd.Text;
            }
          
            byte[] cmd = Encoding.Default.GetBytes(cmdData);
            Program.asc_hex(cmd, send_cmd, cmd.Length);

            //异或
            for (int i = 0; i < cmd.Length / 2; i++)
            {
                send_cmd[cmd.Length / 2] = (byte)(send_cmd[cmd.Length / 2] ^ send_cmd[i]);
            }

            st = Program.sam_slt_protocol(icdev, (byte)comboBoxSetcard.SelectedIndex, (Int16)(cmd.Length / 2+1), send_cmd, ref rLen, return_data);
            if (st == 0)
            {
                byte[] rec_data = new byte[100];
                Program.hex_asc(return_data, rec_data, rLen + 3);      //由于rLen是从receive_data[2]开始，CPU卡应答命令的长度，所以加2
                listBox1.Items.Add("sam_slt_protocol ok!   " + Encoding.Default.GetString(rec_data));
            }
            else
            {
                listBox1.Items.Add("sam_slt_protocol error!  " + st.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
