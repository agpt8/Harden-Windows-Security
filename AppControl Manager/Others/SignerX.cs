// MIT License
//
// Copyright (c) 2023-Present - Violet Hansen - (aka HotCakeX on GitHub) - Email Address: spynetgirl@outlook.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// See here for more information: https://github.com/HotCakeX/Harden-Windows-Security/blob/main/LICENSE
//

using System.Collections.Generic;

namespace AppControlManager.Others;

// Information that's included in an App Control policy
internal sealed class SignerX(
	string id,
	string name,
	string certRoot,
	string? certPublisher,
	string? certIssuer,
	string[]? certEKU,
	string? certOemID,
	string[] fileAttribRef,
	Dictionary<string,
	Dictionary<string, string>> fileAttrib,
	string signerScope,
	bool isWHQL,
	bool isAllowed,
	bool hasEKU
	)
{
	internal string ID { get; set; } = id;
	internal string Name { get; set; } = name;
	internal string CertRoot { get; set; } = certRoot;
	internal string? CertPublisher { get; set; } = certPublisher;
	internal string? CertIssuer { get; set; } = certIssuer;
	internal string[]? CertEKU { get; set; } = certEKU;
	internal string? CertOemID { get; set; } = certOemID;
	internal string[] FileAttribRef { get; set; } = fileAttribRef;
	internal Dictionary<string, Dictionary<string, string>> FileAttrib { get; set; } = fileAttrib;
	internal string SignerScope { get; set; } = signerScope;
	internal bool IsWHQL { get; set; } = isWHQL;
	internal bool IsAllowed { get; set; } = isAllowed;
	internal bool HasEKU { get; set; } = hasEKU;
}
