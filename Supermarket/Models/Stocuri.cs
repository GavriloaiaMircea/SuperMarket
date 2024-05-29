using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models
{
    public class Stoc
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CodDeBare { get; set; }  
        public decimal Cantitate { get; set; }
        public string UnitateDeMasura { get; set; }
        public DateTime? DataAprovizionarii { get; set; }
        public DateTime? DataExpirarii { get; set; }
        public decimal PretAchizitie { get; set; }
        public decimal PretVanzare { get; set; }
    }

}
