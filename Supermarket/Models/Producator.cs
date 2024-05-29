using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models
{
    public class Producator
    {
        public int ProducatorID { get; set; }
        public string NumeProducator { get; set; }
        public string TaraOrigine { get; set; }
        public bool IsDeleted { get; set; }
    }

}
