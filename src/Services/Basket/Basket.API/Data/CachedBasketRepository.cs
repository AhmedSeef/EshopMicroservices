namespace Basket.API.Data
{
    public class CachedBasketRepository : IBasketRepository
    {
        private readonly IBasketRepository _repository;
        private readonly IDistributedCache _cache;

        public CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket = await _repository.GetBasket(userName, cancellationToken);
            await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var existingData = await _cache.GetStringAsync(basket.UserName, cancellationToken);
            var existingCart = existingData is not null
                ? JsonSerializer.Deserialize<ShoppingCart>(existingData)
                : new ShoppingCart(basket.UserName);

            foreach (var item in basket.Items)
            {
                var existingItem = existingCart.Items.FirstOrDefault(x => x.ProductName == item.ProductName);
                if (existingItem is not null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    existingCart.Items.Add(item);
                }
            }

            await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(existingCart), cancellationToken);
            return existingCart;
        }


        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var jsonData = await _cache.GetStringAsync(userName, cancellationToken);
            if (jsonData is null) return false;

            var baskets = JsonSerializer.Deserialize<List<ShoppingCart>>(jsonData);
            if (baskets is null || !baskets.Any()) return false;

            await _cache.RemoveAsync(userName, cancellationToken);
            return await _repository.DeleteBasket(userName, cancellationToken);
        }
    }
}
