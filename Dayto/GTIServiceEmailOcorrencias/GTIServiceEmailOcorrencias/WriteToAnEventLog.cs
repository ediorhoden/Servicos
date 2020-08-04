using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIServiceEmailOcorrencias
{
    class WriteToAnEventLog
    {
        public void RegistraLog(string sEvent, int nStatus)
        {
            string sSource;
            string sLog;

            sSource = "GTIServiceEmailOcorrencias";
            sLog = "GTIServiceEmailOcorrencias";


            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            switch (nStatus)
            {
                case 1:

                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.SuccessAudit, 234);

                    break;
                case 2:

                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);

                    break;

                case 3:

                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error, 234);

                    break;
            }

        }
    }
}
