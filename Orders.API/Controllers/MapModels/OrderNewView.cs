using System.Collections.Generic;

namespace Orders.API.Controllers.MapModels
{
    public class OrderNewView
    {
        public string Label { get; set; }

        public string DescriptionFromCustomer { get; set; }

        public List<ArtTagView> Tags { get; set; }
    }
}