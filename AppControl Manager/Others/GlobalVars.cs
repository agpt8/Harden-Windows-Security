using System;
using System.IO;

namespace AppControlManager.Others;

// This class defines constant variables
internal static class GlobalVars
{

	// User Mode block rules
	internal static readonly Uri MSFTRecommendedBlockRulesURL = new("https://raw.githubusercontent.com/MicrosoftDocs/windows-itpro-docs/refs/heads/public/windows/security/application-security/application-control/app-control-for-business/design/applications-that-can-bypass-appcontrol.md");

	// Kernel Mode block rules
	internal static readonly Uri MSFTRecommendedDriverBlockRulesURL = new("https://raw.githubusercontent.com/MicrosoftDocs/windows-itpro-docs/refs/heads/public/windows/security/application-security/application-control/app-control-for-business/design/microsoft-recommended-driver-block-rules.md");

	// Storing the path to the Code Integrity Schema XSD file
	internal static readonly string CISchemaPath = Path.Combine(
		Environment.GetEnvironmentVariable("SystemDrive") + @"\",
		"Windows", "schemas", "CodeIntegrity", "cipolicy.xsd");

	// Storing the path to the WDACConfig folder in the Program Files
	internal static readonly string UserConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WDACConfig");

	// Storing the path to User Config JSON file in the WDACConfig folder in the Program Files
	internal static readonly string UserConfigJson = Path.Combine(UserConfigDir, "UserConfigurations", "UserConfigurations.json");

	// Storing the path to the StagingArea folder in the WDACConfig folder in the Program Files
	internal static readonly string StagingArea = Path.Combine(UserConfigDir, "StagingArea");

	// The link to the file that contains the download link for the latest version of the AppControl Manager
	internal static readonly Uri AppUpdateDownloadLinkURL = new("https://raw.githubusercontent.com/HotCakeX/Harden-Windows-Security/refs/heads/main/AppControl%20Manager/DownloadURL.txt");

	// The link to the file that contains the version number of the latest available version of the AppControl Manager
	internal static readonly Uri AppVersionLinkURL = new("https://raw.githubusercontent.com/HotCakeX/Harden-Windows-Security/refs/heads/main/AppControl%20Manager/version.txt");

	// The check for update button on the Update page uses this variable as the text/content for its button
	// It is dynamically updated on app launch when a new version is available
	internal static string updateButtonTextOnTheUpdatePage = "Check for update";

	// Handle of the main Window - acquired in the MainWindow.xaml.cs
	internal static nint hWnd;

	// The filter for the file picker dialog to select XML files
	internal const string XMLFilePickerFilter = "XML file|*.xml";

	// The filter for the file picker dialog to select Any files
	internal const string AnyFilePickerFilter = "Any file (*.*)|*.*";

	// Name of the special automatic supplemental policy
	internal const string AppControlManagerSpecialPolicyName = "AppControlManagerSupplementalPolicy";

	// Path to the AppControlManagerSpecialPolicyName.xml file
	internal static readonly string AppControlManagerSpecialPolicyPath = Path.Combine(AppContext.BaseDirectory, "Resources", $"{AppControlManagerSpecialPolicyName}.xml");

	// Path to the ISGBasedSupplementalPolicy.xml file
	internal static readonly string ISGOnlySupplementalPolicyPath = Path.Combine(AppContext.BaseDirectory, "Resources", "ISGBasedSupplementalPolicy.xml");

	// Get the current OS version
	private static readonly Version CurrentOSVersion = Environment.OSVersion.Version;

	// Version for the build 24H2
	private static readonly Version VersionFor24H2 = new(10, 0, 26100, 0);

	// Determine whether the current OS is older than 24H2
	internal static bool IsOlderThan24H2 => CurrentOSVersion < VersionFor24H2;

	static GlobalVars()
	{
		// Ensure the directory exists
		if (!Directory.Exists(UserConfigDir))
		{
			_ = Directory.CreateDirectory(UserConfigDir);
		}
	}
}