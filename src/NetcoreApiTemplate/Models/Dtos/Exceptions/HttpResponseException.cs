using System.Net;

namespace NetcoreApiTemplate.Models.Dtos.Exceptions
{
    public class HttpResponseException : Exception
    {
        private string _type { get; set; } = "ERROR";
        private string _code { get; set; } = "";
        private string _details { get; set; } = "";
        private string _moreDetails { get; set; } = "";


        private int _statusCode { get; set; } = 500;
        private object? _value { get; set; } = null;

        public HttpResponseException()
        {

        }
        public HttpResponseException(int? statusCode = null, object? value = null)
        {
            this._statusCode = statusCode ?? this._statusCode;
            this._value = value;
        }
        public HttpResponseException(int? statusCode = null, string type = "", string code = "", string details = "", string moreDetails = "")
        {
            this._statusCode = statusCode ?? this._statusCode;

            this._type = !string.IsNullOrEmpty(type) ? type : this._type;
            this._code = !string.IsNullOrEmpty(code) ? code : this._code;
            this._details = !string.IsNullOrEmpty(details) ? details : this._details;
            this._moreDetails = !string.IsNullOrEmpty(moreDetails) ? moreDetails : this._moreDetails;
        }

        public HttpResponseException SetStatusCode(int statusCode)
        {
            this._statusCode = statusCode;
            return this;
        }
        /// <summary>
        /// 404 HttpStatusCode.NotFound
        /// </summary>
        /// <returns></returns>
        public HttpResponseException NotFound()
        {
            this._statusCode = (int)HttpStatusCode.NotFound;
            return this;
        }
        /// <summary>
        /// 400 HttpStatusCode.BadRequest
        /// </summary>
        /// <returns></returns>
        public HttpResponseException BadRequest()
        {
            this._statusCode = (int)HttpStatusCode.BadRequest;
            return this;
        }
        /// <summary>
        /// 415 HttpStatusCode.UnsupportedMediaType
        /// </summary>
        /// <returns></returns>
        public HttpResponseException UnsupportedMediaType()
        {
            this._statusCode = (int)HttpStatusCode.UnsupportedMediaType;
            return this;
        }
        /// <summary>
        /// 500 HttpStatusCode.InternalServerError
        /// </summary>
        /// <returns></returns>
        public HttpResponseException InternalServerError()
        {
            this._statusCode = (int)HttpStatusCode.InternalServerError;
            return this;
        }
        public HttpResponseException Error()
        {
            this._type = "ERROR";
            return this;
        }
        public HttpResponseException Warning()
        {
            this._type = "WARNING";
            return this;
        }
        public HttpResponseException Info()
        {
            this._type = "INFO";
            return this;
        }
        public HttpResponseException Type(string type)
        {
            this._type = type;
            return this;
        }
        public HttpResponseException Code(string code)
        {
            this._code = code;
            return this;
        }
        public HttpResponseException Details(string details)
        {
            this._details = details;
            return this;
        }
        public HttpResponseException MoreDetails(string moreDetails)
        {
            this._moreDetails = moreDetails;
            return this;
        }
        public object GetValue()
        {
            return this._value ?? new
            {
                // -- default error model
                Type = this._type,
                Code = this._code,
                Details = this._details,
                MoreDetails = this._moreDetails
            };
        }
        public int GetStatusCode()
        {
            return this._statusCode;
        }

    }

    public class HttpResponseExceptionDto
    {
        public string? Type { get; set; }
        public string? Code { get; set; }
        public string? Details { get; set; }
        public string? MoreDetails { get; set; }
    }
}
