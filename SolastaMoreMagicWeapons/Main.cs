using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using SolastaModApi;
using ModKit;
using ModKit.Utility;
using SolastaModApi.Extensions;
using System.Collections.Generic;

namespace SolastaMoreMagicWeapons
{
    public static class Main
    {
        public static readonly string MOD_FOLDER = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static Guid ModGuidNamespace = new Guid("16757d1b-518f-4669-af43-1ddf5d23c223");

        [Conditional("DEBUG")]
        internal static void Log(string msg) => Logger.Log(msg);
        internal static void Error(Exception ex) => Logger?.Error(ex.ToString());
        internal static void Error(string msg) => Logger?.Error(msg);
        internal static void Warning(string msg) => Logger?.Warning(msg);
        internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }
        internal static ModManager<Core, Settings> Mod { get; private set; }
        internal static Settings Settings { get { return Mod.Settings; } }

        internal static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;

                Mod = new ModManager<Core, Settings>();
                modEntry.OnToggle = OnToggle;

                Translations.Load(MOD_FOLDER);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool enabled)
        {
            if (enabled)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Mod.Enable(modEntry, assembly);
            }
            else
            {
                Mod.Disable(modEntry, false);
                ReflectionCache.Clear();
            }
            return true;
        }

        private struct MagicItemDataHolder
        {
            public string Name;
            public ItemDefinition Item;
            public RecipeDefinition Recipe;

            public MagicItemDataHolder(string name, ItemDefinition item, RecipeDefinition recipe)
            {
                this.Name = name;
                this.Item = item;
                this.Recipe = recipe;
            }
        }
        internal static void OnGameReady()
        {
            List<ItemDefinition> ThrowingWeapons = new List<ItemDefinition>() {
                DatabaseHelper.ItemDefinitions.Handaxe,
            };

            List<ItemDefinition> ThrowingOnlyWeapons = new List<ItemDefinition>() {
                DatabaseHelper.ItemDefinitions.Javelin,
                DatabaseHelper.ItemDefinitions.Dart,
            };

            List<MagicItemDataHolder> ThrowingToCopy = new List<MagicItemDataHolder>()
            {
                // Same as +1
                new MagicItemDataHolder("Acuteness", DatabaseHelper.ItemDefinitions.Enchanted_Dagger_of_Acuteness,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_DaggerOfAcuteness),
                // Same as +2
                new MagicItemDataHolder("Sharpness", DatabaseHelper.ItemDefinitions.Enchanted_Dagger_of_Sharpness,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_DaggerOfSharpness),
                new MagicItemDataHolder("Souldrinker", DatabaseHelper.ItemDefinitions.Enchanted_Dagger_Souldrinker,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_DaggerSouldrinker),
                new MagicItemDataHolder("Frostburn", DatabaseHelper.ItemDefinitions.Enchanted_Dagger_Frostburn,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_DaggerFrostburn),
            };

            List<ItemDefinition> PrimedInputs = new List<ItemDefinition>()
            {
                DatabaseHelper.ItemDefinitions.Primed_Dagger,
            };

            AddRecipesForWeapons(ThrowingWeapons, ThrowingToCopy, PrimedInputs);
            AddRecipesForWeapons(ThrowingOnlyWeapons, ThrowingToCopy, PrimedInputs, 3);

            List<ItemDefinition> BashingWeapons = new List<ItemDefinition>()
            {
                DatabaseHelper.ItemDefinitions.Club,
                DatabaseHelper.ItemDefinitions.Maul,
            };

            StockItem(DatabaseHelper.MerchantDefinitions.Store_Merchant_Gorim_Ironsoot_Cyflen_GeneralStore, DatabaseHelper.ItemDefinitions.Maul);
            
            List<MagicItemDataHolder> BashingToCopy = new List<MagicItemDataHolder>()
            {
                // Same as +1
                new MagicItemDataHolder("Acuteness", DatabaseHelper.ItemDefinitions.Enchanted_Mace_Of_Acuteness,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_MaceOfAcuteness),
                new MagicItemDataHolder("Bearclaw", DatabaseHelper.ItemDefinitions.Enchanted_Morningstar_Bearclaw,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_MorningstarBearclaw),
                new MagicItemDataHolder("Power", DatabaseHelper.ItemDefinitions.Enchanted_Morningstar_Of_Power,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_MorningstarOfPower),
                new MagicItemDataHolder("Lightbringer", DatabaseHelper.ItemDefinitions.Enchanted_Greatsword_Lightbringer,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_GreatswordLightbringer),
                new MagicItemDataHolder("Punisher", DatabaseHelper.ItemDefinitions.Enchanted_Battleaxe_Punisher,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_BattleaxePunisher),
            };

            List<ItemDefinition> BashingPrimed = new List<ItemDefinition>()
            {
                DatabaseHelper.ItemDefinitions.Primed_Morningstar,
                DatabaseHelper.ItemDefinitions.Primed_Mace,
                DatabaseHelper.ItemDefinitions.Primed_Greatsword,
                DatabaseHelper.ItemDefinitions.Primed_Battleaxe,
            };

            AddRecipesForWeapons(BashingWeapons, BashingToCopy, BashingPrimed);

            List<ItemDefinition> Quarterstaff = new List<ItemDefinition>()
            {
                DatabaseHelper.ItemDefinitions.Quarterstaff,
            };

            List<MagicItemDataHolder> QuarterstaffToCopy = new List<MagicItemDataHolder>()
            {
                // Same as +1
                new MagicItemDataHolder("Acuteness", DatabaseHelper.ItemDefinitions.Enchanted_Mace_Of_Acuteness,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_MaceOfAcuteness),
                new MagicItemDataHolder("Stormblade", DatabaseHelper.ItemDefinitions.Enchanted_Longsword_Stormblade,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongswordStormblade),
                new MagicItemDataHolder("Frostburn", DatabaseHelper.ItemDefinitions.Enchanted_Longsword_Frostburn,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongswordFrostburn),
                new MagicItemDataHolder("Lightbringer", DatabaseHelper.ItemDefinitions.Enchanted_Greatsword_Lightbringer,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_GreatswordLightbringer),
                new MagicItemDataHolder("Dragonblade", DatabaseHelper.ItemDefinitions.Enchanted_Longsword_Dragonblade,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongswordDragonblade),
                new MagicItemDataHolder("Warden", DatabaseHelper.ItemDefinitions.Enchanted_Longsword_Warden,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongswordWarden),
                new MagicItemDataHolder("Whiteburn", DatabaseHelper.ItemDefinitions.Enchanted_Shortsword_Whiteburn,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_ShortswordWhiteburn),
                new MagicItemDataHolder("Souldrinker", DatabaseHelper.ItemDefinitions.Enchanted_Dagger_Souldrinker,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_DaggerSouldrinker),
            };

            List<ItemDefinition> QuarterstaffPrimed = new List<ItemDefinition>()
            {
                DatabaseHelper.ItemDefinitions.Primed_Mace,
                DatabaseHelper.ItemDefinitions.Primed_Longsword,
                DatabaseHelper.ItemDefinitions.Primed_Greatsword,
                DatabaseHelper.ItemDefinitions.Primed_Shortsword,
                DatabaseHelper.ItemDefinitions.Primed_Dagger,
            };

            AddRecipesForWeapons(Quarterstaff, QuarterstaffToCopy, QuarterstaffPrimed);
        }

        private static void AddRecipesForWeapons(List<ItemDefinition> BaseWeapons, List<MagicItemDataHolder> MagicToCopy, List<ItemDefinition> PossiblePrimedItemsToReplace)
        {
            AddRecipesForWeapons(BaseWeapons, MagicToCopy, PossiblePrimedItemsToReplace, 1);
        }

        private static void AddRecipesForWeapons(List<ItemDefinition> BaseWeapons, List<MagicItemDataHolder> MagicToCopy, List<ItemDefinition> PossiblePrimedItemsToReplace, int producedByRecipe)
        {
            foreach (ItemDefinition baseItem in BaseWeapons)
            {
                foreach (MagicItemDataHolder itemData in MagicToCopy)
                {
                    // Generate new items
                    ItemDefinition newItem = ItemBuilder.BuildNewMagicWeapon(baseItem, itemData.Item, itemData.Name);
                    // Generate recipes for items
                    string recipeName = "RecipeEnchanting" + newItem.Name;
                    RecipeBuilder builder = new RecipeBuilder(recipeName, GuidHelper.Create(Main.ModGuidNamespace, recipeName).ToString());
                    builder.AddIngredient(baseItem);
                    foreach (IngredientOccurenceDescription ingredient in itemData.Recipe.Ingredients)
                    {
                        if (PossiblePrimedItemsToReplace.Contains(ingredient.ItemDefinition))
                        {
                            continue;
                        }
                        builder.AddIngredient(ingredient);
                    }
                    builder.SetCraftedItem(newItem, producedByRecipe);
                    builder.SetCraftingCheckData(itemData.Recipe.CraftingHours, itemData.Recipe.CraftingDC, itemData.Recipe.ToolType);
                    RecipeDefinition newRecipe = builder.AddToDB();
                    // Stock item Recipes
                    ItemDefinition craftintgManual = ItemBuilder.BuilderCopyFromItemSetRecipe(newRecipe, DatabaseHelper.ItemDefinitions.CraftingManual_Enchant_Longbow_Of_Accuracy,
                    "CraftingManual_" + newRecipe.Name, DatabaseHelper.ItemDefinitions.CraftingManualRemedy.GuiPresentation, 200);
                    StockItem(DatabaseHelper.MerchantDefinitions.Store_Merchant_Circe, craftintgManual);
                    StockItem(DatabaseHelper.MerchantDefinitions.Store_Merchant_Gorim_Ironsoot_Cyflen_GeneralStore, craftintgManual);
                }

            }
        }

        private static void StockItem(MerchantDefinition merchant, ItemDefinition item)
        {
            StockUnitDescription stockUnit = new StockUnitDescription();
            stockUnit.SetItemDefinition(item);
            stockUnit.SetInitialAmount(1);
            stockUnit.SetInitialized(true);
            stockUnit.SetMaxAmount(2);
            stockUnit.SetMinAmount(1);
            stockUnit.SetStackCount(1);
            stockUnit.SetReassortAmount(1);
            merchant.StockUnitDescriptions.Add(stockUnit);
        }
    }
}
