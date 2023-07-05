using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using UnityModManagerNet;
using UnityEngine;


namespace ConstantStorms.Patches
{
    public static class WanderingStormPatch
    {

        [HarmonyPatch(typeof(WanderingStorm), "UpdateActiveInRegion")]
        public static class UpdateActiveInRegionPatch
        {
            [HarmonyPostfix]
            public static void Postfix(WanderingStorm __instance)
            {
                if (!GameState.playing) return;
                __instance.active = true;
            }
        }

        [HarmonyPatch(typeof(WanderingStorm), "GetRadius")]
        public static class GetRadiusPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref float __result)
            {
                if (!GameState.playing) return;
                __result = 10000000f;
            }
        }

        [HarmonyPatch(typeof(WanderingStorm), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(WanderingStorm __instance)
            {
                if (!GameState.playing) return;

                __instance.gameObject.transform.position = Refs.observerMirror.transform.position;
                __instance.gameObject.transform.position =
                    new Vector3(__instance.gameObject.transform.position.x, 500, __instance.gameObject.transform.position.z);

                var setEmission = typeof(WanderingStorm).GetMethod("SetEmission", BindingFlags.NonPublic | BindingFlags.Instance);

                setEmission.Invoke(__instance, new object[] { true });
            }
        }      
    }
}
