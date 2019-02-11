using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LitmusApp
{
    public partial class frmMain : Form
    {
        #region Properties

        private Timer emailTimer = null;
        private string folderPath = @"C:\GmailImages";
        private int documentCounter = 0;

        #endregion

        #region Constructor
        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region Initialization

        private void InitializeControls()
        {
            wbMail.Navigate(@"https://mail.google.com");
        }

        public void InitializeTimer()
        {
            emailTimer = new Timer();
            emailTimer.Tick += EmailTimer_Tick;
            emailTimer.Interval += 30000;
            emailTimer.Start();
        }

        private void EmailTimer_Tick(object sender, EventArgs e)
        {
            if (wbMail.ReadyState == WebBrowserReadyState.Complete && !wbMail.IsBusy)
            {
                emailTimer.Stop();
                emailTimer = null;
            }
        }

        #endregion

        #region EventHandlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }
       
        private void WbMail_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (wbMail.ReadyState == WebBrowserReadyState.Complete && !wbMail.IsBusy)
             {
                documentCounter++;

                if (documentCounter == 15)
                {
                    PrintScreen();
                    tsbPrint.Enabled = true;
                }
                

            }
            
        }
                
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            PrintScreen();
        }

        #endregion

        #region Private Methods
        private void PrintScreen()
        {
            using (Bitmap bmp = new Bitmap(wbMail.Width, wbMail.Height))
            {
                Image img = wbMail.DrawToImage();

                string filePath = GetFilePath();

                img.Save(filePath);
            }
        }

        private string GetFilePath()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + DateTime.Now.ToString("HH-mm-ss") + ".png";

            string filePath = Path.Combine(folderPath, fileName);

            return filePath;
        }

        #endregion


    }
}
