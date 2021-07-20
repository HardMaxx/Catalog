using Catalog.Api.Api.Dtos;
using Catalog.Api.Api.Entities;

namespace Catalog.Api.Api
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}