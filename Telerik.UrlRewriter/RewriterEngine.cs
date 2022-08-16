using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.UrlRewriter.Configuration;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter
{
	public class RewriterEngine
	{
		public RewriterEngine(IContextFacade contextFacade, RewriterConfiguration configuration)
		{
			if (contextFacade == null)
			{
				throw new ArgumentNullException("contextFacade");
			}
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this.ContextFacade = contextFacade;
			this._configuration = configuration;
		}

		public string ResolveLocation(string location)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			string text = this.ContextFacade.GetApplicationPath();
			if (text.Length > 1)
			{
				text += "/";
			}
			return location.Replace("~/", text);
		}

		public void Rewrite()
		{
			string text = this.ContextFacade.GetRawUrl().Replace("+", " ");
			this.RawUrl = text;
			RewriteContext rewriteContext = new RewriteContext(this, text, this.ContextFacade.GetHttpMethod(), this.ContextFacade.MapPath, this.ContextFacade.GetServerVariables(), this.ContextFacade.GetHeaders(), this.ContextFacade.GetCookies());
			this.ProcessRules(rewriteContext);
			this.AppendHeaders(rewriteContext);
			this.AppendCookies(rewriteContext);
			this.ContextFacade.SetStatusCode((int)rewriteContext.StatusCode);
			if (rewriteContext.Location != text && rewriteContext.StatusCode < HttpStatusCode.BadRequest)
			{
				if (rewriteContext.StatusCode < HttpStatusCode.MultipleChoices)
				{
					this._configuration.Logger.Info(MessageProvider.FormatString(Message.RewritingXtoY, new object[]
					{
						this.ContextFacade.GetRawUrl(),
						rewriteContext.Location
					}));
					this.VerifyResultExists(rewriteContext);
					this.ContextFacade.RewritePath(rewriteContext.Location);
				}
				else
				{
					this._configuration.Logger.Info(MessageProvider.FormatString(Message.RedirectingXtoY, new object[]
					{
						this.ContextFacade.GetRawUrl(),
						rewriteContext.Location
					}));
					this.ContextFacade.SetRedirectLocation(rewriteContext.Location);
				}
			}
			else if (rewriteContext.StatusCode >= HttpStatusCode.BadRequest)
			{
				this.HandleError(rewriteContext);
			}
			else if (this.HandleDefaultDocument(rewriteContext))
			{
				this.ContextFacade.RewritePath(rewriteContext.Location);
			}
			this.SetContextItems(rewriteContext);
		}

		public string Expand(RewriteContext context, string input)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			string result;
			using (StringReader stringReader = new StringReader(input))
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					for (char c = (char)stringReader.Read(); c != '\uffff'; c = (char)stringReader.Read())
					{
						if (c == '$')
						{
							stringWriter.Write(this.Reduce(context, stringReader));
						}
						else
						{
							stringWriter.Write(c);
						}
					}
					result = stringWriter.GetStringBuilder().ToString();
				}
			}
			return result;
		}

		void ProcessRules(RewriteContext context)
		{
			IList rules = this._configuration.Rules;
			int num = 0;
			for (int i = 0; i < rules.Count; i++)
			{
				IRewriteCondition rewriteCondition = rules[i] as IRewriteCondition;
				if (rewriteCondition == null || rewriteCondition.IsMatch(context))
				{
					IRewriteAction rewriteAction = rules[i] as IRewriteAction;
					rewriteAction.Execute(context);
					if (rewriteAction.Processing == RewriteProcessing.StopProcessing)
					{
						this._configuration.Logger.Debug(MessageProvider.FormatString(Message.StoppingBecauseOfRule, new object[0]));
						return;
					}
					if (rewriteAction.Processing == RewriteProcessing.RestartProcessing)
					{
						this._configuration.Logger.Debug(MessageProvider.FormatString(Message.RestartingBecauseOfRule, new object[0]));
						i = 0;
						if (++num > 10)
						{
							throw new InvalidOperationException(MessageProvider.FormatString(Message.TooManyRestarts, new object[0]));
						}
					}
				}
			}
		}

		bool HandleDefaultDocument(RewriteContext context)
		{
			Uri uri = new Uri(this.ContextFacade.GetRequestUrl(), context.Location);
			UriBuilder uriBuilder = new UriBuilder(uri);
			UriBuilder uriBuilder2 = uriBuilder;
			uriBuilder2.Path += "/";
			uri = uriBuilder.Uri;
			if (uri.Host == this.ContextFacade.GetRequestUrl().Host)
			{
				string text = this.ContextFacade.MapPath(uri.AbsolutePath);
				if (Directory.Exists(text))
				{
					foreach (string text2 in RewriterConfiguration.Current.DefaultDocuments)
					{
						string path = Path.Combine(text, text2);
						if (File.Exists(path))
						{
							context.Location = new Uri(uri, text2).AbsolutePath;
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		void VerifyResultExists(RewriteContext context)
		{
			if (string.Compare(context.Location, this.ContextFacade.GetRawUrl()) != 0 && context.StatusCode < HttpStatusCode.MultipleChoices)
			{
				Uri uri = new Uri(this.ContextFacade.GetRequestUrl(), context.Location);
				if (uri.Host == this.ContextFacade.GetRequestUrl().Host)
				{
					string text = this.ContextFacade.MapPath(uri.AbsolutePath);
					if (!File.Exists(text))
					{
						this._configuration.Logger.Debug(MessageProvider.FormatString(Message.ResultNotFound, new object[] { text }));
						context.StatusCode = HttpStatusCode.NotFound;
						return;
					}
					this.HandleDefaultDocument(context);
				}
			}
		}

		void HandleError(RewriteContext context)
		{
			this.ContextFacade.SetStatusCode((int)context.StatusCode);
			IRewriteErrorHandler rewriteErrorHandler = this._configuration.ErrorHandlers[(int)context.StatusCode] as IRewriteErrorHandler;
			if (rewriteErrorHandler != null)
			{
				try
				{
					this._configuration.Logger.Debug(MessageProvider.FormatString(Message.CallingErrorHandler, new object[0]));
					this.ContextFacade.HandleError(rewriteErrorHandler);
					return;
				}
				catch (HttpException)
				{
					throw;
				}
				catch (Exception ex)
				{
					this._configuration.Logger.Fatal(ex.Message, ex);
					throw new HttpException(500, HttpStatusCode.InternalServerError.ToString());
				}
			}
			throw new HttpException((int)context.StatusCode, context.StatusCode.ToString());
		}

		void AppendHeaders(RewriteContext context)
		{
			foreach (object obj in context.Headers)
			{
				string name = (string)obj;
				this.ContextFacade.AppendHeader(name, context.Headers[name]);
			}
		}

		void AppendCookies(RewriteContext context)
		{
			for (int i = 0; i < context.Cookies.Count; i++)
			{
				HttpCookie cookie = context.Cookies[i];
				this.ContextFacade.AppendCookie(cookie);
			}
		}

		void SetContextItems(RewriteContext context)
		{
			this.OriginalQueryString = new Uri(this.ContextFacade.GetRequestUrl(), this.ContextFacade.GetRawUrl()).Query.Replace("?", "");
			this.QueryString = new Uri(this.ContextFacade.GetRequestUrl(), context.Location).Query.Replace("?", "");
			foreach (object obj in context.Properties.Keys)
			{
				string text = (string)obj;
				this.ContextFacade.SetItem(string.Format("Rewriter.{0}", text), context.Properties[text]);
			}
		}

		public string RawUrl
		{
			get
			{
				return (string)this.ContextFacade.GetItem("UrlRewriter.NET.RawUrl");
			}
			set
			{
				this.ContextFacade.SetItem("UrlRewriter.NET.RawUrl", value);
			}
		}

		public string OriginalQueryString
		{
			get
			{
				return (string)this.ContextFacade.GetItem("UrlRewriter.NET.OriginalQueryString");
			}
			set
			{
				this.ContextFacade.SetItem("UrlRewriter.NET.OriginalQueryString", value);
			}
		}

		public string QueryString
		{
			get
			{
				return (string)this.ContextFacade.GetItem("UrlRewriter.NET.QueryString");
			}
			set
			{
				this.ContextFacade.SetItem("UrlRewriter.NET.QueryString", value);
			}
		}

		string Reduce(RewriteContext context, StringReader reader)
		{
			char c = (char)reader.Read();
			string text2;
			if (char.IsDigit(c))
			{
				string text = c.ToString();
				if (char.IsDigit((char)reader.Peek()))
				{
					text += ((char)reader.Read()).ToString();
				}
				if (context.LastMatch != null)
				{
					Group group = context.LastMatch.Groups[Convert.ToInt32(text)];
					if (group != null)
					{
						text2 = group.Value;
					}
					else
					{
						text2 = string.Empty;
					}
				}
				else
				{
					text2 = string.Empty;
				}
			}
			else if (c == '<')
			{
				string groupname;
				using (StringWriter stringWriter = new StringWriter())
				{
					c = (char)reader.Read();
					while (c != '>' && c != '\uffff')
					{
						if (c == '$')
						{
							stringWriter.Write(this.Reduce(context, reader));
						}
						else
						{
							stringWriter.Write(c);
						}
						c = (char)reader.Read();
					}
					groupname = stringWriter.GetStringBuilder().ToString();
				}
				if (context.LastMatch != null)
				{
					Group group2 = context.LastMatch.Groups[groupname];
					if (group2 != null)
					{
						text2 = group2.Value;
					}
					else
					{
						text2 = string.Empty;
					}
				}
				else
				{
					text2 = string.Empty;
				}
			}
			else if (c == '{')
			{
				bool flag = false;
				bool flag2 = false;
				string text3;
				using (StringWriter stringWriter2 = new StringWriter())
				{
					c = (char)reader.Read();
					while (c != '}' && c != '\uffff')
					{
						if (c == '$')
						{
							stringWriter2.Write(this.Reduce(context, reader));
						}
						else
						{
							if (c == ':')
							{
								flag = true;
							}
							else if (c == '(')
							{
								flag2 = true;
							}
							stringWriter2.Write(c);
						}
						c = (char)reader.Read();
					}
					text3 = stringWriter2.GetStringBuilder().ToString();
				}
				if (flag)
				{
					Match match = Regex.Match(text3, "^([^\\:]+)\\:([^\\|]+)(\\|(.+))?$");
					string value = match.Groups[1].Value;
					string value2 = match.Groups[2].Value;
					string value3 = match.Groups[4].Value;
					text2 = this._configuration.TransformFactory.GetTransform(value).ApplyTransform(value2);
					if (text2 == null)
					{
						text2 = value3;
					}
				}
				else if (flag2)
				{
					Match match2 = Regex.Match(text3, "^([^\\(]+)\\((.+)\\)$");
					string value4 = match2.Groups[1].Value;
					string value5 = match2.Groups[2].Value;
					IRewriteTransform transform = this._configuration.TransformFactory.GetTransform(value4);
					if (transform != null)
					{
						text2 = transform.ApplyTransform(value5);
					}
					else
					{
						text2 = text3;
					}
				}
				else
				{
					text2 = context.Properties[text3];
				}
			}
			else
			{
				text2 = c.ToString();
			}
			return text2;
		}

		const string ContextQueryString = "UrlRewriter.NET.QueryString";

		const string ContextOriginalQueryString = "UrlRewriter.NET.OriginalQueryString";

		const string ContextRawUrl = "UrlRewriter.NET.RawUrl";

		RewriterConfiguration _configuration;

		IContextFacade ContextFacade;
	}
}
