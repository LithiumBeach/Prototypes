// Fill out your copyright notice in the Description page of Project Settings.

#pragma once
#include "GameFramework/Actor.h"
#include "TestActorComponent.h"

UCLASS()
class TestActorComponent : public AActor
{
	GENERATED_BODY()
		int Count;

	// Sets default values for this component's properties
	ATestActorComponent()
	{
		// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
		// off to improve performance if you don't need them.
		PrimaryComponentTick.bCanEverTick = true;

		// ...
	}
    
    // Called when the game starts or when spawned.
    void BeginPlay()
    {
        Super::BeginPlay();
        Count = 0;
    }

    // Called every frame.
    void Tick(float DeltaSeconds)
    {
        Super::Tick(DeltaSeconds);
        Count = Count + 1;
        GLog->Log(FString::FromInt(Count));
    }
};

