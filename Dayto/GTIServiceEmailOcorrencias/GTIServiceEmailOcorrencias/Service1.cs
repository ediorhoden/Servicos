using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer;
        private const int minuto = 10000;
        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "GTIServiceEmailOcorrencias";
        }

        protected override void OnStart(string[] args)
        {
            this.timer = new System.Timers.Timer(minuto * 5);  // 5 minutos 
            //this.timer = new System.Timers.Timer(minuto * 1);  // 1 minutos 
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();

            //arquivoLog = new StreamWriter(clEnvioEmail._dirLog + "GTIServiceEmailLog.txt", true);   //Escrevo no arquivo texto no momento que o arquivo for iniciado   
            //arquivoLog.WriteLine("Serviço iniciado em: " + DateTime.Now);   //Limpo o buffer com o método Flush   
            //arquivoLog.Flush();
        }

        protected override void OnStop()
        {
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteToAnEventLog log = new WriteToAnEventLog();
            this.timer.Stop();
            //clEnvioEmail.processaEmail();
            try
            {
                clEnvioEmailConfirmacaoEntradaEntrega.processaEmail();
            }
            catch (Exception ex)
            {


                log.RegistraLog("Erro" + ex, 3);

            }
            finally
            {

                System.ServiceProcess.ServiceBase.Run(new Service1());
                this.timer.Start();
            }
        }
    }
}
