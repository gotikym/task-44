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
    private List<Product> _cart = new List<Product>();

    public void Work()
    {
        const string TakeProduct = "take";
        const string Pay = "pay";
        const string Exit = "exit";
        const string Check = "check";
        const string Wallet = "wallet";
        bool isExit = false;
        Console.WriteLine("Добро пожаловать в наш магазин");

        while (isExit == false)
        {
            Console.WriteLine("Взять товар: " + TakeProduct + "\nОплатить и забрать товар: " +
                Pay + "\nПосмотреть продукты в сумке: " + Check + "\nПосмотреть свои 3 копейки: " +
                Wallet + "\nВыйти из магазина: " + Exit);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case TakeProduct:
                    FillCart();
                    break;

                case Pay:
                    Sale();
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

    private void FillCart()
    {
        Console.Clear();
        Console.WriteLine("Выбирайте и берите то, что вам нужно или просто нравится =)");
        Product product = ChooseProduct();
        _cart.Add(product);
        _saleman.ShiftProducts(product);
    }

    private int CostProducts()
    {
        int costProducts = 0;

        foreach (Product product in _cart)
        {
            costProducts += product.Cost;
        }

        return costProducts;
    }

    private Product ChooseProduct()
    {
        _saleman.ShowProducts();
        Console.WriteLine("Выберите продукт");
        return _saleman.GetProduct(GetNumber());
    }

    private void Sale()
    {
        if (_player.CheckSolvency(CostProducts()))
        {
            foreach (Product product in _cart)
            {
                _player.AddProduct(product);
            }

            _player.ToPay(CostProducts());
            _saleman.TakeMoney(CostProducts());
            _cart.Clear();
        }
        else
        {
            Console.WriteLine("У клиента недостаточно средств для оплаты");
            _saleman.PutProduct(_cart);
            _cart.Clear();
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

    public void AddProduct(Product product)
    {
        inventory.Add(product);
    }

    public void LookWallet()
    {
        Console.WriteLine(money);
    }

    public bool CheckSolvency(int costProducts)
    {
        return money >= costProducts;
    }

    public void ToPay(int costProducts)
    {
        money -= costProducts;
    }
}

class Salesman : Man
{
    public Salesman()
    {
        AddProducts();
    }

    public void ShiftProducts(Product product)
    {
        inventory.Remove(product);
    }

    public void PutProduct(List<Product> _cart)
    {
        foreach (Product product in _cart)
        {
            inventory.Add(new Product(product.Name, product.Description, product.Cost));
        }
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

    public void TakeMoney(int costProducts)
    {
        money += costProducts;
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
