// SPDX-FileCopyrightText: 2022 Cisco Systems, Inc. and/or its affiliates
//
// SPDX-License-Identifier: BSD-3-Clause

using System.ComponentModel.DataAnnotations;

namespace DuoUniversal.Example;

public class Config
{
	[Required]
	[StringLength(20, MinimumLength = 20)]
	public string ClientId { get; set; }

	[Required]
	[StringLength(40, MinimumLength = 40)]
	public string ClientSecret { get; set; }

	[Required]
	public string ApiHost { get; set; }

	[Required]
	[Url]
	public string RedirectUri { get; set; }

	public void Deconstruct(out string clientId, out string clientSecret, out string apiHost, out string redirectUri)
	{
		clientId = this.ClientId;
		clientSecret = this.ClientSecret;
		apiHost = this.ApiHost;
		redirectUri = this.RedirectUri;
	}
}
