namespace apbd_labs2;

public class ContainerShip(int maxWeightTons, int maxContainers, int maxSpeedKnots)
{
    public int MaxWeightTons { get; } = maxWeightTons;

    public int MaxContainers { get; } = maxContainers;

    public int MaxSpeedKnots { get; } = maxSpeedKnots;

    public Dictionary<string, Container> Containers = new Dictionary<string, Container>();

    public void AddContainer(Container container)
    {
        if (Containers.ContainsKey(container.SerialNumber))
        {
            throw new Exception("The ship already contains given container");
        }
        if (Containers.Count + 1 > MaxContainers)
        {
            throw new Exception("The ship can't hold more containers");
        }
        Containers.Add(container.SerialNumber, container);
    }

    public void AddContainers(List<Container> containers)
    {
        foreach (var container in containers)
        {
            AddContainer(container);
        }
    }

    public void RemoveContainer(string containerSerialNumber)
    {
        Containers.Remove(containerSerialNumber);
    }

    public void ReplaceContainer(string containerSerialNumberToRemove, Container newContainer)
    {
        Containers.Remove(containerSerialNumberToRemove);
        AddContainer(newContainer);
    }

    public void MoveContainerToOtherShip(string containerSerialNumberToMove, ContainerShip otherShip)
    {
        if (!Containers.TryGetValue(containerSerialNumberToMove, out var value))
        {
            throw new Exception("Can't to move container that doesn't exist on the ship");
        }
        Containers.Remove(containerSerialNumberToMove);
        otherShip.AddContainer(value);
    }
    
    public void PrintShipWithContainers()
    {
        Console.WriteLine($"Ship max weight: {MaxWeightTons} max speed: {MaxSpeedKnots} max containers: {MaxContainers}");
        Console.WriteLine($"Ship containers (count {Containers.Count}):");
        foreach (var container in Containers)
        {
            container.Value.PrintContainer();
        }
    }
}