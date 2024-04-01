// See https://aka.ms/new-console-template for more information


using apbd_labs2;

// Stworzenie kontenera danego typu

var liquidContainer = new LiquidContainer(true, 500, 250, 500, 1000);
var gasContainer = new GasContainer(2, 500, 250, 500, 1000);
var reeferContainer  = new ReeferContainer("Bananas", 15, 500, 250, 500, 1000);

// Załadowanie ładunku do danego kontenera
liquidContainer.LoadCargo(250);
gasContainer.LoadCargo(500);
reeferContainer.LoadCargo(999);

// Załadowanie kontenera na statek

var ship1 = new ContainerShip(200, 5, 25);
var ship2 = new ContainerShip(200, 5, 25);

ship1.AddContainer(liquidContainer);

// Załadowanie listy kontenerów na statek

var containerList = new List<Container>();
containerList.Add(reeferContainer);
containerList.Add(gasContainer);

ship1.AddContainers(containerList);

// Usunięcie kontenera ze statku
ship1.RemoveContainer(reeferContainer.SerialNumber);

// Rozładowanie kontenera
reeferContainer.EmptyLoad();

// Zastąpienie kontenera na statku o danym numerze innym kontenerem
ship1.ReplaceContainer(liquidContainer.SerialNumber, reeferContainer);

// Możliwość przeniesienie kontenera między dwoma statkami
ship1.MoveContainerToOtherShip(reeferContainer.SerialNumber, ship2);

// Wypisanie informacji o danym kontenerze
reeferContainer.PrintContainer();

// Wypisanie informacji o danym statku i jego ładunku
ship1.PrintShipWithContainers();
ship2.PrintShipWithContainers();