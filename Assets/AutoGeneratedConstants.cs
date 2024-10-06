/* ------------------------ */
/* ---- AUTO GENERATED ---- */
/* ---- AVOID TOUCHING ---- */
/* ------------------------ */

using UnityEngine;
using System.Collections.Generic;
using Framework;

public enum LayerName
{
	Default = 0,
	TransparentFX = 1,
	IgnoreRaycast = -1,
	Ground = 3,
	Water = 4,
	UI = 5
}

public enum SortingLayerName
{
	Default = 0,
	Ui = -739450033
}

public static class Layer
{

	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = -1;
	public const int Ground = 3;
	public const int Water = 4;
	public const int UI = 5;

}

public static class SortingLayer
{

	public const int Default = 0;
	public const int Ui = -739450033;

}

public static class Tag
{

	public const string Untagged = "Untagged";
	public const string Respawn = "Respawn";
	public const string Finish = "Finish";
	public const string EditorOnly = "EditorOnly";
	public const string MainCamera = "MainCamera";
	public const string Player = "Player";
	public const string GameController = "GameController";

}

public static partial class LayerMasks
{

	public static readonly LayerMask ALL_LAYERS = ~0;
	public static readonly LayerMask NO_LAYERS = 0;
	public static readonly LayerMask Default = 1;
	public static readonly LayerMask TransparentFX = 2;
	public static readonly LayerMask IgnoreRaycast = -2147483648;
	public static readonly LayerMask Ground = 8;
	public static readonly LayerMask Water = 16;
	public static readonly LayerMask UI = 32;

}

public static class CollisionMatrix
{

	public static readonly LayerMask ALL_LAYERS = ~0;
	public static readonly LayerMask NO_LAYERS = 0;
	public static readonly LayerMask DefaultCollisionMask = -1;
	public static readonly LayerMask TransparentFXCollisionMask = -1;
	public static readonly LayerMask IgnoreRaycastCollisionMask = -1;
	public static readonly LayerMask GroundCollisionMask = -1;
	public static readonly LayerMask WaterCollisionMask = -1;
	public static readonly LayerMask UICollisionMask = -1;

}

public static class SceneNames
{

	

}

[CreateAssetMenu(fileName = "Float Parameter", menuName = "Scriptable Enum/Float Parameter")]
public partial class FloatParameter
{

	public static FloatParameter[] AllFloatParameters { get { if (__allFloatParameters == null) __allFloatParameters = GetValues<FloatParameter>(); return __allFloatParameters; } }
	public static FloatParameter ArcherRange { get { if (__archerRange == null) __archerRange = GetValue<FloatParameter>("ArcherRange"); return __archerRange; } }
	public static FloatParameter ArcherReloadSpeed { get { if (__archerReloadSpeed == null) __archerReloadSpeed = GetValue<FloatParameter>("ArcherReloadSpeed"); return __archerReloadSpeed; } }
	public static FloatParameter ArmoryWompsAllowed { get { if (__armoryWompsAllowed == null) __armoryWompsAllowed = GetValue<FloatParameter>("ArmoryWompsAllowed"); return __armoryWompsAllowed; } }
	public static FloatParameter ArtilleryRange { get { if (__artilleryRange == null) __artilleryRange = GetValue<FloatParameter>("ArtilleryRange"); return __artilleryRange; } }
	public static FloatParameter ArtilleryReloadSpeed { get { if (__artilleryReloadSpeed == null) __artilleryReloadSpeed = GetValue<FloatParameter>("ArtilleryReloadSpeed"); return __artilleryReloadSpeed; } }
	public static FloatParameter BunksWompsAllowed { get { if (__bunksWompsAllowed == null) __bunksWompsAllowed = GetValue<FloatParameter>("BunksWompsAllowed"); return __bunksWompsAllowed; } }
	public static FloatParameter CanSwimInSea { get { if (__canSwimInSea == null) __canSwimInSea = GetValue<FloatParameter>("CanSwimInSea"); return __canSwimInSea; } }
	public static FloatParameter CanWavesBringSand { get { if (__canWavesBringSand == null) __canWavesBringSand = GetValue<FloatParameter>("CanWavesBringSand"); return __canWavesBringSand; } }
	public static FloatParameter CostArcher { get { if (__costArcher == null) __costArcher = GetValue<FloatParameter>("CostArcher"); return __costArcher; } }
	public static FloatParameter CostArmory { get { if (__costArmory == null) __costArmory = GetValue<FloatParameter>("CostArmory"); return __costArmory; } }
	public static FloatParameter CostArtillery { get { if (__costArtillery == null) __costArtillery = GetValue<FloatParameter>("CostArtillery"); return __costArtillery; } }
	public static FloatParameter CostBunks { get { if (__costBunks == null) __costBunks = GetValue<FloatParameter>("CostBunks"); return __costBunks; } }
	public static FloatParameter CostDigs { get { if (__costDigs == null) __costDigs = GetValue<FloatParameter>("CostDigs"); return __costDigs; } }
	public static FloatParameter CostFactory { get { if (__costFactory == null) __costFactory = GetValue<FloatParameter>("CostFactory"); return __costFactory; } }
	public static FloatParameter CostLab { get { if (__costLab == null) __costLab = GetValue<FloatParameter>("CostLab"); return __costLab; } }
	public static FloatParameter CostLadder { get { if (__costLadder == null) __costLadder = GetValue<FloatParameter>("CostLadder"); return __costLadder; } }
	public static FloatParameter CostRange { get { if (__costRange == null) __costRange = GetValue<FloatParameter>("CostRange"); return __costRange; } }
	public static FloatParameter CostSandcastle { get { if (__costSandcastle == null) __costSandcastle = GetValue<FloatParameter>("CostSandcastle"); return __costSandcastle; } }
	public static FloatParameter DigsWompsAllowed { get { if (__digsWompsAllowed == null) __digsWompsAllowed = GetValue<FloatParameter>("DigsWompsAllowed"); return __digsWompsAllowed; } }
	public static FloatParameter FactorySeaPlastic { get { if (__factorySeaPlastic == null) __factorySeaPlastic = GetValue<FloatParameter>("FactorySeaPlastic"); return __factorySeaPlastic; } }
	public static FloatParameter FactoryWompsAllowed { get { if (__factoryWompsAllowed == null) __factoryWompsAllowed = GetValue<FloatParameter>("FactoryWompsAllowed"); return __factoryWompsAllowed; } }
	public static FloatParameter HubWompsAllowed { get { if (__hubWompsAllowed == null) __hubWompsAllowed = GetValue<FloatParameter>("HubWompsAllowed"); return __hubWompsAllowed; } }
	public static FloatParameter LabSpeedPerWomp { get { if (__labSpeedPerWomp == null) __labSpeedPerWomp = GetValue<FloatParameter>("LabSpeedPerWomp"); return __labSpeedPerWomp; } }
	public static FloatParameter LabTimeToMake { get { if (__labTimeToMake == null) __labTimeToMake = GetValue<FloatParameter>("LabTimeToMake"); return __labTimeToMake; } }
	public static FloatParameter LabWompsAllowed { get { if (__labWompsAllowed == null) __labWompsAllowed = GetValue<FloatParameter>("LabWompsAllowed"); return __labWompsAllowed; } }
	public static FloatParameter MinerDamage { get { if (__minerDamage == null) __minerDamage = GetValue<FloatParameter>("MinerDamage"); return __minerDamage; } }
	public static FloatParameter MinerMoveMultiplier { get { if (__minerMoveMultiplier == null) __minerMoveMultiplier = GetValue<FloatParameter>("MinerMoveMultiplier"); return __minerMoveMultiplier; } }
	public static FloatParameter MiningSpeed { get { if (__miningSpeed == null) __miningSpeed = GetValue<FloatParameter>("MiningSpeed"); return __miningSpeed; } }
	public static FloatParameter PlasticMaxValue { get { if (__plasticMaxValue == null) __plasticMaxValue = GetValue<FloatParameter>("PlasticMaxValue"); return __plasticMaxValue; } }
	public static FloatParameter RangeWompsAllowed { get { if (__rangeWompsAllowed == null) __rangeWompsAllowed = GetValue<FloatParameter>("RangeWompsAllowed"); return __rangeWompsAllowed; } }
	public static FloatParameter RunnerMoveMultiplier { get { if (__runnerMoveMultiplier == null) __runnerMoveMultiplier = GetValue<FloatParameter>("RunnerMoveMultiplier"); return __runnerMoveMultiplier; } }
	public static FloatParameter WavePlasticSpawnChance { get { if (__wavePlasticSpawnChance == null) __wavePlasticSpawnChance = GetValue<FloatParameter>("WavePlasticSpawnChance"); return __wavePlasticSpawnChance; } }
	public static FloatParameter WaveSize { get { if (__waveSize == null) __waveSize = GetValue<FloatParameter>("WaveSize"); return __waveSize; } }
	public static FloatParameter WaveSpawnRate { get { if (__waveSpawnRate == null) __waveSpawnRate = GetValue<FloatParameter>("WaveSpawnRate"); return __waveSpawnRate; } }
	public static FloatParameter WaveSpeed { get { if (__waveSpeed == null) __waveSpeed = GetValue<FloatParameter>("WaveSpeed"); return __waveSpeed; } }
	
	protected static FloatParameter[] __allFloatParameters;
	protected static FloatParameter __archerRange;
	protected static FloatParameter __archerReloadSpeed;
	protected static FloatParameter __armoryWompsAllowed;
	protected static FloatParameter __artilleryRange;
	protected static FloatParameter __artilleryReloadSpeed;
	protected static FloatParameter __bunksWompsAllowed;
	protected static FloatParameter __canSwimInSea;
	protected static FloatParameter __canWavesBringSand;
	protected static FloatParameter __costArcher;
	protected static FloatParameter __costArmory;
	protected static FloatParameter __costArtillery;
	protected static FloatParameter __costBunks;
	protected static FloatParameter __costDigs;
	protected static FloatParameter __costFactory;
	protected static FloatParameter __costLab;
	protected static FloatParameter __costLadder;
	protected static FloatParameter __costRange;
	protected static FloatParameter __costSandcastle;
	protected static FloatParameter __digsWompsAllowed;
	protected static FloatParameter __factorySeaPlastic;
	protected static FloatParameter __factoryWompsAllowed;
	protected static FloatParameter __hubWompsAllowed;
	protected static FloatParameter __labSpeedPerWomp;
	protected static FloatParameter __labTimeToMake;
	protected static FloatParameter __labWompsAllowed;
	protected static FloatParameter __minerDamage;
	protected static FloatParameter __minerMoveMultiplier;
	protected static FloatParameter __miningSpeed;
	protected static FloatParameter __plasticMaxValue;
	protected static FloatParameter __rangeWompsAllowed;
	protected static FloatParameter __runnerMoveMultiplier;
	protected static FloatParameter __wavePlasticSpawnChance;
	protected static FloatParameter __waveSize;
	protected static FloatParameter __waveSpawnRate;
	protected static FloatParameter __waveSpeed;

}