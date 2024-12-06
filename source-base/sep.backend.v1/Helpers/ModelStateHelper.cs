using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace sep.backend.v1.Helpers
{
    public static class ModelStateHelper
    {
        public static string ErrorsToJsonResult(this ModelStateDictionary modelState)
        {
            IEnumerable<KeyValuePair<string, string[]>>? errors = modelState.IsValid
                ? null
                : modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());
            return JsonConvert.SerializeObject(errors);
        }
    }
}
