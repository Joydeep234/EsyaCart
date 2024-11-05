using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsyaCart.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Data.Context
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}
        public DbSet<Account> Account {get;set;}
        public DbSet<Cart> Cart {get;set;}
        public DbSet<Catagory> Catagory {get;set;}
        public DbSet<OrderItems> OrderItems {get;set;}

        public DbSet<Orders> Orders {get;set;}

        public DbSet<Products> Products {get;set;}

        public DbSet<Reviews> Reviews{get;set;}
        public DbSet<UserDetails> UserDetails {get;set;}
        public DbSet<VendorDetails> VendorDetails {get;set;}
    }
}