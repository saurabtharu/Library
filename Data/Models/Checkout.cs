using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class Checkout
    {
        public int Id { get; set; }

        [Required]
        public LibraryAsset LibraryAsset { get; set; }      // such as a book  - one to one relationship
        public LibraryCard LibraryCard { get; set; }        // one to one relationship
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
    }
}