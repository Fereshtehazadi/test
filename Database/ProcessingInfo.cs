using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMaker.Database 
{
    // klass för databastabellen ProcessingInfo
    class ProcessingInfo
    {
        // kolumner i tabellen 
        public int Id { get; set; }
        public String NextPassword { get; set; }
        public long NoOfProcessedPasswords { get; set; }
    }
}
