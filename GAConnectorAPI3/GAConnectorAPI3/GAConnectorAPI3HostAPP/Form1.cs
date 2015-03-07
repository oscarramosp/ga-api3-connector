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
        private ServiceHost host;

        private WebServiceHost hostX;

        public Form1()
        {
            InitializeComponent();
            //hostX = new WebServiceHost(typeof(GAConnectorAPI3.GAApi3Service), new Uri("http://localhost:8090/"));
            //ServiceEndpoint ep = host.AddServiceEndpoint(typeof(GAConnectorAPI3.IGAApi3Service), new WebHttpBinding(), "");
            host = new ServiceHost(typeof(GAConnectorAPI3.GAApi3Service));

            host.Open();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblMessage.Text = "Servicio iniciado";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            host = new ServiceHost(typeof(GAConnectorAPI3.GAApi3Service));
            //hostX = new WebServiceHost(typeof(GAConnectorAPI3.GAApi3Service), new Uri("http://localhost:8090/"));
            //ServiceEndpoint ep = host.AddServiceEndpoint(typeof(GAConnectorAPI3.IGAApi3Service), new WebHttpBinding(), "");
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
