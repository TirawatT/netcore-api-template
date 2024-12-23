using NetcoreApiTemplate.Models.Dtos.Exceptions;
using RestSharp;
using System.Net;

namespace NetcoreApiTemplate.Utilities
{
    public static class ApiTools
    {

        #region Sync
        public static T? Get<T>(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {

            var result = Call(Method.Get, url, param, timeout, contentType)
                            .GetResult<T>();
            return result;
        }
        public static T? Post<T>(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            var result = Call(Method.Post, url, param, timeout, contentType)
                            .GetResult<T>();
            return result;
        }
        public static T? Put<T>(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            var result = Call(Method.Put, url, param, timeout, contentType)
                            .GetResult<T>();
            return result;
        }
        public static T? Delete<T>(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            var result = Call(Method.Delete, url, param, timeout, contentType)
                            .GetResult<T>();
            return result;
        }
        #endregion
        #region Async
        public static void GetAsync(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            CallAsync(Method.Get, url, param, timeout, contentType);
        }
        public static void PostAsync(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            CallAsync(Method.Post, url, param, timeout, contentType);
        }
        public static void PutAsync(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            CallAsync(Method.Put, url, param, timeout, contentType);
        }
        public static void DeleteAsync(string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            CallAsync(Method.Delete, url, param, timeout, contentType);
        }
        #endregion 
        private static RestResponse Call(Method medthod, string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {

            var client = new RestClient(url);

            var request = new RestRequest("", medthod);
            request.AddHeader("Content-Type", contentType);

            if (param != null)
            {
                var bodyParam = param.ToJson();
                request.AddParameter("application/json", bodyParam, ParameterType.RequestBody);
            }
            RestResponse response = client.Execute(request);
            return response;
        }
        private static void CallAsync(Method medthod, string url, object? param = null, int timeout = -1, string contentType = "application/json")
        {
            var client = new RestClient(url);

            var request = new RestRequest("", medthod);
            request.AddHeader("Content-Type", contentType);
            if (param != null)
            {
                var bodyParam = param.ToJson();
                request.AddParameter("application/json", bodyParam, ParameterType.RequestBody);
            }
            client.ExecuteAsync(request);
        }
        private static T? GetResult<T>(this RestResponse response)
        {
            if (!response.IsSuccessful)
                ThrowResponeError(response);
            if (response.Content == null)
                return default;
            var result = response.Content.JsonToObject<T>();
            return result;
        }
        private static void ThrowResponeError(RestResponse response)
        {
            if (response == null) return;

            var url = response.ResponseUri?.OriginalString;
            var error = new HttpResponseExceptionDto();
            error = response?.Content?.JsonToObject<HttpResponseExceptionDto>();
            if (error == null || string.IsNullOrEmpty(error.Details))
            {
                error = new();
                if (string.IsNullOrEmpty(response?.Content))
                {
                    error.Details = response?.ErrorException?.Message;
                    error.MoreDetails = response?.ErrorException?.InnerException?.Message;
                    if (string.IsNullOrEmpty(error.Details))
                        error.Details = response?.StatusDescription;
                }
                else
                {
                    error = new HttpResponseExceptionDto()
                    {
                        Details = response?.Content,
                        MoreDetails = response?.StatusDescription
                    };
                }
            }
            HttpStatusCode statusCode = response?.StatusCode == 0 ? HttpStatusCode.InternalServerError : response!.StatusCode;
            throw new HttpResponseException().SetStatusCode((int)statusCode)
                .Type("ERROR")
                .Code("")
                .Details($"{error.Details}")
                //.MoreDetails($"{error.MoreDetails}\nAPI Error\n({url})");
                .MoreDetails($"{error.MoreDetails}{Environment.NewLine}API Error{Environment.NewLine}({url})");
        }
    }
}

