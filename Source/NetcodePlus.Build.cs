// Copyright 1998-2015 Epic Games, Inc. All Rights Reserved.
//
// UE5.8 port notes:
//   * ModuleRules ctor now takes ReadOnlyTargetRules and must chain : base(Target).
//   * The old UE4.15 layout used PublicIncludePaths "NetcodePlus/Public" and a
//     PrivateIncludePaths reach-in of "UnrealTournament/Classes"/"UnrealTournament/Private".
//     UBT auto-adds this module's own Public/ and Private/ (the .Build.cs sits in Source/),
//     and the UnrealTournament module has no Classes/ dir anymore (flattened during
//     4.15->4.27). We now reach into the UT module the same way the base game module
//     and the ported UTDomGameMode plugin do: explicit Private/Public/Slate paths.
//   * bLegacyPublicIncludePaths keeps the recursive Public/ subdir includes (Glicko2)
//     resolving by bare name.

using UnrealBuildTool;
using System.IO;

public class NetcodePlus : ModuleRules
{
	public NetcodePlus(ReadOnlyTargetRules Target) : base(Target)
	{
		bUseUnity = false;
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
		UnsafeTypeCastWarningLevel = WarningLevel.Off;
		bEnableUndefinedIdentifierWarnings = false;
		bLegacyPublicIncludePaths = true;

		// Vendored Glicko2 (github.com/tronunator/Glicko2). Cross-includes like
		// #include "TeamGlickoRating.h" resolve here without editing the vendored files.
		PublicIncludePaths.AddRange(new string[]
		{
			Path.Combine(ModuleDirectory, "Public", "Glicko2"),
		});

		// Reach into the UnrealTournament game module's private + Slate headers the same
		// way the base module and UTDomGameMode plugin do (bare-name #include "UTCharacter.h",
		// #include "SUTMenuBase.h", etc. resolve through these).
		string UTModuleDir = Path.Combine(ModuleDirectory, "..", "..", "..", "Source", "UnrealTournament");
		PrivateIncludePaths.AddRange(new string[]
		{
			Path.Combine(UTModuleDir, "Public"),
			Path.Combine(UTModuleDir, "Public", "UMG"),
			Path.Combine(UTModuleDir, "Public", "Compat"),
			Path.Combine(UTModuleDir, "Private"),
			Path.Combine(UTModuleDir, "Private", "Slate"),
			Path.Combine(UTModuleDir, "Private", "Slate", "Base"),
			Path.Combine(UTModuleDir, "Private", "Slate", "Dialogs"),
			Path.Combine(UTModuleDir, "Private", "Slate", "Menus"),
			Path.Combine(UTModuleDir, "Private", "Slate", "Panels"),
			Path.Combine(UTModuleDir, "Private", "Slate", "Toasts"),
			Path.Combine(UTModuleDir, "Private", "Slate", "Widgets"),
			Path.Combine(UTModuleDir, "Private", "Slate", "UIWindows"),
			Path.Combine(UTModuleDir, "Private", "Online"),
		});

		PublicDependencyModuleNames.AddRange(new string[]
		{
			"Core",
			"CoreUObject",
			"Engine",
			"UnrealTournament",
			"InputCore",
			"Slate",
			"SlateCore"
		});

		PrivateDependencyModuleNames.AddRange(new string[]
		{
			"AssetRegistry",
			"AppFramework",   // SColorPicker (used by SNCPlusHUDEditor color swatches)
			"Http",
			"Json",
			"JsonUtilities",
			"RenderCore"      // GWhiteTexture (QuickStats DrawArc canvas fallback)
		});
	}
}
