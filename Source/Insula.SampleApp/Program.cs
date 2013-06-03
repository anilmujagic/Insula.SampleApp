using Insula.Data.Orm;
using Insula.SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insula.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Insula.SampleApp.Database;Integrated Security=SSPI";

            using (var db = new DatabaseContext(DatabaseEngine.SqlServer, connectionString))
            {
                PrepareDB(db);

                Console.WriteLine("======================================================================");
                Console.WriteLine("Querying");
                Console.WriteLine();

                var customers = db.Query<Customer>()
                    .GetAll();
                Show("Get all customers", customers);

                customers = db.Query<Customer>()
                    .Where(new { City = "Redmond" })
                    .GetAll();
                Show("Simple filter using object as a sample", customers);

                customers = db.Query<Customer>()
                    .Include("Country")
                    .Where("Customer.CountryID IS NOT NULL AND CustomerID BETWEEN @0 AND @1", 2, 4)
                    .OrderBy("City", "CustomerID DESC")
                    .GetAll();
                Show("Include countries, complex WHERE, ORDER BY", customers);

                ////////////////////////////////////////////////////////////////////////////////////////////////////

                var query = db.Query<Customer>()
                    .Include("Country")
                    .OrderBy("CustomerID DESC");

                Show("Subset (skip 1, take 3)", query.GetSubset(1, 3));
                Show("Top 2", query.GetTop(2));

                var first = query.GetFirst();
                Console.WriteLine("First with country: {0}, {1}", first.Name, first.Country.Name);

                var single = db.Query<Customer>()
                    .Include("Country")
                    .Where(new { CustomerID = 3 })
                    .GetSingle();
                Console.WriteLine("Single: {0}, {1}", single.Name, single.Country.Name);

                var count = db.Query<Customer>()
                    .Where(new { CountryID = "US" })
                    .GetCount();
                Console.WriteLine("Count from US: {0}", count);

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                Console.WriteLine();
                Console.WriteLine("======================================================================");
                Console.WriteLine("CRUD operations");
                Console.WriteLine();

                var repo = db.Repository<Customer>();

                var newCustomer = new Customer
                {
                    Name = "Insula"
                };
                Show("New Customer", newCustomer);
                
                //Insert
                repo.Insert(newCustomer);
                Show("After insert, has a PK auto generated (IDENTITY)", newCustomer);

                //Update
                newCustomer.City = "Munich";
                repo.Update(newCustomer);

                //GetByKey
                var customerFromDB = repo.GetByKey(newCustomer.CustomerID);
                Show("Customer from DB after update", newCustomer);

                //Delete
                repo.Delete(customerFromDB);

                //DeleteByKey
                newCustomer = new Customer
                {
                    Name = "For deletion"
                };
                repo.Insert(newCustomer);
                repo.DeleteByKey(newCustomer.CustomerID);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        #region Helpers

        static void PrepareDB(DatabaseContext db)
        {
            db.ExecuteNonQuery("DELETE FROM Customer");
            db.ExecuteNonQuery("DELETE FROM Country");
            db.ExecuteNonQuery("DBCC CHECKIDENT (Customer, RESEED, 0)");

            var countryRepo = db.Repository<Country>();
            countryRepo.Insert(new Country { CountryID = "US", Name = "United States" });
            countryRepo.Insert(new Country { CountryID = "DE", Name = "Germany" });
            countryRepo.Insert(new Country { CountryID = "BA", Name = "Bosnia" });

            var customerRepo = db.Repository<Customer>();
            customerRepo.Insert(new Customer { Name = "Microsoft", City = "Redmond", CountryID = "US" });
            customerRepo.Insert(new Customer { Name = "Google", CountryID = "US" });
            customerRepo.Insert(new Customer { Name = "Oracle", CountryID = "US" });
            customerRepo.Insert(new Customer { Name = "Ubuntu" });
            customerRepo.Insert(new Customer { Name = "Logosoft", City = "Sarajevo", CountryID = "BA" });
        }

        static void Show(string description, IEnumerable<Customer> customers)
        {
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine(description);
            Console.WriteLine();

            foreach (var c in customers)
            {
                Console.Write("{0,-5}{1,-20}{2,-15}{3,-5}", c.CustomerID, c.Name, c.City ?? "-", c.CountryID ?? "-");

                if (c.Country != null)
                    Console.Write(c.Country.Name);

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        static void Show(string description, Customer customer)
        {
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine(description);
            Console.WriteLine();

            Console.WriteLine("CustomerID: \t{0}\r\nName:\t\t{1}\r\nCity:\t\t{2}\r\nCountryID:\t{3}",
                customer.CustomerID,
                customer.Name,
                customer.City,
                customer.CountryID
            );

            Console.WriteLine();
        }

        #endregion
    }
}
