using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData
{
    public class Order
    {
        [Key] public int OrderId { get; set; }

        public bool IsOver { get; set; }
        public string Label { get; set; }
        public string ReviewFromCustomer { get; set; }
        public string DescriptionFromCustomer { get; set; }
        
        public int ArtId { get; set; }
        public ArtWork Art { get; set; }
        
        public int CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        
        public int AuthorId { get; set; }
        
        public ApplicationUser Author { get; set; }
    }
}