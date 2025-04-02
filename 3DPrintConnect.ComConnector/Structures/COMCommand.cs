namespace _3DPrintConnect.ComConnector.Structures
{
    public struct COMCommand
    {
        public Guid ID;
        public string Command;
        public bool Status;
        public string StringResult;

        public Action<COMCommand>? OnComplite;


    }
}
