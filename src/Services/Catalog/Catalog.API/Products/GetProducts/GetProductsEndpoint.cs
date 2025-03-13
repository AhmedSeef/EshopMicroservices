using Catalog.API.Models;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest();
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var res = await sender.Send(new GetProductsQuery());
                var response = res.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            });
        }
    }
}
