using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    class Program
    {
        static void Main ( string [] args )
        {
            Shop shop = new Shop ();
            bool isWork = true;

            while ( isWork )
            {
                Console.WriteLine ( "Очередь в магазине!" );
                Console.WriteLine ( "1 - создать очередь. " +
                    "2 - обслужить очередь." +
                    "3 - Выход." );
                Console.Write ( "Выберите пункт: \t" );

                int input = int.Parse ( Console.ReadLine () );

                switch ( input )
                {
                    case 1:
                        shop.CreateQueue ();
                        break;
                    case 2:
                        shop.ServerClients ();
                        break;
                    case 3:
                        isWork = false;
                        break;
                    default:
                        Console.WriteLine ("Неверно выбран пункт!!!");
                        break;
                }
                Console.ReadLine ();
                Console.Clear ();
            }
        }
    }

    class Shop
    {
        private Queue<Client> _clients = new Queue<Client> ();
        private List<Product> _products = new List<Product> ();
        private Random _random = new Random();

        public Shop ()
        {
            _products.Add ( new Product ( "Яйца", 30 ) );
            _products.Add ( new Product ( "Молоко", 20 ) );
            _products.Add ( new Product ( "Хлеб", 5 ) );
            _products.Add ( new Product ( "Сыр", 105 ) );
            _products.Add ( new Product ( "Мясо", 50 ) );
            _products.Add ( new Product ( "Вода 1.5л", 12 ) );
            _products.Add ( new Product ( "Макароны", 25 ) );
            _products.Add ( new Product ( "Сахар", 21 ) );
        }

        public void CreateQueue ()
        {
            Console.WriteLine ("Очередь создана!");
            int minimumCountClient = 2;
            int maximumCountClient = 10;
            int countClient = _random.Next ( minimumCountClient, maximumCountClient );

            for ( int i = 0; i < countClient; i++ )
            {
                _clients.Enqueue ( GetClient () );
            }
        }

        public void ServerClients ()
        {
            ServeClientsPrivate ();
            Console.WriteLine ( "Очередь обслужана!" );
        }

        private void ServeClientsPrivate ()
        {
            while ( _clients.Count > 0 )
            {
                _clients.Dequeue ().PurchaseProducts ();
            }
        }

        private Client GetClient ()
        {
            List<Product> products = new List<Product> ();
            int minimumCountProduct = 2;
            int maximumCountProduct = 6;
            int minimumCountMoney = 100;
            int maximumCountMoney = 300;
            int countMoney = _random.Next ( minimumCountMoney, maximumCountMoney );
            int countProduct = _random.Next ( minimumCountProduct, maximumCountProduct );

            for ( int i = 0; i < countProduct; i++ )
            {
                products.Add ( _products [ _random.Next ( 0, _products.Count - 1 ) ] );
            }

            return new Client ( countMoney, products );
        }
    }

    class Client
    {
        private List<Product> _productsInBasket;
        private int _money;

        public Client ( int money, List<Product> productsInBasket )
        {
            _money = money;
            _productsInBasket = productsInBasket;
        }

        public void PurchaseProducts ()
        {
            Random random = new Random ();
            int purchaseAmount = GetEntireBasketCost ();
            ShowProducts ();
            Console.WriteLine ( $"Сумма товаров {purchaseAmount}. У клиента {_money}" );

            if ( _money >= purchaseAmount )
            {
                Console.WriteLine ( $"Клиент оплатил товары на сумму {purchaseAmount} и покинул магазин" );
                Console.WriteLine ("\nНажмитие Enter для продолжения...");
            }
            else
            {
                RemoveProductsClient ();
            }

            Console.ReadKey ();
            Console.Clear ();
        }


        private void ShowProducts ()
        {
            Console.WriteLine ( $"\nКорзина клиена" );

            foreach ( var item in _productsInBasket )
            {
                Console.WriteLine ( $"{item.Name}, цена: {item.Price}" );
            }
        }

        private void RemoveProductsClient ()
        {
            while ( GetEntireBasketCost () >= _money )
            {
                RemoveProduct ();
            }
        }

        private void RemoveProduct ()
        {
            Random random = new Random ();
            int index = random.Next ( 0, _productsInBasket.Count );
            Product productToRemove = _productsInBasket [ index ];
            Console.WriteLine ( $"Клиент отказаться от товара {productToRemove.Name} стоимостью {productToRemove.Price}" );
            _productsInBasket.Remove ( productToRemove );
        }

        private int GetEntireBasketCost ()
        {
            int purchaseAmount = 0;

            foreach ( var product in _productsInBasket )
            {
                purchaseAmount += product.Price;
            }

            return purchaseAmount;
        }
    }

    class Product
    {
        public Product ( string name, int price )
        {
            Price = price;
            Name = name;
        }

        public int Price { get; private set; }
        public string Name { get; private set; }
    }
}
/*Задача:
Написать программу администрирования супермаркетом.
В супермаркете есть очередь клиентов.
У каждого клиента в корзине есть товары, также у клиентов есть деньги.
Клиент когда подходит на кассу получает итоговую сумму покупки и старается её оплатить.
Если оплатить клиент не может, то он рандомный товар из корзины выкидывает до тех пор пока его денег не хватит для оплаты.
Клиентов можно делать ограниченное число на старте программы.*/