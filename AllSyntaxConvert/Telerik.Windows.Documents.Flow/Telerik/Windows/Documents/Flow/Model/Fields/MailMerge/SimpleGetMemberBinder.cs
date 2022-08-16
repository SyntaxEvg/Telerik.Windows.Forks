using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Telerik.Windows.Documents.Flow.Model.Fields.MailMerge
{
	class SimpleGetMemberBinder : GetMemberBinder
	{
		internal SimpleGetMemberBinder(string propertyName)
			: base(propertyName, false)
		{
		}

		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject error)
		{
			return error ?? SimpleGetMemberBinder.Throw(string.Format(SimpleGetMemberBinder.ExceptionFormat, new object[] { target, base.Name }), this.ReturnType);
		}

		internal static DynamicMetaObject Throw(string message, Type returnType)
		{
			return new DynamicMetaObject(Expression.Throw(Expression.New(typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) }), new Expression[] { Expression.Constant(message) }), returnType), BindingRestrictions.Empty);
		}

		internal static readonly string ExceptionFormat = "Property path is not valid. '{0}' does not have a public property named '{1}'.";
	}
}
