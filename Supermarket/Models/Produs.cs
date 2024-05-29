using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models
{
    public class Produs
    {
        public int ProdusID { get; set; }
        public string NumeProdus { get; set; }
        public string CodeDeBare { get; set; }
        public int CategorieID { get; set; }
        public int ProducatorID { get; set; }
        public bool IsDeleted { get; set; }
    }

}
