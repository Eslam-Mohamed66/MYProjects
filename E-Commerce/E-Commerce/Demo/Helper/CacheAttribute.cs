using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Services.CacheService;
using System.Text;

namespace Demo.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var casheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);

            var cashedResponse = await cacheService.GetCacheResponseAsync(casheKey);
            
            if(!string.IsNullOrEmpty(cashedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cashedResponse,
                    ContentType = "appliation/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult response)
                await cacheService.SetCacheResponseAsync(casheKey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }

        private string  GenerateCasheKeyFromRequest(HttpRequest request)
        {
            var casheKey = new StringBuilder();

            casheKey.Append($"{request.Path}");

            foreach (var (key , value) in  request.Query.OrderBy(x => x.Key)) // tuple
            {
                casheKey.Append($"|{key}-{value}");
            }

            return casheKey.ToString();
        }
    }
}
