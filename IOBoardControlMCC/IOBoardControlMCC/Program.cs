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

namespace IOBoardControlMCC
{
    public class IOBoardControlMCC
    {

        bool GeneralError;
        public float RevLevel = 6.73f;
        MccDaq.ErrorInfo NOERRORS = new MccDaq.ErrorInfo();
        MccDaq.ErrorInfo ERRORBOARD= new MccDaq.ErrorInfo(1);

        //Variables requises pour la fonction "FindPortsOfTypes"
        public const int PORTOUT = 1;
        public const int PORTIN = 2;
        public const int PORTDAQOUTSCAN = 3;
        public const int PORTDAQINSCAN = 5;
        public const int PORTOUTSCAN = 5;
        public const int PORTINSCAN = 10;
        public const int BITOUT = 17;
        public const int BITIN = 34;
        public const int FIXEDPORT = 0;
        public const int PROGPORT = 1;
        public const int PROGBIT = 2;
        private int configVal, TypeNum, i;
        private bool DigTypeExists;

        //Variables requises pour la fonction "FindAnalogChansOfType"
        public const int ANALOGINPUT = 1; //Les AnalogType
        public const int ANALOGOUTPUT = 2;
        public const int ANALOGDAQIN = 3;
        public const int ANALOGDAQOUT = 4;
        public const int VOLTAGEINPUT = 5;
        public const int TEMPERATUREIN = 6;
        public const int PRETRIGIN = 9;
        public const int ATRIGIN = 17;


        //Counter
        public const int CTR8254 = 1;
        public const int CTR9513 = 2;
        public const int CTR8536 = 3;
        public const int CTR7266 = 4;
        public const int CTREVENT = 5;
        public const int CTRSCAN = 6;
        public const int CTRTMR = 7;
        public const int CTRQUAD = 8;
        public const int CTRPULSE = 9;


        //AnalogOutput 
        public const int U_ACQ_AIChannel = 1;


        //encoder outputs 
        public const int CNT1U_ENCODER_A_DOChannel = 0;
        public const int CNT1D_ENCODER_B_DOChannel = 1;
        public const int SYNC_IN_24V_DOChannel =2;
        public const int ENCODER_SEL_0_DOChannel =3;
        public const int ENCODER_SEL_1_DOChannel =4;
        public const int ENCODER_SEL_2_DOChannel =5;

        //state
        public const int LED_VERTE_ACQ_DOChannel=6;
        public const int LED_ROUGE_ACQ_DOChannel=7;
        public const int SUP1_ACQ_DIOChannel=8;
        public const int SUP2_ACQ_DIOChannel=9;


        /* Types of configuration information */
        public const int GLOBALINFO = 1;
        public const int BOARDINFO= 2;
        public const int DIGITALINFO= 3;
        public const int COUNTERINFO= 4;
        public const int EXPANSIONINFO =5;
        public const int MISCINFO =6;
        public const int EXPINFOARRAY =7;
        public const int MEMINFO =8;

        /* Types of board configuration information */
        public const int BIBASEADR = 0;        /* Base Address */
        public const int BIBOARDTYPE = 1;      /* Board Type (0x101 - 0x7FFF) */
        public const int BIINTLEVEL = 2;       /* Interrupt level */
        public const int BIDMACHAN = 3;        /* DMA channel */
        public const int BIINITIALIZED = 4;    /* TRUE or FALSE */
        public const int BICLOCK = 5;          /* Clock freq (1, 10 or bus) */
        public const int BIRANGE = 6;          /* Switch selectable range */
        public const int BINUMADCHANS = 7;     /* Number of A/D channels */
        public const int BIUSESEXPS = 8;       /* Supports expansion boards TRUE/FALSE */
        public const int BIDINUMDEVS = 9;      /* Number of digital devices */
        public const int BIDIDEVNUM = 10;      /* Index into digital information */
        public const int BICINUMDEVS = 11;     /* Number of counter devices */
        public const int BICIDEVNUM = 12;      /* Index into counter information */
        public const int BINUMDACHANS = 13;    /* Number of D/A channels */
        public const int BIWAITSTATE = 14;     /* Wait state enabled TRUE/FALSE */
        public const int BINUMIOPORTS = 15;    /* I/O address space used by board */
        public const int BIPARENTBOARD = 16;   /* Board number of parent board */
        public const int BIDTBOARD = 17;       /* Board number of connected DT board */
        public const int BINUMEXPS = 18;       /* Number of EXP boards installed */

        // Counter inputs
        public const int Sync_OUT_ACQ_CTRChannel = 0;     
        public const int Sync_MASTER_ACQ_CTRChannel = 1;

        /* Types of counter device information */
        public const int CIBASEADR = 0;        /* Base address */
        public const int CIINITIALIZED = 1;    /* TRUE or FALSE */
        public const int CICTRTYPE = 2;        /* Counter type 8254, 9513 or 8536 */
        public const int CICTRNUM = 3;         /* Which counter on chip */
        public const int CICONFIGBYTE = 4;     /* Configuration byte */

        /* Trigger output*/
        public const int Sync_IN_ACQ_TMRChannel = 0;

        //Title Information
        public const string CFG_TITLE = "MCC IOBOARD Configuration Message";
        public const string DO_TITLE = "MCC IOBOARD Digital Outputs Message";
        public const string DI_TITLE = "MCC IOBOARD Digital Inputs Message";
        public const string AO_TITLE = "MCC IOBOARD Analog Outputs Message";
        public const string AI_TITLE = "MCC IOBOARD Analog Inputs Message";

        /* Direction*/
        public const int LEFT = 0;
        public const int RIGHT = 1;


        public int ATrigRes;
        public int ATrigRange;
        private MccDaq.MccBoard TestBoard;
        private int ADRes, DARes;
        private MccDaq.Range[] ValidRanges;
        public IOBoardControlMCC() { }



        
        private void InitUL()
        {

            //  Initiate error handling
            //   activating error handling will trap errors like
            //   bad channel numbers and non-configured conditions.
            //   Parameters:
            //     MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
            //     MccDaq.ErrorHandling.StopAll   :if an error is encountered, the program will stop

            clsErrorDefs.ReportError = MccDaq.ErrorReporting.PrintAll;
            clsErrorDefs.HandleError = MccDaq.ErrorHandling.DontStop;
            MccDaq.ErrorInfo ULStat = MccDaq.MccService.ErrHandling
                (clsErrorDefs.ReportError, clsErrorDefs.HandleError);

        }

        private int GetNameOfBoard(MccDaq.MccBoard DaqBoard, ref string BoardName)
        {
            int retVal;
            MccDaq.ErrorInfo ULStat;

            retVal = 1;
            BoardName = "unknown".ToString();
            ULStat = MccDaq.MccService.ErrHandling(MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);
            ULStat = DaqBoard.FlashLED();
            if (ULStat.Value == ErrorInfo.ErrorCode.BadBoardType)
            {
                // FlashLED not supported, use another function
                // to check for live device
                ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction);
                if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
                {
                    DisplayMessage(ULStat);
                    retVal = 0;
                }
            }
            if ((ULStat.Value == ErrorInfo.ErrorCode.BadBoard) | (ULStat.Value == ErrorInfo.ErrorCode.DeadDev)
                | (ULStat.Value == ErrorInfo.ErrorCode.CfgFileNotFound))
            {
                DisplayMessage(ULStat);
                retVal = 0;
            }
            ULStat = MccDaq.MccService.GetBoardName( DaqBoard.BoardNum, ref BoardName);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                retVal = 0;
            }
            return retVal;
        }

        private void DisplayMessage(MccDaq.ErrorInfo Error)
        {
            string ErrMessage;

            if (Error.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                ErrMessage = Error.Message;
                Console.Write("\a\n");
                Console.Write("\n  Error " + (int)Error.Value + " occurred (" + ErrMessage + ")\n");
                if ((Error.Value == ErrorInfo.ErrorCode.BadBoard) | (Error.Value == ErrorInfo.ErrorCode.DeadDev)
                    | (Error.Value == ErrorInfo.ErrorCode.CfgFileNotFound))
                    Console.Write("\n\n  Verify your device is properly installed with Instacal.\n");
                GeneralError = true;
            }
            /*
            else {
                _tprintf(_T("\nPress any key...\n"));
            }
            while (!_kbhit()) {
                Sleep(500);
            }
            //*/
        }

        private void GetNameOfChanType(MccDaq.ChannelType chanType, ref string ChanTypeName)
        {
            switch (chanType)
            {
                case (MccDaq.ChannelType.Analog):
                    ChanTypeName= "ANALOG".ToString();
                    break;
                case (MccDaq.ChannelType.Digital8):
                    ChanTypeName = "DIGITAL8".ToString();
                    break;
                case (MccDaq.ChannelType.Digital16):
                    ChanTypeName = "DIGITAL16".ToString();
                    break;
                case (MccDaq.ChannelType.Ctr16):
                    ChanTypeName = "CTR16".ToString();
                    break;
                case (MccDaq.ChannelType.Ctr32Low):
                    ChanTypeName = "CTR32LOW".ToString();
                    break;
                case (MccDaq.ChannelType.Ctr32High):
                    ChanTypeName = "CTR32HIGH".ToString();
                    break;
                case (MccDaq.ChannelType.CJC):
                    ChanTypeName = "CJC".ToString();
                    break;
                case (MccDaq.ChannelType.TC):
                    ChanTypeName = "TC".ToString();
                    break;
                case (MccDaq.ChannelType.AnalogSE):
                    ChanTypeName = "ANALOGSE".ToString();
                    break;
                case (MccDaq.ChannelType.AnalogDiff):
                    ChanTypeName = "ANALOGDIF".ToString();
                    break;
                case (MccDaq.ChannelType.SetpointStatus):
                    ChanTypeName = "SETPOINTSTATUS".ToString();
                    break;
                case (MccDaq.ChannelType.CtrBank0):
                    ChanTypeName = "CTRBANK0".ToString();
                    break;
                case (MccDaq.ChannelType.CtrBank1):
                    ChanTypeName = "CTRBANK1".ToString();
                    break;
                case (MccDaq.ChannelType.CtrBank2):
                    ChanTypeName = "CTRBANK2".ToString();
                    break;
                case (MccDaq.ChannelType.CtrBank3):
                    ChanTypeName = "CTRBANK3".ToString();
                    break;
                case (MccDaq.ChannelType.PadZero):
                    ChanTypeName = "PADZERO".ToString();
                    break;
                case (MccDaq.ChannelType.Digital):
                    ChanTypeName = "DIGITAL".ToString();
                    break;
                case (MccDaq.ChannelType.Ctr):
                    ChanTypeName = "CTR".ToString();
                    break;
                default :
                    ChanTypeName = "UNKNOWN".ToString();
                    break;
            }
        }

        private void GetNameOfPortType(MccDaq.DigitalPortType PortType, ref string PortTypeName)
        {
            switch (PortType)
            {
                case (MccDaq.DigitalPortType.AuxPort):
                    PortTypeName = "AUXPORT".ToString();
                    break;
                case (MccDaq.DigitalPortType.AuxPort1):
                    PortTypeName = "AUXPORT1".ToString();
                    break;
                case (MccDaq.DigitalPortType.FirstPortA):
                    PortTypeName = "FIRSTPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.FirstPortB):
                    PortTypeName = "FIRSTPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.FirstPortC):
                    PortTypeName = "FIRSTPORTC".ToString();
                    break;
                case (MccDaq.DigitalPortType.FirstPortCH):
                    PortTypeName = "FIRSTPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.SecondPortA):
                    PortTypeName = "SECONDPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.SecondPortB):
                    PortTypeName = "SECONDPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.SecondPortCL):
                    PortTypeName = "SECONDPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.SecondPortCH):
                    PortTypeName = "SECONDPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.ThirdPortA):
                    PortTypeName = "THIRDPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.ThirdPortB):
                    PortTypeName = "THIRDPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.ThirdPortCL):
                    PortTypeName = "THIRDPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.ThirdPortCH):
                    PortTypeName = "THIRDPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.FourthPortA):
                    PortTypeName = "FOURTHPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.FourthPortB):
                    PortTypeName = "FOURTHPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.FourthPortCL):
                    PortTypeName = "FOURTHPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.FourthPortCH):
                    PortTypeName = "FOURTHPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.FifthPortA):
                    PortTypeName = "FIFTHPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.FifthPortB):
                    PortTypeName = "FIFTHPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.FifthPortCL):
                    PortTypeName = "FIFTHPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.FifthPortCH):
                    PortTypeName = "FIFTHPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.SixthPortA):
                    PortTypeName = "SIXTHPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.SixthPortB):
                    PortTypeName = "SIXTHPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.SixthPortCL):
                    PortTypeName = "SIXTHPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.SixthPortCH):
                    PortTypeName = "SIXTHPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.SeventhPortA):
                    PortTypeName = "SEVENTHPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.SeventhPortB):
                    PortTypeName = "SEVENTHPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.SeventhPortCL):
                    PortTypeName = "SEVENTHPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.SeventhPortCH):
                    PortTypeName = "SEVENTHPORTCH".ToString();
                    break;
                case (MccDaq.DigitalPortType.EighthPortA):
                    PortTypeName = "EIGHTHPORTA".ToString();
                    break;
                case (MccDaq.DigitalPortType.EighthPortB):
                    PortTypeName = "EIGHTHPORTB".ToString();
                    break;
                case (MccDaq.DigitalPortType.EighthPortCL):
                    PortTypeName = "EIGHTHPORTCL".ToString();
                    break;
                case (MccDaq.DigitalPortType.EighthPortCH):
                    PortTypeName = "EIGHTHPORTCH".ToString();
                    break;
                default:
                    PortTypeName = "INVALIDPORT".ToString();
                    break;
            }
        }
        public int FindCountersOfType(MccDaq.MccBoard DaqBoard, int CounterType, out int DefaultCtr)
        {
            int NumCounters;
            int ThisType, CounterNum, CtrsFound;
            MccDaq.ErrorInfo ULStat;

            // check supported features by trial 
            // and error with error handling disabled
            ULStat = MccDaq.MccService.ErrHandling
                (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);

            DefaultCtr = -1;
            CtrsFound = 0;
            ULStat = DaqBoard.BoardConfig.GetCiNumDevs(out NumCounters);
            if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
            {
                clsErrorDefs.DisplayError(ULStat);
                return CtrsFound;
            }
            for (int CtrDev = 0; CtrDev < NumCounters; ++CtrDev)
            {
                ULStat = DaqBoard.CtrConfig.GetCtrType(CtrDev, out ThisType);
                if (ThisType == CounterType)
                {
                    ULStat = DaqBoard.CtrConfig.GetCtrNum(CtrDev, out CounterNum);
                    CtrsFound = CtrsFound + 1;
                    if (DefaultCtr == -1)
                    {
                        DefaultCtr = CounterNum;
                    }
                }
            }
            ULStat = MccDaq.MccService.ErrHandling
                (clsErrorDefs.ReportError, clsErrorDefs.HandleError);
            return CtrsFound;
        }

        //Détermine le nombre de port pouvant être passés en type PortType sur la carte (en les programmant).
        public int FindPortsOfType(MccDaq.MccBoard DaqBoard, int PortType,
            out int ProgAbility, out MccDaq.DigitalPortType DefaultPort,
            out int DefaultNumBits, out int FirstBit)

        {
            int ThisType, NumPorts, NumBits;
            int DefaultDev, InMask, OutMask;
            int PortsFound, curCount, curIndex;
            short status;
            bool PortIsCompatible;
            bool CheckBitProg = false;
            MccDaq.DigitalPortType CurPort;
            MccDaq.FunctionType DFunction;
            MccDaq.ErrorInfo ULStat;
            string ConnectionConflict;

            ULStat = MccDaq.MccService.ErrHandling
                (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);

            ConnectionConflict = "This network device is in use by another process or user." +
               System.Environment.NewLine + System.Environment.NewLine +
               "Check for other users on the network and close any applications " +
               System.Environment.NewLine +
               "(such as Instacal) that may be accessing the network device.";

            DefaultPort = (MccDaq.DigitalPortType)(-1);
            CurPort = DefaultPort;
            PortsFound = 0;
            FirstBit = 0;
            ProgAbility = -1;
            DefaultNumBits = 0;
            ULStat = DaqBoard.BoardConfig.GetDiNumDevs(out NumPorts);
            if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
            {
                clsErrorDefs.DisplayError(ULStat);
                return PortsFound;
            }

            if ((PortType == BITOUT) || (PortType == BITIN))
                CheckBitProg = true;
            if ((PortType == PORTOUTSCAN) || (PortType == PORTINSCAN))
            {
                if (NumPorts > 0)
                {
                    DFunction = MccDaq.FunctionType.DiFunction;
                    if (PortType == PORTOUTSCAN)
                    {
                        ULStat = DaqBoard.GetConfig(2, 0, 378, out configVal);
                        DigTypeExists = false;
                        if (configVal == 0)
                            NumPorts = 0;
                        else
                        {
                            for (i = 0; i < configVal; i++)
                            {
                                ULStat = DaqBoard.GetConfig(2, i, 379, out TypeNum);
                                if (TypeNum == 1) DigTypeExists = true;
                                if (TypeNum == 2) DigTypeExists = true;
                                if (TypeNum == 16) DigTypeExists = true;
                            }
                            if (!DigTypeExists) NumPorts = 0;
                        }
                        ULStat = DaqBoard.GetStatus(out status, out curCount, out curIndex, DFunction);
                        if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                            NumPorts = 0;
                    }
                }
                PortType = PortType & (PORTOUT | PORTIN);
            }

            for (int DioDev = 0; DioDev < NumPorts; ++DioDev)
            {
                ProgAbility = -1;
                ULStat = DaqBoard.DioConfig.GetDInMask(DioDev, out InMask);
                ULStat = DaqBoard.DioConfig.GetDOutMask(DioDev, out OutMask);
                if ((InMask & OutMask) > 0)
                    ProgAbility = FIXEDPORT;
                ULStat = DaqBoard.DioConfig.GetDevType(DioDev, out ThisType);
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                    CurPort = (DigitalPortType)Enum.Parse(typeof(DigitalPortType),
                        ThisType.ToString());
                if ((DioDev == 0) && (CurPort == MccDaq.DigitalPortType.FirstPortCL))
                    //a few devices (USB-SSR08 for example)
                    //start at FIRSTPORTCL and number the bits
                    //as if FIRSTPORTA and FIRSTPORTB exist for
                    //compatibiliry with older digital peripherals
                    FirstBit = 16;

                //check if port is set for requested direction 
                //or can be programmed for requested direction
                PortIsCompatible = false;
                switch (PortType)
                {
                    case (PORTOUT):
                        if (OutMask > 0)
                            PortIsCompatible = true;
                        break;
                    case (PORTIN):
                        if (InMask > 0)
                            PortIsCompatible = true;
                        break;
                    default:
                        PortIsCompatible = false;
                        break;
                }
                PortType = (PortType & (PORTOUT | PORTIN));
                if (!PortIsCompatible)
                {
                    if (ProgAbility != FIXEDPORT)
                    {
                        MccDaq.DigitalPortDirection ConfigDirection;
                        ConfigDirection = DigitalPortDirection.DigitalOut;
                        if (PortType == PORTIN)
                            ConfigDirection = DigitalPortDirection.DigitalIn;
                        if ((CurPort == MccDaq.DigitalPortType.AuxPort) && CheckBitProg)
                        {
                            //if it's an AuxPort, check bit programmability
                            ULStat = DaqBoard.DConfigBit(MccDaq.DigitalPortType.AuxPort,
                                FirstBit, ConfigDirection);
                            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                            {
                                //return port to input mode
                                ULStat = DaqBoard.DConfigBit(MccDaq.DigitalPortType.AuxPort,
                                    FirstBit, DigitalPortDirection.DigitalIn);
                                ProgAbility = PROGBIT;
                            }
                            else
                            {
                                if ((ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUseByAnotherProc)
                                    || (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUse))
                                {
                                    System.Windows.Forms.MessageBox.Show(ConnectionConflict, "Device In Use");
                                    break;
                                }
                            }
                        }
                        if (ProgAbility == -1)
                        {
                            //check port programmability
                            ULStat = DaqBoard.DConfigPort(CurPort, ConfigDirection);
                            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                            {
                                //return port to input mode
                                ULStat = DaqBoard.DConfigBit(MccDaq.DigitalPortType.AuxPort,
                                    FirstBit, DigitalPortDirection.DigitalIn);
                                ProgAbility = PROGPORT;
                            }
                            else
                            {
                                if ((ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUseByAnotherProc)
                                    || (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUse))
                                {
                                    System.Windows.Forms.MessageBox.Show(ConnectionConflict, "Device In Use");
                                    break;
                                }
                            }
                        }
                    }
                    PortIsCompatible = !(ProgAbility == -1);
                }

                if (PortIsCompatible)
                    PortsFound = PortsFound + 1;
                int BitVals, BitWeight;
                int TotalVal, CurBit;
                if (DefaultPort == (MccDaq.DigitalPortType)(-1))
                {
                    ULStat = DaqBoard.DioConfig.GetNumBits(DioDev, out NumBits);
                    if (ProgAbility == FIXEDPORT)
                    {
                        //could have different number of input and output bits
                        CurBit = 0;
                        TotalVal = 0;
                        BitVals = OutMask;
                        if (PortType == PORTIN) BitVals = InMask;
                        do
                        {
                            BitWeight = (int)Math.Pow(2, CurBit);
                            TotalVal = BitWeight + TotalVal;
                            CurBit = CurBit + 1;
                        } while (TotalVal < BitVals);
                        NumBits = CurBit;
                    }
                    DefaultNumBits = NumBits;
                    DefaultDev = DioDev;
                    DefaultPort = CurPort;
                }
                if (ProgAbility == PROGBIT) break;
            }
            ULStat = MccDaq.MccService.ErrHandling
                (clsErrorDefs.ReportError, clsErrorDefs.HandleError);
            return PortsFound;
        }

        public int FindAnalogChansOfType(MccDaq.MccBoard DaqBoard,
            int AnalogType, out int Resolution, out MccDaq.Range DefaultRange,
            out int DefaultChan, out MccDaq.TriggerType DefaultTrig)
        {
            short status;
            int curCount, curIndex;
            int ChansFound, IOType;
            MccDaq.FunctionType functionType;
            bool CheckPretrig, CheckATrig = false;
            bool RangeFound;
            MccDaq.Range TestRange, HardRange;
            MccDaq.ErrorInfo ULStat;

            // check supported features by trial 
            // and error with error handling disabled
            ULStat = MccDaq.MccService.ErrHandling
                (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);

            TestBoard = DaqBoard;
            ATrigRes = 0;
            DefaultChan = 0;
            DefaultTrig = TriggerType.TrigPosEdge;
            DefaultRange = MccDaq.Range.NotUsed;
            Resolution = 0;
            IOType = (AnalogType & 3);
            switch (IOType)
            {
                case ANALOGDAQIN:
                case ANALOGINPUT:
                    // Get the number of A/D channels
                    ULStat = DaqBoard.BoardConfig.GetNumAdChans(out ChansFound);
                    if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                    {
                        clsErrorDefs.DisplayError(ULStat);
                        return ChansFound;
                    }
                    if (ChansFound > 0)
                    {
                        // Get the resolution of A/D
                        ULStat = DaqBoard.BoardConfig.GetAdResolution(out ADRes);
                        if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                            Resolution = ADRes;
                        // check ranges for a valid default
                        ULStat = DaqBoard.BoardConfig.GetRange(out HardRange);
                        RangeFound = ((HardRange != MccDaq.Range.NotUsed)
                            & (HardRange != (MccDaq.Range)300)
                            & (HardRange != (MccDaq.Range)301));
                        if (!(RangeFound))
                            RangeFound = TestInputRanges(out TestRange);
                        else
                            TestRange = HardRange;
                        if (RangeFound) DefaultRange = TestRange;
                        if (IOType == ANALOGDAQIN)
                        {
                            functionType = FunctionType.DaqiFunction;
                            ULStat = DaqBoard.GetStatus(out status,
                                out curCount, out curIndex, functionType);
                            if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                            {
                                return 0;
                            }
                        }
                    }
                    break;
                default:
                    // Get the number of D/A channels
                    ULStat = DaqBoard.BoardConfig.GetNumDaChans(out ChansFound);
                    if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                    {
                        clsErrorDefs.DisplayError(ULStat);
                        return ChansFound;
                    }
                    if (ChansFound > 0)
                    {
                        ULStat = TestBoard.GetConfig(2, 0, 292, out DARes);
                        Resolution = DARes;
                        RangeFound = TestOutputRanges(out TestRange);
                        if (RangeFound) DefaultRange = TestRange;
                        if (IOType == ANALOGDAQOUT)
                        {
                            functionType = FunctionType.DaqoFunction;
                            ULStat = DaqBoard.GetStatus(out status,
                                out curCount, out curIndex, functionType);
                            if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                            {
                                return 0;
                            }
                        }
                    }
                    break;
            }

            CheckATrig = ((AnalogType & ATRIGIN) == ATRIGIN);
            if ((ChansFound > 0) & CheckATrig)
            {
                ULStat = DaqBoard.SetTrigger(MccDaq.TriggerType.TrigAbove, 0, 0);
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                {
                    DefaultTrig = MccDaq.TriggerType.TrigAbove;
                    GetTrigResolution();
                }
                else
                    ChansFound = 0;
            }

            CheckPretrig = ((AnalogType & PRETRIGIN) == PRETRIGIN);
            if ((ChansFound > 0) & CheckPretrig)
            {
                // if DaqSetTrigger supported, trigger type is analog
                ULStat = DaqBoard.DaqSetTrigger(MccDaq.TriggerSource.TrigImmediate,
                    MccDaq.TriggerSensitivity.AboveLevel, 0, MccDaq.ChannelType.Analog,
                    DefaultRange, 0.0F, 0.1F, MccDaq.TriggerEvent.Start);
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                    DefaultTrig = MccDaq.TriggerType.TrigAbove;
                else
                {
                    ULStat = DaqBoard.SetTrigger(MccDaq.TriggerType.TrigPosEdge, 0, 0);
                    if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                        DefaultTrig = MccDaq.TriggerType.TrigPosEdge;
                    else
                        ChansFound = 0;
                }
            }
            ULStat = MccDaq.MccService.ErrHandling
                (clsErrorDefs.ReportError, clsErrorDefs.HandleError);
            return ChansFound;
        }

        private bool TestInputRanges(out MccDaq.Range DefaultRange)
        {
            short dataValue;
            int dataHRValue, Options, index;
            MccDaq.ErrorInfo ULStat;
            MccDaq.Range TestRange;
            bool RangeFound = false;
            string ConnectionConflict;

            ConnectionConflict = "This network device is in use by another process or user." +
               System.Environment.NewLine + System.Environment.NewLine +
               "Check for other users on the network and close any applications " +
               System.Environment.NewLine +
               "(such as Instacal) that may be accessing the network device.";

            ValidRanges = new MccDaq.Range[49];
            DefaultRange = MccDaq.Range.NotUsed;
            TestRange = MccDaq.Range.NotUsed;
            Options = 0;
            index = 0;
            foreach (int i in Enum.GetValues(TestRange.GetType()))
            {
                TestRange = (MccDaq.Range)i;
                if (ADRes > 16)
                    ULStat = TestBoard.AIn32(0, TestRange, out dataHRValue, Options);
                else
                    ULStat = TestBoard.AIn(0, TestRange, out dataValue);
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                {
                    if (DefaultRange == MccDaq.Range.NotUsed)
                        DefaultRange = TestRange;
                    ValidRanges.SetValue(TestRange, index);
                    index = index + 1;
                    RangeFound = true;
                }
                else
                {
                    if ((ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUseByAnotherProc)
                        || (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUse))
                    {
                        System.Windows.Forms.MessageBox.Show(ConnectionConflict, "Device In Use");
                        break;
                    }
                }
            }
            Array.Resize(ref ValidRanges, index);
            return RangeFound;

        }

        private bool TestOutputRanges(out MccDaq.Range DefaultRange)
        {
            short dataValue = 0;
            MccDaq.ErrorInfo ULStat;
            MccDaq.Range TestRange;
            bool RangeFound = false;
            int configVal;
            string ConnectionConflict;

            ConnectionConflict = "This network device is in use by another process or user." +
               System.Environment.NewLine + System.Environment.NewLine +
               "Check for other users on the network and close any applications " +
               System.Environment.NewLine +
               "(such as Instacal) that may be accessing the network device.";

            DefaultRange = MccDaq.Range.NotUsed;
            TestRange = (MccDaq.Range)(-5);
            ULStat = TestBoard.AOut(0, TestRange, dataValue);
            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
            {
                ULStat = TestBoard.GetConfig(2, 0, 114, out configVal);
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                {
                    DefaultRange = (MccDaq.Range)configVal;
                    RangeFound = true;
                }
            }
            else
            {
                TestRange = MccDaq.Range.NotUsed;
                foreach (int i in Enum.GetValues(TestRange.GetType()))
                {
                    TestRange = (MccDaq.Range)i;
                    ULStat = TestBoard.AOut(0, TestRange, dataValue);
                    if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                    {
                        if (DefaultRange == MccDaq.Range.NotUsed)
                            DefaultRange = TestRange;
                        RangeFound = true;
                        break;
                    }
                    else
                    {
                        if ((ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUseByAnotherProc)
                            || (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NetDevInUse))
                        {
                            System.Windows.Forms.MessageBox.Show(ConnectionConflict, "Device In Use");
                            break;
                        }
                    }
                }
            }
            return RangeFound;

        }

        private void GetTrigResolution()
        {
            int BoardID, TrigSource;
            MccDaq.ErrorInfo ULStat;

            ULStat = TestBoard.GetConfig(2, 0, 209, out TrigSource);
            ULStat = TestBoard.BoardConfig.GetBoardType(out BoardID);

            switch (BoardID)
            {
                case 95:
                case 96:
                case 97:
                case 98:
                case 102:
                case 165:
                case 166:
                case 167:
                case 168:
                case 177:
                case 178:
                case 179:
                case 180:
                case 203:
                case 204:
                case 205:
                case 213:
                case 214:
                case 215:
                case 216:
                case 217:
                    {
                        //PCI-DAS6030, 6031, 6032, 6033, 6052
                        //USB-1602HS, 1602HS-2AO, 1604HS, 1604HS-2AO
                        //PCI-2511, 2513, 2515, 2517, USB-2523, 2527, 2533, 2537
                        //USB-1616HS, 1616HS-2, 1616HS-4, 1616HS-BNC
                        ATrigRes = 12;
                        ATrigRange = 20;
                        if (TrigSource > 0) ATrigRange = -1;
                    }
                    break;
                case 101:
                case 103:
                case 104:
                    {
                        //PCI-DAS6040, 6070, 6071
                        ATrigRes = 8;
                        ATrigRange = 20;
                        if (TrigSource > 0) ATrigRange = -1;
                    }
                    break;
                default:
                    {
                        ATrigRes = 0;
                        ATrigRange = -1;
                    }
                    break;
            }
        }


        public MccDaq.Range[] GetRangeList()
        {
            MccDaq.Range DefaultRange;
            MccDaq.ErrorInfo ULStat;

            // check supported ranges by trial 
            // and error with error handling disabled
            ULStat = MccDaq.MccService.ErrHandling
                (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);

            TestInputRanges(out DefaultRange);

            ULStat = MccDaq.MccService.ErrHandling
                (clsErrorDefs.ReportError, clsErrorDefs.HandleError);
            return ValidRanges;
        }

        short CIOBCTRL_TestAllIOType(MccDaq.MccBoard DaqBoard)
        {
            
            string BoardName="";
            int ctrType;
            int numCounters;
            int defaultCtr;
            int PortType;
            int NumPorts;
            int ProgAbility;
            MccDaq.DigitalPortType PortNum;
            int NumBits;
            int FirstBit;
            int ChanType;
            int NumAIChans;
            int Resolution;
            MccDaq.Range Range;
            int DefaultChan;
            MccDaq.TriggerType DefaultTrig;
            string TypeName="";


            /* Declare UL Revision Level */
            MccDaq.ErrorInfo ULStat = MccService.DeclareRevision(ref RevLevel);

            

            InitUL();   // Set up error handling

            // get the name of the board
            if (GetNameOfBoard(DaqBoard, ref BoardName)==1) //Si aucune erreur à la récupération du nom de la carte
            {
                MessageBox.Show("get the name of the MEASUREMENT COMPUTING CORP board", "LECTURE NOM CARTE");
                DisplayMessage(NOERRORS);
            }
            for (int i = CTR8254; i <= CTRPULSE; i++)
            {
                ctrType = i;
                numCounters = FindCountersOfType(DaqBoard, ctrType, out defaultCtr);
                if (numCounters == 0)
                {
                    Console.Write("{0:s} (board {1:d}) does not have Event counters.\n", BoardName, DaqBoard.BoardNum);
                    DisplayMessage( NOERRORS );
                    //return 0;
                }
                else
                {
                    Console.Write("{0:s} (board {1:d}) have {2:d} counters of type {3:d}.\n", BoardName, DaqBoard.BoardNum, numCounters, ctrType);
                    DisplayMessage(NOERRORS);
                }
            }
            var Digital_PortType_map = new Dictionary<int,int>();
            Digital_PortType_map.Add(0, PORTOUT);
            Digital_PortType_map.Add(1, PORTIN);
            Digital_PortType_map.Add(2, PORTOUTSCAN);
            Digital_PortType_map.Add(3, PORTDAQOUTSCAN);
            Digital_PortType_map.Add(4, PORTINSCAN);
            Digital_PortType_map.Add(5, PORTDAQINSCAN);
            Digital_PortType_map.Add(6, BITOUT);
            Digital_PortType_map.Add(7, BITIN);
            for (int i = 0; i < Digital_PortType_map.Count; i++)
            {
                Digital_PortType_map.TryGetValue(i,out PortType);
                NumPorts = FindPortsOfType(DaqBoard, PortType, out ProgAbility,
                    out PortNum, out NumBits, out FirstBit);
                GetNameOfPortType((MccDaq.DigitalPortType) PortType, ref TypeName);
                if (NumPorts == 0)
                {
                    Console.Write("{0:s} (board {1:d}) has no compatible digital ports.\n", BoardName, DaqBoard.BoardNum);
                    DisplayMessage(NOERRORS);
                    //return 0;
                }
                else
                {
                    Console.Write("{0:s} (board {1:d}) have {2:d} digital ports of type {3:s}.\n", BoardName, DaqBoard.BoardNum, NumPorts, TypeName);
                    //DisplayMessage(NOERRORS);
                }
            }

           var Analog_PortType_map =  new Dictionary<int, int>(); ;
            Analog_PortType_map.Add(0, ANALOGINPUT);
            Analog_PortType_map.Add(1, ANALOGOUTPUT);
            Analog_PortType_map.Add(2, ANALOGDAQIN);
            Analog_PortType_map.Add(3, ANALOGDAQOUT);
            Analog_PortType_map.Add(4, VOLTAGEINPUT);
            Analog_PortType_map.Add(5, TEMPERATUREIN);
            Analog_PortType_map.Add(6, PRETRIGIN);
            Analog_PortType_map.Add(7, ATRIGIN);
            for (int i = 0; i < Analog_PortType_map.Count(); i++)
            {
                Analog_PortType_map.TryGetValue(i, out ChanType);
                NumAIChans = FindAnalogChansOfType(DaqBoard, ChanType, out Resolution, out Range, out DefaultChan, out DefaultTrig);
                GetNameOfChanType((ChannelType)ChanType, ref TypeName);
                if (NumAIChans == 0)
                {
                    Console.Write("{0:s} (board {1:d}) has no compatible analog channel.\n", BoardName, DaqBoard.BoardNum);
                    DisplayMessage(NOERRORS);
                    //return 0;
                }
                else
                {
                    Console.Write("{0:s} (board {1:d}) have {2:d} analog channel of type {3:d}.\n", BoardName, DaqBoard.BoardNum, NumAIChans, TypeName);
                    //DisplayMessage(NOERRORS);
                }
            }
            return 0;
        }
        short CIOBCTRL_GetTensionAI(MccDaq.MccBoard DaqBoard, int Chan, ref float _Tension)
        {
            float TensionV;
            short Tension, ScaledTension;
            MccDaq.ErrorInfo ULStat;
            int NumAIChans, Resolution, DefaultChan;
            MccDaq.Range Range;
            MccDaq.TriggerType DefaultTrig;

            NumAIChans = FindAnalogChansOfType(DaqBoard, ANALOGINPUT, out Resolution, out Range, out DefaultChan, out DefaultTrig);
            if (Chan < NumAIChans)
            {

                ULStat = DaqBoard.AIn(Chan, Range, out Tension);
                if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
                {
                    DisplayMessage(ULStat);
                    return 0;
                }
                ULStat = DaqBoard.ToEngUnits(Range, Tension, out TensionV);
                if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
                {
                    DisplayMessage(ULStat);
                    return 0;
                }
                ScaledTension = (short) (TensionV * 10);
                _Tension = TensionV;
            }
            return 0;
        }

        short CIOBCTRL_TestAI(MccDaq.MccBoard DaqBoard, int Chan)   //Lecture d'une entrée en continu pour test
        {
            float TensionV;
            short Tension, ScaledTension;
            MccDaq.ErrorInfo ULStat;
            int NumAIChans, Resolution, DefaultChan;
            MccDaq.Range Range;
            MccDaq.TriggerType DefaultTrig;

            NumAIChans = FindAnalogChansOfType(DaqBoard, ANALOGINPUT, out Resolution, out Range, out DefaultChan, out DefaultTrig);
            if (Chan < NumAIChans)
            {
                while(true)
                {
                    ULStat = DaqBoard.AIn(Chan, Range, out Tension);
                    if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
                    {
                        DisplayMessage(ULStat);
                        return 0;
                    }
                    ULStat = DaqBoard.ToEngUnits(Range, Tension, out TensionV);
                    if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
                    {
                        DisplayMessage(ULStat);
                        return 0;
                    }
                    ScaledTension = (short)(TensionV * 10);
                    Console.WriteLine("Tension sur la channel N°{0:d} = {1:F}",Chan, TensionV); 
                }
                
            }
            return 0;
        }

        //Private Set bit on output
        MccDaq.ErrorInfo CIOBCTRL_Set_Digital_Output(MccBoard DaqBoard,short u16DOChannel, bool bState)
        {
            //Variable Declaration
            MccDaq.ErrorInfo UlStat = NOERRORS;
            ///*
            if (bState == true)
            {//set to high state the output
                UlStat = CIOBCTRL_DOset_singlebit(DaqBoard, u16DOChannel); // set output data
            }
            else
            {
                UlStat = CIOBCTRL_DOclear_singlebit(DaqBoard,u16DOChannel); // set output data
            }
            //*/
            return UlStat;
        }

        // Function to set DO single bit
        MccDaq.ErrorInfo CIOBCTRL_DOset_singlebit(MccBoard DaqBoard , short u16DOChannel)
        {
            // write current port
            //TRACE(_T("cbDBitOut process\n"));
            MccDaq.ErrorInfo ULStat=NOERRORS;
            ULStat = DaqBoard.DBitOut(DigitalPortType.AuxPort, u16DOChannel, MccDaq.DigitalLogicState.High);
            if (ULStat.Value != MccDaq.ErrorInfo.ErrorCode.NoErrors)
            {
                MessageBox.Show(("cbDBitOut error"), DO_TITLE);
            }
            return ULStat;
        }


        // Function to clear DO single bit
        MccDaq.ErrorInfo CIOBCTRL_DOclear_singlebit(MccBoard DaqBoard, short u16DOChannel)
        {
            MccDaq.ErrorInfo ULStat=NOERRORS;
            // select bit function
            //TRACE(_T("DOclear_singlebit->cbDBitOut process\n"));
            ULStat = DaqBoard.DBitOut(DigitalPortType.AuxPort, u16DOChannel, MccDaq.DigitalLogicState.Low);
            if (ULStat.Value != MccDaq.ErrorInfo.ErrorCode.NoErrors)
            {
                MessageBox.Show(("cbDBitOut error"), DO_TITLE);
            }
            return ULStat;
        }


        //Set trigger synchronisation
        MccDaq.ErrorInfo CIOBCTRL_Set_Trigger(MccBoard DaqBoard, bool bState)
        {
            //Variable Declaration
            MccDaq.ErrorInfo i16Err = NOERRORS;
            i16Err = CIOBCTRL_Set_Digital_Output(DaqBoard, SYNC_IN_24V_DOChannel, bState);//Call Function
            return i16Err;
        }

        MccDaq.ErrorInfo CIOBCTRL_SendTriggerPulse(MccBoard DaqBoard)
        {
            //Declaration
            Console.WriteLine("Envoi de l'impulsion trigger\r");
            MccDaq.ErrorInfo gg = CIOBCTRL_Set_Trigger(DaqBoard,false);
            if (gg != NOERRORS)
            {
                MessageBox.Show("Erreur ecriture trigger true");
                return gg;
            }
            Thread.Sleep(3000);
            gg = CIOBCTRL_Set_Trigger(DaqBoard,true);
            if (gg != NOERRORS)
            {
                MessageBox.Show("Erreur ecriture trigger false");
            }
            return gg;
        }


        MccDaq.ErrorInfo CIOBCTRL_StartSendTriggerPulses(MccBoard DaqBoard,double frequency, double dutyCycle, int pulseCount)
        {
            //Variable Declaration
            double initialDelay = 0;
            MccDaq.IdleState idleState = IdleState.Low;
            MccDaq.PulseOutOptions options = PulseOutOptions.Default;

            Console.WriteLine("Début d'envoi de l'impulsion trigger\r");
            MccDaq.ErrorInfo ULStat = DaqBoard.PulseOutStart(Sync_IN_ACQ_TMRChannel,ref frequency,ref dutyCycle, pulseCount, ref initialDelay, idleState, options);
            if (ULStat != NOERRORS)
            {
                MessageBox.Show(("cbPulseOutStart error"), DO_TITLE);
                return ULStat;
            }
            return ULStat;
        }

        MccDaq.ErrorInfo CIOBCTRL_StopSendTriggerPulses(MccBoard DaqBoard)
        {
            //Variable Declaration


            Console.WriteLine("Fin d'envoi de l'impulsion trigger\r");
            MccDaq.ErrorInfo ULStat = DaqBoard.PulseOutStop(Sync_IN_ACQ_TMRChannel);
            if (ULStat != NOERRORS)
            {
                MessageBox.Show(("cbPulseOutStop error"), DO_TITLE);
                return ULStat;
            }
            return NOERRORS;
        }
        MccDaq.ErrorInfo CIOBCTRL_DOCnt(MccBoard DaqBoard, int u16CntChannel, int u16NbrCount, int u16TSleep, int u16TActivated, int u16Direction)
        {
            //Variable Declaration
            MccDaq.ErrorInfo i16Err = NOERRORS;
            ushort u16FirstChannel = CNT1U_ENCODER_A_DOChannel, u16SecondChannel = CNT1D_ENCODER_B_DOChannel;

            if (u16Direction == LEFT)
            {
                switch (u16CntChannel)
                {//Counter Channel Verification
                    default:
                        u16FirstChannel = CNT1U_ENCODER_A_DOChannel;
                        u16SecondChannel = CNT1D_ENCODER_B_DOChannel;
                        break;
                }
            }
            else
            {
                switch (u16CntChannel)
                {//Counter Channel Verification
                    default:
                        u16FirstChannel = CNT1D_ENCODER_B_DOChannel;
                        u16SecondChannel = CNT1U_ENCODER_A_DOChannel;
                        break;
                }
            }

            //change current value
            for (int i = 0; i < u16NbrCount; i++)
            {
                i16Err = DaqBoard.DBitOut(DigitalPortType.AuxPort, u16FirstChannel, MccDaq.DigitalLogicState.High);
                i16Err = DaqBoard.DBitOut(DigitalPortType.AuxPort, u16SecondChannel, MccDaq.DigitalLogicState.High);
                i16Err = DaqBoard.DBitOut(DigitalPortType.AuxPort, u16FirstChannel, MccDaq.DigitalLogicState.Low);
                i16Err = DaqBoard.DBitOut(DigitalPortType.AuxPort, u16SecondChannel, MccDaq.DigitalLogicState.Low);
            }
            return i16Err;
        }

        MccDaq.ErrorInfo CIOBCTRL_Clear_SyncOut_Count(MccBoard DaqBoard)
        {
            //Variable Declaration
            MccDaq.ErrorInfo ULStat = DaqBoard.CClear(Sync_OUT_ACQ_CTRChannel);
            return ULStat;
        }
        MccDaq.ErrorInfo CIOBCTRL_Read_SyncOut_Count(MccBoard DaqBoard,int iPulseCount)
        {
            //Variable Declaration
            MccDaq.ErrorInfo ULStat = DaqBoard.CIn32(Sync_OUT_ACQ_CTRChannel, out iPulseCount);
            return ULStat;
        }
        MccDaq.ErrorInfo CIOBCTRL_Clear_SyncMaster_Count(MccBoard DaqBoard)
        {
            //Variable Declaration
            MccDaq.ErrorInfo ULStat = DaqBoard.CClear(Sync_MASTER_ACQ_CTRChannel);
            return ULStat;
        }

        MccDaq.ErrorInfo CIOBCTRL_Read_SyncMaster_Count(MccBoard DaqBoard,int iPulseCount)
        {
            //Variable Declaration
            MccDaq.ErrorInfo ULStat = DaqBoard.CIn32(Sync_OUT_ACQ_CTRChannel, out iPulseCount);
            return ULStat;
        }

        MccDaq.ErrorInfo CIOBCTRL_Config(MccDaq.MccBoard DaqBoard,ref string BoardName)
        {
            // init variable
            string PortTypeName="";
            int PortType, NumPorts, ProgAbility, DefaultNumBits, FirstBit, numCounters, ctrType, defaultCtr, NumAIChans, Resolution, DefaultChan;
            DigitalPortType DefaultPort;
            MccDaq.DigitalPortDirection Direction;
            MccDaq.Range Range;
            MccDaq.TriggerType DefaultTrig;

            /* Declare UL Revision Level */
            MccDaq.ErrorInfo ULStat = MccService.DeclareRevision(ref RevLevel);

            InitUL();   // Set up error handling

            // get the name of the board
            if (GetNameOfBoard(DaqBoard,ref BoardName)==1)
            {
                MessageBox.Show("get the name of the MEASUREMENT COMPUTING CORP board", "LECTURE NOM CARTE");
                DisplayMessage(NOERRORS);
            }
            ULStat = DaqBoard.AInputMode(MccDaq.AInputMode.SingleEnded);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }

            // Determine if device is compatible with this example
            PortType = PORTOUT;
            NumPorts = FindPortsOfType(DaqBoard, PortType, 
                out ProgAbility, out DefaultPort, out DefaultNumBits, out FirstBit);
            if (NumPorts == 0)
            {
                Console.WriteLine("{ 0:s} (board { 1:d}) has no compatible digital ports.", BoardName, DaqBoard.BoardNum);
                DisplayMessage(NOERRORS);
                return ERRORBOARD;
            }
            GetNameOfPortType(DefaultPort,ref PortTypeName);

            
            Direction = DigitalPortDirection.DigitalOut;
            ULStat = DaqBoard.DConfigBit(DefaultPort, CNT1U_ENCODER_A_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, CNT1D_ENCODER_B_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, SYNC_IN_24V_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, ENCODER_SEL_0_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, ENCODER_SEL_1_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, ENCODER_SEL_2_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, LED_VERTE_ACQ_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            ULStat = DaqBoard.DConfigBit(DefaultPort, LED_ROUGE_ACQ_DOChannel, Direction);
            if (ULStat.Value != ErrorInfo.ErrorCode.NoErrors)
            {
                DisplayMessage(ULStat);
                return ERRORBOARD;
            }
            //encoder outputs 
            //CNT1U_ENCODER_A_DOChannel 0
            //CNT1D_ENCODER_B_DOChannel 1
            //SYNC_IN_24V_DOChannel 2
            //ENCODER_SEL_0_DOChannel 3
            //ENCODER_SEL_1_DOChannel 4
            //ENCODER_SEL_2_DOChannel 5
            //
            //LED_VERTE_ACQ_DOChannel 6
            //LED_ROUGE_ACQ_DOChannel 7
            //SUP1_ACQ_DIOChannel 8
            //SUP2_ACQ_DIOChannel 9
            //Direction = DIGITALIN;
            //ULStat = cbDConfigBit(BoardNum, PortNum, TRGIGGER_OUT_DIChannel, Direction);
            //if (ULStat != NOERRORS) {
            //	DisplayMessage(ULStat);
            //	return 0;
            //}
            ULStat = DaqBoard.GetConfig(BOARDINFO, 0, BICINUMDEVS, out numCounters);
            if (ULStat != NOERRORS)
            {
                numCounters = 0;
                return ERRORBOARD;
            }
            int CtrDev;
            bool bDo;
            ctrType = CTREVENT;
            for (CtrDev = 0; CtrDev < numCounters; CtrDev++)
            {
                bDo = false;
                if (CtrDev == Sync_OUT_ACQ_CTRChannel)
                {
                    bDo = true;
                }
                if (CtrDev == Sync_MASTER_ACQ_CTRChannel)
                {
                    bDo = true;
                }
                if (bDo)
                {
                    ULStat = DaqBoard.GetConfig(COUNTERINFO, CtrDev, CICTRTYPE, out ctrType);
                    if (ctrType == CTREVENT)
                    {
                        ULStat = DaqBoard.GetConfig(COUNTERINFO, CtrDev, CICTRNUM, out defaultCtr);
                    }
                }
            }
            ctrType = CTRTMR;
            for (CtrDev = 0; CtrDev < numCounters; CtrDev++)
            {
                bDo = false;
                if (CtrDev == Sync_IN_ACQ_TMRChannel)
                {
                    bDo = true;
                }
                if (bDo)
                {
                    ULStat = DaqBoard.GetConfig(COUNTERINFO, CtrDev, CICTRTYPE, out ctrType);
                    if (ctrType == CTREVENT)
                    {
                        ULStat = DaqBoard.GetConfig(COUNTERINFO, CtrDev, CICTRNUM, out defaultCtr);
                    }
                }
            }
            // Determine if device is compatible with this example
            NumAIChans = FindAnalogChansOfType(DaqBoard, ANALOGINPUT, out Resolution, out Range, out DefaultChan, out DefaultTrig);
            if (NumAIChans == 0)
            {
                Console.WriteLine("{0:s} (board {1:u}) does not have analog input channels.", BoardName, DaqBoard.BoardNum);
                DisplayMessage(NOERRORS);
                return ERRORBOARD;
            }
            DaqBoard.PulseOutStop(Sync_IN_ACQ_TMRChannel);
            ULStat = DaqBoard.PulseOutStop(Sync_OUT_ACQ_CTRChannel);
            ULStat = DaqBoard.PulseOutStop(Sync_MASTER_ACQ_CTRChannel);
            CIOBCTRL_Set_Digital_Output(DaqBoard,CNT1U_ENCODER_A_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard,CNT1D_ENCODER_B_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard, SYNC_IN_24V_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard, ENCODER_SEL_0_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard, ENCODER_SEL_1_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard, ENCODER_SEL_2_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard, LED_VERTE_ACQ_DOChannel, false);
            CIOBCTRL_Set_Digital_Output(DaqBoard, LED_ROUGE_ACQ_DOChannel, false);
            //CIOBCTRL_Set_Digital_Output(SUP1_ACQ_DIOChannel, false);
            //CIOBCTRL_Set_Digital_Output(SUP2_ACQ_DIOChannel, false);
            //CIOBCTRL_Test();
            //CIOBCTRL_TestAI();
            // envoi des pas
            //CIOBCTRL_DOCnt(0, 40, 10, 7, false);
            return ULStat;
        }

        MccDaq.ErrorInfo CIOBCTRL_Test(MccDaq.MccBoard DaqBoard)
        {

            int ctrType, numCounters, defaultCtr, CounterNum, NumAIChans, Resolution, DefaultChan,NumPorts;
            short DataValue;
            double Count;
            float EngUnits;
            MccDaq.ErrorInfo ULStat = NOERRORS;
            MccDaq.Range Range;
            MccDaq.TriggerType DefaultTrig;

            //CIOBCTRL_TestAllIOType();

            //initialisation de la sortie a 0
            //ULStat = cbDBitOut(BoardNum, PortNum, channel_id, 0);
            //CIOBCTRL_TestDO();

            // envoi de l'impulsion:
            CIOBCTRL_SendTriggerPulse(DaqBoard);
            // envoi des pas
            int nb_pas = 40;
            int state0 = 10;
            int state1 = 7;

            CIOBCTRL_DOCnt(DaqBoard, 0, nb_pas, state0, state1, LEFT);


            // Determine if device is compatible with this example
            ctrType = CTREVENT;
            numCounters = FindCountersOfType(DaqBoard, ctrType, out defaultCtr);
            if (numCounters == 0)
            {
                //some scan counters can also work with this example
                ctrType = CTRSCAN;
                numCounters = FindCountersOfType(DaqBoard, ctrType, out defaultCtr);
                if (numCounters == 0)
                {
                    Console.WriteLine("{0:s} (board {1:u}) does not have Event counters.", DaqBoard.BoardName, DaqBoard.BoardNum);
                    DisplayMessage(NOERRORS);
                    return NOERRORS;
                }
            }

            CounterNum = defaultCtr;
            ULStat = DaqBoard.CClear(CounterNum);
            int start = 0, stop = 0;
            ULStat = DaqBoard.CIn32(CounterNum, out start);
            Thread.Sleep(5000);
            ULStat = DaqBoard.CIn32(CounterNum, out stop);
            Count = (double)(stop - start) / 5.0;




            // Determine if device is compatible with this example
            NumAIChans = FindAnalogChansOfType(DaqBoard, ANALOGINPUT, out Resolution, out Range, out DefaultChan, out DefaultTrig);
            if (NumAIChans == 0)
            {
                Console.WriteLine("{0:s} (board {1:u}) does not have analog input channels.", DaqBoard.BoardName, DaqBoard.BoardNum);
                DisplayMessage(NOERRORS);
                return ERRORBOARD;
            }

            ULStat = DaqBoard.AIn(DefaultChan, Range, out DataValue);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }
            ULStat = DaqBoard.ToEngUnits(Range, DataValue, out EngUnits);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }

            ULStat = DaqBoard.AIn(DefaultChan, Range, out DataValue);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }
            ULStat = DaqBoard.ToEngUnits(Range, DataValue, out EngUnits);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }


            //BoardNum = 0;
            ULStat = NOERRORS;
            double initialDelay = 0;
            MccDaq.IdleState idleState = IdleState.Low;
            MccDaq.PulseOutOptions options = PulseOutOptions.Default;
            ctrType = CTRPULSE;
            int pulseCount = 0;


            Console.WriteLine("Demonstration of cbPulsePutStart() and cbPulseOutStop()\n");

            // Determine if device is compatible with this example
            numCounters = FindCountersOfType(DaqBoard, ctrType, out defaultCtr);
            if (numCounters == 0)
            {
                Console.WriteLine("{0:s} (board {1:u}) does support pulse output.", DaqBoard.BoardName, DaqBoard.BoardNum);
                DisplayMessage(NOERRORS);
                return NOERRORS;
            }



            DefaultChan = 0;
            double frequency = 10000.0;
            double dutyCycle = 0.5;
            int PortType = PORTOUT;
            DigitalPortType DefaultPort=DigitalPortType.AuxPort;
            int ProgAbility,DefaultNumBits, FirstBit;
            string portTypeStr="";

            ULStat = DaqBoard.PulseOutStart(DefaultChan, ref frequency,
                ref dutyCycle, pulseCount, ref initialDelay, idleState, options);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }
            DaqBoard.PulseOutStop(DefaultChan);


            // Determine if device is compatible with this example
            NumPorts = FindPortsOfType(DaqBoard, PortType, out ProgAbility,
                out DefaultPort, out DefaultNumBits, out FirstBit);
            if (NumPorts == 0)
            {
                Console.WriteLine("{0:s} (board {0:u}) has no compatible digital ports.", DaqBoard.BoardName, DaqBoard.BoardNum);
                DisplayMessage(NOERRORS);
                return NOERRORS;
            }
            if (DefaultNumBits > 8) 
                DefaultNumBits = 8;
            GetNameOfPortType(DefaultPort,ref portTypeStr);

            Console.WriteLine("Press any key to stop reading digital inputs.\n");
            Console.WriteLine("Change the value read by applying a TTL high or");
            Console.WriteLine("TTL low to one of the inputs on {0:s}.\n", portTypeStr);

            /* configure first valid port for digital input
                Parameters:
                    BoardNum    :the number used by CB.CFG to describe this board.
                    PortNum     :the input port (AUXPORT, FIRSTPORTA, etc)
                    Direction   :sets the port for input or output */

            DigitalPortDirection Direction = DigitalPortDirection.DigitalIn;
            ULStat = DaqBoard.DConfigPort(DefaultPort, Direction);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }
            ULStat = DaqBoard.DIn(DefaultPort, out DataValue);
            if (ULStat != NOERRORS)
            {
                DisplayMessage(ULStat);
                return ULStat;
            }
            return ULStat;
        }


        static void Main()
        {
            var BCMCC = new IOBoardControlMCC();
            MccDaq.MccBoard TestBoard = new MccDaq.MccBoard(0) ;
            string NomCarte="";
            int ret;
            BCMCC.InitUL();
            ret=BCMCC.GetNameOfBoard(TestBoard, ref NomCarte);
            Console.WriteLine(NomCarte);

            int PortType = 32;
            int FirstBit=0;
            int ProgAbility=0;
            MccDaq.DigitalPortType DefPortType = MccDaq.DigitalPortType.AuxPort;
            int DefaultNumBits = 1;
            ret = BCMCC.FindPortsOfType(TestBoard, PortType, out ProgAbility,out DefPortType, out DefaultNumBits, out FirstBit);
            Console.WriteLine("Nombre de port="+ret+"\n");

            int Resolution = 0;
            MccDaq.Range DefaultRange = MccDaq.Range.Ma0To20;
            int DefaultChan = 0;
            TriggerType DefaultTrig = TriggerType.TrigHigh;
            ret = BCMCC.FindAnalogChansOfType(TestBoard, ANALOGINPUT, out Resolution, out DefaultRange, out DefaultChan, out DefaultTrig);
            Console.WriteLine("Nombre de channels=" + ret + "\n");

            BCMCC.CIOBCTRL_TestAllIOType(TestBoard);


            float Tension=0.0f;
            BCMCC.CIOBCTRL_GetTensionAI(TestBoard, 5, ref Tension);
            Console.WriteLine("Tension sur la Channel 5 (attendu -5V) : {0:F}", Tension);

            //BCMCC.CIOBCTRL_TestAI(TestBoard, 3);

            BCMCC.CIOBCTRL_Config(TestBoard,ref NomCarte);
            Console.WriteLine(NomCarte);

        }
    }
}