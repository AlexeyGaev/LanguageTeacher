using Microsoft.AspNetCore.Http;

namespace Translation.Http.Converter {
    public class HttpRequestConverter : IToStringConverter<HttpRequest> {
        public string Convert(HttpRequest request) {
            return "HttpRequest" + request != null ? ":\n " + request.ToString() : " is Null.\n";
        }
    }
}