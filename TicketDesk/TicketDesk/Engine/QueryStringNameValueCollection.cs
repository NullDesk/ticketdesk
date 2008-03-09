using System.Collections.Specialized;
using System.Text;
using System.Web;
namespace TicketDesk.Engine
{
    /// <summary>
    /// An implementation of NameValueCollection with an overridden ToString() method with behavior that matches
    /// the behavior of the HttpRequest.QueryString collection. 
    /// </summary>
    /// <remarks>
    /// The QueryString deserves special explaination. Though the documentation claims that 
    /// HttpRequest.QueryString is a property of type NameValueCollection, it is actually a more specific collection 
    /// type that inherits from NameValueCollection. The HttpRequest.QueryString property casts the specific type to 
    /// the base NVC type. 
    /// 
    /// This is important because the specific implementation of the collection overrides the 
    /// ToString() method of NameValueCollection (which is the inherited logic from System.Object) with a new 
    /// implementation that returns the keys and values from the collection in URL querystring format. 
    /// 
    /// This class is designed to mimik this behavior since I do not know which actual collection class was used by the 
    /// HttpRequest.QueryString property behind the scenes, nor do I know if we had access to that type if we knew what it was.
    /// </remarks>
    public class QueryStringNameValueCollection : NameValueCollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">The source NameValueCollection from which to copy contents for this instance.</param>
        public QueryStringNameValueCollection(NameValueCollection col)
            : base(col)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public QueryStringNameValueCollection()
        {
        }


        /// <summary>
        /// Returns the NameValueCollection contents in querystring form
        /// </summary>
        /// <example> 
        /// "Key1=Value1&Key2=Value2&Key3=Value3"
        /// </example>
        /// <returns>The keys and values of the collection in URL Querystring format.</returns>
        public override string ToString()
        {
            HttpContext context = HttpContext.Current;
            StringBuilder sb = new StringBuilder();
            foreach(string key in base.Keys)
            {
                sb.Append(key);
                sb.Append("=");
                sb.Append(context.Server.UrlEncode(base[key]));
                sb.Append("&");
            }
            if(sb.Length > 0)//only if there is a query string present.
            {
                sb.Remove(sb.Length - 1, 1); //trim off the last '&'
            }
            return sb.ToString();
        }
    }

}