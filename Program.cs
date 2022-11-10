using System;
using System.Collections.Generic;

internal class Program
{
    static void Main(string[] args)
    {
        Shop shop = new Shop();
        shop.Work();
    }
}

class Shop
{
    private Player _player = new Player();
    private Salesman _saleman = new Salesman();

    public void Work()
    {
        const string Buy = "buy";
        const string Exit = "exit";
        const string Check = "check";
        const string Wallet = "wallet";
        bool isExit = false;
        Console.WriteLine("Добро пожаловать в наш магазин");

        while (isExit == false)
        {
            Console.WriteLine("Выбрать и оплатить товар: " +
                Buy + "\nПосмотреть продукты в сумке: " + Check + "\nПосмотреть свои 3 копейки: " +
                Wallet + "\nВыйти из магазина: " + Exit);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case Buy:
                    Trade();
                    break;

                case Check:
                    _player.ShowProducts();
                    break;

                case Wallet:
                    _player.LookWallet();
                    break;

                case Exit:
                    isExit = true;
                    break;
            }
        }
    }

    private void Trade()
    {
        Product product = _saleman.ChooseProduct();
        int costProduct = _saleman.SeeCost(product);

        if (_player.CheckSolvency(costProduct))
        {
            _player.Buy(costProduct, product);
            _saleman.SaleProducts(product, costProduct);
            Console.Clear();
        }
        else
        {
            Console.WriteLine("У клиента недостаточно средств для оплаты\n");
            Console.ReadKey();
        }
    }
}

class Man
{
    protected List<Product> Inventory = new List<Product>();
    protected int Money;

    public Man()
    {
        Money = 0;
        Inventory = new List<Product>();
    }

    public void ShowProducts()
    {
        int number = 0;

        foreach (Product product in Inventory)
        {
            Console.WriteLine(number++ + ": " + product.Name + " " + product.Description + " " + product.Cost);
        }
    }
}

class Player : Man
{
    private Random _random = new Random();

    public Player()
    {
        int minMoney = 250;
        int maxMoney = 1000;
        Money = _random.Next(minMoney, maxMoney);
    }

    public void LookWallet()
    {
        Console.WriteLine(Money);
    }

    public bool CheckSolvency(int costProducts)
    {
        return Money >= costProducts;
    }

    public void Buy(int costProducts, Product product)
    {
        Money -= costProducts;
        Inventory.Add(product);
    }
}

class Salesman : Man
{
    public Salesman()
    {
        AddProducts();
    }

    private bool ProductAvailability()
    {
        return Inventory.Count > 0;
    }

    public int SeeCost(Product product)
    {
        return product.Cost;
    }

    public Product ChooseProduct()
    {
        if (ProductAvailability())
        {
            ShowProducts();
            Console.WriteLine("Выберите продукт");
            return GetProduct();
        }
        else
        {
            Console.WriteLine("У нас кончились продукты, приходите в другой раз");
            Console.ReadKey();
            return null;
        }
    }

    public void SaleProducts(Product product, int costProducts)
    {
        Inventory.Remove(product);
        Money += costProducts;
    }

    private Product GetProduct()
    {
        int indexProduct = 0;
        int numberProduct = Inventory.Count - 1;
        bool isExit = false;

        while (isExit == false)
        {
            indexProduct = GetNumber();

            if (indexProduct > numberProduct)
            {
                Console.WriteLine("У нас нет такого продукта, выбирайте внимательней");                
            }
            else
            {
                isExit = true;                
            }
        }

        return Inventory[indexProduct];
    }

    private int GetNumber()
    {
        bool isParse = false;
        int numberForReturn = 0;

        while (isParse == false)
        {
            string userNumber = Console.ReadLine();
            isParse = int.TryParse(userNumber, out numberForReturn);

            if (isParse == false)
            {
                Console.WriteLine("Вы не корректно ввели число.");
            }
        }

        return numberForReturn;
    }

    private void AddProducts()
    {
        Inventory.Add(new Product("яблоко", "красное, спелое, вкусное", 50));
        Inventory.Add(new Product("мороженое", "Бурёнка, крем-брюле", 40));
        Inventory.Add(new Product("какао", "собрано в Мордоре", 70));
        Inventory.Add(new Product("мяско", "свежее, не замороженное", 170));
        Inventory.Add(new Product("шампанское", "Mondoro Asti", 250));
        Inventory.Add(new Product("хлеб", "черный как тьма, невидим ночью", 300));
    }
}

class Product
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Cost { get; private set; }

    public Product(string name, string description, int cost)
    {
        Name = name;
        Description = description;
        Cost = cost;
    }
}
