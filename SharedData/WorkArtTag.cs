using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData
{
    public class WorkArtTag
    {
        [Key] public int OrderArtTagId { get; set; }

        [InverseProperty("Tags")] public string WorkArtId { get; set; }

        [ForeignKey("ArtTag")] public int ArtTagId { get; set; }
    }
}