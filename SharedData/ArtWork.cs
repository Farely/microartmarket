using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedData
{
    public class ArtWork
    {
        [Key] public int Id { get; set; }

        public ICollection<ArtTag> Tags { get; set; }
        
        public Order Order { get; set; }
        public string Label { get; set; }
        public string FileId { get; set; }
        public string OriginalFileName { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

        public ArtState ArtState { get; set; }
        public ApplicationUser Author { get; set; }
    }
}