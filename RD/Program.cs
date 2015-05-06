using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RD
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        [DllImport("Mwic_32.dll", EntryPoint = "ic_init", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int ic_init(Int16 poud, Int32 baud);

        [DllImport("Mwic_32.dll", EntryPoint = "ic_usbinit", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int ic_usbinit();

        [DllImport("Mwic_32.dll", EntryPoint = "ic_exit", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 ic_exit(int icdev);

        [DllImport("Mwic_32.dll", EntryPoint = "srd_ver", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 srd_ver(int icdev, int len,byte[] ver);
        //public static extern int srd_ver(int icdev, int len, byte[] ver);

        [DllImport("Mwic_32.dll", EntryPoint = "get_status", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 get_status(int icdev, ref Int16 number);

        [DllImport("Mwic_32.dll", EntryPoint = "dv_beep", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 dv_beep(int icdev, Int16 time);

        [DllImport("Mwic_32.dll", EntryPoint = "chk_4442", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 chk_4442(int icdev);

        [DllImport("Mwic_32.dll", EntryPoint = "rsct_4442", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rsct_4442(int icdev, ref Int16 counter);

        [DllImport("Mwic_32.dll", EntryPoint = "swr_4442", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 swr_4442(int icdev, Int16 offset, Int16 len, byte[] w_string);

        [DllImport("Mwic_32.dll", EntryPoint = "srd_4442", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 srd_4442(int icdev, Int16 offset, Int16 len, byte[] r_string);


        [DllImport("Mwic_32.dll", EntryPoint = "csc_4442", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 csc_4442(int icdev, Int16 len, byte[] p_string);


        [DllImport("Mwic_32.dll", EntryPoint = "wsc_4442", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 wsc_4442(int icdev, Int16 len, byte[] p_string);

        [DllImport("Mwic_32.dll", EntryPoint = "rsc_4442", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rsc_4442(int icdev, Int16 len, byte[] p_string);

        [DllImport("Mwic_32.dll", EntryPoint = "asc_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 asc_hex(byte[] asc,  byte[] hex, int length);

        [DllImport("Mwic_32.dll", EntryPoint = "hex_asc", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 hex_asc( byte[] hex,  byte[] asc, int length);

        [DllImport("Mwic_32.dll", EntryPoint = "sam_slt_reset", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 sam_slt_reset(int icdev, byte cardSet, ref Int16 len, byte[] receive_data);

        [DllImport("Mwic_32.dll", EntryPoint = "sam_slt_protocol", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 sam_slt_protocol(int icdev,byte CardType,Int16 sLen, byte[] send_cmd, ref Int16 rLen,byte[] receive_data);
    }
}
