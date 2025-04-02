namespace _3DPrintConnect.ComConnector.Delegates
{
    public delegate void COMError(string message);
    public delegate void COMMessage(string message);
    public delegate void COMDispose();
    public delegate void COMInit();
}
