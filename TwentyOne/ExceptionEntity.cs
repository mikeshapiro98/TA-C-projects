using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{
    public class ExceptionEntity  //If you call a class an entity, people will assume that you are mapping to a database column
    {
        
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
//These are all direct properties of the database that logs exception. We have this entity which maps exactly to our database table. 