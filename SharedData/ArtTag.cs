using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedData
{
    public class ArtTag
    {
        [Key] public int ArtTagId { get; set; }
        
        public ICollection<ArtWork> ArtWorks { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }
}