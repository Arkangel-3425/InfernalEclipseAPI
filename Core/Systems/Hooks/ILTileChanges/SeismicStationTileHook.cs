using System.Reflection;
using MonoMod.RuntimeDetour;
using SOTS;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    public class SeismicStationTileHook : ModSystem
    {
        private Hook _tileLoaderHook;

        private int _cachedSeismicTileType = -1;

        private delegate bool Orig_TileLoader_CanKillTile(int i, int j, int type, ref bool blockDamaged);
        private delegate bool Detour_TileLoader_CanKillTile_Delegate(Orig_TileLoader_CanKillTile orig, int i, int j, int type, ref bool blockDamaged);

        public override void Load()
        {
            MethodInfo tileLoaderCanKill = typeof(TileLoader).GetMethod("CanKillTile", BindingFlags.Public | BindingFlags.Static);

            if (tileLoaderCanKill != null)
            {
                _tileLoaderHook = new Hook(tileLoaderCanKill, new Detour_TileLoader_CanKillTile_Delegate(Detour_TileLoader_CanKillTile));
                _tileLoaderHook.Apply();
            }
        }

        public override void PostSetupContent()
        {
            if (ModContent.TryFind<ModTile>("SOTS", "SeismicStationTile", out ModTile seismicTile))
            {
                _cachedSeismicTileType = seismicTile.Type;
            }
        }

        private bool Detour_TileLoader_CanKillTile(Orig_TileLoader_CanKillTile orig, int i, int j, int type, ref bool blockDamaged)
        {
            if (_cachedSeismicTileType != -1 && type == _cachedSeismicTileType)
            {
                if (SOTSWorld.downedExcavator)
                {
                    return true;
                }
                return false;
            }

            return orig(i, j, type, ref blockDamaged);
        }

        public override void Unload()
        {
            _tileLoaderHook?.Undo();
            _tileLoaderHook = null;
        }
    }
}