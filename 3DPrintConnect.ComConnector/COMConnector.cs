using _3DPrintConnect.ComConnector.Delegates;
using _3DPrintConnect.ComConnector.Structures;
using System.IO.Ports;
using System.Text;

namespace _3DPrintConnect.ComConnector
{
    public class COMConnector : SerialPort, IDisposable
    {
        public event COMError? OnError;
        public event COMDispose? OnDisposed;
        public event COMCommandComplite? OnCommandComplite;

        public event COMStop? OnStop;
        public event COMReload? OnReload;
        public event COMStart? OnStart;


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
                Console.WriteLine($"Connected port {this.PortName}.");
                Console.WriteLine($"BaudRate {this.BaudRate}.");
                return true;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return false;
            }

        }
        List<COMCommand> listingCommands = new List<COMCommand>();
        Dictionary<string, string> ListMessageData = new Dictionary<string, string>();
        StringBuilder MessageData = new StringBuilder();
        bool statusRuningCommands = false;
        Task LiveMessage;




        public COMConnector() : base()
        {
            LiveMessage = new Task(async () =>
            {
                await OnTimerTick();
            });
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
                        CurretCommand.StringResult = data;
                        CurretCommand.Status = false;
                        MessageData.Clear();
                        CurretCommand.OnComplite?.Invoke(CurretCommand);
                        OnCommandComplite?.Invoke(CurretCommand);
                    }
                    else if (data.EndsWith("error"))
                    {

                    }


                }

            }


        }


        int IDCommand = 0;
        COMCommand CurretCommand = new COMCommand();
        async Task OnTimerTick()
        {

            while (true)
            {

                if (!this.IsOpen)
                    continue;
                if (statusRuningCommands)
                {
                    if (listingCommands.Count == 0)
                        continue;



                    if (CurretCommand.Status == false)
                    {
                        CurretCommand = listingCommands[IDCommand];
                        CurretCommand.Status = true;

                        this.SendMessage(CurretCommand.Command);
                        IDCommand++;
                        if (IDCommand >= listingCommands.Count)
                            statusRuningCommands = false;
                    }





                }

                await Task.Delay(10);
            }
        }
        public void Run()
        {
            statusRuningCommands = true;

        }
        public void Stop()
        {
            statusRuningCommands = false;
            IDCommand = 0;
            CurretCommand = new COMCommand();
            OnStart?.Invoke();
        }
        public void Reload()
        {
            Stop();
            Run();
            OnReload?.Invoke();
        }

        public void AddCommand(string command, Action<COMCommand> Result = null)
        {

            listingCommands.Add(new COMCommand()
            {
                Command = command,
                ID = Guid.NewGuid(),
                Status = false,
                OnComplite = Result


            });

        }
        public void AddCommand(COMCommand Command, Action<COMCommand> Result = null)
        {
            Command.ID = Guid.NewGuid();
            Command.OnComplite = Result;
            if (!string.IsNullOrEmpty(Command.Command))
            {
                OnError?.Invoke($"Command is null!");
                return;
            }
            listingCommands.Add(Command);



        }

        public void SendMessage(string command)
        {
            this.WriteLine(command);
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
