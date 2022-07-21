using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GraphicByBiome;

[StaticConstructorOnStartup]
public static class StaticConstructor
{
	static StaticConstructor()
		=> new Harmony("GraphicByBiomeExtension").Patch(AccessTools.Method(typeof(Thing), nameof(Thing.Print)),
			  prefix: new(((Delegate)Thing_Print_Prefix).Method));

	public static void Thing_Print_Prefix(Thing __instance)
	{
		if (__instance.def?.GetModExtension<Extension>() is { } extension)
			ApplyExtension(__instance, extension);
	}

	public static void ApplyExtension(Thing thing, Extension extension)
	{
		if (extension.biomes.Find(t => t.def == thing.MapHeld.Biome) is { } tuple)
			thing.graphicInt = tuple.graphicData.GraphicColoredFor(thing);
	}
}

public class Extension : DefModExtension
{
	public List<BiomeGraphicTuple> biomes;
}

public class BiomeGraphicTuple
{
	public GraphicData graphicData;

	public BiomeDef def;
}