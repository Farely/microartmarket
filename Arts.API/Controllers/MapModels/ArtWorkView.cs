using System.ComponentModel.DataAnnotations;
using SharedData;

namespace Arts.API.Controllers.MapModels
{
    public class ArtWorkView
    {
        public int Id { get; set; }

        public string Description { get; set; }


        [Range((int) ArtState.InProgress, (int) ArtState.Started,
            ErrorMessage = "В заказы нельзя сразу выложить готовую работу(")]
        public ArtState ArtState { get; set; }

        public UserView Author { get; set; }
        public UserView Customer { get; set; }

        public string Path { get; set; }
    }
}