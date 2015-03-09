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

        public Form1()
        {
            InitializeComponent();
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


    }
}
