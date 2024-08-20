namespace magic_world;
class mw_buildings
{
    public static void init()
    {
        for (int i = 1; i < 11; i++)
        {
            newMagicTower($"magic_tower_human_{i}", S.human, $"buildings/human/{i}");
        }
        for (int i = 1; i < 11; i++)
        {
            newMagicTower($"magic_tower_dwarf_{i}", S.orc, $"buildings/dwarf/{i}");
        }
        for (int i = 1; i < 11; i++)
        {
            newMagicTower($"magic_tower_orc_{i}", S.orc, $"buildings/orc/{i}");
        }
        for (int i = 1; i < 11; i++)
        {
            newMagicTower($"magic_tower_elf_{i}", S.orc, $"buildings/elf/{i}");
        }
        RaceBuildOrderAsset kingdom_base = AssetManager.race_build_orders.get("kingdom_base");
        kingdom_base.addBuilding("magic_tower_human_1", 1, pPop: 30, pBuildings: 10);
        Race race = AssetManager.raceLibrary.get(S.human);
        race.building_order_keys.Add("magic_tower_human_1", "magic_tower_human_1");




    }
    public static void newMagicTower(string id, string raceID, string sprite_path)
    {
        BuildingAsset magic_tower = AssetManager.buildings.clone(id, SB.watch_tower_human);
        magic_tower.id = id;
        magic_tower.type = "magic_tower";
        magic_tower.draw_light_area = true;
        magic_tower.draw_light_size = 0.5f;
        magic_tower.sprite_path = sprite_path;
        magic_tower.base_stats[S.health] = 3000f;
        magic_tower.base_stats[S.targets] = 1f;
        magic_tower.base_stats[S.area_of_effect] = 1f;
        magic_tower.base_stats[S.damage] = 30f;
        magic_tower.base_stats[S.knockback] = 1.4f;
        magic_tower.material = "building";
        magic_tower.setShadow(1f, 2f, 0.12f);
        magic_tower.fundament = new BuildingFundament(1, 1, 1, 0);
        magic_tower.cost = new ConstructionCost(0, 20, 1, 5);
        magic_tower.race = raceID;
        magic_tower.canBeDamagedByTornado = false;
        magic_tower.canBePlacedOnBlocks = false;
        magic_tower.canBePlacedOnLiquid = false;
        magic_tower.destroyOnLiquid = true;
        magic_tower.auto_remove_ruin = false;
        magic_tower.affectedByAcid = true;
        magic_tower.canBeAbandoned = true;
        magic_tower.sparkle_effect = false;
        magic_tower.tower = true;
        magic_tower.tower_projectile = "photosphere";
        magic_tower.tower_projectile_offset = 4f;
        magic_tower.tower_projectile_amount = 6;
        magic_tower.burnable = false;
        magic_tower.build_place_borders = true;
        magic_tower.max_zone_range = 0;
        AssetManager.buildings.add(magic_tower);
        AssetManager.buildings.loadSprites(magic_tower);

    }
}