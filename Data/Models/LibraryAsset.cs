using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public abstract class LibraryAsset
    {
        
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }

        [Required]        
        public Status Status { get; set; }


        [Required]        
        public decimal Cost { get; set; }  
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public virtual LibraryBranch Location { get; set; }             // established foreign key relationship between any thing that is library asset and a particular library branch
                                                                    // LibraryBranch's Id will be in this column of Library Asset
    }
}