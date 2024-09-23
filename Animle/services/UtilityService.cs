using Animle.interfaces;
using System.Reflection;
using System.Text.Json;

namespace Animle.services
{
    public static class UtilityService
    {

        public static string GetTypeByNumber(int number)
        {
            switch (number)
            {
                case 0:
                    return "emoji";
                case 1:
                    return "description";
                case 2:
                    return "image";
                case 3:
                    return "shuffled";
                case 4:
                    return "properties";
                default:
                    return "description";
            }
        }
   
        public static string Serialize(object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };


            return JsonSerializer.Serialize(obj, options);
        }

        public static ListResponse<T> GenerateQuery<T>(IQueryCollection queryCollection, List<T> query)
        {
            IEnumerable<T> queryBuilder = query;
            int page = 1;
            int limit = 20;

            foreach (var q in queryCollection)
            {
                switch (q.Key.ToLower())
                {
                    case "sort":
                        var sortParams = q.Value.ToString().Split('-');
                        if (sortParams.Length >= 2)
                        {
                            var property = typeof(T).GetProperty(sortParams[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (property != null)
                            {
                                queryBuilder = sortParams[1].ToLower() == "asc"
                                               ? queryBuilder.OrderBy(x => property.GetValue(x, null))
                                               : queryBuilder.OrderByDescending(x => property.GetValue(x, null));
                            }
                        }
                        break;

                    case "limit":
                        if (int.TryParse(q.Value.ElementAt(0), out int parsedLimit))
                        {
                            if (parsedLimit > 25)
                            {
                                limit = 25;
                            }
                            else
                            {
                                limit = parsedLimit;

                            }

                        }
                        break;

                    case "page":
                        if (int.TryParse(q.Value.ElementAt(0), out int parsedPage))
                        {
                            page = parsedPage;
                        }
                        break;
                }
            }

            queryBuilder = queryBuilder.Skip((page - 1) * limit).Take(limit);

            ListResponse<T> listResponse = new ListResponse<T>
            {
                List = queryBuilder.ToList(),
                Count = query.Count
            };

            return listResponse;
        }
    }
}
