interface FlightRecordInterface
{
    string Destination { get; set; }
    int PilotIDNumber { get; set; }
}

public class FlightRecord : FlightRecordInterface
{
    private string dest;
    private int pilotId;
    public string Destination
    {
        get { return dest; }
        set { dest = value; }
    }
    public int PilotIDNumber
    {
        get { return pilotId + 1; }
        set { pilotId = value - 1; }
    }
}
public class FlightManager
{
    private string dest = "New York";
    private int pilotId = 0;
    private FlightRecord[] flightHistory = new FlightRecord[100];
    private int flightHistoryIndex = -1;
    public string Destination
    {
        get { return dest; }
        set { dest = value; }
    }
    public int PilotIDNumber
    { //hypothetical scenario where the ID had to be stored as starting from 0 but users would see it as starting from 1
        get { return pilotId + 1; }
        set { pilotId = value - 1; }
    }
    public FlightRecord LatestFlight
    {
        get
        {
            if (flightHistoryIndex == -1)
                return null;
            return flightHistory[flightHistoryIndex];
        }
        set
        {
            flightHistoryIndex++;
            flightHistory[flightHistoryIndex] = value;
        }
    }

    public delegate void FlightArrivedHandler(object source, EventArgs args);

    public event FlightArrivedHandler FlightArrived;

    protected virtual void OnFlightArrived()
    {
        if (FlightArrived != null)
            FlightArrived(this, EventArgs.Empty);
    }

    public void SendFlight(int flightId)
    {
        Console.WriteLine("Flight "+flightId+" is now underway. Estimated arrival time: 1 second.");
        Thread.Sleep(1000);
        OnFlightArrived();
    }

    public void PrintFlightHistory()
    {
        for (int i = 0; i <= flightHistoryIndex; i++)
        {
            Console.WriteLine("Flight to " + flightHistory[i].Destination + " piloted by ID " + flightHistory[i].PilotIDNumber);
        }
    }
}

public class FlightControlApp
{
    public void OnFlightArrived(object source, EventArgs eventArgs)
    {
        Console.WriteLine("Flight Control - Flight has arrived!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var flightManager = new FlightManager();
        var flightControlApp = new FlightControlApp();
        flightManager.FlightArrived += flightControlApp.OnFlightArrived;
        Console.WriteLine("Current destination is " + flightManager.Destination + ". Sending flight 1.");
        flightManager.SendFlight(1);
        FlightRecord flight1 = new FlightRecord();
        flight1.Destination = flightManager.Destination;
        flight1.PilotIDNumber = flightManager.PilotIDNumber;
        flightManager.LatestFlight = flight1;
        Console.WriteLine("Press any key to send the next flight to France.");
        Console.ReadKey();
        Console.WriteLine("Changing destination for next flight to France.");
        flightManager.Destination = "France";
        Console.WriteLine("Current destination is " + flightManager.Destination + ". Sending flight 2.");
        flightManager.SendFlight(2);
        FlightRecord flight2 = new FlightRecord();
        flight2.Destination = flightManager.Destination;
        flight2.PilotIDNumber = flightManager.PilotIDNumber;
        flightManager.LatestFlight = flight2;
        Console.ReadKey();
        Console.WriteLine("Printing flight history...");
        flightManager.PrintFlightHistory();
        Console.ReadKey();
        
    }
}