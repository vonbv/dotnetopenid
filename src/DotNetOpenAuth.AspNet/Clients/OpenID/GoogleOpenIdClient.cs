﻿//-----------------------------------------------------------------------
// <copyright file="GoogleOpenIdClient.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System.Collections.Generic;
	using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
	using DotNetOpenAuth.OpenId.RelyingParty;

	/// <summary>
	/// Represents Google OpenID client.
	/// </summary>
	public sealed class GoogleOpenIdClient : OpenIdClient {
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="GoogleOpenIdClient"/> class.
		/// </summary>
		public GoogleOpenIdClient()
			: base("google", "https://www.google.com/accounts/o8/id") {}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the extra data obtained from the response message when authentication is successful.
		/// </summary>
		/// <param name="response">
		/// The response message. 
		/// </param>
		/// <returns>
		/// </returns>
		protected override Dictionary<string, string> GetExtraData(IAuthenticationResponse response) {
			FetchResponse fetchResponse = response.GetExtension<FetchResponse>();
			if (fetchResponse != null) {
				var extraData = new Dictionary<string, string>();
				extraData.AddItemIfNotEmpty("email", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email));
				extraData.AddItemIfNotEmpty(
					"country", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.Country));
				extraData.AddItemIfNotEmpty("firstName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First));
				extraData.AddItemIfNotEmpty("lastName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last));

				return extraData;
			}

			return null;
		}

		/// <summary>
		/// Called just before the authentication request is sent to service provider.
		/// </summary>
		/// <param name="request">
		/// The request. 
		/// </param>
		protected override void OnBeforeSendingAuthenticationRequest(IAuthenticationRequest request) {
			// Attribute Exchange extensions
			var fetchRequest = new FetchRequest();
			fetchRequest.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, isRequired: true));
			fetchRequest.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.HomeAddress.Country, isRequired: false));
			fetchRequest.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, isRequired: false));
			fetchRequest.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, isRequired: false));

			request.AddExtension(fetchRequest);
		}

		#endregion
	}
}
