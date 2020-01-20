
using JsonModSettings;
using ModSettings;

namespace nonStroyBearspear
{
    internal class nonStroyBearspearSettings : JsonModSettingsBase<nonStroyBearspearSettings>
    {



        [Section("Spear Struggle:")]
        [Name("Tap Strength")]
        [Description("How much the base tap strength settings is scaled for a spear struggle.")]
        [Slider(0, 5)]
        public float TapstrengthScale = 1f;

        [Name("Damage Received")]
        [Description("How much the base received damage settings is scaled for a spear struggle.")]
        [Slider(0, 5)]
        public float DamageReceivedScale = 1f;

        [Name("Damage Received Interval")]
        [Description("How much the base damage interval settings is scaled for a spear struggle.")]
        [Slider(0, 5)]
        public float DamageReceivedIntervalScale = 1f;

        [Name("Clothing Damage")]
        [Description("How much the base clothing damage settings is scaled for a spear struggle.")]
        [Slider(0, 5)]
        public float ClothingDamageScale = 1f;

        [Section("struggle Damage:")]
        [Name("Damage success")]
        [Description("How much damage the bear receives is scaled for a successful spear struggle. A successful spear struggle happens when the player mangages to fill the tapbar during struggle.")]
        [Slider(0, 3f, 31)]
        public float DamageSuccess = 1f;


        [Name("Bleed time success")]
        [Description("How much the bleeding out time of the bear is scaled for a successful spear struggle. A successful spear struggle happens when the player mangages to fill the tapbar during struggle. Set to 0 if you want to disable bleeding.")]
        [Slider(0, 3f, 31)]
        public float BleedSuccess = 1f;

        
        [Name("Damage failed")]
        [Description("How much damage the bear recieves is scaled for a failed spear struggle. A failed spear struggle happens when the player does not mangage to fill the tapbar during struggle.")]
        [Slider(0, 3f, 31)]
        public float Damagefailed = 0.5f;


        [Name("Bleed time failed")]
        [Description("How much the bleeding out time of the bear is scaled for a failed spear struggle. A failed spear struggle happens when the player does not mangage to fill the tapbar during struggle. Set to 0 if you want to disable bleeding.")]
        [Slider(0, 3f, 31)]
        public float Bleedfailed = 2f;


        [Section("Spear:")]
        [Name("Degrade")]
        [Description("How much condition the spear loses per struggle.")]
        [Slider(0, 100f, 101)]
        public float Degrade = 25f;

        [Name("Weight")]
        [Description("How heavy the spear is in kg.")]
        [Slider(1f, 5f, 41)]
        public float Weight = 3f;


        [Section("Crafting:")]
        [Name("Cost Saplings")]
        [Description("How many Saplings are needed to craft a spear.")]
        [Slider(1, 5)]
        public int CostSaplings = 2;

        [Name("Cost Scrapmetal")]
        [Description("How much Scrapmetal is needed to craft a spear head.")]
        [Slider(1, 5)]
        public int CostHead = 3;

        [Name("Duration Spear")]
        [Description("How long the crafting duration of the spear is in minutes. Because the crafting duration gets multiplied by the tool speed the actual time will be less. Half the value if huntingkife is used for crafting")]
        [Slider(0, 300)]
        public int DurationSpear = 150;

        [Name("Duration Spearhead")]
        [Description("How long the crafting duration of the spearhead is in minutes.")]
        [Slider(0, 300)]
        public int DurationSpearhead = 90;



        [Section("Harvesting:")]
        [Name("Spear Harvest Saplings")]
        [Description("How many Saplings can be harvested by a spear.")]
        [Slider(0, 5)]
        public int SpearHarvestSaplings = 1;

        [Name("BrokenSpear Harvest Saplings")]
        [Description("How many Saplings can be harvested by a broken spear.")]
        [Slider(0, 5)]
        public int BrokenSpearHarvestSaplings = 1;

        [Name("BrokenSpear Harvest Scrapmetal")]
        [Description("How much Scrapmetal can be harvested by a broken spear.")]
        [Slider(0, 5)]
        public int BrokenSpearHarvestScrapmetal = 2;





        public static void OnLoad()
        {
            Instance = JsonModSettingsLoader.Load<nonStroyBearspearSettings>();
        }
    }
}
