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

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HardenWindowsSecurity;

internal static partial class GUIProtectWinSecurity
{
	// During offline mode, this is the path that the button for MicrosoftSecurityBaselineZipPath assigns
	internal static string MicrosoftSecurityBaselineZipPath = string.Empty;

	// During offline mode, this is the path that the button for Microsoft365AppsSecurityBaselineZipPath assigns
	internal static string Microsoft365AppsSecurityBaselineZipPath = string.Empty;

	// During offline mode, this is the path that the button for LGPOZipPath assigns
	internal static string LGPOZipPath = string.Empty;

	// List of all the selected categories in a thread safe way
	internal static ConcurrentQueue<string> SelectedCategories = new();

	// List of all the selected subcategories in a thread safe way
	internal static ConcurrentQueue<string> SelectedSubCategories = new();

	// Set a flag indicating that the required files for the Offline operation mode have been processed
	// When the execute button was clicked, so it won't run twice
	internal static bool StartFileDownloadHasRun;

	// Defining the correlation between Categories and which Sub-Categories they activate
	internal static Hashtable correlation = new(StringComparer.OrdinalIgnoreCase)
			{
				{ "MicrosoftSecurityBaselines", new string[] { "SecBaselines_NoOverrides" } },
				{ "MicrosoftDefender", new string[] { "MSFTDefender_SAC", "MSFTDefender_NoDiagData", "MSFTDefender_NoScheduledTask", "MSFTDefender_BetaChannels" } },
				{ "LockScreen", new string[] { "LockScreen_CtrlAltDel", "LockScreen_NoLastSignedIn" } },
				{ "UserAccountControl", new string[] { "UAC_NoFastSwitching", "UAC_OnlyElevateSigned" } },
				{ "WindowsNetworking", new string[] { "WindowsNetworking_BlockNTLM" } },
				{ "TLSSecurity", new string[] { "TLSSecurity_BattleNetClient" } },
				{ "MiscellaneousConfigurations", new string[] { "Miscellaneous_WindowsProtectedPrint", "MiscellaneousConfigurations_LongPathSupport", "MiscellaneousConfigurations_StrongKeyProtection", "MiscellaneousConfigurations_ReducedTelemetry" } },
				{ "DeviceGuard", new string[] { "DeviceGuard_MandatoryVBS" } },
				{ "CountryIPBlocking", new string[] { "CountryIPBlocking_OFAC" } },
				{ "DownloadsDefenseMeasures", new string[] { "DangerousScriptHostsBlocking" } }
			};

	internal static ListView? categories;
	internal static ListView? subCategories;
	internal static CheckBox? selectAllCategories;
	internal static CheckBox? selectAllSubCategories;

	// fields for Log related elements
	internal static TextBox? txtFilePath;
	internal static Button? logPath;
	internal static ToggleButton? log;
	internal static ToggleButton? EventLogging;

	// fields for Offline-Mode related elements
	internal static ToggleButton? enableOfflineMode;
	internal static Button? microsoftSecurityBaselineZipButton;
	internal static TextBox? microsoftSecurityBaselineZipTextBox;
	internal static Button? microsoft365AppsSecurityBaselineZipButton;
	internal static TextBox? microsoft365AppsSecurityBaselineZipTextBox;
	internal static Button? lgpoZipButton;
	internal static TextBox? lgpoZipTextBox;

	// Flag to run the event for view load only once to prevent file download multiple times when switching between views etc.
	internal static bool LoadEventHasBeenTriggered;

	internal static ComboBox? ProtectionPresetComboBox;

	internal static string? SelectedProtectionPreset;

	// Defining the presets configurations for the protection
	internal static Dictionary<string, Dictionary<string, List<string>>> PresetsIntel = new(StringComparer.OrdinalIgnoreCase)
		{
			{
			"preset: basic", new Dictionary<string, List<string>>
		{
			{ "Categories", new List<string> { "MicrosoftSecurityBaselines", "Microsoft365AppsSecurityBaselines", "MicrosoftDefender", "DeviceGuard", "OptionalWindowsFeatures" } },
			{ "SubCategories", new List<string> {} }
		}
		},
		{
		"preset: recommended", new Dictionary<string, List<string>>
		{
			{ "Categories", new List<string> { "MicrosoftSecurityBaselines", "Microsoft365AppsSecurityBaselines", "MicrosoftDefender", "AttackSurfaceReductionRules", "BitLockerSettings", "DeviceGuard", "TLSSecurity", "LockScreen", "UserAccountControl", "WindowsFirewall", "OptionalWindowsFeatures", "WindowsNetworking", "MiscellaneousConfigurations", "WindowsUpdateConfigurations", "EdgeBrowserConfigurations", "DownloadsDefenseMeasures", "NonAdminCommands" } },
			{ "SubCategories", new List<string> { "WindowsNetworking_BlockNTLM", "DangerousScriptHostsBlocking","MiscellaneousConfigurations_LongPathSupport" } }
		}
		},
		{
	   "preset: complete", new Dictionary<string, List<string>>
		{
			{ "Categories", new List<string> { "MicrosoftSecurityBaselines", "Microsoft365AppsSecurityBaselines", "MicrosoftDefender", "AttackSurfaceReductionRules", "BitLockerSettings", "DeviceGuard", "TLSSecurity", "LockScreen", "UserAccountControl", "WindowsFirewall", "OptionalWindowsFeatures", "WindowsNetworking", "MiscellaneousConfigurations", "WindowsUpdateConfigurations", "EdgeBrowserConfigurations", "CountryIPBlocking", "DownloadsDefenseMeasures", "NonAdminCommands" } },
			{ "SubCategories", new List<string> { "WindowsNetworking_BlockNTLM", "Miscellaneous_WindowsProtectedPrint", "CountryIPBlocking_OFAC", "DangerousScriptHostsBlocking","MiscellaneousConfigurations_StrongKeyProtection", "MiscellaneousConfigurations_LongPathSupport" } }
		}
		}
		};
}
