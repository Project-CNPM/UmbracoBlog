//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v8.15.0
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder.Embedded;

namespace Umbraco.Web.PublishedModels
{
	/// <summary>Settings</summary>
	[PublishedModel("settings")]
	public partial class Settings : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		public new const string ModelTypeAlias = "settings";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Settings, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public Settings(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Email Settings Admin Accounts
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("emailSettingsAdminAccounts")]
		public virtual string EmailSettingsAdminAccounts => this.Value<string>("emailSettingsAdminAccounts");

		///<summary>
		/// Email Settings From Address: This is email that it will be appear to be form
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("emailSettingsFromAddress")]
		public virtual string EmailSettingsFromAddress => this.Value<string>("emailSettingsFromAddress");

		///<summary>
		/// Email Test Accounts: This comma delimited list of email addresses will be used to send all emails to when in test mode
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("emailTestAccounts")]
		public virtual string EmailTestAccounts => this.Value<string>("emailTestAccounts");

		///<summary>
		/// Recaptcha Secret Key
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("recaptchaSecretKey")]
		public virtual string RecaptchaSecretKey => this.Value<string>("recaptchaSecretKey");

		///<summary>
		/// Recaptcha Site Key
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("recaptchaSiteKey")]
		public virtual string RecaptchaSiteKey => this.Value<string>("recaptchaSiteKey");

		///<summary>
		/// Site Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("siteName")]
		public virtual string SiteName => this.Value<string>("siteName");

		///<summary>
		/// Test Mode
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.15.0")]
		[ImplementPropertyType("testMode")]
		public virtual bool TestMode => this.Value<bool>("testMode");
	}
}
