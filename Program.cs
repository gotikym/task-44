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
    Player player = new Player();
    Salesman saleman = new Salesman();
    private List<Product> _cart = new List<Product>();

    public void Work()
    {
        const string Take = "take";
        const string Pay = "pay";
        const string Exit = "exit";
        const string Check = "check";
        const string Wallet = "wallet";
        bool isExit = false;
        Console.WriteLine("Добро пожаловать в наш магазин");

        while (isExit == false)
        {
            Console.WriteLine("Взять товар: " + Take + "\nОплатить и забрать товар: " +
                Pay + "\nПосмотреть продукты в сумке: " + Check + "\nПосмотреть свои 3 копейки: " +
                Wallet + "\nВыйти из магазина: " + Exit);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case Take:
                    FillCart();
                    break;

                case Pay:
                    Sale();
                    break;

                case Check:
                    player.LookBag();
                    break;

                case Wallet:
                    player.LookWallet();
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
        Product product = ChoiceProduct();
        _cart.Add(product);
        saleman.ShiftProducts(product);
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

    private Product ChoiceProduct()
    {
        saleman.ShowProducts();
        Console.WriteLine("Выберите продукт");
        return saleman.GetProduct(GetNumber());
    }

    private void Sale()
    {
        if (player.CheckSolvency(CostProducts()))
        {
            foreach (Product product in _cart)
            {
                player.AddProduct(product);
            }
            
            player.ToPay(CostProducts());
            saleman.TakeMoney(CostProducts());
            _cart.Clear();
        }
        else
        {
            Console.WriteLine("У клиента недостаточно средств для оплаты");
            saleman.ReturnProduct(_cart);
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

class Player
{
    private Random _random = new Random();
    private List<Product> _bag = new List<Product>();
    private int _money;
    int minMoney = 250;
    int maxMoney = 1000;

    public Player()
    {
        _bag = new List<Product>();
        _money = _random.Next(minMoney, maxMoney);
    }

    public void AddProduct(Product product)
    {
        _bag.Add(product);
    }

    public void LookWallet()
    {
        Console.WriteLine(_money);
    }

    public void LookBag()
    {
        foreach (Product product in _bag)
        {
            Console.WriteLine(product.Name + " " + product.Description);
        }
    }

    public bool CheckSolvency(int costProducts)
    {
        return _money >= costProducts;
    }

    public void ToPay(int costProducts)
    {
        _money -= costProducts;
    }
}

class Salesman
{
    private List<Product> _products = new List<Product>();
    private int _cashBox;

    public Salesman()
    {
        _products = new List<Product>();
        _cashBox = 0;
        AddProducts();
    }

    public void ShowProducts()
    {
        int number = 0;

        foreach (Product product in _products)
        {
            Console.WriteLine(number++ + ": " + product.Name + " " + product.Description + " " + product.Cost);
        }
    }

    public void ShiftProducts(Product product)
    {
        _products.Remove(product);
    }

    public void ReturnProduct(List<Product> _cart)
    {
        foreach (Product product in _cart)
        {
            _products.Add(new Product(product.Name, product.Description, product.Cost));
        }
    }

    public Product GetProduct(int indexProduct)
    {
        return _products[indexProduct];
    }

    public void AddProducts()
    {
        _products.Add(new Product("яблоко", "красное, спелое, вкусное", 50));
        _products.Add(new Product("мороженое", "Бурёнка, крем-брюле", 40));
        _products.Add(new Product("какао", "собрано в Мордоре", 70));
        _products.Add(new Product("мяско", "свежее, не замороженное", 170));
        _products.Add(new Product("шампанское", "Mondoro Asti", 250));
        _products.Add(new Product("хлеб", "черный как тьма, невидим ночью", 300));
    }

    public void TakeMoney(int costProducts)
    {
        _cashBox += costProducts;
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
