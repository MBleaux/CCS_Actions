using MccDaq;
using System;
using System.Diagnostics;
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
        /* J'ai trouvé ce struct en rapport.h de C++ */
        //structure of command
        struct stCommand
        {
            string cstrCmd;//Command
            bool bRtnValid;
        };

        struct stAppInfo
        {
            //NIC index
            int m_iSelected_ethernet_card;
        };

        //Structure de param�tres
        struct stReportInfo
        {
            //File path
            string m_FilePath;
            string m_Ref1Path;
            string m_Ref2Path;

            int m_iTypeLED; // 0 : LED "normale", 1 : LED CREE

            float m_fAlpha;
            float m_fBeta;
            float m_fGamma;

            int m_iBluePixelsDarkMin;
            int m_iBluePixelsDarkMax;
            int m_iRedPixelsDarkMin;
            int m_iRedPixelsDarkMax;

            //Number of operations
            int m_iNbrOp;

            //Board slot
            int m_iSlot;

            //Anlog parameters
            float m_AnalogRateMax;
            float m_AnalogRateMin;

            //Nbr pas codeurs
            int m_CoderStep;
            
            //Dark and whiteref Rate
            int m_SeuilMaxDark;
            int m_SeuilDriftMaxDark;
            int m_SeuilMinWhiteRef;
            
            //Led rate
            int m_SeuilMax100Led;
            int m_SeuilMin100Led;
            float m_SeuilMax50Led;
            float m_SeuilMin50Led;
            int m_SeuilMax0Led;
            int m_SeuilMin0Led;
            float m_fType1_RateFactor;
            /*int m_SeuilMax100Arc;
            int m_SeuilMin100Arc;
            int m_SeuilMax0Arc;
            int m_SeuilMin0Arc;*/

            //6 parameters for RVB intensity
            int m_SeuilMaxRLed;
            int m_SeuilMinRLed;
            int m_SeuilMaxGLed;
            int m_SeuilMinGLed;
            int m_SeuilMaxBLed;
            int m_SeuilMinBLed;

            //6 parameters for centering
            int m_CenterMaxRLed;
            int m_CenterMinRLed;
            int m_CenterMaxGLed;
            int m_CenterMinGLed;
            int m_CenterMaxBLed;
            int m_CenterMinBLed;
            float m_IntensityRate;
            //RS Baud rate
            int m_BaudRate;
            int m_RsChannel;

            //Number of command
            int m_CmdNbr;
            //Lissage number
            int m_LissageNbr;

            //Spectrum level verification (with whiteref)
            int[] m_IntensityCheckPixPos = new int[3];
            int[] m_LevelMinIntensity = new int[3];
            int[] m_LevelMaxIntensity = new int[3];

            int m_iNbSwaps;
            float m_fMaxAverageSwapTimeAccepted;
            float m_fMaxSwapTimeAccepted;

            int m_iTriggerOutRateMin;
            int m_iTriggerOutRateMax;
            int m_iTriggerMasterRateMin;
            int m_iTriggerMasterRateMax;
            int m_iEncoderMin;
            int m_iEncoderMax;
            float m_fTensionMin;
            float m_fTensionMax;
            float m_fCourantMin;
            float m_fCourantMax;
            int m_iOffsetMax;
            int m_iOffsetMin;
            int m_iWrefMax;
            int m_iWrefMin;
            int m_SeuilMaxWhiteRef;
            float m_SeuilMax10Led;
            float m_SeuilMin10Led;
            float m_TExpo50Min;
            float m_TExpo50Max;
            float m_TExpo10Min;
            float m_TExpo10Max;
            int m_iSelectionCodeur;
        };

        //private parameters
        struct stValidateParameters
        {
            //Private variable value
            int m_HtmlFile;
            int m_Ref1File;
            int m_Ref2File;
            int m_iTypeLED; // 0 : LED "normale", 1 : LED CREE
            int m_SlotNbr;
            int m_Alpha;
            int m_Beta;
            int m_Gamma;
            int m_Ledop;
            int m_Bauds;
            int m_RSC;
            int m_LedRMax100;
            int m_LedRMin100;
            int m_LedRMax50;
            int m_LedRMin50;
            int m_LedRMax0;
            int m_LedRMin0;
            int m_fType1_RateFactor;
            int m_AnalogMax;
            int m_AnalogMin;
            int m_RLedMax;
            int m_RLedMin;
            int m_GLedMax;
            int m_GLedMin;
            int m_BLedMax;
            int m_BLedMin;
            int m_Pixel;
            int m_IntensityRate;
            int m_LissageNbr;

            /*
            //Spectrum level verification (with whiteref)
            int m_IntensityCheckPixPos[3];
            int m_LevelMinIntensity[3];
            int m_LevelMaxIntensity[3];
            */

            // Spectrum level verification (with whiteref)
            int[] m_IntensityCheckPixPos = new int[3];
            int[] m_LevelMinIntensity = new int[3];
            int[] m_LevelMaxIntensity = new int[3];

            // Swap time parameters for prima multi-channel
            int m_iNbSwaps;
            int m_fMaxAverageSwapTimeAccepted;
            int m_fMaxSwapTimeAccepted;

            int m_SeuilMaxDark;
            int m_SeuilDriftMaxDark;
            int m_iBluePixelsDarkMin;
            int m_iBluePixelsDarkMax;
            int m_iRedPixelsDarkMin;
            int m_iRedPixelsDarkMax;

            int m_wScanRateSync;
            int m_wScanRateSign;
            int m_wScanRateMeas;

            int m_iCoefficientTension;
            int m_iCoefficientCourant;

            int m_iTriggerOutRateMin;
            int m_iTriggerOutRateMax;
            int m_iTriggerMasterRateMin;
            int m_iTriggerMasterRateMax;
            int m_iEncoderMin;
            int m_iEncoderMax;
            int m_fTensionMin;
            int m_fTensionMax;
            int m_fCourantMin;
            int m_fCourantMax;
            int m_iOffsetMax;
            int m_iOffsetMin;
            int m_iWrefMax;
            int m_iWrefMin;
            int m_SeuilMinWhiteRef;
            int m_SeuilMaxWhiteRef;
            int m_LedRMax10;
            int m_LedRMin10;
            int m_TExpo50Min;
            int m_TExpo50Max;
            int m_TExpo10Min;
            int m_TExpo10Max;
            int m_iSelectionCodeur;
        };

        /* J'ai trouvé ce struct en MchrType.h de C++ */
        enum enChrType
        {
            MCHR_150,
            MCHR_450,
            MCHR_XE,
            MCHR_XE_E,
            MCHR_CCS_ALPHA,
            MCHR_CCS_PRIMA,
            MCHR_CCS_OPTIMA,
            MCHR_CCS_OPTIMA_PLUS,
            MCHR_CCS_ULTIMA,
            MCHR_STIL_VIZIR,
            MCHR_CCS_INITIAL,
            MCHR_ZENITH,
            MCHR_ZENITH_5K,
            MCHR_DUO,
            MCHR_TRIO,
            MCHR_RUBY,
            MCHR_SPS_ALPHA_1,
            MCHR_CCS_EXTREMA,
            MCHR_MAX_MODEL
        };

        /* Fichier CCS_Actions.h */
        /*NOTE - Partie publique*/
        public const int MCHR_ERROR_NONE = 1;
        public const int NoError = 0;
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
            char[] rawData = new char[1024];
        };

        //private parameters

        //FIXME - Il faut reviser ces types de variables: CString (string) , BOOL (bool), stValidateParameters (il est defini en haut) , enChrType (il est defini en haut)
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

        //FIXME - Il faut reviser ce type de variable: PTCHAR -> string
        public struct stFtpPath
        {
            public string pzName;
            public string pzPath;
            public string pzPathBP;
            public string pzDiagDirName;
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

        /****************************
        definition des variables
        *****************************/
        public ushort[] m_pAcqSignal1 = new ushort[2048];//Spectrum 1
        public ushort[] m_pAcqSignal2 = new ushort[2048];//Spectrum 2
        public float[] m_pAcqSig1 = new ushort[2048];//Spectrum 1
        public float[] m_pAcqSig2 = new ushort[2048];//Spectrum 2
        public float[] m_pAcqRef1 = new ushort[2048];//Spectrum 1
        public float[] m_pAcqRef2 = new ushort[2048];//Spectrum 2
        public int m_NbrSpectrum;//Spectrums number
        public CCriticalSection m_ccsStop;//critical variable
        // Section critique pour l'acc�s des information de status dans le thread d'acquisition des signaux
        public CCriticalSection m_SectionCritiqueInfoStatus;
        // Section critique pour l'acc�s des information de status dans le thread d'acquisition des signaux
        public CCriticalSection m_SectionCritiqueConsole;
        public float[,] m_BarySig1 = new float[20, 2];//baycenter
        public float[,] m_BarySig2 = new float[20, 2];
        public float m_MaxSig1;//Maximum
        public float m_MaxSig1DarkBlue;//Maximum
        public float m_MaxSig1DarkRed;//Maximum
        public float m_MaxSig2;
        public bool m_OffsetEn;//sub offset
        public int m_iErrEndOfWRWindow;//state of end of internal window for WR
        public listenparam_t m_listenparam;
        public NetworkDeviceConfig m_MulticastParam;

        public threadparam_t m_thMulticast;//Thread Multicast of status
        public threadparam_t m_thTrigger;
        public threadparam_t m_thAcqSpectre;//Thread acquisition of statu
        public threadparam_t m_thDiagnostic;

        //IOBoard controller
        public CIOBoardControl* m_pCIOB;//IO board control
        public char m_cSelectionCodeur;

        /**************
        Functions
        **************/
        //Constructor
        CCCS_Actions(void);//create constructor

        //String format
        public static string CCCSA_CStringFormat(string cstrMessage, string cstrValue)
        {
            Console.WriteLine("Print Message: " + cstrMessage + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }

        public static string CCCSA_CStringFormat(string cstrMessage, int iValue)
        {
            string cstrValue = iValue.ToString();
            Console.WriteLine("Print Message: " + cstrMessage + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }

        public static string CCCSA_CStringFormat(string cstrMessage, uint iValue)
        {
            string cstrValue = iValue.ToString();
            Console.WriteLine("Print Message: " + cstrMessage + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }

        public static string CCCSA_CStringFormat(string cstrMessage)
        {
            Console.WriteLine("Print Message: " + cstrMessage + "\r\n");
            return cstrMessage;
        }

        public static string CCCSA_CStringFormat(string cstrMessage, float fValue)
        {
            string cstrValue = fValue.ToString();
            Console.WriteLine("Print Message: " + cstrMessage + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }

        public static string CCCSA_CStringFormat(string cstrMessage, float fMinValue, float fMaxValue)
        {
            string cstrValue;
            if (fMaxValue == -1.0f)
            {
                cstrValue = " (a partir de " + fMinValue.ToString("F1") + ") Valeur ";
            }
            else
            {
                cstrValue = " (de " + fMinValue.ToString("F1") + " à " + fMaxValue.ToString("F1") + ") Valeur ";
            }
            Console.WriteLine("Print Message: " + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }

        public static string CCCSA_CStringFormat(string cstrMessage, int fMinValue, int fMaxValue)
        {
            string cstrValue;
            if (fMaxValue == -1)
            {
                cstrValue = " (a partir de " + fMinValue.ToString() + ") Valeur ";
            }
            else
            {
                cstrValue = " (de " + fMinValue.ToString() + " à " + fMaxValue.ToString() + ") Valeur ";
            }
            Console.WriteLine("Print Message: " + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }

        public static string CCCSA_CStringFormat(string cstrMessage, float fValue, float fMinValue, float fMaxValue, string unites)
        {
            string cstrValue;
            if (fMaxValue == -1.0f)
            {
                cstrValue = " (a partir de " + fMinValue.ToString("F1") + ") " + unites + " ";
            }
            else
            {
                cstrValue = " " + fValue.ToString("F1") + " (de " + fMinValue.ToString("F1") + " à " + fMaxValue.ToString("F1") + ") " + unites + " ";
            }
            Console.WriteLine("Print Message: " + cstrValue + "\r\n");
            return cstrMessage + cstrValue;
        }
        
        //Function on report
        //FIXME - Il faut regarder .Add()
        public void CCCSA_AddStringToReport(string message)
        {
            m_pInfoReport.Add(message);
        }

        // Function to set report data
        public void CCCSA_SetReport(stCommand stCReport, List<string> cstrIReport)
        {
            Console.WriteLine("Set report");
            m_pCmdReport = stCReport;
            m_pInfoReport = cstrIReport;
        }

        public void CCCSA_GetReport(out stCommand stCReport, out List<string> cstrIReport)
        {
            Console.WriteLine("Get report");
            stCReport = m_pCmdReport;
            cstrIReport = m_pInfoReport;
        }

        //Return console
        //FIXME - Il faut regarder ce qu'il serait CEdit et ses derivations
        public void CCCSA_SetConsole(CEdit pCEdit)
        {
            m_pCEdit = pCEdit;
        }

        public void CCCSA_SetConsole(string cstrMessErr, bool bClr)
        {
            string cstrMessage;
            char[] bWindowText = new char[10000];

            Array.Clear(bWindowText, 0, 10000);

            if (m_pCEdit != null)
            {
                m_SectionCritiqueConsole.Lock();
                Console.WriteLine("Lock m_SectionCritiqueConsole set BOOLEAN in console");

                if (!bClr)
                {
                    m_pCEdit.GetWindowTextA(bWindowText, 10000);
                    cstrMessage = new string(bWindowText) + cstrMessErr + "\r\n";
                }
                else
                {
                    cstrMessage = cstrMessErr + "\r\n";
                }

                m_pCEdit.SetWindowTextA(cstrMessage);
                m_pCEdit.LineScroll(m_pCEdit.GetLineCount());
                m_pCEdit.UpdateWindow();
                m_SectionCritiqueConsole.Unlock();
                Console.WriteLine("Unlock m_SectionCritiqueConsole set BOOLEAN in console");
            }
        }

        //FIXME - Il faut regarder les valeurs qu'on vient de MchrError.h du projet originale
        public int CCCSA_SetConsole(string cstrMessErr, short shError)
        {
            short ushTypeError = MCHR_ERROR_NONE;
            string cstrMessage, cstrValue; // La variable cstrRetour a été suprimée
            char[] bWindowText = new char[10000];

            Array.Clear(bWindowText, 0, 10000);

            if (shError != MCHR_ERROR_NONE)
            {
                ushTypeError = (short)(MCHR_GetLastError(m_iID) - MCHR_ERROR_CODE);

                if (MCHR_GetLastError(m_iID) == MCHR_ERROR_DIALOG_CHR)
                    return CODE_OK;

                cstrValue = string.Format(": Erreur {0:X3}\r\n", ushTypeError);

                if (m_pCEdit != null)
                {
                    m_SectionCritiqueConsole.Lock();
                    Console.WriteLine("Lock m_SectionCritiqueConsole set SHORT in console");

                    m_pCEdit.GetWindowTextA(bWindowText, 10000);
                    cstrMessage = new string(bWindowText) + cstrMessErr + cstrValue;
                    m_pCEdit.SetWindowTextA(cstrMessage);
                    m_pCEdit.UpdateWindow();
                    m_SectionCritiqueConsole.Unlock();
                    Console.WriteLine("Unlock m_SectionCritiqueConsole set SHORT in console");
                }

                return CODE_ERROR;
            }

            return CODE_OK;
        }

        public int CCCSA_SetConsole(string cstrMessErr, int iError)
        {
            string cstrMessage, cstrValue;
            char[] bWindowText = new char[10000];

            Array.Clear(bWindowText, 0, 10000);

            if (iError < CODE_OK)
            {
                cstrValue = string.Format(": Erreur {0}\r\n", iError);

                if (m_pCEdit != null)
                {
                    m_SectionCritiqueConsole.Lock();
                    Console.WriteLine("Lock m_SectionCritiqueConsole set INT in console");

                    m_pCEdit.GetWindowTextA(bWindowText, 10000);
                    cstrMessage = new string(bWindowText) + cstrMessErr + cstrValue;
                    m_pCEdit.SetWindowTextA(cstrMessage);
                    m_pCEdit.UpdateWindow();
                    m_SectionCritiqueConsole.Unlock();
                    Console.WriteLine("Unlock m_SectionCritiqueConsole set INT in console");
                }

                return CODE_ERROR;
            }

            return CODE_OK;
        }

        //Returned on configuration value
        //FIXME - Il faut regarder le type de variable int et MCHR_ID
        public int CCCSA_GetID()
        {
            Console.WriteLine("Get ID: " + m_iID);
            return m_iID;
        }

        public int CCCSA_GetNbrOfPixel()
        {
            Console.WriteLine("Get Number of pixel: " + m_stStatuParam.m_iNbrOfPixel);
            return m_stStatuParam.m_iNbrOfPixel;
        }

        //FIXME - Il faut regarder la fonction MCHR_GetStatus()
        public void CCCSA_SetCcsStatus()
        {
            short shMessage = MCHR_GetStatus(m_iID);
            switch (shMessage)
            {
                case MCHR_STATUS_NOT_INITIALIZED:
                    m_cstrCcsStatus = "Pas initialis�";
                    break;
                case MCHR_STATUS_INITIALIZED:
                    m_cstrCcsStatus = "Initialisation";
                    break;
                case MCHR_STATUS_INIT_FAILED:
                    m_cstrCcsStatus = "Erreur d'initialisation";
                    break;
                case MCHR_STATUS_WAIT_COMMAND:
                    m_cstrCcsStatus = "Attente commande";
                    break;
                case MCHR_STATUS_COMMAND_IN_PROGRESS:
                    m_cstrCcsStatus = "Traitement commande";
                    break;
                case MCHR_STATUS_ACQUISITION_IN_PROGRESS:
                    m_cstrCcsStatus = "Traitement acquisition";
                    break;
                case MCHR_STATUS_CONTINUOUS_ACQ_IN_PROGRESS:
                    m_cstrCcsStatus = "Acquisition continue";
                    break;
                case MCHR_STATUS_STOP_ACQ_IN_PROGRESS:
                    m_cstrCcsStatus = "Arret d'acquisition";
                    break;
                default:
                    m_cstrCcsStatus = "Statu invalide";
                    break;
            }
        }

        //FIXME - Il faut regarder le type de variable U16 et I16, m_pCIOB.CIOBCTRL_Read_Bench(ref u16iv)
        public void CCCSA_SetBenchStatus()
        {
            ushort u16iv = 0;
            short i16Err = m_pCIOB.CIOBCTRL_Read_Bench(ref u16iv);

            if (i16Err == NoError)
            {
                if (u16iv == 0)
                {
                    m_cstrBenchStatus = "D�connect�";
                    m_pCIOB.CIOBCTRL_Set_Digital_Output(LED2_DOChannel, true);
                    m_stStatuParam.m_bBenchStatus = false;
                }
                else
                {
                    m_cstrBenchStatus = "Connect�";
                    m_stStatuParam.m_bBenchStatus = true;
                }
            }
            else
            {
                m_pCIOB.CIOBCTRL_Set_Digital_Output(LED2_DOChannel, true);
                m_cstrBenchStatus = "Statu invalide";
                m_stStatuParam.m_bBenchStatus = false;
            }
        }

        public void CCCSA_GetConnectionStatus(out string cstrBench, out string cstrCCS, out string cstrPower, out float Puissance)
        {
            string cstrValue = m_fPuissance.ToString("0.000");
            cstrCCS = m_cstrCcsStatus;
            cstrBench = m_cstrBenchStatus;
            Puissance = m_fPuissance;
            cstrPower = cstrValue;
        }

        public int CCCSA_GetConnectionStatus()
        {
            Console.WriteLine("Get link status: " + m_stStatuParam.m_DeviceConnected);
            return m_stStatuParam.m_DeviceConnected;
        }

        public void CCCSA_GetPowerParam(out float fAlpha, out float fBeta, out float fGamma)
        {
            Console.WriteLine("Parameter Alpha: " + m_stInfoParam.m_fAlpha);
            fAlpha = m_stInfoParam.m_fAlpha;
            Console.WriteLine("Parameter Beta: " + m_stInfoParam.m_fBeta);
            fBeta = m_stInfoParam.m_fBeta;
            Console.WriteLine("Parameter Gamma: " + m_stInfoParam.m_fGamma);
            fGamma = m_stInfoParam.m_fGamma;
        }

        public int CCCSA_GetNbrOfCmd()
        {
            Console.WriteLine("Get Number of commands: " + m_stInfoParam.m_CmdNbr);
            return m_stInfoParam.m_CmdNbr;
        }

        public int CCCSA_GetNbrCmdStatus()
        {
            Console.WriteLine("Get number of Command Status: " + m_stStatuParam.m_bCmdsStatus);
            return m_stStatuParam.m_bCmdsStatus;
        }

        public void CCCSA_GetHtmlPath(out string cstrPath, out int iParamErr)
        {
            Console.WriteLine("Get html path: " + m_stInfoParam.m_FilePath);
            cstrPath = m_stInfoParam.m_FilePath;
            iParamErr = m_stStatuParam.m_bParametersStatus.m_HtmlFile;
        }

        public void CCCSA_SetHtmlPath(ref string cstrPath, ref int iParamErr)
        {
            Console.WriteLine("Set html path: " + cstrPath);
            m_stInfoParam.m_FilePath = cstrPath;
            m_stStatuParam.m_bParametersStatus.m_HtmlFile = iParamErr;
        }

        public void CCCSA_GetHtmlDir(ref string cstrDir, ref int iParamErr)
        {
            Console.WriteLine("Get html dir: " + m_cstrReportDir);
            cstrDir = m_cstrReportDir;
            iParamErr = m_stStatuParam.m_bParametersStatus.m_HtmlFile;
        }

        public void CCCSA_SetHtmlDir(ref string cstrDir, ref int iParamErr)
        {
            Console.WriteLine("Set html dir: " + cstrDir);
            m_cstrReportDir = cstrDir;
            m_stStatuParam.m_bParametersStatus.m_HtmlFile = iParamErr;
        }

        //FIXME - Il faut regarder les variables qui manquent (MCHR)
        public int CCCSA_Init()
        {
            // Variable declaration
            char[] txt = new char[200];
            short shUsbDeviceNumber = 0, t = 0;
            string[] bUsbDeviceList = new string[MCHR_MAX_SENSOR];
            CCCSA_SetConsole("Extinction led rouge", Convert.ToInt32(m_pCIOB.CIOBCTRL_Set_Red_Led(false)));
            CCCSA_SetConsole("Extinction led verte", Convert.ToInt32(m_pCIOB.CIOBCTRL_Set_Green_Led(false)));
            CCCSA_SetConsole("Extinction led bleu", Convert.ToInt32(m_pCIOB.CIOBCTRL_Set_Blue_Led(false)));

            // Variable initialization
            if (m_iID <= 0)
            {
                // init captor ID
                COleDateTime time = DateTime.Now;
                string sRapportDeTest = string.Format("<H3>Rapport de test {0}</H3></HR>", time.ToString());
                m_pInfoReport.Add(sRapportDeTest);
                m_pInfoReport.Add(string.Format("Banc_De_Test_CCS version : {0}", VERSION_BANC_DE_TEST));
                CCCSA_GetDllVersion();

                for (int j = 0; j < MCHR_MAX_SENSOR; j++)
                {
                    bUsbDeviceList[j] = new string('\0', MCHR_USB_DEVICE_NAME_LENGTH);
                }

                Array.Clear(txt, 0, txt.Length);

                // Get current devices list which are connected
                m_stStatuParam.m_DeviceDetected = CCCSA_SetConsole("Aucun CCCS USB Pr�sent", MCHR_GetUsbDeviceList(bUsbDeviceList, ref shUsbDeviceNumber));
                if (m_stStatuParam.m_DeviceDetected == CODE_OK)
                {
                    // Validate connection
                    if (shUsbDeviceNumber == 0)
                    {
                        m_stStatuParam.m_DeviceDetected = CONNECT_ERROR;
                        CCCSA_SetConsole("No Device is connected!!!", m_stStatuParam.m_DeviceDetected);
                    }
                    else if (shUsbDeviceNumber > 1)
                    {
                        m_stStatuParam.m_DeviceType = bUsbDeviceList[0];
                        MessageBox.Show("Plusieurs CCS sont connect�!!! Evaluation du premier CCS.");
                    }
                    else
                    {
                        m_stStatuParam.m_DeviceType = bUsbDeviceList[0];
                    }
                    m_pInfoReport.Add(string.Format("Type :  {0}", m_stStatuParam.m_DeviceType));
                }

                // Delete list of chr
                for (int j = 0; j < MCHR_MAX_SENSOR; j++)
                {
                    bUsbDeviceList[j] = null;
                }
            }
            else
            {
                CCCSA_SetConsole("CCCS USB d�ja pr�sent");
            }

            OutputDebugString(m_stStatuParam.m_DeviceType);
            OutputDebugString("\n\r");
            
            if (m_stStatuParam.m_DeviceType.Contains("OPTIMA"))
            {
                if (m_stStatuParam.m_DeviceType.Contains("OPTIMA+") ||
                    m_stStatuParam.m_DeviceType.Contains("OPTIMA PLUS"))
                {
                    OutputDebugString("OPTIMA PLUS TROUVE\n\r");
                    m_cstrFilePath = "C:\\OPTIMA_PLUS_settings.ini";
                    CCCSA_InitParameters();
                }
                else if (m_stStatuParam.m_DeviceType.Contains("OPTIMA STAR"))
                {
                    OutputDebugString("EXTREMA TROUVE\n\r");
                    m_cstrFilePath = "C:\\EXTREMA_settings.ini";
                    CCCSA_InitParameters();
                }
                else
                {
                    OutputDebugString("OPTIMA TROUVE\n\r");
                    m_cstrFilePath = "C:\\OPTIMA_settings.ini";
                    CCCSA_InitParameters();
                }
            }
            else if (m_stStatuParam.m_DeviceType.Contains("PRIMA"))
            {
                OutputDebugString("PRIMA TROUVE\n\r");
                m_cstrFilePath = "C:\\PRIMA_settings.ini\0";
                CCCSA_InitParameters();
                OutputDebugString("PRIMA INIT\n\r");
            }
            else if (m_stStatuParam.m_DeviceType.Contains("INITIAL"))
            {
                OutputDebugString("INITIAL TROUVE\n\r");
                m_cstrFilePath = "C:\\PRIMA_settings.ini\0";
                CCCSA_InitParameters();
                OutputDebugString("INITIAL INIT\n\r");
            }
            else if (m_stStatuParam.m_DeviceType.Contains("IR") || m_stStatuParam.m_DeviceType.Contains("VIZIR"))
            {
                OutputDebugString("VIZIR TROUVE\n\r");
                m_cstrFilePath = "C:\\VIZIR_settings.ini\0";
                CCCSA_InitParameters();
                OutputDebugString("VIZIR INIT\n\r");
            }
            else if (m_stStatuParam.m_DeviceType.Contains("EXTREMA"))
            {
                OutputDebugString("EXTREMA TROUVE\n\r");
                m_cstrFilePath = "C:\\EXTREMA_settings.ini\0";
                CCCSA_InitParameters();
                OutputDebugString("EXTREMA INIT\n\r");
            }
            else if (m_stStatuParam.m_DeviceType.Contains("ULTIMA"))
            {
                OutputDebugString("ULTIMA TROUVE\n\r");
                m_cstrFilePath = "C:\\ULTIMA_settings.ini\0";
                CCCSA_InitParameters();
            }

            return m_stStatuParam.m_DeviceDetected;
        }
        
        //FIXME - Il faut reviser ce qu'il s'agit NDD_network_device_print_hello(); NDD_init_lib(HOST_MODE, 0);
        public int CCCSA_Init_Zenith()
        {
            // Variable declaration
            string txt;
            short shUsbDeviceNumber = 0;
            int t = 0;
            int nIndex = 0;
            //FIXME - Il manque d'ajouter des fonctions provient de CIOBCTRL
            CCCSA_SetConsole("Extinction led rouge", (int)m_pCIOB.CIOBCTRL_Set_Red_Led(false));
            CCCSA_SetConsole("Extinction led verte", (int)m_pCIOB.CIOBCTRL_Set_Green_Led(false));
            CCCSA_SetConsole("Extinction led bleu", (int)m_pCIOB.CIOBCTRL_Set_Blue_Led(false));
            
            // Variable initialization
            if (m_iID <= 0)
            {
                // Get current DLL version
                COleDateTime time = COleDateTime.GetCurrentTime();
                string sRapportDeTest = string.Format("<H3>Rapport de test {0}</H3></HR>", time.Format());
                m_pInfoReport.Add(sRapportDeTest);
                m_pInfoReport.Add(CCCSA_CStringFormat("Banc_De_Test_Controleur version : ", new CString(VERSION_BANC_DE_TEST)));
                CCCSA_GetDllVersion();
                NDD_network_device_print_hello(); // Je ne sais pas s'il existe ça fonction
                NDD_init_lib(HOST_MODE, 0);
                int l_NbIface = NDD_get_nb_multicast_ifaddrs();
                string[] l_ifaddrtable = new string[l_NbIface];
                string[] l_ifaddrDesctable = new string[l_NbIface];
                
                for (int i = 0; i < l_NbIface; i++)
                {
                    l_ifaddrtable[i] = "0.0.0.0";
                    l_ifaddrDesctable[i] = new char[DESCRIPTION_NETWORK_ADAPTER_LENGTH];
                    Array.Fill(l_ifaddrDesctable[i], '\0');
                }
                
                NDD_get_multicast_ifaddr_list(l_ifaddrtable, l_ifaddrDesctable);
                
                CCCSA_SetConsole("Ethernet card List :");
                for (int i = 0; i < l_NbIface; i++)
                {
                    txt = $"{i + 1}. {l_ifaddrDesctable[i]}";
                    CCCSA_SetConsole(txt);
                }
                
                if (l_NbIface >= m_stAppParam.m_iSelected_ethernet_card && 0 < m_stAppParam.m_iSelected_ethernet_card)
                {
                    Array.Copy(Encoding.ASCII.GetBytes(l_ifaddrtable[m_stAppParam.m_iSelected_ethernet_card - 1]), m_listenparam.ifaddr, IPADDRESS_LENGTH);
                    m_listenparam.timeoutMilliseconds = 300;
                    m_listenparam.shDeviceNumber = shUsbDeviceNumber;
                    NDD_print_version();
                    m_pInfoReport.Add(CCCSA_CStringFormat("Multicast Library version : ", new CString(NDD_get_version())));
                    CCCSA_OpenThread(ref m_thMulticast, Multicast, this);
                    Thread.Sleep(1000);
                    int ret = 1;
                    HostConfig l_hostcfg = new HostConfig();
                    Array.Fill(l_hostcfg.MACAddr_RO, (byte)0);
                    l_hostcfg.ProcessID = 0;
                    NDD_write_raw_data(m_listenparam.ifaddr, NET_DEV_DISCOVER_OPCODE, ref l_hostcfg, null);
                    Thread.Sleep(1000);
                    CCCSA_CloseThread(ref m_thMulticast);
                }
                else
                {
                    CCCSA_SetConsole("Veuillez sélectionner une carte ethernet valide");
                    return m_stStatuParam.m_DeviceDetected;
                }
                
                for (int i = 0; i < l_NbIface; i++)
                {
                    l_ifaddrtable[i] = null;
                    l_ifaddrDesctable[i] = null;
                }
                
                l_ifaddrtable = null;
                l_ifaddrDesctable = null;
                
                shUsbDeviceNumber = m_listenparam.shDeviceNumber;
                char[] Ip = new char[20];
                
                for (nIndex = 0; nIndex < ADDRESS_LEN; nIndex++)
                {
                    Ip[4 * nIndex] = (char)m_MulticastParam.IpAddr_RW[nIndex];
                    Ip[4 * nIndex + 1] = '.';
                }
                
                // Get current devices list which are connected
                // m_stStatuParam.m_DeviceDetected = CCCSA_SetConsole("Aucun Zenith identifie sur le réseau", MCHR_GetUsbDeviceList(bUsbDeviceList, ref shUsbDeviceNumber));
                
                if (m_stStatuParam.m_DeviceDetected == CODE_OK)
                {
                    // Validate connection
                    if (shUsbDeviceNumber == 0)
                    {
                        m_stStatuParam.m_DeviceDetected = CONNECT_ERROR;
                        CCCSA_SetConsole("No Device is detected!!!", m_stStatuParam.m_DeviceDetected);
                    }
                    else if (shUsbDeviceNumber > 1)
                    {
                        m_csSensorName = CString.Format("{0}_{1}", m_MulticastParam.Model_RO, m_MulticastParam.SerialNumber_RO);
                        m_stStatuParam.m_DeviceType = $"{m_csSensorName}_{Ip}";
                        txt = $"Plusieurs ZENITH sont detectés!!! Evaluation du controleur :\n {m_stStatuParam.m_DeviceType}";
                        AfxMessageBox(txt);
                    }
                    else
                    {
                        m_csSensorName = CString.Format("{0}_{1}", m_MulticastParam.Model_RO, m_MulticastParam.SerialNumber_RO);
                        m_stStatuParam.m_DeviceType = $"{m_csSensorName}_{Ip}";
                    }
                    
                    m_pInfoReport.Add(CCCSA_CStringFormat("Type : ", m_stStatuParam.m_DeviceType));
                }
            }
            else
            {
                CCCSA_SetConsole("ZENITH déjà présent");
            }
            
            OutputDebugString(m_stStatuParam.m_DeviceType);
            OutputDebugString("\n\r");
            
            if (m_stStatuParam.m_DeviceType.Find("ZENITH", 0) >= 0)
            {
                if (m_stStatuParam.m_DeviceType.Find("ZENITH2CH", 0) >= 0 || m_stStatuParam.m_DeviceType.Find("ZENITH2V", 0) >= 0)
                {
                    OutputDebugString("ZENITH 2 VOIES TROUVE\n\r");
                    m_cstrFilePath = Path.Combine(m_cstrAppDir, "ZENITH2V_settings.ini");
                    CCCSA_InitParameters();
                }
                else if (m_stStatuParam.m_DeviceType.Find("ZENITH5K", 0) >= 0)
                {
                    OutputDebugString("ZENITH5K TROUVE\n\r");
                    m_cstrFilePath = Path.Combine(m_cstrAppDir, "ZENITH5K_settings.ini");
                    CCCSA_InitParameters();
                }
                else
                {
                    OutputDebugString("ZENITH TROUVE\n\r");
                    m_cstrFilePath = Path.Combine(m_cstrAppDir, "ZENITH_settings.ini");
                    CCCSA_InitParameters();
                }
            }
            
            return m_stStatuParam.m_DeviceDetected;
        }

        //FIXME - Il manque la fonction int CCCSA_Init_Zenith();

        public int CCCS_Actions.CCCSA_Disconnect()
        {
            // free event memory
            if (m_iID > 0)
            {
                CCCSA_Release();
                Console.WriteLine("DECONNEXION du port");
                m_pCIOB.CIOBCTRL_Set_432_422(false); // set board in 232 case
                m_ccsStop.Lock();
                if (CCCSA_SetConsole("Fermeture de la connexion", MCHR_CloseChr(m_iID)) != CODE_OK)
                {
                    MessageBox.Show("Impossible de fermer le port.");
                    return CODE_ERROR;
                }
                m_ccsStop.Unlock();
                Thread.Sleep(200);
                m_iID = 0;
            }
            return CODE_OK;
        }

        public int CCCS_Actions.CCCSA_GetDeviceType()
        {
            return m_stStatuParam.m_iCcsType;
        }

        public void CCCS_Actions.CCCSA_SetIOControls(CIOBoardControl CIOBCtrls)
        {
            Console.WriteLine("Set IO controls");
            m_pCIOB = CIOBCtrls; // Set current pointer
        }

        public int CCCS_Actions.CCCSA_SetTrigger(bool bState)
        {
            Console.WriteLine("Set trigger state: " + (bState ? "true" : "false"));
            if (m_pCIOB.CIOBCTRL_Set_Trigger(bState) != NoError) return CARTE_ERROR; // return error
            return CODE_OK;
        }

        public void CCCS_Actions.CCCSA_Release()
        {
            // Désallocation des handles d'événements
            Console.WriteLine("Verification des threads et handle");
            m_stStatuParam.m_DeviceConnected = CODE_ERROR; // end off thread
            //CCCSA_CloseThread(&m_thMulticast);
            CCCSA_CloseAcqSpectre();
        }

        public void CCCS_Actions.CCCSA_OpenAcqSpectre()
        {
            m_DisplaySignalEvent = new ManualResetEvent(false);
            m_AvailableSignalEvent = new ManualResetEvent(false);
            CCCSA_OpenThread(ref m_thAcqSpectre, AcqSpectrum, this); // Acquisition des spectres.
        }

        public void CCCS_Actions.CCCSA_CloseAcqSpectre()
        {
            Console.WriteLine("Fermeture du thread acquisition");
            CCCSA_CloseThread(ref m_thAcqSpectre);
            if (m_DisplaySignalEvent != null)
            {
                m_DisplaySignalEvent.Close();
                m_DisplaySignalEvent = null;
            }
            if (m_AvailableSignalEvent != null)
            {
                m_AvailableSignalEvent.Close();
                m_AvailableSignalEvent = null;
            }
        }

        public void CCCS_Actions.CCCSA_InitThread(threadparam tht)
        {
            tht.m_dwThreadId = 0;
            tht.lpParameter = IntPtr.Zero;
            int nIndex;
            // create all events for process 
            for (nIndex = 0; nIndex < EVENT_NUMBER; nIndex++)
            {
                tht.m_OnEvent[nIndex] = IntPtr.Zero;
            }
            tht.hThread = IntPtr.Zero;
        }

        public void CCCS_Actions.CCCSA_OpenThread(threadparam tht, ThreadStart lpStartAddress, IntPtr lpParam)
        {
            CCCSA_InitThread(tht); // Je ne sais pas s'il y a une fonction commme ça
            int nIndex;
            // create all events for process 
            for (nIndex = 0; nIndex < EVENT_NUMBER; nIndex++)
            {
                tht.m_OnEvent[nIndex] = CreateEvent(IntPtr.Zero, false, false, null);
            }
            tht.lpParameter = lpParam;
            tht.hThread = CreateThread(IntPtr.Zero, 0, lpStartAddress, tht.lpParameter, 0, out tht.m_dwThreadId);
        }

        public void CCCS_Actions.CCCSA_CloseThread(threadparam tht, uint dwMilliseconds)
        {
            if (tht.hThread != IntPtr.Zero)
            {
                SetEvent(tht.m_OnEvent[EVENT_KILL_THREAD]);
                if (WaitForSingleObject(tht.m_OnEvent[EVENT_KILL_THREAD_DO], dwMilliseconds) != WAIT_OBJECT_0)
                {
                    TerminateThread(tht.hThread, 0);
                }
                CloseHandle(tht.hThread);
                tht.hThread = IntPtr.Zero;
            }
            int nIndex;
            // release process events
            for (nIndex = 0; nIndex < EVENT_NUMBER; nIndex++)
            {
                if (tht.m_OnEvent[nIndex] != IntPtr.Zero)
                {
                    CloseHandle(tht.m_OnEvent[nIndex]);
                    tht.m_OnEvent[nIndex] = IntPtr.Zero;
                }
            }
            tht.lpParameter = IntPtr.Zero;
            tht.m_dwThreadId = 0;
        }

        //FIXME - Fonctions du CBIOCTRL
        public void CCCS_Actions.CCCSA_RaCLuxP(float fAlpha, float fBeta, float fGamma)
        {
            float fIntensity = 0.0f;
            // generate float array data
            m_pCIOB.CIOBCTRL_Read_Photo_Detector(ref fIntensity);
            // generate led power
            m_fPuissance = fAlpha * fIntensity * fIntensity + fBeta * fIntensity + fGamma;
        }

        //

        public int CCCSA_Test_Spectrometre_Centrage()
        {
            // Variable declaration
            int iErr = CODE_OK;
            int BufferIndex = -1;
            int MeasureIndex = -1;
            float fAvr = 0.0f;
            char[] banswer = new char[200];
            string cstrValue;
            uint Events = 0;
            // Channel and Lamps table
            short shErr = MCHR_ERROR_NONE;
            float[,] SpectrumData = new float[20, 2];
            string cstrValidDataBR = " : Valide";
            string cstrValidDataBV = " : Valide";
            string cstrValidDataBB = " : Valide";
            string cstrValidDataIR = " : Valide";
            string cstrValidDataIV = " : Valide";
            string cstrValidDataIB = " : Valide";

            // Erase memory for red acquisition
            Array.Clear(banswer, 0, 200);
            Array.Clear(SpectrumData, 0, 20 * 2);
            m_OffsetEn = false; // set subtraction of offset

            if (m_stStatuParam.m_bParametersStatus.m_Pixel == CODE_OK &&
                m_stStatuParam.m_bParametersStatus.m_RLedMax == CODE_OK &&
                m_stStatuParam.m_bParametersStatus.m_RLedMin == CODE_OK &&
                m_stStatuParam.m_bParametersStatus.m_GLedMax == CODE_OK &&
                m_stStatuParam.m_bParametersStatus.m_GLedMin == CODE_OK &&
                m_stStatuParam.m_bParametersStatus.m_BLedMax == CODE_OK &&
                m_stStatuParam.m_bParametersStatus.m_BLedMin == CODE_OK)
            {
                switch (m_stStatuParam.m_iCcsType)
                {
                    case MCHR_CCS_ULTIMA:
                        MessageBox.Show("D�connectez la source x�non.");
                        m_ccsStop.Lock(); // Lock section
                        Trace.WriteLine("Lock send LPA3 from centrage test\r\n");
                        iErr = CCCSA_SetConsole("MCHR_SendCommand ", MCHR_SendCommand(m_iID, "LPA3", banswer)); // change source
                        break;
                    case MCHR_CCS_INITIAL:
                    case MCHR_CCS_PRIMA:
                    case MCHR_CCS_OPTIMA:
                    case MCHR_CCS_OPTIMA_PLUS:
                    case MCHR_CCS_EXTREMA:
                    case MCHR_STIL_VIZIR:
                        m_ccsStop.Lock(); // Lock section
                        Trace.WriteLine("Lock set led 0 from centrage test\r\n");
                        iErr = CCCSA_SetConsole("MCHR_SendCommand ", MCHR_SendCommand(m_iID, "LPA1", banswer));
                        CCCSA_SetConsole(CCCSA_CStringFormat("MCHR_SetLed ", 0), MCHR_SetLed(m_iID, 0));
                        break;
                    default:
                        iErr = CODE_ERROR;
                        break;
                }

                CCCSA_SetConsole("Impossible de changer la fr�quence d'acquisition", MCHR_SetScanRate(m_iID, m_wScanRate));
                m_ccsStop.Unlock(); // Unlock section
                Trace.WriteLine("Unlock from centrage test\r\n");

                // Set Red Led to On
                if (CCCSA_SetConsole("Allumage led rouge", (int)m_pCIOB.CIOBCTRL_Set_Red_Led(true)) != CODE_OK)
                {
                    iErr = CARTE_ERROR; // switch on red led
                    cstrValidDataBR = ": pas test�"; // Set information on report
                    cstrValidDataIR = ": inconnue"; // Set information on report
                }
                else
                {
                    if (CCCSA_SetConsole("Allumage led vert", (int)m_pCIOB.CIOBCTRL_Set_Green_Led(true)) != CODE_OK)
                    {
                        iErr = CARTE_ERROR; // switch on green led
                        cstrValidDataBV = ": pas test�"; // Set information on report
                        cstrValidDataIV = ": inconnue"; // Set information on report
                    }
                    else
                    {
                        if (CCCSA_SetConsole("Allumage led bleu", (int)m_pCIOB.CIOBCTRL_Set_Blue_Led(true)) != CODE_OK)
                        {
                            iErr = CARTE_ERROR; // switch on blue led
                            cstrValidDataBB = ": pas test�"; // Set information on report
                            cstrValidDataIB = ": inconnue"; // Set information on report
                        }
                        else if (iErr == CODE_OK)
                        {
                            Thread.Sleep(1000);
                            if (CCCSA_Barycenter(m_pAcqSig1, m_BarySig1) != CODE_OK) // calculate barycenter
                            {
                                cstrValidDataBR = ": erreur de pic"; // Set information on report
                                cstrValidDataBV = ": erreur de pic"; // Set information on report
                                cstrValidDataBB = ": erreur de pic"; // Set information on report
                                cstrValidDataIR = ": erreur de pic"; // Set information on report
                                cstrValidDataIV = ": erreur de pic"; // Set information on report
                                cstrValidDataIB = ": erreur de pic"; // Set information on report
                                iErr = CODE_ERROR;
                            }
                            else
                            {
                                if (m_BarySig1[2, 1] > (float)m_stInfoParam.m_CenterMaxRLed || m_BarySig1[2, 1] < (float)m_stInfoParam.m_CenterMinRLed) // verify if it's a good value
                                {
                                    cstrValidDataBR = " : Pas Valide";
                                    iErr = CODE_ERROR; // return error code
                                }
                                if (m_BarySig1[1, 1] > (float)m_stInfoParam.m_CenterMaxGLed || m_BarySig1[1, 1] < (float)m_stInfoParam.m_CenterMinGLed) // verify if it's a good value
                                {
                                    cstrValidDataBV = " : Pas Valide";
                                    iErr = CODE_ERROR; // return error code
                                }
                                if (m_BarySig1[0, 1] > (float)m_stInfoParam.m_CenterMaxBLed || m_BarySig1[0, 1] < (float)m_stInfoParam.m_CenterMinBLed) // verify if it's a good value
                                {
                                    cstrValidDataBB = " : Pas Valide";
                                    iErr = CODE_ERROR; // return error code
                                }
                                if (m_BarySig1[2, 0] > (float)m_stInfoParam.m_SeuilMaxRLed || m_BarySig1[2, 0] < (float)m_stInfoParam.m_SeuilMinRLed) // verify if it's a good value
                                {
                                    cstrValidDataIR = " : Pas Valide";
                                    iErr = CODE_ERROR; // return error code
                                }
                                if (m_BarySig1[1, 0] > (float)m_stInfoParam.m_SeuilMaxGLed || m_BarySig1[1, 0] < (float)m_stInfoParam.m_SeuilMinGLed) // verify if it's a good value
                                {
                                    cstrValidDataIV = " : Pas Valide";
                                    iErr = CODE_ERROR; // return error code
                                }
                                if (m_BarySig1[0, 0] > (float)m_stInfoParam.m_SeuilMaxBLed || m_BarySig1[0, 0] < (float)m_stInfoParam.m_SeuilMinBLed) // verify if it's a good value
                                {
                                    cstrValidDataIB = " : Pas Valide";
                                    iErr = CODE_ERROR; // return error code
                                }
                            }
                        }
                        else
                        {
                            cstrValidDataBR = ": pas test�"; // Set information on report
                            cstrValidDataBV = ": pas test�"; // Set information on report
                            cstrValidDataBB = ": pas test�"; // Set information on report
                            cstrValidDataIR = ": inconnue"; // Set information on report
                            cstrValidDataIV = ": inconnue"; // Set information on report
                            cstrValidDataIB = ": inconnue"; // Set information on report
                            iErr = CODE_ERROR; // erreur valide
                        }
                    }
                }
            }
            else
            {
                cstrValidDataBR = ": pas test�"; // Set information on report
                cstrValidDataBV = ": pas test�"; // Set information on report
                cstrValidDataBB = ": pas test�"; // Set information on report
                cstrValidDataIR = ": pas test�"; // Set information on report
                cstrValidDataIV = ": pas test�"; // Set information on report
                cstrValidDataIB = ": pas test�"; // Set information on report
                iErr = CODE_ERROR; // erreur valide
            }
            m_pCIOB.CIOBCTRL_Set_Green_Led(false);
            m_pCIOB.CIOBCTRL_Set_Blue_Led(false);
            m_pCIOB.CIOBCTRL_Set_Red_Led(false);
            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Barycentre rouge", (float)m_stInfoParam.m_CenterMinRLed, (float)m_stInfoParam.m_CenterMaxRLed), m_BarySig1[2, 1]) + cstrValidDataBR); // Set information on report
            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Barycentre vert", (float)m_stInfoParam.m_CenterMinGLed, (float)m_stInfoParam.m_CenterMaxGLed), m_BarySig1[1, 1]) + cstrValidDataBV); // Set information on report
            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Barycentre bleu", (float)m_stInfoParam.m_CenterMinBLed, (float)m_stInfoParam.m_CenterMaxBLed), m_BarySig1[0, 1]) + cstrValidDataBB); // Set information on report
            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Intensit� rouge", (float)m_stInfoParam.m_SeuilMinRLed, (float)m_stInfoParam.m_SeuilMaxRLed), m_BarySig1[2, 0]) + cstrValidDataIR); // Set information on report
            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Intensit� vert", (float)m_stInfoParam.m_SeuilMinGLed, (float)m_stInfoParam.m_SeuilMaxGLed), m_BarySig1[1, 0]) + cstrValidDataIV); // Set information on report
            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Intensit� bleu", (float)m_stInfoParam.m_SeuilMinBLed, (float)m_stInfoParam.m_SeuilMaxBLed), m_BarySig1[0, 0]) + cstrValidDataIB); // Set information on report
            m_pInfoReport.Add(""); // Set information on report
            return iErr;
        }

        //

        //

        public int CCCS_Actions.CCCSA_Test_Sorties_ANA()
        {
            short iRet = 0;
            uint u32Count = 0;
            ushort[] u16iv1 = new ushort[AI_BUFFERSIZE];
            ushort[] u16iv2 = new ushort[AI_BUFFERSIZE];
            ushort[] u16iv3 = new ushort[AI_BUFFERSIZE];
            ushort[] u16iv4 = new ushort[AI_BUFFERSIZE];
            int iv1 = -1, iv2 = -1, iv3 = -1, iv4 = -1, iErr = CODE_OK;
            double f64Voltage = 0.0;
            float fv1 = 0.0f, fv2 = 0.0f, fv3 = 0.0f, fv4 = 0.0f;

            // envoyer commandes pour mettre +10V
            short shRet = 0;
            char[] banswer = new char[100];
            string cstrValidAO1_p10v = " : Valide";
            string cstrValidAO1_m10v = " : Valide";
            string cstrValidAO2_p10v = " : Valide";
            string cstrValidAO2_m10v = " : Valide";

            //Erase memory
            Array.Clear(u16iv1, 0, AI_BUFFERSIZE);
            Array.Clear(u16iv2, 0, AI_BUFFERSIZE);
            Array.Clear(u16iv3, 0, AI_BUFFERSIZE);
            Array.Clear(u16iv4, 0, AI_BUFFERSIZE);

            if (m_stStatuParam.m_bBenchStatus == true && m_stStatuParam.m_bParametersStatus.m_AnalogMax == CODE_OK && m_stStatuParam.m_bParametersStatus.m_AnalogMin == CODE_OK)
            {
                Console.WriteLine("Lock CCCSA_Test_Sorties_ANA 1");
                m_ccsStop.Lock(); //Lock section

                if (CCCSA_SetConsole("Selection de analog output", MCHR_SendCommand(m_iID, "DBW106,0", banswer)) == CODE_OK)
                {
                    // lire entrée analogique 1 et 2
                    if (CCCSA_SetConsole("Ecriture de analog output 1", MCHR_SendCommand(m_iID, "DBW136,0", banswer)) == CODE_OK)
                    {
                        if (CCCSA_SetConsole("Ecriture de analog output 2", MCHR_SendCommand(m_iID, "DBW137,19193131008", banswer)) == CODE_OK)
                        {
                            m_ccsStop.Unlock(); //Unlock section

                            CCCSA_SetConsole("Lecture analog output 1", (int)m_pCIOB.CIOBCTRL_Read_AnalogOutputs1(u16iv1, AI_BUFFERSIZE, ref u32Count));
                            CCCSA_GetAvr(u16iv1, ref iv1, u32Count);
                            m_pCIOB.CIOBCTRL_AIconvert(iv1, ref f64Voltage);
                            fv1 = (float)f64Voltage;
                            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Analog output 1 test +10v", m_stInfoParam.m_AnalogRateMax, -1.0f), fv1)); //print values in report
                            f64Voltage = 0.0;

                            CCCSA_SetConsole("Lecture analog output 2", (int)m_pCIOB.CIOBCTRL_Read_AnalogOutputs2(u16iv2, AI_BUFFERSIZE, ref u32Count));
                            CCCSA_GetAvr(u16iv2, ref iv2, u32Count);
                            m_pCIOB.CIOBCTRL_AIconvert(iv2, ref f64Voltage);
                            fv2 = (float)f64Voltage;
                            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Analog output 2 test +10v", m_stInfoParam.m_AnalogRateMax, -1.0f), fv2));
                            f64Voltage = 0.0;
                        }
                        else
                        {
                            m_ccsStop.Unlock(); //Unlock section
                            Console.WriteLine("Unlock CCCSA_Test_Sorties_ANA 1");
                            m_pInfoReport.Add(new string("Valeur analog output 2 test +10V (Value) Impossible d'écrire le port")); //print error report
                        }
                    }
                    else
                    {
                        m_ccsStop.Unlock(); //Unlock section
                        m_pInfoReport.Add(new string("Valeur analog output 2 test +10V (Value) Impossible d'écrire le port")); //print error report
                    }

                    m_ccsStop.Lock(); //Lock section
                    // lire entrée analogique 1 et 2
                    if (CCCSA_SetConsole("Ecriture analog output 1", MCHR_SendCommand(m_iID, "DBW136,0000000000", banswer)) == CODE_OK)
                    {
                        if (CCCSA_SetConsole("Ecriture analog output 2", MCHR_SendCommand(m_iID, "DBW137,0000000000", banswer)) == CODE_OK)
                        {
                            m_ccsStop.Unlock(); //Unlock section

                            CCCSA_SetConsole("Lecture analog output 1", (int)m_pCIOB.CIOBCTRL_Read_AnalogOutputs1(u16iv3, AI_BUFFERSIZE, ref u32Count));
                            CCCSA_GetAvr(u16iv3, ref iv3, u32Count);
                            m_pCIOB.CIOBCTRL_AIconvert(iv3, ref f64Voltage);
                            fv3 = (float)f64Voltage;
                            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Analog output 1 test -10v", m_stInfoParam.m_AnalogRateMin, -1.0f), fv3)); //print values in report
                            f64Voltage = 0.0;

                            CCCSA_SetConsole("Lecture analog output 2", (int)m_pCIOB.CIOBCTRL_Read_AnalogOutputs2(u16iv4, AI_BUFFERSIZE, ref u32Count));
                            CCCSA_GetAvr(u16iv4, ref iv4, u32Count);
                            m_pCIOB.CIOBCTRL_AIconvert(iv4, ref f64Voltage);
                            fv4 = (float)f64Voltage;
                            m_pInfoReport.Add(CCCSA_CStringFormat(CCCSA_CStringFormat("Analog output 2 test -10v", m_stInfoParam.m_AnalogRateMin, -1.0f), fv4));
                            f64Voltage = 0.0;
                        }
                        else
                        {
                            m_ccsStop.Unlock(); //Unlock section
                            m_pInfoReport.Add(new string("Analog output 2 test -10v: Impossible d'écrire le port")); //print error report
                        }
                    }
                    else
                    {
                        m_ccsStop.Unlock(); //Unlock section
                        m_pInfoReport.Add(new string("Analog output test -10v: Impossible d'écrire le port")); //print error report
                    }
                }
                else
                {
                    m_pInfoReport.Add(new string("Valeur analog output 1 test +10v: Impossible d'écrire le port")); //print error report
                    m_pInfoReport.Add(new string("Valeur analog output 2 test +10v: Impossible d'écrire le port"));
                    m_pInfoReport.Add(new string("Valeur analog output 1 test -10v: Impossible d'écrire le port"));
                    m_pInfoReport.Add(new string("Valeur analog output 2 test -10v: Impossible d'écrire le port"));
                }
            }
            else
            {
                m_pInfoReport.Add(new string("Valeur analog output 1 test +10v: Pas testé")); //print error report
                m_pInfoReport.Add(new string("Valeur analog output 2 test +10v: Pas testé"));
                m_pInfoReport.Add(new string("Valeur analog output 1 test -10v: Pas testé"));
                m_pInfoReport.Add(new string("Valeur analog output 2 test -10v: Pas testé"));
            }

            // Vérification.
            if (iErr == CODE_OK)
            {
                if (fv1 < m_stInfoParam.m_AnalogRateMax)
                {
                    cstrValidAO1_p10v = new string(" : Pas Valide");
                    iErr = CODE_ERROR; //not good value
                }
                if (fv2 < m_stInfoParam.m_AnalogRateMax)
                {
                    cstrValidAO2_p10v = new string(" : Pas Valide");
                    iErr = CODE_ERROR; //not good value
                }
                if (fv3 > m_stInfoParam.m_AnalogRateMin)
                {
                    cstrValidAO1_m10v = new string(" : Pas Valide");
                    iErr = CODE_ERROR; //not good value
                }
                if (fv4 > m_stInfoParam.m_AnalogRateMin)
                {
                    cstrValidAO2_m10v = new string(" : Pas Valide");
                    iErr = CODE_ERROR; //not good value
                }
                m_pInfoReport[m_pInfoReport.Count - 4] = m_pInfoReport[m_pInfoReport.Count - 4] + cstrValidAO1_p10v; //print error report
                m_pInfoReport[m_pInfoReport.Count - 3] = m_pInfoReport[m_pInfoReport.Count - 3] + cstrValidAO2_p10v;
                m_pInfoReport[m_pInfoReport.Count - 2] = m_pInfoReport[m_pInfoReport.Count - 2] + cstrValidAO1_m10v;
                m_pInfoReport[m_pInfoReport.Count - 1] = m_pInfoReport[m_pInfoReport.Count - 1] + cstrValidAO2_m10v;
            }
            return iErr;
        }

        //FIXME - Il y a des fonctions unconnu (MCHR_SetLed(m_iID, percent))
        public int CCCSA_TensionCourantZenith(int percent, ref float Puissance)
        {
            int iErr = CODE_OK;
            int iMoy = 100;
            float fCourant = 0.0f;
            float fTension = 0.0f;
            float fCourantSum = 0.0f;
            float fTensionSum = 0.0f;

            if (m_pCIOB.GetType() == typeof(CIOBoardControlMCC))
            {
                Puissance = 0.0f;

                CCCSA_SetConsole("MCHR set led", MCHR_SetLed(m_iID, percent));
                Thread.Sleep(500);

                string cstr;

                cstr = string.Format("Courant attendue {0:F1} A", (m_stInfoParam.m_fCourantMin + m_stInfoParam.m_fCourantMax) / 2.0f);
                for (int i = 0; i < iMoy; i++)
                {
                    ((CIOBoardControlMCC)m_pCIOB).CIOBCTRL_GetCourantAI(ref fCourant); //FIXME - Peut-être qu'il a des problèmes là
                    fCourantSum += fCourant;
                }
                fCourant = fCourantSum / (float)iMoy;
                fCourant *= m_stStatuParam.m_fCoefficientCourant;

                if (fCourant > m_stInfoParam.m_fCourantMax || fCourant < m_stInfoParam.m_fCourantMin)
                {
                    iErr = CODE_ERROR;
                    m_pInfoReport.Add(string.Format("{0} : pas valide", string.Format(string.Format(cstr, m_stInfoParam.m_fCourantMin, m_stInfoParam.m_fCourantMax), fCourant)))); //print values in report
                }
                else
                {
                    m_pInfoReport.Add(string.Format("{0} : Valide", string.Format(string.Format(cstr, m_stInfoParam.m_fCourantMin, m_stInfoParam.m_fCourantMax), fCourant)))); //print values in report
                }

                cstr = string.Format("Tension attendue {0:F1} V", (m_stInfoParam.m_fTensionMin + m_stInfoParam.m_fTensionMax) / 2.0f);
                for (int i = 0; i < iMoy; i++)
                {
                    ((CIOBoardControlMCC)m_pCIOB).CIOBCTRL_GetTensionAI(ref fTension);
                    fTensionSum += fTension;
                }
                fTension = fTensionSum / (float)iMoy;
                fTension *= m_stStatuParam.m_fCoefficientTension;

                if (fTension > m_stInfoParam.m_fTensionMax || fTension < m_stInfoParam.m_fTensionMin)
                {
                    iErr = CODE_ERROR;
                    m_pInfoReport.Add(string.Format("{0} : pas valide", string.Format(string.Format(cstr, m_stInfoParam.m_fTensionMin, m_stInfoParam.m_fTensionMax), fTension)))); //print values in report
                }
                else
                {
                    m_pInfoReport.Add(string.Format("{0} : Valide", string.Format(string.Format(cstr, m_stInfoParam.m_fTensionMin, m_stInfoParam.m_fTensionMax), fTension)))); //print values in report
                }

                string l_csPuissance;
                Puissance = fTension * fCourant;
                l_csPuissance = string.Format(": {0} Watt", Puissance);
                m_pInfoReport.Add(string.Format("Puissance d'alimentation mesurée LED {0}{1}", percent, l_csPuissance)); //print values in report
            }
            else
            {
                iErr = CODE_ERROR;
            }

            return iErr;
        }

        public int CCCSA_Test_Des_Triggers_In(enControllerFamily _enControllerFamily)
        {
            int iErr = CODE_OK, iErr1 = CODE_OK;
            float fAvr = -1;
            ulong Event;
            char[] banswer = new char[200];
            float[][] pArrayIntensity = new float[1][];
            MCHR_tyAcqParam AcqParameters = new MCHR_tyAcqParam(); //FIXME - Problème

            // create thread to generate rising edge on CCS
            IntPtr hBreakAcquisitionEvent = CreateEvent(IntPtr.Zero, false, false, null);

            // init variable
            Array.Clear(banswer, 0, banswer.Length);
            Array.Clear(AcqParameters, 0, AcqParameters.Length);

            if (m_stStatuParam.m_bBenchStatus == true)
            {
                // Allocation of memory space for stocking the measured data
                for (int i = 0; i < 1; i++)
                {
                    pArrayIntensity[i] = new float[1024];
                }

                // creation of synchronization events
                AcqParameters.TriggerFlag = true;
                AcqParameters.TriggerType = MCHR_TYPE_TRG;
                AcqParameters.NumberPointsTRE = 1;
                AcqParameters.HighLevelOrRisingEdgeActivated = MCHR_RISING_EDGE;
                AcqParameters.NumberOfBuffers = 1;
                AcqParameters.BufferLength = 1024;
                AcqParameters.NumberOfPointsBeforeSignal = 0;
                AcqParameters.NumberOfPoints = 0;
                AcqParameters.EventEndBuffer = IntPtr.Zero;
                AcqParameters.EventAcquire_n_Points = IntPtr.Zero;
                AcqParameters.EventEndAcquire = IntPtr.Zero;
                AcqParameters.EventEndMeasurement = IntPtr.Zero;
                AcqParameters.EventStartingAcquisition = hBreakAcquisitionEvent;

                m_ccsStop.Lock();
                Console.WriteLine("Lock trigger in\r\n");
                iErr = CCCSA_SetConsole("Get data", MCHR_GetAltitudeMeasurement(m_iID, AcqParameters, IntPtr.Zero, pArrayIntensity, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero));

                if (iErr == CODE_OK)
                {
                    Event = WaitForSingleObject(hBreakAcquisitionEvent, 1000);

                    switch (Event)
                    {
                        case WAIT_OBJECT_0:
                            iErr = CODE_ERROR;
                            m_pInfoReport.Add(string.Format("Detection trigger in {0}", _enControllerFamily == enControllerFamily.ZENITH ? " 24V" : "") + " auto d�clench�");
                            break;

                        default:
                            // envoi de l'impulsion:
                            m_pCIOB.CIOBCTRL_SendTriggerPulse();

                            Event = WaitForSingleObject(hBreakAcquisitionEvent, 1000);

                            switch (Event)
                            {
                                case WAIT_OBJECT_0:
                                    m_pInfoReport.Add(string.Format("Detection trigger in {0}", _enControllerFamily == enControllerFamily.ZENITH ? " 24V" : "") + " : Valide");
                                    break;

                                default:
                                    iErr = CODE_ERROR;
                                    m_pInfoReport.Add(string.Format("Detection trigger in {0}", _enControllerFamily == enControllerFamily.ZENITH ? " 24V" : "") + " : Pas Valide");
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    m_pInfoReport.Add(string.Format("Detection trigger in {0}", _enControllerFamily == enControllerFamily.ZENITH ? " 24V" : "") + " : Impossible de mettre en mode trigger");
                }

                CloseHandle(hBreakAcquisitionEvent);

                for (int i = 0; i < 1; i++)
                {
                    pArrayIntensity[i] = null;
                }

                MCHR_Abort(m_iID); //FIXME - Problème
                m_ccsStop.Unlock();
                Console.WriteLine("Unlock trigger in\r\n");
            }
            else
            {
                iErr = CODE_ERROR;
                m_pInfoReport.Add(string.Format("Detection trigger in {0}", _enControllerFamily == enControllerFamily.ZENITH ? " 24V" : "") + " : D�connect�e");
            }

            return iErr;
        }

        public int CCCSA_Test_Des_Triggers_Out()
        {
            ushort u16iv = 0;
            int iErr = CODE_OK, iCnt = 0;
            bool bUp = false, bEqual = false, bDown = false;
            uint dwIDThread;
            IntPtr hTriggerOut = IntPtr.Zero;

            // create thread to start acquisition
            if (m_stStatuParam.m_bBenchStatus == true)
            {
                hTriggerOut = CreateThread(IntPtr.Zero, 0, DelayedStartAcqTrgo, this, 0, out dwIDThread);

                do
                {
                    m_pCIOB.CIOBCTRL_Read_SyncOut(ref u16iv);

                    if (u16iv > 0 && bUp == false)
                    {
                        bUp = true;
                    }
                    else if (u16iv == 0 && bUp == true && bEqual == false)
                    {
                        bEqual = true;
                    }
                    else if (u16iv == 0 && bEqual == true && bDown == false)
                    {
                        bDown = true;
                        break;
                    }

                    iCnt++;
                } while ((bEqual == true && bUp == true && bDown == true) || (iCnt < 200));

                if (iCnt >= 200 && (bEqual != true || bUp != true || bDown != true))
                {
                    iErr = CODE_ERROR;
                    m_pInfoReport.Add("D�tection trigger out: Pas valide");
                }
                else
                {
                    iErr = CODE_OK;
                    m_pInfoReport.Add("D�tection trigger out: Valide");
                }

                if (hTriggerOut != IntPtr.Zero)
                {
                    TRACE("Fermeture du thread counters\r\n");
                    WaitForSingleObject(hTriggerOut, INFINITE);
                    hTriggerOut = IntPtr.Zero;
                }
            }
            else
            {
                iErr = CODE_ERROR;
                m_pInfoReport.Add("D�tection trigger out: Non connect�e");
            }

            return iErr;
        }

        //

        public int CCCSA_Test_Des_Codeurs()
        {
            int iErr = CODE_OK;

            // reset des codeurs
            if (CCCSA_ResetDesCodeurs() == CODE_ERROR)
                return CODE_ERROR;

            // boucle sur les 3 codeurs
            // temporaires
            uint C1, C2, C3;
            // avant apres
            uint iAvantC1 = 0, iAvantC2 = 0, iAvantC3 = 0;
            uint iApresC1 = 0, iApresC2 = 0, iApresC3 = 0;
            uint iDeltaC1 = 0, iDeltaC2 = 0, iDeltaC3 = 0;

            // lecture des codeurs
            for (int i = 1; i < 4; i++)
            {
                if (CCCSA_LectureCodeurs(out C1, out C2, out C3) == CODE_OK)
                {
                    // lu
                    // on mémorise les valeurs avant
                    if (i == 1) iAvantC1 = C1;
                    if (i == 2) iAvantC2 = C2;
                    if (i == 3) iAvantC3 = C3;
                }
                else
                {
                    // erreur
                    m_pInfoReport.Add("Codeur 1: Pas testé");
                    m_pInfoReport.Add("Codeur 2: Pas testé");
                    m_pInfoReport.Add("Codeur 3: Pas testé");
                    return CODE_ERROR;
                }

                // envoi des pas
                m_pCIOB.CIOBCTRL_DOCnt(i, NBR_PAS_ENVOI_CODEUR, T_STATE0_CODEUR, T_STATE1_CODEUR, LEFT);

                // lecture des codeurs
                if (CCCSA_LectureCodeurs(out C1, out C2, out C3) == CODE_OK)
                {
                    // lu
                    // on mémorise les valeurs apres
                    if (i == 1) iApresC1 = C1;
                    if (i == 2) iApresC2 = C2;
                    if (i == 3) iApresC3 = C3;
                }
                else
                {
                    // erreur
                    m_pInfoReport.Add("Codeur 1: Pas testé");
                    m_pInfoReport.Add("Codeur 2: Pas testé");
                    m_pInfoReport.Add("Codeur 3: Pas testé");
                    return CODE_ERROR;
                }
            }

            // Calcul des pas effectués
            if (iApresC1 > iAvantC1)
            {
                iDeltaC1 = iApresC1 - iAvantC1;
            }
            else
            {
                iDeltaC1 = iAvantC1 - iApresC1;
            }

            if (iApresC2 > iAvantC2)
            {
                iDeltaC2 = iApresC2 - iAvantC2;
            }
            else
            {
                iDeltaC2 = iAvantC2 - iApresC2;
            }

            if (iApresC3 > iAvantC3)
            {
                iDeltaC3 = iApresC3 - iAvantC3;
            }
            else
            {
                iDeltaC3 = iAvantC3 - iApresC3;
            }

            // Test Final
            if (iDeltaC1 == NBR_PAS_ENVOI_CODEUR * 4)
            {
                m_pInfoReport.Add(string.Format("Codeur 1: {0} : Valide", float(NBR_PAS_ENVOI_CODEUR * 4), -1.0f));
            }
            else
            {
                m_pInfoReport.Add(string.Format("Codeur 1: {0} : Pas valide", float(NBR_PAS_ENVOI_CODEUR * 4), -1.0f));
                iErr = CODE_ERROR;
            }

            if (iDeltaC2 == NBR_PAS_ENVOI_CODEUR * 4)
            {
                m_pInfoReport.Add(string.Format("Codeur 2: {0} : Valide", float(NBR_PAS_ENVOI_CODEUR * 4), -1.0f));
            }
            else
            {
                m_pInfoReport.Add(string.Format("Codeur 2: {0} : Pas valide", float(NBR_PAS_ENVOI_CODEUR * 4), -1.0f));
                iErr = CODE_ERROR;
            }

            if (iDeltaC3 == NBR_PAS_ENVOI_CODEUR * 4)
            {
                m_pInfoReport.Add(string.Format("Codeur 3: {0} : Valide", float(NBR_PAS_ENVOI_CODEUR * 4), -1.0f));
            }
            else
            {
                m_pInfoReport.Add(string.Format("Codeur 3: {0} : Pas valide", float(NBR_PAS_ENVOI_CODEUR * 4), -1.0f));
                iErr = CODE_ERROR;
            }

            // comment est-on arrivé ici ????
            return iErr;
        }

        public int CCCSA_Test_Des_Codeurs_Zenith(bool bSetDefaultValue, ushort bDirection)
        {
            char[] txt = new char[char.MaxValue];
            int iErr = CODE_OK;
            
            if (bSetDefaultValue)
            {
                m_cSelectionCodeur = m_stInfoParam.m_iSelectionCodeur;
                m_pCIOB.CIOBCTRL_Set_Digital_Output(ENCODER_SEL_0_DOChannel, (bool)(m_cSelectionCodeur & 0x01));
                m_pCIOB.CIOBCTRL_Set_Digital_Output(ENCODER_SEL_1_DOChannel, (bool)((m_cSelectionCodeur & 0x02) >> 1));
                m_pCIOB.CIOBCTRL_Set_Digital_Output(ENCODER_SEL_2_DOChannel, (bool)((m_cSelectionCodeur & 0x04) >> 2));
                Thread.Sleep(2000);
                
                // reset dos codeurs
                if (CCCSA_ResetDesCodeurs_Zenith() == CODE_ERROR)
                    return CODE_ERROR;
                
                // setar valor padrão dos codeurs
                if (CCCSA_ValeurDefautCodeurs_Zenith(CCCS_ValeurDefautDebutCodeur()) == CODE_ERROR)
                    return CODE_ERROR;
            }
            
            // boucle sur les 3 codeurs 
		    // temporaires:
            uint[] C = new uint[ZENITH_ENCODER_COUNT];
            uint[] iAvantC = new uint[ZENITH_ENCODER_COUNT];
            uint[] iApresC = new uint[ZENITH_ENCODER_COUNT];
            uint[] iDeltaC = new uint[ZENITH_ENCODER_COUNT];
            
            // leitura dos codeurs
            if (CCCSA_LectureCodeurs_Zenith(C, ZENITH_ENCODER_COUNT) == CODE_OK)
            {
                // lu
			    // on m�morise les valeurs avant
                for (int i = 0; i < ZENITH_ENCODER_COUNT; i++)
                {
                    iAvantC[i] = C[i];
                }
            }
            else
            {
                return CODE_ERROR;
            }
            
            // envoi des pas
            m_pCIOB.CIOBCTRL_DOCnt(0, NBR_PAS_ENVOI_CODEUR, T_STATE0_CODEUR, T_STATE1_CODEUR, bDirection);
            
            // lecture des codeurs
            if (CCCSA_LectureCodeurs_Zenith(C, ZENITH_ENCODER_COUNT) == CODE_OK)
            {
                // lu
			    // on m�morise les valeurs apres
                for (int i = 0; i < ZENITH_ENCODER_COUNT; i++)
                {
                    iApresC[i] = C[i];
                }
            }
            else
            {
                return CODE_ERROR;
            }
            
            string ctxt;
            
            // Calcul des pas effectu�s
            for (int i = 0; i < ZENITH_ENCODER_COUNT; i++)
            {
                if (m_cSelectionCodeur == i + 1 || m_cSelectionCodeur == 7)
                {
                    if (iApresC[i] > iAvantC[i])
                    {
                        iDeltaC[i] = iApresC[i] - iAvantC[i];
                    }
                    else
                    {
                        iDeltaC[i] = iAvantC[i] - iApresC[i];
                    }
                    
                    // Test Final
                    txt = string.Format("Codeur {0}", i + 1).ToCharArray();
                    
                    if (iDeltaC[i] == NBR_PAS_ENVOI_CODEUR * 4)
                    {
                        ctxt = CCCSA_CStringFormat(CCCSA_CStringFormat(new string(txt), (float)iAvantC[i], -1.0f), (float)iApresC[i]) + " : Valide";
                    }
                    else
                    {
                        ctxt = CCCSA_CStringFormat(CCCSA_CStringFormat(new string(txt), (float)iAvantC[i], -1.0f), (float)iApresC[i]) + " : Pas valide";
                        iErr = CODE_ERROR;
                    }
                }
                else
                {
                    // erreur
                    txt = string.Format("Codeur {0}", i + 1).ToCharArray();
                    
                    if (iDeltaC[i] == NBR_PAS_ENVOI_CODEUR * 4)
                    {
                        ctxt = CCCSA_CStringFormat(CCCSA_CStringFormat(new string(txt), (float)iAvantC[i], -1.0f), (float)iApresC[i]) + " : Pas testé";
                    }
                    else
                    {
                        ctxt = CCCSA_CStringFormat(CCCSA_CStringFormat(new string(txt), (float)iAvantC[i], -1.0f), (float)iApresC[i]) + " : Pas testé";
                    }
                }
                
                m_pInfoReport.Add(ctxt);
            }
            
            // comment est-on arriv� ici ????
            return iErr;
        }

        public int CCCSA_Test_Des_Ports_Serie(enTypeLink shLinkType)
        {
            // connecter le ccs en port serie avec la DLL.
            short shErr = MCHR_ERROR_NONE;
            char[] bResponse = new char[400];
            int iErr = CODE_OK;
            Array.Clear(bResponse, 0, bResponse.Length); //reset response memory

            m_stStatuParam.m_DeviceConnected = CODE_ERROR; //end off thread
            if (!CCCSA_IsZenith())
            {
                if (CCCSA_Disconnect() == CODE_OK && CCCSA_Connect(SERIAL_232, 115200) == CODE_OK)
                {
                    // envoyer une commande pour tester ( VER? )
                    if (CCCSA_Test_Commande_VER(SERIAL_232) != CODE_OK)
                    {
                        iErr = CODE_ERROR;
                    }
                }
                else
                {
                    m_pInfoReport.Add(new CString("Port RS232 : pas r�ussi � se connecter")); //set report
                    iErr = CODE_ERROR;
                }
            }
            if (CCCSA_Disconnect() == CODE_OK && CCCSA_Connect(SERIAL_422, 115200) == CODE_OK)
            {
                // envoyer une commande pour tester ( VER? )
                if (CCCSA_Test_Commande_VER(SERIAL_422) != CODE_OK)
                {
                    iErr = CODE_ERROR;
                }
            }
            else
            {
                m_pInfoReport.Add(new string("Port RS422 : pas r�ussi � se connecter"));
                iErr = CODE_ERROR;
            }
            CCCSA_Disconnect();
            CCCSA_Connect(shLinkType);
            return iErr;
        }

        
        /////////////
        public void CCCSA_SetAppDir(string AppDir)
        {
            m_cstrAppDir = AppDir;
        }

        public void CCCSA_SetAppFilePath(string AppFilePath)
        {
            m_cstrAppFilePath = AppFilePath;
        }

        public void CCCSA_SetSelected_ethernet_card(int Selected_ethernet_card)
        {
            m_iSelected_ethernet_card = Selected_ethernet_card;
            m_stAppParam.m_iSelected_ethernet_card = m_iSelected_ethernet_card;
        }

        public void CCCSA_SetIOBoardControlType(enIOBoardControl IOBoardControlType)
        {
            m_enIOBoardControlType = IOBoardControlType;
        }

        public void CCCSA_SetDeviceDetected(int DeviceDetected)
        {
            m_stStatuParam.m_DeviceDetected = DeviceDetected;
        }

        //

        int m_iChannelCount;

        public long CCCSA_GetAcquisitionFrequency()
        {
            Console.WriteLine("Get Acquisition Frequency: " + m_lAcquisitionFrequency);
            return m_lAcquisitionFrequency; // return free frequency
        }

        public ushort CCCSA_GetAverage()
        {
            Console.WriteLine("Get Average: " + m_wAverage);
            return m_wAverage; // return average
        }

        public int CCCS_TriggerInPulseCount()
        {
            return (int)(0.5 * ((double)m_stInfoParam.m_iTriggerOutRateMax + (double)m_stInfoParam.m_iTriggerOutRateMin));
        }

        //

        public int CCCS_ValeurDefautDebutCodeur()
        {
            return m_stInfoParam.m_iEncoderMin;
        }

        //

        public IntPtr CCCSA_DisplaySignalEvent()
        {
            return m_DisplaySignalEvent;
        }

        public IntPtr CCCSA_AvailableSignalEvent()
        {
            return m_AvailableSignalEvent;
        }

        public string CCCSA_GetDiagnosticFilePath()
        {
            return m_cstrDiagnosticFilePath;
        }

        public string CCCSA_GetReportFileName()
        {
            return m_cstrReportFileName;
        }

        public string CCCSA_GetReportFolder()
        {
            return m_cstrReportFolder;
        }

        public stReportInfo CCCSA_GetInfoParam()
        {
            return m_stInfoParam;
        }
        /* Partie privée */

        /* FIXME - Faire les corrections sur les types:
         * MCHR_ID, MCHR_tyAcqParam, enDigitalOutputChannel, WORD (short), HZIP, TCHAR, CString, enIOBoardControl, CStringArray*
         * stCommand*, CEdit*, HANDLE, BOOL (bool), stReportInfo, stAppInfo
         */

        /****************************
	    definition des variables
	    *****************************/
	    private MCHR_ID m_iID; // ID of the sensor
        private MCHR_tyAcqParam m_stAcqParam; // handle acquisition
        private enDigitalOutputChannel m_enOutputChannel[16];//enum configuration
        private short m_wScanRate;//current scanrate
        private long m_lAcquisitionFrequency;//current FreeFrequency
        private short m_wAverage;//current Average
        private int m_iONP;
        private int m_iOFP;
        private int m_iMNW;
        private int m_iMXW;
        private short m_wMaxDarkRedThreshold;
        private short m_wMaxDarkBlueThreshold;
        private short m_wMaxDarkThreshold;
        private short m_wOffset;
        private HZIP m_hz;
        private long m_operatingTimeCounter;

        //Database for parameters
        private stParameters m_stStatuParam;

        //Current Power of led
        private float m_fPuissance;//Puissance value
                           //Current file
        private TCHAR m_cstrFilePath[MAX_CHAR_PATH];//file path
        private string m_cstrAppDir;//file path
        private string m_cstrAppFilePath;//file path
        private string m_cstrDiagnosticFilePath;//file path
        private string m_cstrReportDir;//file path
        private string m_cstrReportFolder;
        private string m_cstrReportFileName;//file path
        private int m_iSelected_ethernet_card;
        private enIOBoardControl m_enIOBoardControlType;
        //Report declaration

        /* J'ai modifié CStringArray par List<string> */
        /* CStringArray* m_pInfoReport;//info report */
        //private List<string> infoReport = new List<string>();
        //private stCommand* m_pCmdReport;//command report
        private stCommand m_pCmdReport;
        private List<string> m_pInfoReport;
        private CEdit* m_pCEdit;//Current pointer on CEdit
        int CCCSA_GetParameters();//return parameters
        private HANDLE m_DisplaySignalEvent;
        private HANDLE m_AvailableSignalEvent;
        private HANDLE m_DiagnosticDoneSignalEvent;

        //Parameters
        private stReportInfo m_stInfoParam;
        private stAppInfo m_stAppParam;

        //Threads
        private string m_cstrCcsStatus;//CCS statu
        private bool m_bAlwaysTRUE;
        private string m_cstrBenchStatus;//bench statu
        private string m_cstrPowerStatus;//bench statu
        private string m_csSensorName;
        private string m_szFirmwareVersion;
        private string m_szVersion;

        //NOTE - Private:
        /**************
        Functions
        **************/
        //String and message
	    //int CCCSA_Box(CString cstrMessErr,CString cstrTitleErr,UINT uintType,HWND hWnd/*=NULL*/,short shError);
        public static int CCCSA_ReadFile(string filePath, float[] dataBuff)
        {
            //Declaration variable
            StreamReader streamReader = null; // Equivalente ao objeto CStudioFile em C++
            float[] fDataBuff = new float[m_stStatuParam.m_iNbrOfPixel]; 

            //init variables
            Array.Clear(fDataBuff, 0, m_stStatuParam.m_iNbrOfPixel); // Copia os dados do buffer "fDataBuff" para "pdaraBuff"
            int iFileLength = 0, iPos = 0;

            // Console.WriteLine ao invés de OutputDebugString e AfxMessageBox
            // string.Format ao invés de sprintf
            // Array.Clear ao invés de ZeroMemory
            Console.WriteLine("Nom Fic : {0}, Nb Pixel = {1}\n", filePath, m_stStatuParam.m_iNbrOfPixel);
            
            try
            {
                streamReader = new StreamReader(filePath);
                //read first data of file to have length of data
                string line = streamReader.ReadLine();
                if (line != null)
                {
                    iFileLength = int.Parse(line);
                    if (iFileLength != m_stStatuParam.m_iNbrOfPixel)
                    {
                        Console.WriteLine("Les données sont erronées {0}\n", filePath);
                        return CODE_ERROR;
                    }
                    else
                    {
                        Console.WriteLine("Lecture file :\n");
                        while ((line = streamReader.ReadLine()) != null && iPos < m_stStatuParam.m_iNbrOfPixel)
                        {
                            fDataBuff[iPos] = float.Parse(line);
                            iPos++;

                            Console.Write("{0},", iPos);
                            Console.WriteLine();
                        }
                    }
                    Array.Copy(fDataBuff, dataBuff, iFileLength);
                }
                else
                {
                    Console.WriteLine("Pas de définition du nombre de donnée {0}\n", filePath);
                    return CODE_ERROR;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problème d'ouverture fichier {0}: {1}\n", filePath, ex.Message);
                return CODE_ERROR;
            }
            finally
            {
                if (streamReader != null) streamReader.Close();
            }
            return CODE_OK;
        }
        
        public int CCCSA_WriteFile(string cstrPath, ushort[] pDataBuff, int size, ref string csmsg)
        {
            /*
            lpBuff => char[]
            WORD   => ushort
            CString* csmsg => ref string csmsg
            */
            string MessDebug;
            StreamWriter streamWriter; // Equivalente ao objeto CStudioFile em C++
            char[] lpBuff = new char[MAX_CHAR_CMD];

            int iFileLength = 0, iPos = 0;
            // Console.WriteLine ao invés de OutputDebugString e AfxMessageBox
            // string.Format ao invés de sprintf
            MessDebug = string.Format("Nom Fic : {0}, Nb Pixel = {1}\n\r", cstrPath, size);
            Console.WriteLine(MessDebug);

            try
            {
                streamWriter = new StreamWriter(cstrPath);

                if (!string.IsNullOrEmpty(csmsg))
                {
                    streamWriter.Write(csmsg);
                }

                while (iPos < size)
                {
                    iPos++;
                    string csData = string.Format("{0}\n", pDataBuff[iPos]);
                    streamWriter.Write(csData);

                    MessDebug = string.Format("{0},", iPos);
                    Console.WriteLine(MessDebug);
                }

                streamWriter.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problème d'ouverture fichier : {0}\n", cstrPath);
                return CODE_ERROR;
            }

            return CODE_OK;
        }
    
        //FIXME - m_pAcqRef1 e m_pAcqRef2
        public int CCCSA_InitParameters()
        {
            if (CCCSA_GetParameters() != CODE_OK)
                return CODE_ERROR;

            Debug.WriteLine("\n\r Après CCCSA_GetParameters\n\r");

            if (m_stStatuParam.m_bParametersStatus.m_Ref1File != CODE_OK)
            {
                MessageBox.Show("Impossible de charger la référence 1");
                return CODE_ERROR;
            }
            else
            {
                if (CCCSA_ReadFile(m_stInfoParam.m_Ref1Path, m_pAcqRef1) != CODE_OK)
                    return CODE_ERROR;
            }

            if (m_stStatuParam.m_bParametersStatus.m_Ref1File != CODE_OK)
            {
                MessageBox.Show("Impossible de charger la référence 2");
                return CODE_ERROR;
            }
            else
            {
                if (CCCSA_ReadFile(m_stInfoParam.m_Ref2Path, m_pAcqRef2) != CODE_OK)
                    return CODE_ERROR;
            }

            return CODE_OK;
        }

        //Configuration and driver
	    //int CCCSA_SendCommands(char *bCmd,int iCmdLength,BOOL bState);

        //Returned configuration value
        //FIXME - Está faltando a função CCCSA_SetConsole, reavaliar o código
        public int CCCSA_GetDllVersion()
        {
            // Variable declaration
            char[] txt = new char[200];
            int iErr = CODE_OK;
            
            // Give version number of DLL
            // m_ccsStop.Lock(); // Lock section
            // Console.WriteLine("Lock get version");
            iErr = CCCSA_SetConsole("Impossible de lire la verion de la DLL", MCHR_GetVersion(txt, 25)); // return error
            // m_ccsStop.Unlock(); // Unlock section
            // Console.WriteLine("Lock get version");
            m_pInfoReport.Add("Version de DLL : " + new string(txt)); // Set current DLL version
            m_szVersion = new string(txt);
            Console.WriteLine("DLL version: " + new string(txt));
            
            return iErr;
        }

        //FIXME - Está faltando CNumero_De_Serie, m_ccsStop.Lock();, CCCSA_SetConsole, MCHR_GetSerialNumber(m_iID, txt, 200), 
        public int CCCSA_GetSerialNumber()
        {
            // Variable declaration
            char[] txt = new char[200];
            CNumero_De_Serie NumDeSerie = new CNumero_De_Serie();
            int iCurrentNum = 0, iErr = CODE_OK;
            int iResponse = -1;

            // Initialize variable
            for (int d = 0; d < 200; d++)
                txt[d] = '\0'; // initialize char

            // Give serial number of the device
            m_ccsStop.Lock();
            Console.WriteLine("Lock thread to get serial number");
            if (CCCSA_SetConsole("Impossible de lire le numéro de série", MCHR_GetSerialNumber(m_iID, txt, 200)) != CODE_OK)
            {
                m_pInfoReport.Add("Numéro de série : inconnu");
                iErr = CODE_ERROR;
            }
            else if (new string(txt).Contains("9999")) // verify if not default value
            {
                do
                {
                    NumDeSerie.DoModal(); // call window
                    iResponse = NumDeSerie.GetSerialNumber(out iCurrentNum); // Give response
                    if (iResponse != 0)
                        MessageBox.Show("Serial number invalide."); // verify current serial
                } while (iResponse != 0); // verify current serial

                CCCSA_SetSerialNumber(iCurrentNum);

                if (CCCSA_SetConsole("Can't have serial number of the device.", MCHR_GetSerialNumber(m_iID, txt, 200)) != CODE_OK)
                {
                    m_pInfoReport.Add("Serial Number : inconnu"); // set invalid serial number
                    iErr = CODE_ERROR;
                }
            }
            m_ccsStop.Unlock();
            Console.WriteLine("Unlock thread to get serial number");

            if (iErr == CODE_OK)
                m_pInfoReport.Add("Numéro de série : " + new string(txt)); // Set current CCS serial number

            return iErr;
        }

        //FIXME - Está faltando CCCSA_SetConsole e MCHR_SendSerialNumber(m_iID, new string(buffer))
        public int CCCSA_SetSerialNumber(int iNumber)
        {
            // char bResponse[100]; // response of CCS
            char[] buffer = new char[10];
            int iErr = CODE_OK;
            sprintf(buffer, "%d", iNumber);
            
            // m_ccsStop.Lock(); // Lock section
            // TRACE(_T("Lock CCCSA_SetSerialNumber\r\n"));
            iErr = CCCSA_SetConsole("Set serial number", MCHR_SendSerialNumber(m_iID, new string(buffer))); // set serial number
            // m_ccsStop.Unlock(); // Unlock section
            // TRACE(_T("Unlock CCCSA_SetSerialNumber\r\n"));
            
            return iErr;
        }

        //Returned CRC value for serial number
        public uint CRC(uint[] ZoneMem, int LngZone)
        {
            uint CrcCalcule = 0;
            while (LngZone > 0)
            {
                CrcCalcule += ZoneMem[0];
                ZoneMem++;
                LngZone--;
            }
            CrcCalcule = 0xffffffff - CrcCalcule; // Crc calculated
            return CrcCalcule;
        }

        //Returned INI configuration file
        //FIXME - Verificar a função : GetPrivateProfileInt
	    public int CCCS_Actions_GetInformation(string bSection, string bkey, ref uint uintData, string bPath)
        {
            //Declaration Variable
            uint uintTmp = 0;
            Console.WriteLine("Get Section: " + bSection + " " + bkey);
            uintTmp = (uint)GetPrivateProfileInt(bSection, bkey, int.MaxValue, bPath);//save current data
            if (uintTmp == int.MaxValue)
            {
                uintData = 0;
                return CODE_ERROR;
            }
            uintData = uintTmp;
            return CODE_OK;
        }

        //FIXME - Verificar a função : GetPrivateProfileString
        public int CCCS_Actions_GetInformation(string bSection, string bkey, StringBuilder bData, int iLength, string bPath)
        {
            Debug.Assert(!string.IsNullOrEmpty(bSection));
            Debug.Assert(!string.IsNullOrEmpty(bkey));
            Debug.Assert(bData != null);
            Debug.Assert(!string.IsNullOrEmpty(bPath));
            //Declaration Variable
            uint uintData;
            bData.Clear();
            Console.WriteLine("Get Section: " + bSection + " " + bkey);
            uintData = GetPrivateProfileString(bSection, bkey, null, bData, iLength, bPath);
            if (uintData == 0 || bData == null)
            {
                return CODE_ERROR;
            }
            return CODE_OK;
        }
        
        //FIXME - Verificar a função : GetPrivateProfileString
        public int CCCS_Actions_GetInformationPath(string bSection, string bkey, out string bData, int iLength, string bPath)
        {
            Debug.Assert(!string.IsNullOrEmpty(bSection));
            Debug.Assert(!string.IsNullOrEmpty(bkey));
            Debug.Assert(!string.IsNullOrEmpty(bPath));
            //Declaration Variable
            uint uintData;
            char[] lData = new char[MAX_CHAR_PATH];
            Array.Clear(lData, 0, MAX_CHAR_PATH);
            StringBuilder dataBuilder = new StringBuilder(MAX_CHAR_PATH);
            Console.WriteLine("Get Section: " + bSection + " " + bkey);
            uintData = GetPrivateProfileString(bSection, bkey, null, lData, iLength, bPath);
            bData = string.Format("{0}\\{1}", m_cstrAppDir, new string(lData));

            if (uintData == 0 || bData == null)
                return CODE_ERROR;

            return CODE_OK;
        }

        //FIXME - Ler e testar as quatro funções a seguir:
        public void CCCSA_VerifyParameters(string cstrMessage, string bpSection, string bpKey, ref string cstrpRtnData, ref int ipParamValid)
        {
            char[] bData = new char[MAX_CHAR_PATH];
            Array.Clear(bData, 0, MAX_CHAR_PATH);
            ipParamValid = CCCSA_SetConsole(cstrMessage, CCCSA_GetInformationPath(bpSection, bpKey, bData, MAX_CHAR_PATH, m_cstrFilePath));
            cstrpRtnData = new string(bData);
        }

        public void CCCSA_VerifyParameters(string cstrMessage, string bpSection, string bpKey, ref float fpRtnData, ref int ipParamValid)
        {
            char[] bData = new char[MAX_CHAR_CMD];
            Array.Clear(bData, 0, MAX_CHAR_CMD);
            ipParamValid = CCCSA_SetConsole(cstrMessage, CCCSA_GetInformation(bpSection, bpKey, bData, MAX_CHAR_CMD, m_cstrFilePath));
            fpRtnData = float.Parse(new string(bData));
        }

        public void CCCSA_VerifyParameters(string cstrMessage, string bpSection, string bpKey, ref int ipRtnData, ref int ipParamValid)
        {
            uint bData = 0;
            ipParamValid = CCCSA_SetConsole(cstrMessage, CCCSA_GetInformation(bpSection, bpKey, ref bData, m_cstrFilePath));
            ipRtnData = (int)bData;
        }

        public void CCCSA_VerifyAppParameters(string cstrMessage, string bpSection, string bpKey, ref int ipRtnData, ref int ipParamValid)
        {
            uint bData = 0;
            ipParamValid = CCCSA_SetConsole(cstrMessage, CCCSA_GetInformation(bpSection, bpKey, ref bData, m_cstrAppFilePath.GetBuffer()));
            ipRtnData = (int)bData;
        }

        //FIXME - Alterações: WideCharToMultiByte para Encoding.Default.GetByteCount
        public string WideCharToAnsi(string wUnicode)
        {
            string szAnsi = null;
            if (!string.IsNullOrEmpty(wUnicode))
            {
                int size = Encoding.Default.GetByteCount(wUnicode);
                byte[] ansiBytes = new byte[size];
                Encoding.Default.GetBytes(wUnicode, 0, wUnicode.Length, ansiBytes, 0);
                szAnsi = Encoding.Default.GetString(ansiBytes);
            }
            return szAnsi;
        }

        //FIXME - Colocar função: static DWORD WINAPI CCCSA_GetDiagnosticThread_ZENITH(LPVOID pParam);

        //FIXME - Revisar
        private static int maxcount = 0;
        private static int count = 0;
        public void CCCSA_GetDiagCallBack_ZENITH(bool b, long Param1, long Param2)
        {
            maxcount = 0;
            count = 0;
            if (Param2 != 0 && b && Param1 == 0)
            {
                Console.WriteLine("GetFileCallBack Param");
            }

            if (b)
            {
                maxcount = (int)Param2;
                count = 0;
                //::PostMessage(m_WindowToNotifyDiagnostic, WM_MS_DIAGNOSTIC_PROGRESS, Param2, 1);
            }
            else
            {
                //::PostMessage(m_WindowToNotifyDiagnostic, WM_MS_DIAGNOSTIC_PROGRESS, Param1, 0);
                count++;
            }
        }

        //FIXME - Revisar: falta CloseZip()
        private int m_hz = 0;
        public bool CloseZipHandle()
        {
            if (m_hz != 0)
            {
                //Operation complete, close the zip
                CloseZip(m_hz);
                m_hz = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        //FIXME - Revisar: tipo HZIP
        public int GetZipHandle()
        {
            return m_hz;
        }

   }
}