using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Core;
using DAL.Core.Interfaces;

namespace DAL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }




    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

                await CreateUserAsync("admin", "ivt8471M!", "Inbuilt Administrator", "evictionboss@gmail.com", "+1 (702) 506-8089", new string[] { adminRoleName });
                await CreateUserAsync("user", "ivt8471M!", "Inbuilt Standard User", "johnpaulaldrin@gmail.com", "+1 (702) 849-9722", new string[] { userRoleName });

                _logger.LogInformation("Inbuilt account generation completed");
            }



            if (!await _context.Customers.AnyAsync() && !await _context.DocumentCategories.AnyAsync())
            {
                _logger.LogInformation("Seeding initial data");

                //Customer - EvictionDocuments
                //Customer - DUIDocuments
                Customer cust_1 = new Customer
                {
                    Name = "Ebenezer Monney",
                    Email = "contact@ebenmonney.com",
                    Status = Status.Agent,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

                Customer cust_2 = new Customer
                {
                    Name = "Itachi Uchiha",
                    Email = "uchiha@narutoverse.com",
                    PhoneNumber = "+81123456789",
                    Address = "Some fictional Address, Street 123, Konoha",
                    City = "Konoha",
                    Status = Status.Landlord,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

                Customer cust_3 = new Customer
                {
                    Name = "John Doe",
                    Email = "johndoe@anonymous.com",
                    PhoneNumber = "+18585858",
                    Address = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio.
                    Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet",
                    City = "Lorem Ipsum",
                    Status = Status.Landlord,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

                Customer cust_4 = new Customer
                {
                    Name = "Jane Doe",
                    Email = "Janedoe@anonymous.com",
                    PhoneNumber = "+18585858",
                    Address = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio.
                    Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet",
                    City = "Lorem Ipsum",
                    Status = Status.Landlord,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };



                DocumentCategory prodCat_1 = new DocumentCategory
                {
                    Name = "None",
                    Description = "Default category. Products that have not been assigned a category",
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };



                Document prod_1 = new Document
                {
                    Name = "BMW M6",
                    Description = "Yet another masterpiece from the world's best car manufacturer",
                    BuyingPrice = 109775,
                    SellingPrice = 114234,
                    UnitsInStock = 12,
                    IsActive = true,
                    DocumentCategory = prodCat_1,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

                Document prod_2 = new Document
                {
                    Name = "Nissan Patrol",
                    Description = "A true man's choice",
                    BuyingPrice = 78990,
                    SellingPrice = 86990,
                    UnitsInStock = 4,
                    IsActive = true,
                    DocumentCategory = prodCat_1,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };



                Eviction ordr_1 = new Eviction
                {
                    Discount = 500,
                    Cashier = await _context.Users.FirstAsync(),
                    Customer = cust_1,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    EvictionDetails = new List<EvictionDetail>()
                    {
                        new EvictionDetail() {UnitPrice = prod_1.SellingPrice, Quantity=1, Document = prod_1 },
                        new EvictionDetail() {UnitPrice = prod_2.SellingPrice, Quantity=1, Document = prod_2 },
                    }
                };

                Eviction ordr_2 = new Eviction
                {
                    Cashier = await _context.Users.FirstAsync(),
                    Customer = cust_2,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    EvictionDetails = new List<EvictionDetail>()
                    {
                        new EvictionDetail() {UnitPrice = prod_2.SellingPrice, Quantity=1, Document = prod_2 },
                    }
                };


                _context.Customers.Add(cust_1);
                _context.Customers.Add(cust_2);
                _context.Customers.Add(cust_3);
                _context.Customers.Add(cust_4);

                _context.Documents.Add(prod_1);
                _context.Documents.Add(prod_2);

                _context.Evictions.Add(ordr_1);
                _context.Evictions.Add(ordr_2);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Seeding initial data completed");
            }
        }



        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Item1)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");


            return applicationUser;
        }
    }
}
