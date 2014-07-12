using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcMovie.Models
{
    public class Ammo
    {
        public int AmmoID { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string NumberRounds { get; set; }
        public int PricePerRounds { get; set; }
        public string Grains { get; set; }
        public string Caliber { get; set; }
        public string Manufacturer { get; set; }
        public string Retailer { get; set; }
        public string RetailerURL { get; set; }
        public Boolean InStock { get; set; }
        public string NumberInSock { get; set; } 
        public string ProductURL { get; set; }
        public AmmoRetailer MyAmmoRetailer {get; set;}

    }

    public class AmmoDBContext: DbContext
    {

        public DbSet<Ammo> Ammos { get; set; }
        public DbSet<AmmoRetailer> AmmoRetailers { get; set; }
    }


   }