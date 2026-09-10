using SOTS;

public class ThoriumShopAdjustments : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (ModContent.TryFind<ModNPC>("ThoriumMod", "ConfusedZombie", out ModNPC confusedZombie))
        {
            if (shop.NpcType == confusedZombie.Type)
            {
                // Excavator Summon Station
                if (ModContent.TryFind<ModItem>("SOTS", "SeismicStation", out ModItem excavatorStation))
                {
                    Condition downedExcavator = new Condition("Mods.IEoR.Conditions.DownedExcavator", () => SOTSWorld.downedExcavator);
                    shop.Add(excavatorStation.Type, downedExcavator); //Boss cant be fought outside Arena need to fix -Arkangel
                }

                // Polaris Summon Station
                if (ModContent.TryFind<ModItem>("SOTS", "FrostArtifact", out ModItem polarisStation))
                {
                    Condition downedPolaris = new Condition("Mods.IEoR.Conditions.DownedPolaris", () => SOTSWorld.downedAmalgamation);
                    shop.Add(polarisStation.Type, downedPolaris); //works fine no notes -Arkangel
                }
            }
        }
    }
}