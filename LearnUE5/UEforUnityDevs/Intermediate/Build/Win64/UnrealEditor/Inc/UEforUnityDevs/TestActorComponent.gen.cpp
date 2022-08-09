// Copyright Epic Games, Inc. All Rights Reserved.
/*===========================================================================
	Generated code exported from UnrealHeaderTool.
	DO NOT modify this manually! Edit the corresponding .h files instead!
===========================================================================*/

#include "UObject/GeneratedCppIncludes.h"
#include "UEforUnityDevs/TestActorComponent.h"
PRAGMA_DISABLE_DEPRECATION_WARNINGS
void EmptyLinkFunctionForGeneratedCodeTestActorComponent() {}
// Cross Module References
	UEFORUNITYDEVS_API UClass* Z_Construct_UClass_UTestActorComponent_NoRegister();
	UEFORUNITYDEVS_API UClass* Z_Construct_UClass_UTestActorComponent();
	ENGINE_API UClass* Z_Construct_UClass_UActorComponent();
	UPackage* Z_Construct_UPackage__Script_UEforUnityDevs();
// End Cross Module References
	void UTestActorComponent::StaticRegisterNativesUTestActorComponent()
	{
	}
	IMPLEMENT_CLASS_NO_AUTO_REGISTRATION(UTestActorComponent);
	UClass* Z_Construct_UClass_UTestActorComponent_NoRegister()
	{
		return UTestActorComponent::StaticClass();
	}
	struct Z_Construct_UClass_UTestActorComponent_Statics
	{
		static UObject* (*const DependentSingletons[])();
#if WITH_METADATA
		static const UECodeGen_Private::FMetaDataPairParam Class_MetaDataParams[];
#endif
		static const FCppClassTypeInfoStatic StaticCppClassTypeInfo;
		static const UECodeGen_Private::FClassParams ClassParams;
	};
	UObject* (*const Z_Construct_UClass_UTestActorComponent_Statics::DependentSingletons[])() = {
		(UObject* (*)())Z_Construct_UClass_UActorComponent,
		(UObject* (*)())Z_Construct_UPackage__Script_UEforUnityDevs,
	};
#if WITH_METADATA
	const UECodeGen_Private::FMetaDataPairParam Z_Construct_UClass_UTestActorComponent_Statics::Class_MetaDataParams[] = {
		{ "BlueprintSpawnableComponent", "" },
		{ "ClassGroupNames", "Custom" },
		{ "IncludePath", "TestActorComponent.h" },
		{ "ModuleRelativePath", "TestActorComponent.h" },
	};
#endif
	const FCppClassTypeInfoStatic Z_Construct_UClass_UTestActorComponent_Statics::StaticCppClassTypeInfo = {
		TCppClassTypeTraits<UTestActorComponent>::IsAbstract,
	};
	const UECodeGen_Private::FClassParams Z_Construct_UClass_UTestActorComponent_Statics::ClassParams = {
		&UTestActorComponent::StaticClass,
		"Engine",
		&StaticCppClassTypeInfo,
		DependentSingletons,
		nullptr,
		nullptr,
		nullptr,
		UE_ARRAY_COUNT(DependentSingletons),
		0,
		0,
		0,
		0x00B000A4u,
		METADATA_PARAMS(Z_Construct_UClass_UTestActorComponent_Statics::Class_MetaDataParams, UE_ARRAY_COUNT(Z_Construct_UClass_UTestActorComponent_Statics::Class_MetaDataParams))
	};
	UClass* Z_Construct_UClass_UTestActorComponent()
	{
		if (!Z_Registration_Info_UClass_UTestActorComponent.OuterSingleton)
		{
			UECodeGen_Private::ConstructUClass(Z_Registration_Info_UClass_UTestActorComponent.OuterSingleton, Z_Construct_UClass_UTestActorComponent_Statics::ClassParams);
		}
		return Z_Registration_Info_UClass_UTestActorComponent.OuterSingleton;
	}
	template<> UEFORUNITYDEVS_API UClass* StaticClass<UTestActorComponent>()
	{
		return UTestActorComponent::StaticClass();
	}
	DEFINE_VTABLE_PTR_HELPER_CTOR(UTestActorComponent);
	struct Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_TestActorComponent_h_Statics
	{
		static const FClassRegisterCompiledInInfo ClassInfo[];
	};
	const FClassRegisterCompiledInInfo Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_TestActorComponent_h_Statics::ClassInfo[] = {
		{ Z_Construct_UClass_UTestActorComponent, UTestActorComponent::StaticClass, TEXT("UTestActorComponent"), &Z_Registration_Info_UClass_UTestActorComponent, CONSTRUCT_RELOAD_VERSION_INFO(FClassReloadVersionInfo, sizeof(UTestActorComponent), 3724767993U) },
	};
	static FRegisterCompiledInInfo Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_TestActorComponent_h_3408247006(TEXT("/Script/UEforUnityDevs"),
		Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_TestActorComponent_h_Statics::ClassInfo, UE_ARRAY_COUNT(Z_CompiledInDeferFile_FID_UEforUnityDevs_Source_UEforUnityDevs_TestActorComponent_h_Statics::ClassInfo),
		nullptr, 0,
		nullptr, 0);
PRAGMA_ENABLE_DEPRECATION_WARNINGS
