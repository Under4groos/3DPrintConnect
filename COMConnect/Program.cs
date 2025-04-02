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

        //connector.AppendCommand("G28" , (data) =>
        //{
        //    Console.WriteLine($"===== {data.StringResult}");
        //});
        //connector.AppendCommand("G1 F1500", (data) =>
        //{
        //    Console.WriteLine($"===== {data.StringResult}");
        //});

        //connector.AppendCommand("M503", (data) =>
        //{
        //    Console.WriteLine($"===== {data.StringResult}");

        //    File.WriteAllText($"{data.Command}_{data.ID}.txt", data.StringResult);
        //});

        connector.AppendCommand($"G1 F2000");
        for (int i = 0; i < 5; i++)
        {
          
            connector.AppendCommand($"G1 X{i * 20} Y10");
        }


        connector.Run();
    }
    Console.ReadLine();
}