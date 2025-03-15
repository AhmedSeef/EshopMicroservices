namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);
    public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var dummyItems = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    Quantity = 1,
                    Color = "Red",
                    Price = 10.99m,
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product 1"
                },
                new ShoppingCartItem
                {
                    Quantity = 2,
                    Color = "Blue",
                    Price = 20.99m,
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product 2"
                }
            };

            var dummyCart = new ShoppingCart(query.UserName)
            {
                Items = dummyItems,
            };

            // Return dummy data
            return await Task.FromResult(new GetBasketResult(dummyCart));
        }
    }
}
