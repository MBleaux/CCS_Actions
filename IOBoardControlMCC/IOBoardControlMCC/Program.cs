using MccDaq;
using System;
using ErrorDefs;
using System.ComponentModel.DataAnnotations;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Runtime.ConstrainedExecution;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;
using System.Security.Policy;
using Microsoft.VisualBasic.Logging;
using static System.Windows.Forms.Design.AxImporter;

//NOTE - Copie du projet IOBoardControlMCC - Cópia do projeto IOBoardControlMCC

namespace CCS_Actions
{
    public class CCS_Actions
    {

        //SECTION - Commeçant le fichier CCS_Actions a partir de cette section - Começando o arquivo CCS_Actions a partir desta seção
        //NOTE - Declaration écrit dans le fichier CCS_Action.h - Declaração escrita no arquivo CCS_Action.h
        public const int CODE_OK = 0;
        public const int CODE_ERROR = -1;
        public const int ALREADYCONNECT_ERROR = -2;
        public const int CONNECT_ERROR = -3;
        public const int CARTE_ERROR = -4;
        public const int NBR_PAS_ENVOI_CODEUR = 40;
        public const int T_STATE0_CODEUR = 10;
        public const int T_STATE1_CODEUR = 7;

        // Define Max Min and number of point
        public const int MAXLED = 100;
        public const int MINLED = 0;

        //Others define
        public const string ACTIONS_TITLE = "Actions Class Message";
        public const string MCHR_TITLE = "MCHR DLL Message";

        //Acquisition buffer
        public const int ACQ_BUFFERLENTH = 32768;
        public const int ACQ_NBRBUFFER = 1;
        public const int ACQ_UPTRGRATE = 65500;
        public const int ACQ_DOWNTRGRATE = 10;
        public const int MAX_CHAR_CMD = 100;
        public const int MAX_CHAR_PATH = 256;
        public const int LISSAGE_LENGTH = 20;

        //Do not change the order of data
        public const int EVENT_KILL_THREAD = 0;
        public const int EVENT_KILL_THREAD_DO = 1;
        public const int EVENT_NUMBER = 2;
        public const int STANDARD_TIMEOUT = 5000;
        public const int READ_COUNTER_DATA_TIMEOUT = 20000;
        public const int READ_ENCODER_DATA_TIMEOUT = 200;
        public const int HZ2MS = 1000;
        public const int ZENITH_ENCODER_COUNT = 5;
        public const int STRING_DEFAULT_LENGTH = 100;

        //Connection Type enumeration
        public enum enTypeLink
        {
            USB,
            SERIAL_232,
            SERIAL_422,
            ETHERNET,
            NOT_CONNECTED
        };
        
        public enum enTypeSignalTest
        {
            DARK,
            WREF,
            LED,
            EXPO,
            NBR_TEST_SIGNAL
        };

        public enum enRatio
        {
            ZERO,
            DIX = 10,
            CINQUANTE = 50,
            CENT = 100,
            NBR_RATIO
        };

        public enum enControllerFamily
        {
            CCS,
            ZENITH
        };

        //NOTE - Il faut chercher comme traduire les commandes suivantes - Até aqui funciona ou aparenta funcionar :| em seguida, tem q pesquisar pra ver se funciona mesmo

        // Add structure for exchange only raw buffer, conversion to any type of structure depends of operationCode

        //FIXME - Problemas com os tipos utilizados
        typedef struct readDataParam
        {
            enOperationCode operationcode;
            HostConfig hostcfgrecv;
            char rawData[BUF_SIZE];
        } readDataParam_t;

        //FIXME - Problemas com os tipos utilizados
        //private parameters
        struct stParameters
        {
            //Private variable value
            public int m_DeviceConnected;//Device connection state
            public int m_DeviceDetected;//Detected device
            public CString m_DeviceType;//Detected device type
            public BOOL m_bBenchStatus;//Bench connection state
            public enTypeLink m_entlLinkStatus;//Link type
            public stValidateParameters m_bParametersStatus;//Bench connection state
            public int m_bCmdsStatus;//Bench connection state
            public enChrType m_iCcsType;//Ccs Type
            public int m_iNbrOfPixel;//number of pixel
            public int m_wScanRateSync;
            public int m_wScanRateSign;
            public int m_wScanRateMeas;

            public float m_fCoefficientTension;
            public float m_fCoefficientCourant;
        };

        //FIXME - Verifica se funciona
        public struct listenparam_t
        {
            public char[] ifaddr;
            public int timeoutMilliseconds;
            public short shDeviceNumber;
        }

        //FIXME - Problemas com os tipos
        public struct threadparam_t
        {
            public HANDLE hThread;
            public DWORD m_dwThreadId;
            public HANDLE m_OnEvent[EVENT_NUMBER];
            public CCriticalSection critical;
            public LPVOID lpParameter;
        }

        // KL_24-08-2020 fill path from BP to save by FTP
        enum enFtpPath { _DEFAULT_PATH, _BDD_PATH, _LOG_PATH, _LOGS_PATH, _CRASH_DUMP_PATH, MAX_NBR_PATH };

        //FIXME - Problemas com o tipo PTCHAR
        public struct stFtpPath
        {
            public PTCHAR pzName;
            public PTCHAR pzPath;
            public PTCHAR pzPathBP;
            public PTCHAR pzDiagDirName;
        }

        extern stFtpPath tyFtpPath[MAX_NBR_PATH];
        //FIXME - Problemas com ponteiros
        public struct tagDIAG_GROUP_FILES_ZENITH
        {
            public CString groupName;
            public int numberOfFiles;
            //long *filesSize;
            //CString *pszFilePath;
            public PMCHR_FILE_DATA pInfoFile;
        }DIAG_GROUP_FILES_ZENITH, * PDIAG_GROUP_FILES_ZENITH;

        //FIXME - Problemas com ponteiros
        public struct tagASYNCDIAGNOCTIC_ZENITH
        {
            //CDMyHtmlDialog* pInstance;
            public LPVOID pInstance;
            public int numberOfGroup;
            public DIAG_GROUP_FILES_ZENITH groupFiles[MAX_NBR_PATH];
            //HWND m_hWnd;
        } ASYNCDIAGNOSTIC_ZENITH, * PASYNCDIAGNOSTIC_ZENITH;
        
        //!SECTION

        public CCS_Actions() { }
        
   }
}