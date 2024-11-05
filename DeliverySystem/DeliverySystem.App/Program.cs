// -
using System;


namespace DeliverySystem
{
    internal class Program
    {
        /// <summary>
        /// Создание ордеров и передача их на отправку
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("---");

            Order<HomeDelivery, ProductSet<Product>> order1
                = Utilities.BuildDeliveryOrder<HomeDelivery>();

            Order<PickPointDelivery, ProductSet<Product>> order2
                = Utilities.BuildDeliveryOrder<PickPointDelivery>();

            Order<ShopDelivery, ProductSet<Product>> order3
                = Utilities.BuildDeliveryOrder<ShopDelivery>();

            Utilities.Transportation(order1);
            Utilities.Transportation(order2);
            Utilities.Transportation(order3);

            Console.WriteLine("---");
            Console.WriteLine("bye");
            Console.ReadKey();
        }
    }


    /// <summary>
    /// Классы, описывающие необходимую доставку
    /// </summary>

    internal abstract class Delivery 
    {
        protected internal abstract int ID { get; }
        protected internal int Duration;
        public string Address { get; set; } = string.Empty;
        public Delivery() { }
    }

    internal class HomeDelivery: Delivery 
    {
        protected internal override int ID { get; } = 0;
        
        public HomeDelivery() : base()
        {
            base.Duration = 5;
            base.Address = "8129 Jones Street, New York, NY 10027";
        }
    }

    internal class PickPointDelivery: Delivery 
    {
        protected internal override int ID { get; } = 1;
        
        public PickPointDelivery() : base()
        {
            base.Duration = 10;
            base.Address = "7172 Valley Farms Rd., Brooklyn, NY 11229";
        }
    }

    internal class ShopDelivery: Delivery 
    {
        protected internal override int ID { get; } = 2;
        
        public ShopDelivery() : base()
        {
            base.Duration = 15;
            base.Address = "702 Livingston St., Bronx, NY 10473";
        }
    }


    /// <summary>
    /// Класс, описывающий ордер
    /// Включает в себя доставку и набор товаров
    /// </summary>
    /// <typeparam name="TDelivery"></typeparam>
    /// <typeparam name="TStruct"></typeparam>
    internal class Order <TDelivery, TStruct>
        where TDelivery: Delivery where TStruct: struct
    {
        public TDelivery Delivery;

        public readonly int Number;

        public string Description;

        public TStruct Products;

        public Order(TDelivery delivery, TStruct products)
        {
            this.Delivery = delivery;
            this.Products = products;
            this.Number = Utilities.OrderNumber;
            Description = (string.Empty).BuildDeliveryDescription(delivery.ID);
        }
    }


    /// <summary>
    /// Классы, описывающие товары
    /// </summary>

    internal abstract class Product
    {
        public string Name;
        public string Price;
    }

    internal class Book : Product
    {
        public string Author;
    }

    internal class Car : Product
    {
        public string Vendor;
    }


    /// <summary>
    /// Структура, включающая в себя набор товаров
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    internal struct ProductSet<TProduct> where TProduct: Product
    {
        private TProduct[] set; // = Array.Empty<TProduct>();

        // public ProductSet() { set = Array.Empty<TProduct>(); }

        public ProductSet(TProduct[] set) { this.set = set; }

        public TProduct this[int index]
        {
            get
            {
                if (0 <= index && index < set.Length)
                {
                    return set[index];
                }
                else
                {
                    return null!;
                }
            }
            set
            {
                set[index] = value;
            }
        }

        public int Count { get { return set.Length; } }
    }


    /// <summary>
    /// Инструменты для создания ордера и отправки созданного ордера
    /// </summary>
    internal static class Utilities
    {
      private static int orderNumber = 800;

      public static int OrderNumber { get { return ++orderNumber; } }

      internal static Order<TDelivery, ProductSet<Product>> BuildDeliveryOrder<TDelivery>()
          where TDelivery: Delivery, new()
      {
        ProductSet<Product> set = new ProductSet<Product>([
            new Book() { Name = "Book", Author = "Library", Price = "10" },
            new Car() { Name = "Car", Vendor = "Workshop", Price = "100" } ]);

        TDelivery delivery = new TDelivery();

        Order<TDelivery, ProductSet<Product>> order
            = new Order<TDelivery, ProductSet<Product>>(delivery, set);

        return order;
      }


      internal static void Transportation<TDelivery>(Order<TDelivery, ProductSet<Product>> order) where TDelivery: Delivery
      {
          Console.WriteLine("transportation. order. number: [" + order.Number + "]");
          Console.WriteLine("transportation. order. description: [" + order.Description + "]");
          Console.WriteLine("transportation. products. count: [" + order.Products.Count + "]");
          Console.WriteLine("transportation. product1. name: [" + order.Products[0]?.Name + "]");
          Console.WriteLine("transportation. product1. price: [" + order.Products[0]?.Price + "]");
          Console.WriteLine("transportation. delivery. address: [" + order.Delivery.Address + "]");
          Console.WriteLine("transportation. duration. time: [" + order.Delivery.Duration + "]");
      }
    }


    /// <summary>
    /// Класс с методом расширения,
    ///  вписывающий тип доставки по переданному в аргументе id
    /// </summary>
    internal static class StringExtensions
    {
        public static string BuildDeliveryDescription(this string str, int id) 
        {
            string res;
            switch (id)
            {
                case 0:
                    res = "HomeDelivery";
                    break;
                case 1:
                    res = "PickPointDelivery";
                    break;
                case 2:
                    res = "ShopDelivery";
                    break;
                default:
                    res = string.Empty;
                    break;
            }
            return res;
        }
    }

}

