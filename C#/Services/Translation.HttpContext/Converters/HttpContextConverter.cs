using Microsoft.AspNetCore.Http;

namespace Translation.Http.Converter {
    public class HttpContextConverter : IToStringConverter<HttpContext> {
        HttpRequestConverter requestConverter = new HttpRequestConverter();
        public string Convert(HttpContext context) {
            string result = string.Empty;
            result += requestConverter.Convert(context.Request);
            // TODO
            return context.ToString();
        }
    }
}