﻿using System.Collections.Generic;
using AppControlManager.SiPolicy;

namespace AppControlManager.SiPolicyIntel;

// For Levels: FilePublisher, SignedVersion
internal sealed class FilePublisherSignerRule
{
	internal required List<FileAttrib> FileAttribElements { get; set; }
	internal AllowedSigner? AllowedSignerElement { get; set; }
	internal DeniedSigner? DeniedSignerElement { get; set; }
	internal CiSigner? CiSignerElement { get; set; }
	internal required Signer SignerElement { get; set; }
	internal required SSType SigningScenario { get; set; }
	internal required Authorization Auth { get; set; }
}
