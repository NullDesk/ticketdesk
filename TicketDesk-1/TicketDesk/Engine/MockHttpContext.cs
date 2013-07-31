// NOTE: This code is based on the following article:
// http://righteousindignation.gotdns.org/blog/archive/2004/04/13/149.aspx

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;

namespace TicketDesk.Engine
{
	public sealed class MockHttpContext
	{
		
		private const string ContextKeyAspSession = "AspSession";
		private const string ThreadDataKeyAppPath = ".appPath";
		private const string ThreadDataKeyAppPathValue = "c:\\inetpub\\wwwroot\\webapp\\";
		private const string ThreadDataKeyAppVPath = ".appVPath";
		private const string ThreadDataKeyAppVPathValue = "/webapp";
		private const string WorkerRequestPage = "default.aspx";

		private HttpContext context = null;

		private MockHttpContext() : base()
		{
		}

		public MockHttpContext(bool isSecure)
			: this()
		{
			Thread.GetDomain().SetData(
				MockHttpContext.ThreadDataKeyAppPath, MockHttpContext.ThreadDataKeyAppPathValue);
			Thread.GetDomain().SetData(
				MockHttpContext.ThreadDataKeyAppVPath, MockHttpContext.ThreadDataKeyAppVPathValue);
			SimpleWorkerRequest request = new WorkerRequest(MockHttpContext.WorkerRequestPage, 
				string.Empty, new StringWriter(), isSecure);
			this.context = new HttpContext(request);

			HttpSessionStateContainer container = new HttpSessionStateContainer(
				Guid.NewGuid().ToString("N"), new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 
				5, true, HttpCookieMode.AutoDetect, SessionStateMode.InProc, false);

			HttpSessionState state = Activator.CreateInstance(
				 typeof(HttpSessionState),
				 BindingFlags.Public | BindingFlags.NonPublic |
				 BindingFlags.Instance | BindingFlags.CreateInstance,
				 null,
				 new object[] { container }, CultureInfo.CurrentCulture) as HttpSessionState;
			this.context.Items[ContextKeyAspSession] = state;
		}

		public HttpContext Context
		{
			get
			{
				return this.context;
			}
		}

		private class WorkerRequest : SimpleWorkerRequest
		{
			private bool isSecure = false;

			public WorkerRequest(string page, string query, TextWriter output, bool isSecure)
				: base(page, query, output)
			{
				this.isSecure = isSecure;
			}

			public override bool IsSecure()
			{
				return this.isSecure;
			}
		}
	}
}
