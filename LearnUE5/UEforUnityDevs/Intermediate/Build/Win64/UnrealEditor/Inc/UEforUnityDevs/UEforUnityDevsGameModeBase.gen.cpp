// Copyright Epic Games, Inc. All Rights Reserved.
/*===========================================================================
	Generated code exported from UnrealHeaderTool.
	DO NOT modify this manually! Edit the corresponding .h files instead!
===========================================================================*/

#include "UObject/GeneratedCppIncludes.h"
#include "UEforUnityDevs/UEforUnityDevsGameModeBase.h"
PRAGMA_DISABLE_DEPRECATION_WARNINGS
void EmptyLinkFunctionForGeneratedCodeUEforUnityDevsGameModeBase() {}
// Cross Module References
	UEFORUNITYDEVS_API UClass* Z_Construct_UClass_AUEforUnityDevsGameModeBase_NoRegister();
	UEFORUNITYDEVS_API UClass* Z_Construct_UClass_AUEforUnityDevsGameModeBase();
	ENGINE_API UClass* Z_Construct_UClass_AGameModeBase();
	UPackage* Z_Construct_UPackage__Script_UEforUnityDevs();
// End Cross Module References
	void AUEforUnityDevsGameModeBase::StaticRegisterNativesAUEforUnityDevsGameModeBase()
	{
	}
	IMPLEMENT_CLASS_NO_AUTO_REGISTRATION(AUEforUnityDevsGameModeBase);
	UClass* Z_Construct_UClass_AUEforUnityDevsGameModeBase_NoRegister()
	{
		return AUEforUnityDevsGameModeBase::StaticClass();
	}
	struct Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics
	{
		static UObject* (*const DependentSingletons[])();
#if WITH_METADATA
		static const UECodeGen_Private::FMetaDataPairParam Class_MetaDataParams[];
#endif
		static const FCppClassTypeInfoStatic StaticCppClassTypeInfo;
		static const UECodeGen_Private::FClassParams ClassParams;
	};
	UObject* (*const Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::DependentSingletons[])() = {
		(UObject* (*)())Z_Construct_UClass_AGameModeBase,
		(UObject* (*)())Z_Construct_UPackage__Script_UEforUnityDevs,
	};
#if WITH_METADATA
	const UECodeGen_Private::FMetaDataPairParam Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::Class_MetaDataParams[] = {
		{ "Comment", "/**\n * \n */" },
		{ "HideCategories", "Info Rendering MovementReplication Replication Actor Input Movement Collision Rendering HLOD WorldPartition DataLayers Transformation" },
		{ "IncludePath", "UEforUnityDevsGameModeBase.h" },
		{ "ModuleRelativePath", "UEforUnityDevsGameModeBase.h" },
		{ "ShowCategories", "Input|MouseInput Input|TouchInput" },
	};
#endif
	const FCppClassTypeInfoStatic Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::StaticCppClassTypeInfo = {
		TCppClassTypeTraits<AUEforUnityDevsGameModeBase>::IsAbstract,
	};
	const UECodeGen_Private::FClassParams Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::ClassParams = {
		&AUEforUnityDevsGameModeBase::StaticClass,
		"Game",
		&StaticCppClassTypeInfo,
		DependentSingletons,
		nullptr,
		nullptr,
		nullptr,
		UE_ARRAY_COUNT(DependentSingletons),
		0,
		0,
		0,
		0x009002ACu,
		METADATA_PARAMS(Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::Class_MetaDataParams, UE_ARRAY_COUNT(Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::Class_MetaDataParams))
	};
	UClass* Z_Construct_UClass_AUEforUnityDevsGameModeBase()
	{
		if (!Z_Registration_Info_UClass_AUEforUnityDevsGameModeBase.OuterSingleton)
		{
			UECodeGen_Private::ConstructUClass(Z_Registration_Info_UClass_AUEforUnityDevsGameModeBase.OuterSingleton, Z_Construct_UClass_AUEforUnityDevsGameModeBase_Statics::ClassParams);
		}
		return Z_Registration_Info_UClass_AUEforUnityDevsGameModeBase.OuterSingleton;
	}
	template<> UEFORUNITYDEVS_API UClass* StaticClass<AUEforUnityDevsGameModeBase>()
	{
		return AUEforUnityDevsGameModeBase::StaticClass();
	}
	DEFINE_VTABLE_PTR_HELPER_CTOR(AUEforUnityDevsGameModeBase);
	struct Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_UEforUnityDevsGameModeBase_h_Statics
	{
		static const FClassRegisterCompiledInInfo ClassInfo[];
	};
	const FClassRegisterCompiledInInfo Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_UEforUnityDevsGameModeBase_h_Statics::ClassInfo[] = {
		{ Z_Construct_UClass_AUEforUnityDevsGameModeBase, AUEforUnityDevsGameModeBase::StaticClass, TEXT("AUEforUnityDevsGameModeBase"), &Z_Registration_Info_UClass_AUEforUnityDevsGameModeBase, CONSTRUCT_RELOAD_VERSION_INFO(FClassReloadVersionInfo, sizeof(AUEforUnityDevsGameModeBase), 3421639679U) },
	};
	static FRegisterCompiledInInfo Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_UEforUnityDevsGameModeBase_h_892808349(TEXT("/Script/UEforUnityDevs"),
		Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_UEforUnityDevsGameModeBase_h_Statics::ClassInfo, UE_ARRAY_COUNT(Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_UEforUnityDevsGameModeBase_h_Statics::ClassInfo),
		nullptr, 0,
		nullptr, 0);
PRAGMA_ENABLE_DEPRECATION_WARNINGS
