using AutoMapper;
using E_Commerce.Core.Interfaces;
using E_Commerce.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }
        public UnitOfWork(ApplicationDbContext context,IFileProvider fileProvider,IMapper mapper)
        {
            _fileProvider = fileProvider;
            _mapper = mapper;
                _context = context;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context,_fileProvider,_mapper);
        }
    }
}
