using _3DPrintConnect.ComConnector;

using (COMConnector connector = new COMConnector())
{
    connector.OnError += (result) =>
    {
        Console.WriteLine(result);
    };
    connector.OnDisposed += () =>
    {
        Console.WriteLine("IDisposable");
    };
    

    if (connector.Connect("COM21"))
    {
        Console.WriteLine("Connected");

        connector.AppendCommand("M503");
        connector.Run();
    }
    Console.ReadLine();
}