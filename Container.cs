namespace apbd_labs2;

public abstract class Container
{
    private static int _lastSerialNumber;

    private readonly int _serialNumber;
    public int CargoWeight { get; protected set; }
    public int Height { get; }
    public int ContainerNetWeight { get; }
    public int Deep { get; }

    public string SerialNumber => "KON-" + _conatainerTypeSymbol + "-" + _serialNumber;

    public int MaxCargoWeight { get; }

    private readonly string _conatainerTypeSymbol;

    protected Container(int height, int containerNetWeight, int deep, int maxCargoWeight,
        string containerTypeSymbol)
    {
        CargoWeight = 0;
        Height = height;
        ContainerNetWeight = containerNetWeight;
        Deep = deep;
        _serialNumber = Interlocked.Increment(ref _lastSerialNumber);
        _conatainerTypeSymbol = containerTypeSymbol;
        MaxCargoWeight = maxCargoWeight;
    }

    public void EmptyLoad()
    {
        CargoWeight = 0;
    }

    public void LoadCargo(int weight)
    {
        if (CargoWeight + weight > MaxCargoWeight)
        {
            throw new OverfillException();
        }

        CargoWeight += weight;
    }
    
    public void PrintContainer()
    {
        Console.WriteLine($"Container {SerialNumber} height: {Height} weight: {CargoWeight} net weight: {ContainerNetWeight} max cargo weight: {MaxCargoWeight}");
    }

}

public class LiquidContainer(
    bool hazardCargo,
    int height,
    int containerNetWeight,
    int deep,
    int maxCargoWeight)
    : Container(height, containerNetWeight, deep, maxCargoWeight, "L"), IHazardNotifier
{
    public void Notify(string textMessage)
    {
        Console.WriteLine(
            "Hazard situation in container number: " + SerialNumber + " Situation message: " + textMessage);
    }

    public new void LoadCargo(int weight)
    {
        if (hazardCargo && ((double)CargoWeight + weight) / MaxCargoWeight > 0.5)
        {
            Notify("Tried to load liquid container for hazard cargo above 50%");
        }
        else if (((double)CargoWeight + weight) / MaxCargoWeight > 0.9)
        {
            Notify("Tried to load liquid container above 90%");
        }
        else
        {
            base.LoadCargo(weight);
        }
    }
}

public class GasContainer(
    int pressure,
    int height,
    int containerNetWeight,
    int deep,
    int maxCargoWeight)
    : Container(height, containerNetWeight, deep, maxCargoWeight, "G"), IHazardNotifier
{
    public int Pressure { get; } = pressure;

    public void Notify(string textMessage)
    {
        Console.WriteLine(
            "Hazard situation in container number: " + SerialNumber + " Situation message: " + textMessage);
    }

    public new void EmptyLoad()
    {
        CargoWeight = (int)((double)CargoWeight / 100 * 5);
    }

    public new void LoadCargo(int weight)
    {
        try
        {
            base.LoadCargo(weight);
        }
        catch (OverfillException e)
        {
            Notify("Tried to load more than allowed to gas container");
            throw;
        }
    }
}

public class ReeferContainer : Container
{
    private static readonly Dictionary<string, double> temperatureByTypes;
    
    static ReeferContainer() {
        temperatureByTypes = new Dictionary<string, double>(); 
        temperatureByTypes.Add("Bananas", 13.3);
        temperatureByTypes.Add("Chocolate", 18);
        temperatureByTypes.Add("Fish", 2);
        temperatureByTypes.Add("Meat", -15);
        temperatureByTypes.Add("Ice cream", -18);
        temperatureByTypes.Add("Frozen pizza", -30);
        temperatureByTypes.Add("Cheese", 7.2);
        temperatureByTypes.Add("Sausages", 5);
        temperatureByTypes.Add("Butter", 20.5);
        temperatureByTypes.Add("Eggs", 19);
    }
    
    public string ProductType { get; }

    public double Temperature { get; }

    public ReeferContainer(
        string productType,
        double temperature,
        int height,
        int containerNetWeight,
        int deep,
        int maxCargoWeight) : base(height, containerNetWeight, deep, maxCargoWeight, "C")
    {
        ProductType = productType;
        Temperature = temperature;

        if (temperatureByTypes.ContainsKey(productType))
        {
            if (temperatureByTypes[productType] > temperature)
            {
                throw new Exception("Try to create reefer container with lower temperature than expected for given product type");
            }
        }
        else
        {
            throw new Exception("Try to create reefer container for not expected product type " + productType);
        }
    }
}

public class OverfillException : Exception
{
}