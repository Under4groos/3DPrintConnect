using _3DPrintConnect.ComConnector;
bool b = false;
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
        Console.WriteLine(command.ID);
        Console.WriteLine(command.StringResult);

        if (command.Command == "c-" && b == false)
        {
            connector.Reload();
            b = true;
        }
            
    };

    if (connector.Connect("COM21"))
    {

        connector.AppendCommand("g" , (data) =>
        {
            Console.WriteLine($"===== {data.ID}");
        });
         
        connector.AppendCommand("a-");

       // connector.AppendCommand("G28");

        connector.Run();
    }
    Console.ReadLine();
}