using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models
{
    public class Bon
    {
        public int BonID { get; set; }
        public DateTime DataEliberarii { get; set; }
        public string Casier { get; set; }
        public decimal Total { get; set; }
        public List<BonProdus> Produse { get; set; } = new List<BonProdus>();
    }
}
