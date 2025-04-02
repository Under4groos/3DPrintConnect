using _3DPrintConnect.ComConnector.Delegates;
using System.IO.Ports;
using System.Text;

namespace _3DPrintConnect.ComConnector
{
    public class COMConnector : SerialPort, IDisposable
    {
        public event COMError? OnError;
        public event COMDispose? OnDisposed;

        private string fileLog = "CommandLog.log";

        public virtual void ConfigInit()
        {
            this.DtrEnable = true;
            this.RtsEnable = true;

            this.DataBits = 8;
            this.Parity = Parity.None;
            this.StopBits = StopBits.One;
            this.Handshake = Handshake.None;
        }
        public virtual bool Connect(string portName, int baudRate = 115200)
        {
            this.ConfigInit();
            this.PortName = portName;
            this.BaudRate = baudRate;
            try
            {
                this.Open();

                return true;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return false;
            }

        }
        List<string> listingCommands = new List<string>();
        Dictionary<string, string> ListMessageData = new Dictionary<string, string>();
        StringBuilder MessageData = new StringBuilder();
        bool status = false;
        Task LiveMessage;

        string CurretCommand = string.Empty;


        public COMConnector() : base()
        {
            LiveMessage = new Task(OnTimerTick);
            LiveMessage.Start();
             
            base.DataReceived += COMConnector_DataReceived;
        }




        private void COMConnector_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is SerialPort serial)
            {
                string indata = serial.ReadExisting();

                MessageData.AppendLine(indata);

                if ((MessageData.ToString().Trim() is string data))
                {
                    if (data.EndsWith("ok"))
                    {
                        if (!string.IsNullOrEmpty(CurretCommand))
                        {
                            ListMessageData.Add(CurretCommand, data);
                        }
                    }
                    else if(data.EndsWith("error"))
                    {

                    }
                    
                        
                }

            }


        }

        void OnTimerTick()
        {
            Console.WriteLine("OnTimerTick");
            while (true)
            {
                if (!this.IsOpen)
                    continue;
                if (status)
                {

                    for (int i = 0; i < listingCommands.Count; i++)
                    {
                        string command = listingCommands[i];
                        if (string.IsNullOrEmpty(command))
                            continue;

                        CurretCommand = command;
                        this.WriteLine(command);
                    }
                    status = false;
                }
            }
        }
        public void Run()
        {
            status = true;
        }
        public void Stop()
        {
            status = false;
        }


        public void AppendCommand(string command)
        {
            listingCommands.Add(command);
        }


        public void SendMessage(string command)
        {
            try
            {
                this.WriteLine(command);


            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);

            }
        }




        public virtual void Disconnect()
        {
            listingCommands.Clear();
            if (this.IsOpen)
                this?.Close();
        }

        protected override void Dispose(bool disposing)
        {
            OnDisposed?.Invoke();
            base.Dispose(disposing);


        }
    }
}
