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
        /* J'ai trouvé ce struct en rapport.h et MchrType.h de C++ */
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

        /* J'ai modifié CStringArray par List<string> */
        /* CStringArray* m_pInfoReport;//info report */
        List<string> infoReport = new List<string>();

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

        
   }
}