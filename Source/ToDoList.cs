using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ToDoList;

public class ToDoList : Mod
{
    public static DirectoryInfo SettingsDir;

    public ToDoList(ModContentPack content) : base(content)
    {
        new Harmony("Garethp.rimworld.ToDoList.main").PatchAll();
    }
}