using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData
{
    public class ArtWorkTag
    {
        [Key] public int ArtWorkTagId { get; set; }

        [ForeignKey("ArtWork")] public string ArtWorkId { get; set; }

        [ForeignKey("ArtTag")] public int ArtTagId { get; set; }
    }
}