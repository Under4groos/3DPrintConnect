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
    connector.OnCommandComplite += (command) =>
    {

        Console.WriteLine("\n-----------\n");

        Console.WriteLine(command.StringResult);



    };

    if (connector.Connect("COM21"))
    {



        int xy = 80;
        connector.AddCommand($"G28");
        connector.AddCommand($"G1 Z85");
        connector.AddCommand($"G1 X100 Y100");


        connector.Run();
    }
    Console.ReadLine();
}