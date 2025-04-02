using _3DPrintConnect.ComConnector.Structures;

namespace _3DPrintConnect.ComConnector.Delegates
{
    public delegate void COMError(string message);
    public delegate void COMMessage(string message);
    public delegate void COMStop();
    public delegate void COMReload();
    public delegate void COMStart();



    public delegate void COMCommandComplite(COMCommand command);

    public delegate void COMDispose();
    public delegate void COMInit();
}
