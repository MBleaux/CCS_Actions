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