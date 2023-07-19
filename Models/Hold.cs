using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Hold
    {
        public int Id { get; set; }

        public virtual LibraryAsset LibraryAsset { get; set; }      // for which hold has been requested
        public virtual LibraryCard LibraryCard { get; set; }       // from which ID the hold for the asset has been requested
        public DateTime HoldPlaced { get; set; }
    }
}