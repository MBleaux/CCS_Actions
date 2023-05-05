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
using System.Reflection.Metadata;

//NOTE - Copie du projet IOBoardControlMCC - Cópia do projeto IOBoardControlMCC

namespace CCS_Actions
{
    public class CCS_Actions
    {
        /*NOTE - Partie publique*/

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

        // Add structure for exchange only raw buffer, conversion to any type of structure depends of operationCode

        //FIXME - Il faut reviser ces types de variables
        struct readDataParam_t
        {
            enOperationCode operationcode;
            HostConfig hostcfgrecv;
            char rawData[BUF_SIZE];
        };

        //private parameters

        //FIXME - Il faut reviser ces types de variables: CString (string) , BOOL (bool) stValidateParameters , enChrType
        struct stParameters
        {
            //Private variable value
            public int m_DeviceConnected;//Device connection state
            public int m_DeviceDetected;//Detected device
            public string m_DeviceType;//Detected device type
            public bool m_bBenchStatus;//Bench connection state
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

        //FIXME - Il faut tester si ce façon marche bien
        public struct listenparam_t
        {
            public char[] ifaddr;
            public int timeoutMilliseconds;
            public short shDeviceNumber;
        }

        //FIXME - Il faut reviser ces types de variables: HANDLE , DWORD (long) , CCriticalSection , LPVOID
        public struct threadparam_t
        {
            public HANDLE hThread;
            public long m_dwThreadId;
            public HANDLE m_OnEvent[EVENT_NUMBER];
            public CCriticalSection critical;
            public LPVOID lpParameter;
        }

        // KL_24-08-2020 fill path from BP to save by FTP
        enum enFtpPath { _DEFAULT_PATH, _BDD_PATH, _LOG_PATH, _LOGS_PATH, _CRASH_DUMP_PATH, MAX_NBR_PATH };

        //FIXME - Il faut reviser ce type de variable: PTCHAR
        public struct stFtpPath
        {
            public PTCHAR pzName;
            public PTCHAR pzPath;
            public PTCHAR pzPathBP;
            public PTCHAR pzDiagDirName;
        }

        extern stFtpPath tyFtpPath[MAX_NBR_PATH];
        //FIXME - Problème avec les pointeurs
        public struct tagDIAG_GROUP_FILES_ZENITH
        {
            public string groupName;
            public int numberOfFiles;
            //long *filesSize;
            //CString *pszFilePath;
            public PMCHR_FILE_DATA pInfoFile;
        }DIAG_GROUP_FILES_ZENITH, * PDIAG_GROUP_FILES_ZENITH;

        //FIXME - Problème avec les pointeurs
        public struct tagASYNCDIAGNOCTIC_ZENITH
        {
            //CDMyHtmlDialog* pInstance;
            public LPVOID pInstance;
            public int numberOfGroup;
            public DIAG_GROUP_FILES_ZENITH groupFiles[MAX_NBR_PATH];
            //HWND m_hWnd;
        } ASYNCDIAGNOSTIC_ZENITH, * PASYNCDIAGNOSTIC_ZENITH;

        /* Partie privée */

        /* FIXME - Faire les corrections sur les types:
         * MCHR_ID, MCHR_tyAcqParam, enDigitalOutputChannel, WORD (short), HZIP, TCHAR, CString, enIOBoardControl, CStringArray*
         * stCommand*, CEdit*, HANDLE, BOOL (bool), stReportInfo, stAppInfo
         */

        /****************************
	    definition des variables
	    *****************************/
	    MCHR_ID m_iID; // ID of the sensor
        MCHR_tyAcqParam m_stAcqParam; // handle acquisition
        enDigitalOutputChannel m_enOutputChannel[16];//enum configuration
        short m_wScanRate;//current scanrate
        long m_lAcquisitionFrequency;//current FreeFrequency
        short m_wAverage;//current Average
        int m_iONP;
        int m_iOFP;
        int m_iMNW;
        int m_iMXW;
        short m_wMaxDarkRedThreshold;
        short m_wMaxDarkBlueThreshold;
        short m_wMaxDarkThreshold;
        short m_wOffset;
        HZIP m_hz;
        long m_operatingTimeCounter;

        //Database for parameters
        stParameters m_stStatuParam;

        //Current Power of led
        float m_fPuissance;//Puissance value
                           //Current file
        TCHAR m_cstrFilePath[MAX_CHAR_PATH];//file path
        string m_cstrAppDir;//file path
        string m_cstrAppFilePath;//file path
        string m_cstrDiagnosticFilePath;//file path
        string m_cstrReportDir;//file path
        string m_cstrReportFolder;
        string m_cstrReportFileName;//file path
        int m_iSelected_ethernet_card;
        enIOBoardControl m_enIOBoardControlType;
        //Report declaration
        CStringArray* m_pInfoReport;//info report
        stCommand* m_pCmdReport;//command report
        CEdit* m_pCEdit;//Current pointer on CEdit
        int CCCSA_GetParameters();//return parameters
        HANDLE m_DisplaySignalEvent;
        HANDLE m_AvailableSignalEvent;
        HANDLE m_DiagnosticDoneSignalEvent;

        //Parameters
        stReportInfo m_stInfoParam;
        stAppInfo m_stAppParam;

        //Threads
        string m_cstrCcsStatus;//CCS statu
        bool m_bAlwaysTRUE;
        string m_cstrBenchStatus;//bench statu
        string m_cstrPowerStatus;//bench statu
        string m_csSensorName;
        string m_szFirmwareVersion;
        string m_szVersion;

        /**************
        Functions
        **************/
        //String and message
        //int CCCSA_Box(CString cstrMessErr,CString cstrTitleErr,UINT uintType,HWND hWnd/*=NULL*/,short shError);
        int CCCSA_ReadFile(CString cstrPath, float* pDataBuff)
        {
            char MessDebug[500];
            //Declaration variable
            CStdioFile cfileFile;
            char lpBuff[MAX_CHAR_CMD];
            float* fDataBuff = new float[m_stStatuParam.m_iNbrOfPixel];

            //init variables
            ZeroMemory(lpBuff, sizeof(char) * MAX_CHAR_CMD);
            ZeroMemory(fDataBuff, sizeof(float) * m_stStatuParam.m_iNbrOfPixel);
            int iFileLength = 0, iPos = 0;

            sprintf(MessDebug, "Nom Fic : %s, Nb Pixel = %d\n\r", cstrPath, m_stStatuParam.m_iNbrOfPixel);
            OutputDebugString(MessDebug);
            if (cfileFile.Open(cstrPath, CFile::modeRead) == 0)
            {//validate path file
                AfxMessageBox(CCCSA_CStringFormat(_T("Probl�me d'ouverture fichier"), cstrPath));
                return CODE_ERROR;
            }
            else
            {
                //read first data of file to have length of data
                if (cfileFile.ReadString(lpBuff, MAX_CHAR_CMD))
                {
                    iFileLength = int(atoi(lpBuff));
                    if (iFileLength != m_stStatuParam.m_iNbrOfPixel)
                    {
                        AfxMessageBox(CCCSA_CStringFormat(_T("Les donn�es sont erron�es "), cstrPath));
                        return CODE_ERROR;
                    }
                    else
                    {
                        OutputDebugString("Lecture file :\n\r");
                        while (cfileFile.ReadString(lpBuff, MAX_CHAR_CMD) && iPos < m_stStatuParam.m_iNbrOfPixel)
                        {
                            fDataBuff[iPos] = float(atof(lpBuff));
                            iPos++;

                            sprintf(MessDebug, "%d,", iPos);
                            OutputDebugString(MessDebug);
                            OutputDebugString("\n\r");
                        }
                    }
                    cfileFile.Close();
                    memcpy(pDataBuff, fDataBuff, iFileLength * sizeof(float));
                }
                else
                {
                    AfxMessageBox(CCCSA_CStringFormat(_T("Pas de d�finition du nombre de donn�e"), cstrPath));
                    return CODE_ERROR;
                }
            }
            SAFE_DELETEA(fDataBuff);
            return CODE_OK;
        }

        int CCCSA_WriteFile(CString cstrPath, WORD* pDataBuff, int size, CString* csmsg = NULL);
        int CCCSA_InitParameters();//Init Parameters

        public CCS_Actions() { }
        
   }
}