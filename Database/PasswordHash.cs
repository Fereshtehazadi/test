using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMaker.Database 
{
    // klass för databastabellen PasswordHash
    class PasswordHash
    {
        // kolumner i tabellen 
        public int Id { get; set; }
        public String Password { get; set; }
        public String HashSha256 { get; set; }
        public String HashSha384 { get; set; }
        public String HashSha512 { get; set; }
    }
}
