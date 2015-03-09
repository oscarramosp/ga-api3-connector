using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace GAConnectorAPI3HostAPP
{
    public partial class Form1 : Form
    {
        private WebServiceHost host;

        //http://localhost:8080/GAApi3Service/ExtractData?Profile=90392707&Dimensions=ga:date&Metrics=ga:sessions&StartDate=2015-01-01&EndDate=2015-01-01&extraCols=1&extraData=1
        //http://localhost:8080/GAApi3Service/ExtractDataRaw?Profile=90392707&Dimensions=ga:date&Metrics=ga:sessions&StartDate=2015-01-01&EndDate=2015-01-01&extraCols=1&extraData=1
        //ga:pagePath

        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            host = new WebServiceHost(typeof(GAConnectorAPI3.GAApi3Service));

            host.Open();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblMessage.Text = "Servicio iniciado";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            host = new WebServiceHost(typeof(GAConnectorAPI3.GAApi3Service));
            host.Open();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblMessage.Text = "Servicio iniciado";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            host.Close();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblMessage.Text = "Servicio detenido";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            host.Close();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            ntfIconMinim.BalloonTipTitle = "Host GAApi3Connector";
            ntfIconMinim.BalloonTipText = "Para abrir la aplicación realizar dobleclick sobre el icono";
            ntfIconMinim.BalloonTipIcon = ToolTipIcon.Info;
            if (FormWindowState.Minimized == this.WindowState)
            {
                ntfIconMinim.Visible = true;
                ntfIconMinim.ShowBalloonTip(2000);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                ntfIconMinim.Visible = false;
            }
        }

        private void ntfIconMinim_Click(object sender, MouseEventArgs e)
        {
            this.Show();
            ntfIconMinim.Visible = false;
        }


    }
}
