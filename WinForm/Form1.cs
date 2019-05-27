using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LinqToDB.Mapping;
using System.Net.Sockets;
using NK;
using NK.Entity;
using NK.ENum; 
using NK.Communicate;
using NK.Data;
using NK.Data.Helper;
using NK.Data.Manager;
using NK.OS;
using System.Linq.Expressions;
namespace WinForm
{
   
    public partial class Form1 : Form
    {
        SocketServer ser = new SocketServer();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ss = "";
            bool sss= ss.IsNullable();
            ProtocolType ProtocolType = ProtocolType.Tcp;
            Dictionary<string, int> Protocol_Type = EnumEx.EnumToList(ProtocolType);
            ser.ResponseFlags += Ser_ResponseFlags;
            ser.ReceiveData += Ser_ReceiveData;
            ser.ResponsetData += Ser_ResponsetData;
        }

        private byte[] Ser_ResponsetData(NK.Class.CommunicateSession Session, string Flags, string SubConnection, int Step, ref byte[] Data)
        {
            return null;
        }

        private byte[] Ser_ReceiveData(NK.Class.CommunicateSession Session, string Flags, ref byte[] Data)
        {
            string aa = System.Text.Encoding.ASCII.GetString(Data);
            return null;
        }

        private byte[] Ser_ResponseFlags(NK.Class.CommunicateSession Session, ref string Flags, ref List<string> SubConnection, int Step, ref byte[] Data)
        {
            Flags = "124";
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetSet net = new NetSet();
            net.Address_Family = System.Net.Sockets.AddressFamily.InterNetwork;
            net.IPAddress = "";
            net.Port = 4001;
            net.Enable = true;
            ser.Connection = net.ToJson();
            ser.FlagCount = 1;
            ser.DataCount = 1;
            ser.InquiryTime = 3*60;
            ser.IntegralPoint = true;
            ser.Start();
         
        }
 
        private void button2_Click(object sender, EventArgs e)
        {
            DBInfo mysql = new DBInfo();
            mysql.Mode = DBType.MYSQL;
            mysql.Url = "192.168.1.83";
            mysql.Port = 0;
            mysql.DataBaseName = "NKFrame";
            mysql.User = "root";
            mysql.Password = "123";
            mysql.Enable = true;

            DBInfo mssql = new DBInfo();
            mssql.Mode = DBType.MSSQL;
            mssql.Url = "192.168.1.82";
            mssql.Port = 0;
            mssql.DataBaseName = "NKFrame";
            mssql.User = "nksa";
            mssql.Password = "123";
            mssql.Enable = true;

            DBInfo oracle = new DBInfo();
            oracle.ConfigName = "test";
            oracle.Mode = DBType.Oracle;
            oracle.Url = "192.168.1.81";
            oracle.Port = 0;
            oracle.DataBaseName = "orcl";
            oracle.User = "nksa";
            oracle.Password = "123";
            oracle.Enable = true;

            this.dbInfoUI1.Info = mssql;
 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ser.Stop();
        } 
        private void button4_Click(object sender, EventArgs e)
        {
            string ss = "";
        }

        private bool Https_postEvent(NK.Class.HttpListenerSession Session, Dictionary<string, object> UrlPram, Dictionary<string, object> PostData, string RawData)
        {
            return true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DBInfo oracle = new DBInfo();
            oracle.ConfigName = "test";
            oracle.Mode = DBType.Oracle;
            oracle.Url = "192.168.1.81";
            oracle.Port = 0;
            oracle.DataBaseName = "orcl";
            oracle.User = "nksa";
            oracle.Password = "123";
            oracle.Enable = true;
 
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DBInfo dB = new DBInfo();
            dB.Enable = true;
            dB.ConfigName = "ttt";
            dB.Mode = DBType.MongoDB;
            dB.Url = "192.168.1.7";
            dB.DataBaseName = "EBS_v2";
            long size = 0, reco = 0;
            MongoLinker db = new MongoLinker(dB);
            DateTime Sdt = new DateTime(2018,11,28,0,0,0, DateTimeKind.Utc);
            DateTime edt = new DateTime(2018, 12, 1,23,59,59, DateTimeKind.Utc);
            var sws = db.History<ProductDailyReport>(Sdt, edt, 1, 10, out size, out reco);
        }
    }
}
