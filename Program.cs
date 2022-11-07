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
                    Sale(ChooseProduct());
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

    private Product ChooseProduct()
    {
        if(_saleman.CheckProducts())
        {
            _saleman.ShowProducts();
            Console.WriteLine("Выберите продукт");
            return _saleman.GetProduct(GetNumber());
        }
        else
        {
            Console.WriteLine("У нас кончились продукты, приходите в другой раз");
            Console.ReadKey();
            return null;
        }
    }

    private void Sale(Product product)
    {
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

    private int GetNumber()
    {
        bool isParse = false;
        int numberForReturn = 0;

        while (isParse == false)
        {
            string userNumber = Console.ReadLine();

            if ((isParse = int.TryParse(userNumber, out int number)) == false)
            {
                Console.WriteLine("Вы не корректно ввели число.");
            }

            numberForReturn = number;
        }

        return numberForReturn;
    }
}

class Man
{
    protected List<Product> inventory = new List<Product>();
    protected int money;

    public Man()
    {
        money = 0;
        inventory = new List<Product>();
    }

    public void ShowProducts()
    {
        int number = 0;

        foreach (Product product in inventory)
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
        int _minMoney = 250;
        int _maxMoney = 1000;
        money = _random.Next(_minMoney, _maxMoney);
    }

    public void LookWallet()
    {
        Console.WriteLine(money);
    }

    public bool CheckSolvency(int costProducts)
    {
        return money >= costProducts;
    }

    public void Buy(int costProducts, Product product)
    {
        money -= costProducts;
        inventory.Add(product);
    }
}

class Salesman : Man
{
    public Salesman()
    {
        AddProducts();
    }

    public bool CheckProducts()
    {
        return inventory.Count > 0;
    }

    public int SeeCost(Product product)
    {
        int costProducts = 0;

        for(int i = 0; i < inventory.Count; i++)
        {
            if(product == inventory[i])
            {
                costProducts += product.Cost;
            }
        }

        return costProducts;
    }

    public void SaleProducts(Product product, int costProducts)
    {
        inventory.Remove(product);
        money += costProducts;
    }

    public Product GetProduct(int indexProduct)
    {
        return inventory[indexProduct];
    }

    private void AddProducts()
    {
        inventory.Add(new Product("яблоко", "красное, спелое, вкусное", 50));
        inventory.Add(new Product("мороженое", "Бурёнка, крем-брюле", 40));
        inventory.Add(new Product("какао", "собрано в Мордоре", 70));
        inventory.Add(new Product("мяско", "свежее, не замороженное", 170));
        inventory.Add(new Product("шампанское", "Mondoro Asti", 250));
        inventory.Add(new Product("хлеб", "черный как тьма, невидим ночью", 300));
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
