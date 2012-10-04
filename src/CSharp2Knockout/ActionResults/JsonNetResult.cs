using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace CSharp2Knockout.ActionResults
{
    public class JsonNetResult : ActionResult
    {
        public JsonNetResult()
        {
        }

        public JsonNetResult(object responseBody)
        {
            ResponseBody = responseBody;
        }

        public JsonNetResult(object responseBody, JsonSerializerSettings settings)
            : this(responseBody)
        {
            Settings = settings;
        }

        /// <summary>Gets or sets the serialiser settings</summary> 
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>Gets or sets the encoding of the response</summary> 
        public Encoding ContentEncoding { get; set; }

        /// <summary>Gets or sets the content type for the response</summary> 
        public string ContentType { get; set; }

        /// <summary>Gets or sets the body of the response</summary> 
        public object ResponseBody { get; set; }

        /// <summary>Gets the formatting types depending on whether we are in debug mode</summary> 
        public Formatting Formatting
        {
            get
            {
                return Debugger.IsAttached ? Formatting.Indented : Formatting.None;
            }
        }

        /// <summary> 
        /// Serialises the response and writes it out to the response object 
        /// </summary> 
        /// <param name="context">The execution context</param> 
        public override void ExecuteResult(ControllerContext context)
        {
            Contract.Requires(context != null);

            HttpResponseBase response = context.HttpContext.Response;

            // set content type 
            if(!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            // set content encoding 
            if(ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if(ResponseBody != null)
            {
                var answer = JsonConvert.SerializeObject(ResponseBody, Formatting, Settings);
                response.Write(answer);
            }
        }
    }

}
