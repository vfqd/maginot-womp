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
	Default = 0
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
	public static FloatParameter BunksWompsAllowed { get { if (__bunksWompsAllowed == null) __bunksWompsAllowed = GetValue<FloatParameter>("BunksWompsAllowed"); return __bunksWompsAllowed; } }
	public static FloatParameter CanSwimInSea { get { if (__canSwimInSea == null) __canSwimInSea = GetValue<FloatParameter>("CanSwimInSea"); return __canSwimInSea; } }
	public static FloatParameter DigsWompsAllowed { get { if (__digsWompsAllowed == null) __digsWompsAllowed = GetValue<FloatParameter>("DigsWompsAllowed"); return __digsWompsAllowed; } }
	public static FloatParameter HubWompsAllowed { get { if (__hubWompsAllowed == null) __hubWompsAllowed = GetValue<FloatParameter>("HubWompsAllowed"); return __hubWompsAllowed; } }
	public static FloatParameter MinerDamage { get { if (__minerDamage == null) __minerDamage = GetValue<FloatParameter>("MinerDamage"); return __minerDamage; } }
	public static FloatParameter MinerMoveMultiplier { get { if (__minerMoveMultiplier == null) __minerMoveMultiplier = GetValue<FloatParameter>("MinerMoveMultiplier"); return __minerMoveMultiplier; } }
	public static FloatParameter MiningSpeed { get { if (__miningSpeed == null) __miningSpeed = GetValue<FloatParameter>("MiningSpeed"); return __miningSpeed; } }
	public static FloatParameter PlasticMaxValue { get { if (__plasticMaxValue == null) __plasticMaxValue = GetValue<FloatParameter>("PlasticMaxValue"); return __plasticMaxValue; } }
	public static FloatParameter RunnerMoveMultiplier { get { if (__runnerMoveMultiplier == null) __runnerMoveMultiplier = GetValue<FloatParameter>("RunnerMoveMultiplier"); return __runnerMoveMultiplier; } }
	public static FloatParameter WavePlasticSpawnChance { get { if (__wavePlasticSpawnChance == null) __wavePlasticSpawnChance = GetValue<FloatParameter>("WavePlasticSpawnChance"); return __wavePlasticSpawnChance; } }
	public static FloatParameter WaveSize { get { if (__waveSize == null) __waveSize = GetValue<FloatParameter>("WaveSize"); return __waveSize; } }
	public static FloatParameter WaveSpawnRate { get { if (__waveSpawnRate == null) __waveSpawnRate = GetValue<FloatParameter>("WaveSpawnRate"); return __waveSpawnRate; } }
	public static FloatParameter WaveSpeed { get { if (__waveSpeed == null) __waveSpeed = GetValue<FloatParameter>("WaveSpeed"); return __waveSpeed; } }
	
	protected static FloatParameter[] __allFloatParameters;
	protected static FloatParameter __bunksWompsAllowed;
	protected static FloatParameter __canSwimInSea;
	protected static FloatParameter __digsWompsAllowed;
	protected static FloatParameter __hubWompsAllowed;
	protected static FloatParameter __minerDamage;
	protected static FloatParameter __minerMoveMultiplier;
	protected static FloatParameter __miningSpeed;
	protected static FloatParameter __plasticMaxValue;
	protected static FloatParameter __runnerMoveMultiplier;
	protected static FloatParameter __wavePlasticSpawnChance;
	protected static FloatParameter __waveSize;
	protected static FloatParameter __waveSpawnRate;
	protected static FloatParameter __waveSpeed;

}