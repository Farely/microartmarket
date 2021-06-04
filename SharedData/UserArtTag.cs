using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData
{
    public class UserArtTag
    {
        [Key] public int UserArtTagId { get; set; }

        [ForeignKey("ApplicationUserId")] public string ApplicationUserId { get; set; }

        [ForeignKey("ArtTag")] public int ArtTagId { get; set; }

        public ArtTag ArtTag { get; set; }


        [Range(0, 5)] public int Rate { get; set; }
    }
}