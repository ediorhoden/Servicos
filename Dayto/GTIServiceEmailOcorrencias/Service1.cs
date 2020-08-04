using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GTIServiceEmailOcorrencias
{
    public partial class Serviço_Day : ServiceBase
    {
        public Serviço_Day()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer timer = new Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            clEnvioEmailConfirmacaoEntradaEntrega.processaEmail();
            

        }

        protected override void OnStop()
        {
        }

        

       

    }
}
