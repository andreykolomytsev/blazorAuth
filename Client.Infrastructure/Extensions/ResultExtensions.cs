using AuthClient.Shared.Wrapper;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthClient.Client.Infrastructure.Extensions
{
    internal static class ResultExtensions
    {
        /// <summary>
        /// Десериализуем ответ к объекту
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return responseObject;
        }

        /// <summary>
        /// Десериализуем ответ
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static async Task<IResult> ToResult(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return responseObject;
        }

        /// <summary>
        /// Десериализуем ответ к объекту с пагинацией
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static async Task<PaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return responseObject;
        }
    }
}