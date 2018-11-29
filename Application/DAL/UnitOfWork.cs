 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        ICustomerRepository _customers;
        IDocumentRepository _Documents;
        IEvictionsRepository _evictions;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }



        public ICustomerRepository Customers
        {
            get
            {
                if (_customers == null)
                    _customers = new CustomerRepository(_context);

                return _customers;
            }
        }



        public IDocumentRepository Documents
        {
            get
            {
                if (_Documents == null)
                    _Documents = new DocumentRepository(_context);

                return _Documents;
            }
        }



        public IEvictionsRepository evictions
        {
            get
            {
                if (_evictions == null)
                    _evictions = new EvictionRepository(_context);

                return _evictions;
            }
        }




        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
