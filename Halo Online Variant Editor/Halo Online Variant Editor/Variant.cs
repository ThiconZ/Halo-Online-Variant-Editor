using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Halo_Online_Variant_Editor
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
    }

    public enum RoundsReset : byte
    {
        [Description("Nothing")]
        Nothing = 0x00,
        [Description("Players Only")]
        PlayersOnly = 0x02,
        [Description("Everything")]
        Everything = 0x06
    }
    public enum TimeLimit : byte
    {
        [Description("No Limit")]
        NoLimit = 0,
        [Description("1 Minute")]
        Min1 = 1,
        [Description("2 Minutes")]
        Min2 = 2,
        [Description("3 Minutes")]
        Min3 = 3,
        [Description("4 Minutes")]
        Min4 = 4,
        [Description("5 Minutes")]
        Min5 = 5,
        [Description("6 Minutes")]
        Min6 = 6,
        [Description("7 Minutes")]
        Min7 = 7,
        [Description("8 Minutes")]
        Min8 = 8,
        [Description("9 Minutes")]
        Min9 = 9,
        [Description("10 Minutes")]
        Min10 = 10,
        [Description("12 Minutes")]
        Min12 = 12,
        [Description("15 Minutes")]
        Min15 = 15,
        [Description("20 Minutes")]
        Min20 = 20,
        [Description("30 Minutes")]
        Min30 = 30,
        [Description("45 Minutes")]
        Min45 = 45,
        [Description("60 Minutes")]
        Min60 = 60
    }
    public enum Spectating : byte
    {
        [Description("Not Allowed")]
        NotAllowed = 0,
        [Description("Allowed")]
        Allowed = 8
    }
    public enum NumberOfRounds : byte
    {
        [Description("1 Round")]
        R1 = 1,
        [Description("2 Rounds")]
        R2 = 2,
        [Description("3 Rounds")]
        R3 = 3,
        [Description("4 Rounds")]
        R4 = 4,
        [Description("5 Rounds")]
        R5 = 5,
        [Description("6 Rounds")]
        R6 = 6,
        [Description("7 Rounds")]
        R7 = 7,
        [Description("8 Rounds")]
        R8 = 8,
        [Description("9 Rounds")]
        R9 = 9,
        [Description("10 Rounds")]
        R10 = 10
    }
    public enum LivesPerRound : byte
    {
        [Description("Unlimited")]
        Unlimited = 0,
        [Description("1 Life")]
        L1 = 1,
        [Description("2 Lives")]
        L2 = 2,
        [Description("3 Lives")]
        L3 = 3,
        [Description("4 Lives")]
        L4 = 4,
        [Description("5 Lives")]
        L5 = 5,
        [Description("6 Lives")]
        L6 = 6,
        [Description("7 Lives")]
        L7 = 7,
        [Description("8 Lives")]
        L8 = 8,
        [Description("9 Lives")]
        L9 = 9,
        [Description("10 Lives")]
        L10 = 10
    }
    public enum RespawnTime : byte
    {
        [Description("Instant")]
        S0 = 0,
        [Description("1 Second")]
        S1 = 1,
        [Description("2 Seconds")]
        S2 = 2,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("4 Seconds")]
        S4 = 4,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("6 Seconds")]
        S6 = 6,
        [Description("7 Seconds")]
        S7 = 7,
        [Description("8 Seconds")]
        S8 = 8,
        [Description("9 Seconds")]
        S9 = 9,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("11 Seconds")]
        S11 = 11,
        [Description("12 Seconds")]
        S12 = 12,
        [Description("13 Seconds")]
        S13 = 13,
        [Description("14 Seconds")]
        S14 = 14,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("16 Seconds")]
        S16 = 16,
        [Description("17 Seconds")]
        S17 = 17,
        [Description("18 Seconds")]
        S18 = 18,
        [Description("19 Seconds")]
        S19 = 19,
        [Description("20 Seconds")]
        S20 = 20,
        [Description("21 Seconds")]
        S21 = 21,
        [Description("22 Seconds")]
        S22 = 22,
        [Description("23 Seconds")]
        S23 = 23,
        [Description("24 Seconds")]
        S24 = 24,
        [Description("25 Seconds")]
        S25 = 25,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds")]
        S45 = 45,
        [Description("60 Seconds")]
        S60 = 60,
        [Description("90 Seconds")]
        S90 = 90,
        [Description("2 Minutes")]
        M2 = 120,
        [Description("3 Minutes")]
        M3 = 180
    }
    public enum SuicidePenalty : byte
    {
        [Description("None")]
        None = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("60 Seconds")]
        S60 = 60,
        [Description("2 Minutes")]
        M2 = 120,
        [Description("3 Minutes")]
        M3 = 180
    }
    public enum BetrayalPenalty : byte
    {
        [Description("None")]
        None = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("60 Seconds")]
        S60 = 60,
        [Description("2 Minutes")]
        M2 = 120,
        [Description("3 Minutes")]
        M3 = 180
    }
    public enum DamageResistance : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("10%")]
        P_10,
        [Description("50%")]
        P_50,
        [Description("90%")]
        P_90,
        [Description("100% (Normal)")]
        P_100,
        [Description("110%")]
        P_110,
        [Description("150%")]
        P_150,
        [Description("200%")]
        P_200,
        [Description("300%")]
        P_300,
        [Description("500%")]
        P_500,
        [Description("1000%")]
        P_1000,
        [Description("2000%")]
        P_2000,
        [Description("Invulnerable")]
        Invulnerable,
        [Description("1500%")]
        P_1500,
        [Description("3000%")]
        P_3000,
        [Description("Invincible")]
        Invincible = 15
    }
    public enum ShieldMultiplier : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("No Shields")]
        None,
        [Description("Normal Shields")]
        Normal,
        [Description("2X Overshields")]
        Over2x,
        [Description("3X Overshields")]
        Over3x,
        [Description("4X Overshields")]
        Over4x
    }
    public enum ShieldRechargeRate : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("-25% (Decay)")]
        P_N25,
        [Description("-10% (Decay)")]
        P_N10,
        [Description("-5% (Decay)")]
        P_N5,
        [Description("0% (No Recharge)")]
        P_0,
        [Description("50% (Slower)")]
        P_50,
        [Description("90% (Slower)")]
        P_90,
        [Description("100% (Normal)")]
        P_100,
        [Description("110% (Faster)")]
        P_110,
        [Description("200% (Faster)")]
        P_200
    }
    public enum ShieldVampirism : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("Disabled")]
        Disabled,
        [Description("10% Leech")]
        P_10,
        [Description("25% Leech")]
        P_25,
        [Description("50% Leech")]
        P_50,
        [Description("100% Leech")]
        P_100
    }
    public enum ToggleBoolean : byte
    {
        Unchanged = 0,
        Enabled = 1,
        Disabled = 2
    }
    public enum Boolean : int
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum DamageModifier : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("0%")]
        P_0,
        [Description("25%")]
        P_25,
        [Description("50%")]
        P_50,
        [Description("75%")]
        P_75,
        [Description("90%")]
        P_90,
        [Description("100%")]
        P_100,
        [Description("110%")]
        P_110,
        [Description("125%")]
        P_125,
        [Description("150%")]
        P_150,
        [Description("200%")]
        P_200,
        [Description("300%")]
        P_300,
        [Description("Instant Kill")]
        InstantKill
    }
    public enum InfiniteAmmo : byte
    {
        Unchanged = 0,
        Disabled = 1,
        Enabled = 2,
        [Description("Bottomless Clip")]
        Bottomless = 3
    }
    public enum PlayerSpeed : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("25%")]
        P_25,
        [Description("50%")]
        P_50,
        [Description("75%")]
        P_75,
        [Description("90%")]
        P_90,
        [Description("100%")]
        P_100,
        [Description("110%")]
        P_110,
        [Description("125%")]
        P_125,
        [Description("150%")]
        P_150,
        [Description("200%")]
        P_200,
        [Description("300%")]
        P_300
    }
    public enum PlayerGravity : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("50%")]
        P_50,
        [Description("75%")]
        P_75,
        [Description("100%")]
        P_100,
        [Description("150%")]
        P_150,
        [Description("200%")]
        P_200,
        [Description("0%")]
        P_0,
        [Description("15%")]
        P_015,
        [Description("25%")]
        P_025,
        [Description("35%")]
        P_035,
        [Description("125%")]
        P_125
    }
    public enum VehicleUse : byte
    {
        Unchanged = 0,
        None,
        Passenger,
        Full
    }
    public enum MotionTrackerMode : short
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("Off")]
        Off,
        [Description("Allies Only")]
        Allies,
        [Description("Normal Mode")]
        Normal,
        [Description("Enhanced Mode")]
        Enhanced
    }
    public enum MotionTrackerRange : short
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("10 Meters")]
        m10 = 1,
        [Description("15 Meters [UNSUPPORTED]")]
        m15 = 2,
        [Description("25 Meters")]
        m25 = 3,
        [Description("50 Meters [UNSUPPORTED]")]
        m50 = 4,
        [Description("75 Meters")]
        m75 = 5,
        [Description("100 Meters [UNSUPPORTED]")]
        m100 = 6,
        [Description("150 Meters")]
        m150 = 7
    }
    public enum ActiveCamo : byte
    {
        Unchanged = 0,
        Off = 1,
        Bad = 2,
        Poor = 3,
        Good = 4
    }
    public enum Waypoint : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("No Waypoint")]
        No,
        [Description("Visible To Allies")]
        Allies,
        [Description("Visible To Everyone")]
        Everyone,
        [Description("No NameTag")]
        NoNameTag,
        [Description("No NameTag (Team Only)")]
        NoNameTagTeamOnly
    }
    public enum ForcedColor : byte
    {
        Unchanged = 0,
        Off,
        Red,
        Blue,
        Green,
        Orange,
        Purple,
        Gold,
        Brown,
        Pink,
        White,
        Black,
        Zombie
    }
    public enum PlayerSize : byte
    {
        Unchanged = 0,
        [Description("10%")]
        P_10 = 2,
        [Description("15%")]
        P_15 = 3,
        [Description("25%")]
        P_25 = 4,
        [Description("35%")]
        P_35 = 5,
        [Description("50%")]
        P_50 = 6,
        [Description("75%")]
        P_75 = 7,
        [Description("100%")]
        P_100 = 8,
        [Description("125%")]
        P_125 = 9,
        [Description("150%")]
        P_150 = 10,
        [Description("200%")]
        P_200 = 11,
        [Description("350%")]
        P_350 = 12,
        [Description("500%")]
        P_500 = 13,
        [Description("750%")]
        P_750 = 14,
        [Description("1000%")]
        P_1000 = 15,
        [Description("1500%")]
        P_1500 = 16,
        [Description("2000%")]
        P_2000 = 17,
        [Description("3000%")]
        P_3000 = 18,
    }
    public enum VisualEffect : byte
    {
        Unchanged = 0,
        None = 1,
        [Description("Team Glow")]
        TeamGlow = 2,
        [Description("Black Glow")]
        BlackGlow = 3,
        [Description("White Glow")]
        WhiteGlow = 4
    }
    public enum SynchronizeWithTeam
    {
        Disabled = 0x10,
        Enabled = 0x11
    }
    public enum RespawnOnKills
    {
        Disabled = 0x00,
        Enabled = 0x08
    }
    public enum RespawnTimeGrowth
    {
        [Description("Disabled")]
        Disabled = 0,
        [Description("1 Second [UNSUPPORTED]")]
        Seconds1 = 1,
        [Description("2 Seconds")]
        Seconds2 = 2,
        [Description("3 Seconds")]
        Seconds3 = 3,
        [Description("5 Seconds")]
        Seconds5 = 5,
        [Description("10 Seconds")]
        Seconds10 = 10,
        [Description("15 Seconds")]
        Seconds15 = 15,
        [Description("20 Seconds [UNSUPPORTED]")]
        Seconds20 = 20,
        [Description("30 Seconds [UNSUPPORTED]")]
        Seconds30 = 30
    }
    public enum GrenadesOnMap
    {
        [Description("None")]
        None = 0,
        [Description("Map Default")]
        MapDefault = 1
    }
    public enum IndestructibleVehicles
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum Duration
    {
        [Description("None")]
        None = 0,
        [Description("1 Second")]
        S1 = 1,
        [Description("2 Seconds")]
        S2 = 2,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("4 Seconds")]
        S4 = 4,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("60 Seconds")]
        S60 = 60
    }
    public enum FriendlyFire : short
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum BetrayalBooting : short
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum TeamChanging : short
    {
        [Description("Not Allowed")]
        NotAllowed = 0,
        [Description("Allowed")]
        Allowed = 1,
        [Description("Balancing Only")]
        Balanced = 2
    }
    public enum WeaponsOnMap : short
    {
        [Description("Map Default")]
        MapDefault = 0,
        //[Description("Map Default (-1 Fix)")]
        //MapDefaultN1Fix = -1, // If the variant is replicated and WeaponsOnMap or VehicleSet is not changed, it will be -1. However, -1 functions exactly like Map Default. If you change the setting to something else, save it, then switch it back to Map Default and save it, it will be 0 from now on
        [Description("Assault Rifles")]
        AssaultRifles = 1,
        [Description("Duals")]
        Duals = 2,
        [Description("Gravity Hammers")]
        GravityHammers = 3,
        [Description("Spartan Lasers")]
        SpartanLasers = 4,
        [Description("Rocket Launchers")]
        RocketLaunchers = 5,
        [Description("Shotguns")]
        Shotguns = 6,
        [Description("Sniper Rifles")]
        SniperRifles = 7,
        [Description("Energy Swords")]
        EnergySwords = 8,
        [Description("Random")]
        Random = -2,
        [Description("No Power Weapons")]
        NoPowerWeapons = 9,
        [Description("No Snipers")]
        NoSnipers = 10,
        [Description("No Weapons")]
        NoWeapons = 11,
        [Description("No Upgrades")]
        NoUpgrades = 13
    }
    public enum VehicleSet : short
    {
        [Description("Map Default")]
        MapDefault = 0,
        //[Description("Map Default (-1 Fix)")]
        //MapDefaultN1Fix = -1, // If the variant is replicated and WeaponsOnMap or VehicleSet is not changed, it will be -1. However, -1 functions exactly like Map Default. If you change the setting to something else, save it, then switch it back to Map Default and save it, it will be 0 from now on
        [Description("All Vehicles")]
        AllVehicles = 9,
        [Description("No Aircraft")]
        NoAircraft = 8,
        [Description("No Tanks")]
        NoTanks = 7,
        [Description("No Light Ground")]
        NoLightGround = 6,
        [Description("Aircraft Only")]
        AircraftOnly = 5,
        [Description("Tanks Only")]
        TanksOnly = 4,
        [Description("Light Ground Only")]
        LightGroundOnly = 3,
        [Description("Mongooses Only")]
        MongoosesOnly = 2,
        [Description("No Vehicles")]
        NoVehicles = 1
    }
    public enum GameType : int
    {
        UNK = 0,
        CTF = 1,
        Slayer = 2,
        Oddball = 3,
        KOTH = 4,
        Forge = 5,
        VIP = 6,
        Juggernaut = 7,
        Territories = 8,
        Assault = 9,
        Infection = 10
    }
    public enum GameTypeOrder : int
    {
        UNK = 0,
        CTF = 1,
        Slayer = 2,
        Oddball = 3,
        KOTH = 4,
        Forge = 10,
        VIP = 9,
        Juggernaut = 5,
        Territories = 6,
        Assault = 7,
        Infection = 8
    }
    public enum GameTypeExtension : int
    {
        unk = 0,
        ctf = 1,
        slayer = 2,
        oddball = 3,
        koth = 4,
        forge = 5,
        vip = 6,
        jugg = 7,
        terries = 8,
        assault = 9,
        zombiez = 10
    }
    public enum GameCode : int
    {
        CTF = 23437400,
        Slayer = 23437160,
        Oddball = 23437208,
        KOTH = 23437256,
        Forge = -1,
        VIP = 23437496,
        Juggernaut = 23437544,
        Territories = 23437352,
        Assault = 23437448,
        Infection = 23437304
    }
    public enum GrenadeCount : byte
    {
        [Description("Unchanged")]
        Unchanged = 0,
        [Description("Map Default")]
        MapDefault = 1,
        [Description("None")]
        None = 2
    }
    public enum Weapon : byte
    {
        [Description("Assault Rifle")]
        AssaultRifle = 0x01,
        [Description("Battle Rifle")]
        BattleRifle = 0x00,
        [Description("Brute Shot")]
        BruteShot = 0x0d,
        [Description("Gravity Hammer")]
        GravityHammer = 0x12,
        [Description("Magnum")]
        Magnum = 0x07,
        [Description("Needler")]
        Needler = 0x08,
        [Description("Plasma Pistol")]
        PlasmaPistol = 0x02,
        [Description("Plasma Rifle")]
        PlasmaRifle = 0x09,
        [Description("Rocket Launcher")]
        RocketLauncher = 0x0a,
        [Description("Shotgun")]
        Shotgun = 0x0b,
        [Description("SMG")]
        SMG = 0x04,
        [Description("Sniper Rifle")]
        SniperRifle = 0x0c,
        [Description("Spartan Laser")]
        SpartanLaser = 0x10,
        [Description("Energy Sword")]
        EnergySword = 0x06,
        [Description("Spiker")]
        Spiker = 0x03,
        [Description("Carbine")]
        Carbine = 0x05,
        [Description("Beam Rifle")]
        BeamRifle = 0x0f,
        [Description("Mauler")]
        Mauler = 0x13,
        [Description("Flamethrower")]
        Flamethrower = 0x14,
        [Description("Missile Pod")]
        MissilePod = 0x15,
        [Description("Fuel Rod Gun")]
        FuelRodGun = 0x17,
        [Description("Sentinel Beam")]
        SentinelBeam = 0x16,
        [Description("DMR")]
        DMR = 0x18,
        [Description("Assault Rifle (ACC)")]
        AssaultRifle_ACC = 0x1d,
        [Description("Assault Rifle (DMG)")]
        AssaultRifle_DMG = 0x1a,
        [Description("Assault Rifle (ROF)")]
        AssaultRifle_ROF = 0x1b,
        [Description("Assault Rifle (PWR)")]
        AssaultRifle_PWR = 0x1e,
        [Description("Battle Rifle (ACC)")]
        BattleRifle_ACC = 0x20,
        [Description("Battle Rifle (DMG)")]
        BattleRifle_DMG = 0x22,
        [Description("Battle Rifle (MAG)")]
        BattleRifle_MAG = 0x21,
        [Description("Battle Rifle (RNG)")]
        BattleRifle_RNG = 0x23,
        [Description("Battle Rifle (ROF)")]
        BattleRifle_ROF = 0x1f,
        [Description("Battle Rifle (PWR)")]
        BattleRifle_PWR = 0x24,
        [Description("DMR (ACC)")]
        DMR_ACC = 0x26,
        [Description("DMR (DMG)")]
        DMR_DMG = 0x28,
        [Description("DMG (MAG)")]
        DMR_MAG = 0x29,
        [Description("DMR (RNG)")]
        DMR_RNG = 0x25,
        [Description("DMR (ROF)")]
        DMR_ROF = 0x27,
        [Description("DMR (PWR)")]
        DMR_PWR = 0x2a,
        [Description("SMG (ACC)")]
        SMG_ACC = 0x2c,
        [Description("SMG (DMG)")]
        SMG_DMG = 0x2e,
        [Description("SMG (ROF)")]
        SMG_ROF = 0x2b,
        [Description("SMG (PWR)")]
        SMG_PWR = 0x30,
        [Description("Plasma Rifle (PWR)")]
        PlasmaRifle_PWR = 0x36,
        [Description("Carbine (ACC)")]
        Carbine_ACC = 0x3b,
        [Description("Carbine (DMG)")]
        Carbine_DMG = 0x3a,
        [Description("Carbine (MAG)")]
        Carbine_MAG = 0x38,
        [Description("Carbine (RNG)")]
        Carbine_RNG = 0x39,
        [Description("Carbine (ROF)")]
        Carbine_ROF = 0x37,
        [Description("Carbine (PWR)")]
        Carbine_PWR = 0x3c,
        [Description("Mauler (PWR)")]
        Mauler_PWR = 0x3f,
        [Description("Magnum (DMG)")]
        Magnum_DMG = 0x41,
        [Description("Magnum (PWR)")]
        Magnum_PWR = 0x42,
        [Description("Plasma Pistol (PWR)")]
        PlasmaPistol_PWR = 0x43,
        [Description("NONE")]
        None = 0x11,
        [Description("NONE (Melee)")]
        NoneMelee = 0x0e,
        [Description("Machinegun Turret")]
        MachinegunTurret = 0x44,
        [Description("Plasma Cannon")]
        PlasmaCannon = 0x45,
        [Description("Unchanged")]
        Unchanged = 0xfe,
        [Description("Map Default")]
        MapDefault = 0xff,
        [Description("Random")]
        Random = 0xfd
    }
    public enum BombArmingTime :short
    {
        [Description("Instant")]
        Instant = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds [UNSUPPORTED]")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds [UNSUPPORTED]")]
        S45 = 45,
        [Description("60 Seconds [UNSUPPORTED]")]
        S60 = 60,
        [Description("2 Minutes [UNSUPPORTED]")]
        M2 = 120
    }
    public enum BombDisarmingTime : short
    {
        [Description("Instant [UNSUPPORTED]")]
        Instant = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds [UNSUPPORTED]")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds [UNSUPPORTED]")]
        S45 = 45,
        [Description("60 Seconds [UNSUPPORTED]")]
        S60 = 60,
        [Description("2 Minutes [UNSUPPORTED]")]
        M2 = 120
    }
    public enum BombResetTime : short
    {
        [Description("Instant [UNSUPPORTED]")]
        Instant = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds [UNSUPPORTED]")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds [UNSUPPORTED]")]
        S45 = 45,
        [Description("60 Seconds")]
        S60 = 60,
        [Description("2 Minutes [UNSUPPORTED]")]
        M2 = 120
    }
    public enum SuddenDeath : short
    {
        [Description("No Limits")]
        NoLimit = -1,
        [Description("Disabled")]
        Disabled = 0,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds")]
        S45 = 45,
        [Description("1 Minute")]
        M1 = 60,
        [Description("2 Minutes")]
        M2 = 120
    }
    public enum AssaultBombMode : short
    {
        [Description("Multi Bomb")]
        MultiBomb = 0,
        [Description("One Sided")]
        OneSided = 1,
        [Description("Neutral Bomb")]
        NeutralBomb = 2
    }
    public enum DetonationsToWin : short
    {
        [Description("Unlimited")]
        Unlimited = 0,
        [Description("1")]
        D1 = 1,
        [Description("3")]
        D3 = 3,
        [Description("5")]
        D5 = 5,
        [Description("10")]
        D10 = 10,
        [Description("15")]
        D15 = 15,
        [Description("20")]
        D20 = 20,
        [Description("25")]
        D25 = 25,
        [Description("50")]
        D50 = 50
    }
    public enum TeamScoring : short
    {
        [Description("Sum Of Team")]
        SumOfTeam = 0,
        [Description("Minimum Score")]
        MinimumScore = 1,
        [Description("Maximum Score")]
        MaximumScore = 2
    }
    public enum TeamScoringKOTH : short
    {
        [Description("Sum Of Team")]
        SumOfTeam = 0,
        [Description("Minimum Score")]
        MinimumScore = 1,
        [Description("Maximum Score")]
        MaximumScore = 2,
        [Description("Team Control")]
        TeamControl = 3
    }
    public enum ScoreToWin : short
    {
        [Description("Unlimited")]
        Unlimited = -1,
        [Description("1")]
        S1 = 1,
        [Description("3")]
        S3 = 3,
        [Description("5")]
        S5 = 5,
        [Description("10")]
        S10 = 10,
        [Description("15")]
        S15 = 15,
        [Description("20")]
        S20 = 20,
        [Description("25")]
        S25 = 25,
        [Description("50")]
        S50 = 50,
        [Description("75")]
        S75 = 75,
        [Description("100")]
        S100 = 100,
        [Description("150")]
        S150 = 150,
        [Description("200")]
        S200 = 200,
        [Description("250")]
        S250 = 250
    }
    public enum ScoreOptionsPointsShort : short
    {
        [Description("-10")]
        P_N10 = -10,
        [Description("-9")]
        P_N9 = -9,
        [Description("-8")]
        P_N8 = -8,
        [Description("-7")]
        P_N7 = -7,
        [Description("-6")]
        P_N6 = -6,
        [Description("-5")]
        P_N5 = -5,
        [Description("-4")]
        P_N4 = -4,
        [Description("-3")]
        P_N3 = -3,
        [Description("-2")]
        P_N2 = -2,
        [Description("-1")]
        P_N1 = -1,
        [Description("0")]
        P_0 = 0,
        [Description("1")]
        P_1 = 1,
        [Description("2")]
        P_2 = 2,
        [Description("3")]
        P_3 = 3,
        [Description("4")]
        P_4 = 4,
        [Description("5")]
        P_5 = 5,
        [Description("6")]
        P_6 = 6,
        [Description("7")]
        P_7 = 7,
        [Description("8")]
        P_8 = 8,
        [Description("9")]
        P_9 = 9,
        [Description("10")]
        P_10 = 10
    }
    public enum ScoreOptionsPointsByte : byte
    {
        [Description("-10")]
        P_N10 = 0xF6,
        [Description("-9")]
        P_N9 = 0xF7,
        [Description("-8")]
        P_N8 = 0xF8,
        [Description("-7")]
        P_N7 = 0xF9,
        [Description("-6")]
        P_N6 = 0xFA,
        [Description("-5")]
        P_N5 = 0xFB,
        [Description("-4")]
        P_N4 = 0xFC,
        [Description("-3")]
        P_N3 = 0xFD,
        [Description("-2")]
        P_N2 = 0xFE,
        [Description("-1")]
        P_N1 = 0xFF,
        [Description("0")]
        P_0 = 0,
        [Description("1")]
        P_1 = 1,
        [Description("2")]
        P_2 = 2,
        [Description("3")]
        P_3 = 3,
        [Description("4")]
        P_4 = 4,
        [Description("5")]
        P_5 = 5,
        [Description("6")]
        P_6 = 6,
        [Description("7")]
        P_7 = 7,
        [Description("8")]
        P_8 = 8,
        [Description("9")]
        P_9 = 9,
        [Description("10")]
        P_10 = 10
    }
    public enum TeamMode : byte
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum NumberOfFlags : byte
    {
        [Description("Multiple Flags")]
        MultipleFlags = 0,
        [Description("One Flag")]
        OneFlag = 1,
        [Description("Neutral")]
        Neutral = 2 // This is not a standard UI setting - don't know if this breaks stuff
    }
    public enum FlagReturnTime : short
    {
        [Description("Disabled")]
        Disabled = -1,
        [Description("Instant")]
        Instant = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("30 Seconds")]
        S30 = 30        
    }
    public enum FlagResetTime : short
    {
        [Description("Disabled")]
        Disabled = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds [UNSUPPORTED]")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds [UNSUPPORTED]")]
        S45 = 45,
        [Description("60 Seconds")]
        S60 = 60,
        [Description("2 Minutes")]
        M2 = 120
    }
    public enum CapturesToWin : short
    {
        [Description("Unlimited")]
        Unlimited = 0,
        [Description("1")]
        D1 = 1,
        [Description("3")]
        D3 = 3,
        [Description("5")]
        D5 = 5,
        [Description("10")]
        D10 = 10,
        [Description("15")]
        D15 = 15,
        [Description("20")]
        D20 = 20,
        [Description("25")]
        D25 = 25,
        [Description("50")]
        D50 = 50
    }
    public enum FlagWaypoint : byte
    {
        UNK = 0,
        Always,
        UNK2,
        NotInSingleFlag
    }
    public enum RespawnOnCaptureByte : byte
    {
        Disabled = 0,
        OnAllyCapture,
        OnEnemyCapture,
        OnAnyCapture
    }
    public enum HillMovement : short
    {
        [Description("No Movement")]
        NoMovement = 0,
        [Description("10 Seconds")]
        S10 = 1,
        [Description("15 Seconds")]
        S15 = 2,
        [Description("30 Seconds")]
        S30 = 3,
        [Description("1 Minute")]
        M1 = 4,
        [Description("2 Minutes")]
        M2 = 5,
        [Description("3 Minutes")]
        M3 = 6,
        [Description("4 Minutes")]
        M4 = 7,
        [Description("5 Minutes")]
        M5 = 8
    }
    public enum HillMovementOrder : short
    {
        Random = 0,
        Sequence = 256
    }
    public enum BallCount : byte
    {
        [Description("1")]
        C1 = 1,
        [Description("2")]
        C2 = 2,
        [Description("3")]
        C3 = 3
    }
    public enum InitialBallDelay : short
    {
        [Description("None")]
        None = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds [UNSUPPORTED]")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds [UNSUPPORTED]")]
        S45 = 45,
        [Description("60 Seconds [UNSUPPORTED]")]
        S60 = 60
    }
    public enum BallRespawnDelay : short
    {
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds")]
        S20 = 20,
        [Description("25 Seconds")]
        S25 = 25,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds")]
        S45 = 45,
        [Description("60 Seconds")]
        S60 = 60,
        [Description("90 Seconds")]
        S90 = 90,
        [Description("2 Minutes")]
        M2 = 120
    }
    public enum FirstJuggernaut : byte
    {
        [Description("Random")]
        Random = 0,
        [Description("First Kill")]
        FirstKill = 1,
        [Description("First Death")]
        FirstDeath = 2
    }
    public enum NextJuggernaut : byte
    {
        [Description("Killer Of Juggernaut")]
        KillerOfJuggernaut = 0,
        [Description("Killed By Juggernaut")]
        KilledByJuggernaut = 1,
        [Description("Unchanged")]
        Unchanged = 2,
        [Description("Random")]
        Random = 3
    }
    public enum AlliedAgainstJuggernaut : byte
    {
        [Description("Not Allied")]
        NotAllied = 0,
        [Description("Allied")]
        Allied = 1
    }
    public enum GoalZones : byte
    {
        Disabled = 0,
        Enabled = 4
    }
    public enum RespawnOnLoneJuggernaut : byte
    {
        Disabled = 0,
        Enabled = 2
    }
    public enum GoalZoneMovement : byte
    {
        [Description("No Movement")]
        NoMovement = 0,
        [Description("10 Seconds")]
        S10 = 1,
        [Description("15 Seconds")]
        S15 = 2,
        [Description("30 Seconds")]
        S30 = 3,
        [Description("1 Minute")]
        M1 = 4,
        [Description("2 Minutes")]
        M2 = 5,
        [Description("3 Minutes")]
        M3 = 6,
        [Description("4 Minutes")]
        M4 = 7,
        [Description("5 Minutes")]
        M5 = 8,
        [Description("Move On Arrival")]
        MoveOnArrival = 9,
        [Description("Move On New Juggernaut")]
        MoveOnNewJuggernaut = 10
    }
    public enum GoalZoneOrder : byte
    {
        Random = 0,
        Sequence = 1
    }
    public enum NextJuggernautDelay : byte
    {
        [Description("No Delay")]
        NoDelay = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("4 Seconds")]
        S4 = 4,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,

    }
    public enum OneSidedTerritories : short
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum LockAfterCapture : short
    {
        Disabled = 0,
        Enabled = 2
    }
    public enum RespawnOnCapture : short
    {
        [Description("Disabled")]
        Disabled = 0,
        [Description("Allied Captures")]
        AlliedCaptures = 1,
        [Description("Enemy Captures")]
        EnemyCaptures = 2,
        [Description("All Captures")]
        AllCaptures = 3
    }
    public enum TerritoryCaptureTime : short
    {
        [Description("Instant")]
        Instant = 0,
        [Description("3 Seconds")]
        S3 = 3,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds")]
        S45 = 45,
        [Description("1 Minute")]
        M1 = 1,
        [Description("2 Minutes")]
        M2 = 2
    }
    public enum NextZombie : byte
    {
        Winner = 0,
        Chump = 1,
        Unchanged = 2,
        Random = 3
    }
    public enum InitialZombieCount : byte
    {
        [Description("25%")]
        P_25 = 0,
        [Description("50%")]
        P_50 = 1,
        [Description("1")]
        C1 = 2,
        [Description("2")]
        C2 = 3,
        [Description("3")]
        C3 = 4,
        [Description("4")]
        C4 = 5,
        [Description("5")]
        C5 = 6,
        [Description("6")]
        C6 = 7,
        [Description("7")]
        C7 = 8,
        [Description("8")]
        C8 = 9,
        [Description("9")]
        C9 = 10,
        [Description("10")]
        C10 = 11,
        [Description("11")]
        C11 = 12,
        [Description("12")]
        C12 = 13,
        [Description("13")]
        C13 = 14,
        [Description("14")]
        C14 = 15,
        [Description("15")]
        C15 = 16
    }
    public enum SafeHavenMovement : byte
    {
        [Description("Havens Disabled")]
        HavensDisabled = 0,
        [Description("Random Order")]
        RandomOrder = 1,
        [Description("Sequential Order")]
        SequentialOrder = 2
    }
    public enum SafeHavenMovementTime : byte
    {
        [Description("Disabled")]
        Disabled = 0,
        [Description("5 Seconds")]
        S5 = 5,
        [Description("10 Seconds")]
        S10 = 10,
        [Description("15 Seconds")]
        S15 = 15,
        [Description("20 Seconds")]
        S20 = 20,
        [Description("30 Seconds")]
        S30 = 30,
        [Description("45 Seconds")]
        S45 = 45,
        [Description("1 Minute")]
        M1 = 60,
        [Description("2 Minutes")]
        M2 = 120
    }
    public enum SingleVIP : short
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum VIPDeathEndsRound : short
    {
        Disabled = 0,
        Enabled = 4
    }
    public enum GoalAreas : short
    {
        Disabled = 0,
        Enabled = 2
    }
    public enum NextVIP : byte
    {
        [Description("Random")]
        Random = 0,
        [Description("Next Death")]
        NextDeath = 2,
        [Description("Unchanging")]
        Unchanging = 3
    }
    public enum GoalMovement : byte
    {
        [Description("No Movement")]
        NoMovement = 0,
        [Description("10 Seconds")]
        S10 = 1,
        [Description("15 Seconds")]
        S15 = 2,
        [Description("30 Seconds")]
        S30 = 3,
        [Description("1 Minute")]
        M1 = 4,
        [Description("2 Minutes")]
        M2 = 5,
        [Description("3 Minutes")]
        M3 = 6,
        [Description("4 Minutes")]
        M4 = 7,
        [Description("5 Minutes")]
        M5 = 8,
        [Description("Move On Arrival")]
        MoveOnArrival = 9,
        [Description("Move On VIP Change")]
        MoveOnVIPChange = 10
    }
    public enum InfluenceRadius : short
    {
        [Description("Disabled")]
        Disabled = 0,
        [Description("10 Meters")]
        M10 = 3,
        [Description("15 Meters")]
        M15 = 5,
        [Description("25 Meters")]
        M25 = 8,
        [Description("30 Meters")]
        M30 = 10,
        [Description("40 Meters")]
        M40 = 13,
        [Description("50 Meters")]
        M50 = 16
    }
    /// <summary>
    /// These traits modify shield strength and resistance to damage.
    /// </summary>
    public struct ShieldsAndHealth
    {
        public DamageResistance DamageResistance; // Unchanged(0x00) 10%(0x01) 50%(0x02) 90%(0x03) 100%(0x04) 110%(0x05) 150%(0x06) 200%(0x07) 300%(0x08) 500%(0x09) 1000%(0x0a) 2000%(0x0b) Invulnerable(0x0c)
        public ShieldMultiplier ShieldMultiplier; // Unchanged(0x00) None(0x01) Normal(0x02) 2xOver(0x03) 3xOver(0x04) 4xOver(0x05)
        public ShieldRechargeRate ShieldRechargeRate; // Unchanged(0x00) -25%(0x01) -10%(0x02) -5%(0x03) 0%(0x04) 50%(0x05) 90%(0x06) 100%(0x07) 110%(0x08) 200%(0x09)
        public ShieldVampirism ShieldVampirism; // Unchanged(0x00) Disabled(0x01) 10%(0x02) 25%(0x03) 50%(0x04) 100%(0x05)
        public ToggleBoolean ImmuneToHeadshots; // Unchanged(0x00) Enabled(0x01) Disabled(0x02)
    }
    /// <summary>
    /// These traits modify a player's weapons and how much damage they inflict.
    /// </summary>
    public struct WeaponsAndDamage
    {
        public GrenadeCount GrenadeCount;
        public Weapon PrimaryWeapon;
        public Weapon SecondaryWeapon;
        public DamageModifier DamageModifier; // Unchanged(0x00) 0%(0x01) 25%(0x02) 50%(0x03) 75%(0x04) 90%(0x05) 100%(0x06) 110%(0x7) 125%(0x08) 150%(0x09) 200%(0x0a) 300%(0x0b) Instant Kill(0x0c)
        public ToggleBoolean GrenadeRegen; // Unchanged(0x00) Enabled(0x01) Disabled(0x02)
        public InfiniteAmmo InfiniteAmmo; // Unchanged(0x00) Disabled(0x01) Enabled(0x02) Bottomless (0x03)
        public ToggleBoolean WeaponPickup; // Unchanged(0x00) Enabled(0x01) Disabled(0x02)
    }
    /// <summary>
    /// These traits modify how a player moves and uses vehicles.
    /// </summary>
    public struct Movement
    {
        public PlayerSpeed PlayerSpeed; // Unchanged(0x00) 25%(0x01) 50%(0x02) 75%(0x03) 90%(0x04) 100%(0x05) 110%(0x06) 125%(0x07) 150%(0x08) 200%(0x09) 300%(0x0a)
        public PlayerGravity PlayerGravity; // Unchanged(0x00) 50%(0x01) 75%(0x02) 100%(0x03) 150%(0x04) 200%(0x05)
        public VehicleUse VehicleUse; // Unchanged(0x00) None(0x01) Pass(0x02) Full(0x03)
    }
    /// <summary>
    /// These traits affect a player's sensors.
    /// </summary>
    public struct Sensors
    {
        public MotionTrackerMode MotionTrackerMode; // Unchanged(0x00) Off(0x01) Allies(0x02) Normal(0x03) Enhanced(0x04)
        public MotionTrackerRange MotionTrackerRange; // Unchanged(0x00) 10m(0x01) 25m(0x03) 75m(0x05) 150(0x07)
    }
    /// <summary>
    /// These traits affect the player's appearance.
    /// </summary>
    public struct Appearance
    {
        public ActiveCamo ActiveCamo; // Unchanged(0x00) Off(0x01) Poor(0x03) Good(0x04)
        public Waypoint Waypoint; // Unchanged(0x00) No(0x01) Allies(0x02) Everyone(0x03)
        public ForcedColor ForcedColor; // Unchanged(0x00) Off(0x01) Red(0x02) Blue(0x03) Green(0x04) Orange(0x05) Purple(0x06) Gold(0x07) Brown(0x08) Pink(0x09) White(0x0a) Black(0x0b) Zombie(0x0c)
        public PlayerSize PlayerSize; // Unchanged(0x00), None(0x01), TeamGlow(0x02), BlackGlow(0x03), WhiteGlow(0x04) -- This is a setting from the Halo 3 Beta
    }
    /// <summary>
    /// These settings further modify respawning.
    /// </summary>
    public struct RespawnModifiers
    {
        public SynchronizeWithTeam SynchronizeWithTeam; // Disabled(0x10) Enabled(0x11) : [SWT + ROK(Result)] Disabled + Disabled(0x18), Enabled + Enabled(0x19)
        public RespawnTimeGrowth RespawnTimeGrowth; // Dying increases respawn time, killing decreases it (in seconds) Disabled(0x00), 2Seconds(0x02), 3Seconds(0x03), 5Seconds(0x05), 10Seconds(0x0A), 15Seconds(0x0F)
        public RespawnOnKills RespawnOnKills; // Shared variable value with SyncWithTeam (SyncWithTeam Bytes + RespawnOnKill Bytes = Result Value) Disabled(0x00) Enabled(0x08)
    }
    /// <summary>
    /// These traits are applied to a player for a short time after respawning. This will override all other traits.
    /// </summary>
    public struct RespawnTraits
    {
        public Duration Duration;
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// This determines which abilities are given by Overshield/Red powerups and for how long.
    /// </summary>
    public struct OvershieldPowerupTraits
    {
        public Duration Duration;
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// This determines which abilities are given by Active Camo/Blue powerups and for how long.
    /// </summary>
    public struct ActiveCamoPowerupTraits
    {
        public Duration Duration;
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// This determines which abilities are given by Custom powerups and for how long.
    /// </summary>
    public struct CustomPowerupTraits
    {
        public Duration Duration;
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits are applied to a player for a short time after respawning. This will override all other traits.
    /// </summary>
    public struct BasePlayerTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players who are carrying a bomb.
    /// </summary>
    public struct BombCarrierTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to a player who is carrying an enemy flag.
    /// </summary>
    public struct FlagCarrierTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to a player who is in first place or tied for first.
    /// </summary>
    public struct LeaderTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players who are in the hill.
    /// </summary>
    public struct OnHillTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players carrying the ball.
    /// </summary>
    public struct BallCarrierTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits are secondary, most likely unused, traits found in Assault Specific Settings.
    /// </summary>
    public struct SecondaryTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to the player who is the Juggernaut.
    /// </summary>
    public struct JuggernautTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players near one of their own territories.
    /// </summary>
    public struct DefenderTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players capturing an enemy territory.
    /// </summary>
    public struct AttackerTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to Zombies.
    /// </summary>
    public struct ZombieTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players who start the round as Zombies. These override Zombie traits.
    /// </summary>
    public struct AlphaZombieTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to non-Zombies inside of a safe haven.
    /// </summary>
    public struct SafeHavenTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to the last non-Zombie player alive.
    /// </summary>
    public struct LastManTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players on a team with a VIP.
    /// </summary>
    public struct VIPTeamTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to players who are standing near their VIP.
    /// </summary>
    public struct VIPProximityTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These traits apply to the player who is the VIP.
    /// </summary>
    public struct VIPTraits
    {
        public ShieldsAndHealth ShieldsAndHealth;
        public WeaponsAndDamage WeaponsAndDamage;
        public Movement Movement;
        public Sensors Sensors;
        public Appearance Appearance;
    }
    /// <summary>
    /// These are more specialized respawn settings.
    /// </summary>
    public struct AdvancedRespawnSettings
    {
        public RespawnModifiers RespawnModifiers;
        public RespawnTraits RespawnTraits;
    }
    /// <summary>
    /// These settings affect player respawning.
    /// </summary>
    public struct RespawnSettings
    {
        public RespawnTime RespawnTimeServer;
        public RespawnTime RespawnTimeMenu;
        public SuicidePenalty SuicidePenalty;
        public BetrayalPenalty BetrayalPenalty;
        public LivesPerRound LivesPerRound;
        public AdvancedRespawnSettings AdvancedRespawnSettings;
    }
    /// <summary>
    /// These settings control the player's weapons and objects on the map.
    /// </summary>
    public struct WeaponsAndVehicles
    {
        // Primary Weapon/Secondary Weapon are the same variable as General Settings > Base Player Traits > Weapons And Damage > Primary Weapon/Secondary Weapon
        public GrenadesOnMap GrenadesOnMap; // GoM = Map Default or None, IV = Disabled or Enabled - None + Disabled = (0x00), Map Default + Disabled = (0x01), None + Enabled = (0x02), Map Default + Enabled = (0x03)
        public WeaponsOnMap WeaponsOnMap;
        public VehicleSet VehicleSet;
        public IndestructibleVehicles IndestructibleVehicles; // Grenades on Map and Indestructible Vehicles share a variable
        public OvershieldPowerupTraits OvershieldPowerupTraits; // Overshield/Red Powerups
        public ActiveCamoPowerupTraits ActiveCamoPowerupTraits; // Active Camo/Blue Powerups
        public CustomPowerupTraits CustomPowerupTraits; // Custom/Agility/Yellow Powerups
    }
    /// <summary>
    /// These settings are common to all game types.
    /// </summary>
    public struct GeneralSettings
    {
        public NumberOfRounds NumberOfRounds;
        public TimeLimit TimeLimit;
        public RoundsReset RoundsReset;
        public TeamMode TeamMode;
        public Spectating Spectating;
        public BasePlayerTraits BasePlayerTraits;
        public RespawnSettings RespawnSettings;
        public TeamChanging TeamChanging;
        public FriendlyFire FriendlyFire;
        public BetrayalBooting BetrayalBooting;
        // public TeamScoring TeamScoring; // This is technically a general setting but for now will be handled as variant specific
    }
    /// <summary>
    /// Contains all actual Variant Specific Settings structures.
    /// </summary>
    public struct VariantSettings
    {
        public CTFSettings CTFSettings;
        public SlayerSettings SlayerSettings;
        public OddballSettings OddballSettings;
        public KOTHSettings KOTHSettings;
        public VIPSettings VIPSettings;
        public JuggernautSettings JuggernautSettings;
        public TerritoriesSettings TerritoriesSettings;
        public AssaultSettings AssaultSettings;
        public InfectionSettings InfectionSettings;
    }
    /// <summary>
    /// Additional settings specific to CTF games.
    /// </summary>
    public struct CTFSettings
    {
        public CapturesToWin CapturesToWin;
        public NumberOfFlags NumberOfFlags;
        public SuddenDeath SuddenDeath;
        public Boolean FlagAtHomeToScore;
        public FlagReturnTime FlagReturnTime;
        public FlagResetTime FlagResetTime;
        public FlagWaypoint FlagWaypoint;
        public RespawnOnCaptureByte RespawnOnCapture;
        public FlagCarrierTraits FlagCarrierTraits;
    }
    /// <summary>
    /// Additional settings specific to Slayer games.
    /// </summary>
    public struct SlayerSettings
    {
        public TeamScoring TeamScoring;
        public ScoreToWin ScoreToWin;
        public ScoreOptionsPointsShort KillPoints;
        public ScoreOptionsPointsByte AssistPoints;
        public ScoreOptionsPointsByte DeathPoints;
        public ScoreOptionsPointsByte SuicidePoints;
        public ScoreOptionsPointsByte BetrayalPoints;
        public ScoreOptionsPointsByte LeaderKillBonus;
        public ScoreOptionsPointsByte EliminationBonus;
        public ScoreOptionsPointsByte AssassinationBonus;
        public ScoreOptionsPointsByte HeadshotBonus;
        public ScoreOptionsPointsByte BeatdownBonus;
        public ScoreOptionsPointsByte StickyBonus;
        public ScoreOptionsPointsByte SplatterBonus;
        public ScoreOptionsPointsByte SpreeBonus;
        public LeaderTraits LeaderTraits;
    }
    /// <summary>
    /// Additional settings specific to Oddball games.
    /// </summary>
    public struct OddballSettings
    {
        public TeamScoring TeamScoring;
        public Boolean AutoPickup;
        public ScoreToWin ScoreToWin;
        public ScoreOptionsPointsShort CarryingPoints;
        public ScoreOptionsPointsByte KillPoints;
        public ScoreOptionsPointsByte BallKillPoints;
        public ScoreOptionsPointsByte BallCarrierKillPoints;
        public BallCount BallCount;
        public InitialBallDelay InitialBallDelay;
        public BallRespawnDelay BallRespawnDelay;
        public BallCarrierTraits BallCarrierTraits;
    }
    /// <summary>
    /// Additional settings specific to KOTH games.
    /// </summary>
    public struct KOTHSettings
    {
        public TeamScoringKOTH TeamScoring;
        public ScoreToWin ScoreToWin;
        public HillMovement HillMovement;
        public HillMovementOrder HillMovementOrder;
        public ScoreOptionsPointsByte OnHillPoints;
        public ScoreOptionsPointsByte OffHillPoints;
        public ScoreOptionsPointsByte UncontestedControlPoints;
        public ScoreOptionsPointsByte KillPoints;
        public OnHillTraits OnHillTraits;
    }
    /// <summary>
    /// Additional settings specific to VIP games.
    /// </summary>
    public struct VIPSettings
    {
        public ScoreToWin ScoreToWin;
        public SingleVIP SingleVIP;
        public VIPDeathEndsRound VIPDeathEndsRound;
        public GoalAreas GoalAreas;
        public ScoreOptionsPointsByte KillPoints;
        public ScoreOptionsPointsByte VIPTakedownPoints;
        public ScoreOptionsPointsByte KillAsVIPPoints;
        public ScoreOptionsPointsByte VIPDeathPoints;
        public ScoreOptionsPointsByte GoalArrivalPoints;
        public ScoreOptionsPointsByte SuicidePoints;
        public ScoreOptionsPointsByte BetrayalPoints;
        public ScoreOptionsPointsByte VIPBetrayalPoints;
        public NextVIP NextVIP;
        public GoalMovement GoalMovement;
        public GoalZoneOrder GoalMovementOrder;
        public InfluenceRadius InfluenceRadius;
        public VIPTeamTraits VIPTeamTraits;
        public VIPProximityTraits VIPProximityTraits;
        public VIPTraits VIPTraits;
    }
    /// <summary>
    /// Additional settings specific to Juggernaut games.
    /// </summary>
    public struct JuggernautSettings
    {
        public ScoreToWin ScoreToWin;
        public FirstJuggernaut FirstJuggernaut;
        public NextJuggernaut NextJuggernaut;
        public AlliedAgainstJuggernaut AlliedAgainstJuggernaut;
        public GoalZones GoalZones;
        public RespawnOnLoneJuggernaut RespawnOnLoneJuggernaut;
        public GoalZoneMovement GoalZoneMovement;
        public GoalZoneOrder GoalZoneOrder;
        public ScoreOptionsPointsByte KillPoints;
        public ScoreOptionsPointsByte TakedownPoints;
        public ScoreOptionsPointsByte KillAsJuggernautPoints;
        public ScoreOptionsPointsByte GoalArrivalPoints;
        public ScoreOptionsPointsByte SuicidePoints;
        public ScoreOptionsPointsByte BetrayalPoints;
        public NextJuggernautDelay NextJuggernautDelay;
        public JuggernautTraits JuggernautTraits;
    }
    /// <summary>
    /// Additional settings specific to Territories games.
    /// </summary>
    public struct TerritoriesSettings
    {
        public OneSidedTerritories OneSidedTerritories;
        public LockAfterCapture LockAfterCapture;
        public RespawnOnCapture RespawnOnCapture;
        public TerritoryCaptureTime TerritoryCaptureTime;
        public SuddenDeath SuddenDeath;
        public DefenderTraits DefenderTraits;
        public AttackerTraits AttackerTraits;
    }
    /// <summary>
    /// Additional settings specific to Assault games.
    /// </summary>
    public struct AssaultSettings
    {
        public DetonationsToWin DetonationsToWin;
        public AssaultBombMode AssaultBombMode;
        public Boolean ResetOnDisarm;
        public BombArmingTime BombArmingTime;
        public BombDisarmingTime BombDisarmingTime;
        public BombArmingTime BombFuseTime;
        public BombResetTime BombResetTime;
        public SuddenDeath SuddenDeath;
        public BombCarrierTraits BombCarrierTraits;
        public SecondaryTraits SecondaryTraits;
    }
    /// <summary>
    /// Additional settings specific to Infection games.
    /// </summary>
    public struct InfectionSettings
    {
        public Boolean RespawnOnHavenMove;
        public SafeHavenMovement SafeHavenMovement;
        public NextZombie NextZombie;
        public InitialZombieCount InitialZombieCount;
        public SafeHavenMovementTime SafeHavenMovementTime;
        public ScoreOptionsPointsByte ZombieKillPoints;
        public ScoreOptionsPointsByte InfectionPoints;
        public ScoreOptionsPointsByte SafeHavenArrivalPoints;
        public ScoreOptionsPointsByte SuicidePoints;
        public ScoreOptionsPointsByte BetrayalPoints;
        public ScoreOptionsPointsByte LastManStandingBonus;
        public ZombieTraits ZombieTraits;
        public AlphaZombieTraits AlphaZombieTraits;
        public SafeHavenTraits SafeHavenTraits;
        public LastManTraits LastManTraits;
    }
    public class Variant
    {
        public string variantNameMenu; // game variant name displayed within the menus
        public string variantNameGame; // game variant name displayed within the game
        public string descriptionMenu; // game variant description displayed within the menus
        public string descriptionGame; // game variant description displayed within the game
        public string currentAuthorName; // name of player that made the most recent save to file
        public string originalAuthorName; // name of player that made the first version of the variant
        public GameTypeOrder gameTypeMenuOrder; // Not entirely sure where this is used, but it's here
        public GameType gameTypeMenu; // game type value displayed within the menus
        public GameTypeOrder gameTypeGameOrder; // Like before, not sure where this is used, but it exists here as well
        public GameType gameTypeGame; // game type value displayed within the game
        public GameType gameTypeInit; // another game type variable
        public GameCode gameCode; // game type code that is set based on original archetype used for file
        public byte[] currentAuthorXUID; // 8 bytes defining the current variant author's XUID, or NULL if Bungie
        public byte[] originalAuthorXUID; // 8 bytes defining the original variant author's XUID, or NULL if Bungie
        public long fileSize; // current file size of variant
        public long originalFileSize; // file size of original variant
        public long fileCreatedTimestamp; // Seconds since UNIX epoch when the most recent save was made
        public long originalFileCreatedTimestamp; // Seconds since UNIX epoch when the original variant file was created
        public string metadata; // 40 bytes of metadata that are either null or set to the original variant this is based off of
        public short buildVersionMajor; // build version major number
        public short buildVersionMinor; // build version minor number
        public WeaponsAndVehicles weaponsAndVehicles;
        public GeneralSettings generalSettings;
        public VariantSettings variantSettings;
        public string variantType; // MPVR (Multiplayer Variant) || MAPV (Map Variant)

        public bool Load(BinaryReader b)
        {
            int length = (int)b.BaseStream.Length;
            // BLF
            b.ReadChars(4); // _blf
            b.ReadInt32(); // size
            b.ReadInt16(); // unk0
            b.ReadInt16(); // unk1
            b.ReadInt16(); // unk2
            b.ReadChars(34); // unk3

            // CHDR - Content Header
            b.ReadChars(4); // _chdr
            b.ReadInt32(); // size
            b.ReadInt32(); // content version
            buildVersionMajor = b.ReadInt16(); // build version major number
            buildVersionMinor = b.ReadInt16(); // build version minor number
            b.ReadBytes(8); // checksum
            variantNameMenu = new string(b.ReadChars(32)).Replace("\0", ""); // variantName
            descriptionMenu = new string(b.ReadChars(128)).Replace("\0", ""); // variantDescription
            currentAuthorName = new string(b.ReadChars(16)).Replace("\0", ""); // createdPlayerGamertag
            gameTypeMenuOrder = (GameTypeOrder)b.ReadInt32(); // Game type order for menu
            b.ReadInt32(); // is user valid
            currentAuthorXUID = b.ReadBytes(8); // current variant author's XUID
            fileSize = b.ReadInt64(); // fileSize (as reported in the menu of the game; in bytes size) BC 03 00 00 = GameTypeVariant (956), F0 E1 00 00 = MapVariant (57840)

            if (fileSize != 956)
            {
                // Ask user what they want to do if this isn't detected as the expected GameTypeVariant file size
                // There may be no harm in loading the rest of the file so they can pick what to do next
                if (System.Windows.Forms.MessageBox.Show("FileSize is " + fileSize + ".\nThis is an unhandled value currently.\nDo you want to abort loading this file?\n\n[Yes] will abort loading this file.\n[No] will continue to load this file.\n\nIt is NOT SAFE to save this file with the current value. If you do so, some data loss may occur in the resulting file.\nIf this file was created and edited with only the in-game UI, please report this with the variant file included and any useful information.", "Unhandled Value Detected!", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }

            fileCreatedTimestamp = b.ReadInt64(); // fileCreatedTimestamp - Seconds since the UNIX epoch
            b.ReadInt32(); // unk4
            b.ReadInt32(); // campaign ID
            b.ReadInt32(); // map ID
            gameTypeMenu = (GameType)b.ReadInt32(); // gameType (1 = CTF, 2 = Slayer, 3 = Oddball, 4 = KOTH, 5 = Forge, 6 = VIP, 7 = Juggernaut, 8 = Territories, 9 = Assault, 10 = Infection)
            b.ReadInt32(); // campaign difficulty
            b.ReadInt32(); // campaign insertion index
            b.ReadInt32(); // is survival
            b.ReadInt32(); // game id

            // VR
            variantType = new string(b.ReadChars(4)); // mpvr (Multiplayer Variant) || mapv (Map Variant)
            b.ReadInt32(); // size
            b.ReadInt32(); // content version
            gameTypeInit = (GameType)b.ReadInt32(); // Game Type that appears to ultimately control how the game type is read
            gameCode = (GameCode)b.ReadInt32(); // Not entirely sure but it seems only the first two bytes change - based off original variant type (ex. slayer, ctf, vip) changing it does not appear to do anything
            b.ReadBytes(4); // unused/padding
            // Map Variants do not have a metadata section, they go right into the variant name instead
            if (fileSize != 57840)
                metadata = new string(b.ReadChars(40)).Replace("\0", ""); // will contain name of variant this one is based off of - does not always have value set
            variantNameGame = new string(b.ReadChars(32)).Replace("\0", ""); // variantName
            descriptionGame = new string(b.ReadChars(128)).Replace("\0", ""); // variantDescription
            originalAuthorName = new string(b.ReadChars(16)).Replace("\0", ""); // originalPlayerGamertag
            gameTypeGameOrder = (GameTypeOrder)b.ReadInt32(); // gameType Order
            b.ReadInt32(); // is user valid
            originalAuthorXUID = b.ReadBytes(8); // Original variant author XUID
            originalFileSize = b.ReadInt64(); // fileSize (as reported in the menu of the game - in bytes size)
            originalFileCreatedTimestamp = b.ReadInt64(); // fileCreatedTimestamp - Seconds since the UNIX epoch
            b.ReadInt32(); // unk5
            b.ReadInt32(); // campaign ID
            b.ReadInt32(); // map ID
            gameTypeGame = (GameType)b.ReadInt32(); // gameType (1 = CTF, 2 = Slayer, 3 = Oddball, 4 = KOTH, 5 = Forge, 6 = VIP, 7 = Juggernaut, 8 = Territories, 9 = Assault, 10 = Infection)
            b.ReadInt32(); // campaign difficulty
            b.ReadInt32(); // campaign insertion index
            b.ReadInt32(); // is survival
            b.ReadInt32(); // game id

            if (variantType == "mapv")
                return true;
            if (fileSize == 57840)
                return true;

            // Start dealing with game type variant settings
            byte RoundsResetVariable = b.ReadByte(); // roundsReset (0x06 = Everything, 0x00 = Nothing, 0x02 = Players Only) : Team Mode : Enabled += 0x01 (Everything + Enabled = 7), Disabled += 0x00 (Everything + Disabled = 6)
            /*
             * Settings Used:
             * Rounds Reset (0, 2, 6)
             * Team Mode (0, 1)
             * Spectating (0, 8)
             * 
             * 0 = Reset Nothing; FFA; No Spectating
             * 2 = Reset Players; FFA; No Spectating
             * 6 = Reset Everything; FFA; No Spectating
             * 
             * 1 = Reset Nothing; Team Game; No Spectating
             * 3 = Reset Players; Team Game; No Spectating
             * 7 = Reset Everything; Team Game; No Spectating
             * 
             * 8 = Reset Nothing; FFA; Spectating
             * 10 = Reset Players; FFA; Spectating
             * 14 = Reset Everything; FFA; Spectating
             * 
             * 9 = Reset Nothing; Team Game; Spectating
             * 11 = Reset Players; Team Game; Spectating
             * 15 = Reset Everything; Team Game; Spectating
             */
            switch (RoundsResetVariable)
            {
                case 0x00:
                    generalSettings.RoundsReset = RoundsReset.Nothing;
                    generalSettings.TeamMode = TeamMode.Disabled;
                    generalSettings.Spectating = Spectating.NotAllowed;
                    break;
                case 0x01:
                    generalSettings.RoundsReset = RoundsReset.Nothing;
                    generalSettings.TeamMode = TeamMode.Enabled;
                    generalSettings.Spectating = Spectating.NotAllowed;
                    break;
                case 0x02:
                    generalSettings.RoundsReset = RoundsReset.PlayersOnly;
                    generalSettings.TeamMode = TeamMode.Disabled;
                    generalSettings.Spectating = Spectating.NotAllowed;
                    break;
                case 0x03:
                    generalSettings.RoundsReset = RoundsReset.PlayersOnly;
                    generalSettings.TeamMode = TeamMode.Enabled;
                    generalSettings.Spectating = Spectating.NotAllowed;
                    break;
                case 0x06:
                    generalSettings.RoundsReset = RoundsReset.Everything;
                    generalSettings.TeamMode = TeamMode.Disabled;
                    generalSettings.Spectating = Spectating.NotAllowed;
                    break;
                case 0x07:
                    generalSettings.RoundsReset = RoundsReset.Everything;
                    generalSettings.TeamMode = TeamMode.Enabled;
                    generalSettings.Spectating = Spectating.NotAllowed;
                    break;
                case 0x08:
                    generalSettings.RoundsReset = RoundsReset.Nothing;
                    generalSettings.TeamMode = TeamMode.Disabled;
                    generalSettings.Spectating = Spectating.Allowed;
                    break;
                case 0x09:
                    generalSettings.RoundsReset = RoundsReset.Nothing;
                    generalSettings.TeamMode = TeamMode.Enabled;
                    generalSettings.Spectating = Spectating.Allowed;
                    break;
                case 0x0A:
                    generalSettings.RoundsReset = RoundsReset.PlayersOnly;
                    generalSettings.TeamMode = TeamMode.Disabled;
                    generalSettings.Spectating = Spectating.Allowed;
                    break;
                case 0x0B:
                    generalSettings.RoundsReset = RoundsReset.PlayersOnly;
                    generalSettings.TeamMode = TeamMode.Enabled;
                    generalSettings.Spectating = Spectating.Allowed;
                    break;
                case 0x0E:
                    generalSettings.RoundsReset = RoundsReset.Everything;
                    generalSettings.TeamMode = TeamMode.Disabled;
                    generalSettings.Spectating = Spectating.Allowed;
                    break;
                case 0x0F:
                    generalSettings.RoundsReset = RoundsReset.Everything;
                    generalSettings.TeamMode = TeamMode.Enabled;
                    generalSettings.Spectating = Spectating.Allowed;
                    break;
                default:
                    // Give user option to recover as this can be generally be safe to recover from unlike other settings
                    bool canSupportTeams = true;
                    if (gameTypeMenu == GameType.Juggernaut || gameTypeMenu == GameType.Infection)
                    {
                        canSupportTeams = false;
                    }
                    if (System.Windows.Forms.MessageBox.Show("RoundsResetVariable is " + (int)RoundsResetVariable + ".\nThis is an unhandled value currently.\n\n[Retry] will force this to " + (canSupportTeams ? "0x07 (Rounds Reset = Everything, Team Mode = Enabled)." : "0x06 (Rounds Reset = Everything, Team Mode = Disabled).") + "\n[Cancel] will leave this value alone.\n\nIt is NOT SAFE to save this file with the current value. If you do so, some data loss may occur in the resulting file.\nIf this file was created and edited with only the in-game UI, please report this with the variant file included and any useful information.", "Unhandled Value Detected!", System.Windows.Forms.MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Retry)
                    {
                        generalSettings.RoundsReset = RoundsReset.Everything;
                        // Default Spectating to Not Allowed to make it easier
                        generalSettings.Spectating = Spectating.NotAllowed;
                        // Only force to Team Mode Enabled if the mode can support teams
                        if (canSupportTeams == true)
                        {
                            generalSettings.TeamMode = TeamMode.Enabled;
                        }
                        else
                        {
                            generalSettings.TeamMode = TeamMode.Disabled;
                        }
                    }
                    break;
            }

            generalSettings.TimeLimit = (TimeLimit)b.ReadByte(); // timeLimit (in minutes) 0x00 = No Limit, 0x01 = 1 Minute, 0x02 = 2 Minutes
            generalSettings.NumberOfRounds = (NumberOfRounds)b.ReadByte(); // numberOfRounds 0x01 = 0x01 Round, 0x02 = 2 Rounds, 0x03 = 3 Rounds
            b.ReadByte(); // Early Victory Win Count?
            byte respawnModifiersSynchronizeWithTeamRespawnOnKills = b.ReadByte(); // Synchronize respawns with team, [10]16 = Disabled, [11]17 = Enabled, [18]24 = Respawn on Kills, [19]25 = Sync with Team + Respawn on Kill [General Settings > Respawn Options > Advanced Respawn Settings > Respawn Modifiers]
            switch (respawnModifiersSynchronizeWithTeamRespawnOnKills)
            {
                // Unsure why this has been 0 before, but the settings were both displayed as Disabled in the in-game UI
                case 0:
                case 16:
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam = SynchronizeWithTeam.Disabled;
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills = RespawnOnKills.Disabled;
                    break;
                case 17:
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam = SynchronizeWithTeam.Enabled;
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills = RespawnOnKills.Disabled;
                    break;
                case 24:
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam = SynchronizeWithTeam.Disabled;
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills = RespawnOnKills.Enabled;
                    break;
                case 25:
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam = SynchronizeWithTeam.Enabled;
                    generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills = RespawnOnKills.Enabled;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("respawnModifiersSynchronizeWithTeamRespawnOnKills is:\nHex: " + respawnModifiersSynchronizeWithTeamRespawnOnKills.ToString() + "\nDec: " + (int)respawnModifiersSynchronizeWithTeamRespawnOnKills + "\nAborting Variant Loading...");
                    return false;
            }

            generalSettings.RespawnSettings.LivesPerRound = (LivesPerRound)b.ReadByte(); // livesPerRound [General Settings > Respawn Options]
            b.ReadByte(); // Shared Team Lives?
            generalSettings.RespawnSettings.RespawnTimeServer = (RespawnTime)b.ReadByte(); // respawnTimeServer [General Settings > Respawn Options]
            generalSettings.RespawnSettings.RespawnTimeMenu = (RespawnTime)b.ReadByte(); // respawnTimeMenu [General Settings > Respawn Options]
            generalSettings.RespawnSettings.SuicidePenalty = (SuicidePenalty)b.ReadByte(); // suicidePenaltyMenu [General Settings > Respawn Options]
            generalSettings.RespawnSettings.BetrayalPenalty = (BetrayalPenalty)b.ReadByte(); // betrayalPenaltyMenu [General Settings > Respawn Options]
            generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnTimeGrowth = (RespawnTimeGrowth)b.ReadByte(); // respawnTimeGrowth (Dying increases respawn time, killing decreases it in seconds) [General Settings > Respawn Options > Advanced Respawn Settings > Respawn Modifiers]
            generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Duration = (Duration)b.ReadInt32(); // respawnTraitsDuration (How long in seconds player is given Respawn Traits) [General Settings > Respawn Options > Advanced Respawn Settings > Respawn Traits]

            ParseTraits(ref b, ref generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth, ref generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage, ref generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement, ref generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance, ref generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Sensors);

            short generalSettingsFriendlyFireBetrayalBooting = b.ReadInt16(); // Friendly Fire/Betrayal Booting - FF & BB Disabled(0x00) FF Enabled & BB Disabled(0x01) FF Disabled & BB Enabled(0x02) FF & BB Enabled(0x03) [General Settings]
            switch (generalSettingsFriendlyFireBetrayalBooting)
            {
                case 0:
                    generalSettings.FriendlyFire = FriendlyFire.Disabled;
                    generalSettings.BetrayalBooting = BetrayalBooting.Disabled;
                    break;
                case 1:
                    generalSettings.FriendlyFire = FriendlyFire.Enabled;
                    generalSettings.BetrayalBooting = BetrayalBooting.Disabled;
                    break;
                case 2:
                    generalSettings.FriendlyFire = FriendlyFire.Disabled;
                    generalSettings.BetrayalBooting = BetrayalBooting.Enabled;
                    break;
                case 3:
                    generalSettings.FriendlyFire = FriendlyFire.Enabled;
                    generalSettings.BetrayalBooting = BetrayalBooting.Enabled;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("generalSettingsFriendlyFireBetrayalBooting is " + (int)generalSettingsFriendlyFireBetrayalBooting + ".\nAborting Variant Loading...");
                    return false;
            }
            generalSettings.TeamChanging = (TeamChanging)b.ReadInt16(); // generalSettingsTeamChanging

            int weaponsAndVehiclesGrenadesOnMapIndestructibleVehicles = b.ReadInt32(); // weaponsAndVehiclesGrenadesOnMapIndestructibleVehicles - GoM = Map Default or None, IV = Disabled or Enabled - None + Disabled = 00, Map Default + Disabled = 01, None + Enabled = 02, Map Default + Enabled = 03
            switch (weaponsAndVehiclesGrenadesOnMapIndestructibleVehicles)
            {
                case 0:
                    weaponsAndVehicles.GrenadesOnMap = GrenadesOnMap.None;
                    weaponsAndVehicles.IndestructibleVehicles = IndestructibleVehicles.Disabled;
                    break;
                case 1:
                    weaponsAndVehicles.GrenadesOnMap = GrenadesOnMap.MapDefault;
                    weaponsAndVehicles.IndestructibleVehicles = IndestructibleVehicles.Disabled;
                    break;
                case 2:
                    weaponsAndVehicles.GrenadesOnMap = GrenadesOnMap.None;
                    weaponsAndVehicles.IndestructibleVehicles = IndestructibleVehicles.Enabled;
                    break;
                case 3:
                    weaponsAndVehicles.GrenadesOnMap = GrenadesOnMap.MapDefault;
                    weaponsAndVehicles.IndestructibleVehicles = IndestructibleVehicles.Enabled;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("weaponsAndVehiclesGrenadesOnMapIndestructibleVehicles is " + (int)weaponsAndVehiclesGrenadesOnMapIndestructibleVehicles + ".\nAborting Variant Loading...");
                    return false;
            }

            ParseTraits(ref b, ref generalSettings.BasePlayerTraits.ShieldsAndHealth, ref generalSettings.BasePlayerTraits.WeaponsAndDamage, ref generalSettings.BasePlayerTraits.Movement, ref generalSettings.BasePlayerTraits.Appearance, ref generalSettings.BasePlayerTraits.Sensors);

            weaponsAndVehicles.WeaponsOnMap = (WeaponsOnMap)b.ReadInt16(); // weaponsAndVehiclesWeaponsOnMap
            if ((short)weaponsAndVehicles.WeaponsOnMap == -1)
                weaponsAndVehicles.WeaponsOnMap = 0;
            weaponsAndVehicles.VehicleSet = (VehicleSet)b.ReadInt16(); // weaponsAndVehiclesVehicleSet
            if ((short)weaponsAndVehicles.VehicleSet == -1)
                weaponsAndVehicles.VehicleSet = 0;

            ParseTraits(ref b, ref weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth, ref weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage, ref weaponsAndVehicles.OvershieldPowerupTraits.Movement, ref weaponsAndVehicles.OvershieldPowerupTraits.Appearance, ref weaponsAndVehicles.OvershieldPowerupTraits.Sensors);
            ParseTraits(ref b, ref weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth, ref weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage, ref weaponsAndVehicles.ActiveCamoPowerupTraits.Movement, ref weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance, ref weaponsAndVehicles.ActiveCamoPowerupTraits.Sensors);
            ParseTraits(ref b, ref weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth, ref weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage, ref weaponsAndVehicles.CustomPowerupTraits.Movement, ref weaponsAndVehicles.CustomPowerupTraits.Appearance, ref weaponsAndVehicles.CustomPowerupTraits.Sensors);

            weaponsAndVehicles.OvershieldPowerupTraits.Duration = (Duration)b.ReadByte(); // overshieldPowerupOptionsDuration
            weaponsAndVehicles.ActiveCamoPowerupTraits.Duration = (Duration)b.ReadByte(); // activeCamoPowerupOptionsDuration
            weaponsAndVehicles.CustomPowerupTraits.Duration = (Duration)b.ReadByte(); // customPowerupOptionsDuration

            b.ReadByte(); // unk6
            b.ReadByte(); // is_dirty
            b.ReadByte(); // unk7

            switch (gameTypeMenu)
            {
                case GameType.UNK:
                    break;
                case GameType.CTF:
                    b.ReadBytes(2); // unk0
                    variantSettings.CTFSettings.FlagAtHomeToScore = (Boolean)b.ReadByte();
                    variantSettings.CTFSettings.FlagWaypoint = (FlagWaypoint)b.ReadByte(); // 0 = UNK, 1 = Always?, 2 = UNK, 3 = Not In Single Flag?
                    variantSettings.CTFSettings.NumberOfFlags = (NumberOfFlags)b.ReadByte();
                    variantSettings.CTFSettings.RespawnOnCapture = (RespawnOnCaptureByte)b.ReadByte(); // 0 = Disabled, 1 = On Ally Capture, 2 = On Enemy Capture, 3 = On Any Capture
                    variantSettings.CTFSettings.FlagReturnTime = (FlagReturnTime)b.ReadInt16();
                    variantSettings.CTFSettings.SuddenDeath = (SuddenDeath)b.ReadInt16();
                    variantSettings.CTFSettings.CapturesToWin = (CapturesToWin)b.ReadInt16();
                    b.ReadBytes(2); // unk1
                    variantSettings.CTFSettings.FlagResetTime = (FlagResetTime)b.ReadInt16();

                    // Flag Carrier Traits
                    ParseTraits(ref b, ref variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth, ref variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage, ref variantSettings.CTFSettings.FlagCarrierTraits.Movement, ref variantSettings.CTFSettings.FlagCarrierTraits.Appearance, ref variantSettings.CTFSettings.FlagCarrierTraits.Sensors);
                    break;
                case GameType.Slayer:
                    variantSettings.SlayerSettings.TeamScoring = (TeamScoring)b.ReadInt16(); // slayerScoringOptionsTeamScoring
                    variantSettings.SlayerSettings.ScoreToWin = (ScoreToWin)b.ReadInt16();
                    b.ReadInt16(); // unk0
                    variantSettings.SlayerSettings.KillPoints = (ScoreOptionsPointsShort)b.ReadInt16();
                    variantSettings.SlayerSettings.AssistPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.DeathPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.SuicidePoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.BetrayalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.LeaderKillBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.EliminationBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.AssassinationBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.HeadshotBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.BeatdownBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.StickyBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.SplatterBonus = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.SlayerSettings.SpreeBonus = (ScoreOptionsPointsByte)b.ReadByte();

                    // Leader Traits
                    ParseTraits(ref b, ref variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth, ref variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage, ref variantSettings.SlayerSettings.LeaderTraits.Movement, ref variantSettings.SlayerSettings.LeaderTraits.Appearance, ref variantSettings.SlayerSettings.LeaderTraits.Sensors);
                    break;
                case GameType.Oddball:
                    variantSettings.OddballSettings.TeamScoring = (TeamScoring)b.ReadInt16();
                    variantSettings.OddballSettings.AutoPickup = (Boolean)b.ReadInt32();
                    variantSettings.OddballSettings.ScoreToWin = (ScoreToWin)b.ReadInt16();
                    b.ReadInt16(); //unk0
                    variantSettings.OddballSettings.CarryingPoints = (ScoreOptionsPointsShort)b.ReadInt16();
                    variantSettings.OddballSettings.KillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.OddballSettings.BallKillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.OddballSettings.BallCarrierKillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.OddballSettings.BallCount = (BallCount)b.ReadByte();
                    variantSettings.OddballSettings.InitialBallDelay = (InitialBallDelay)b.ReadInt16();
                    variantSettings.OddballSettings.BallRespawnDelay = (BallRespawnDelay)b.ReadInt16();

                    // Ball Carrier Traits
                    ParseTraits(ref b, ref variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth, ref variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage, ref variantSettings.OddballSettings.BallCarrierTraits.Movement, ref variantSettings.OddballSettings.BallCarrierTraits.Appearance, ref variantSettings.OddballSettings.BallCarrierTraits.Sensors);
                    break;
                case GameType.KOTH:
                    variantSettings.KOTHSettings.TeamScoring = (TeamScoringKOTH)b.ReadInt16();
                    b.ReadInt32(); // unk0
                    variantSettings.KOTHSettings.ScoreToWin = (ScoreToWin)b.ReadInt16();
                    b.ReadInt16(); // unk1

                    short variantSettingsKOTHSettingsHillMovementOrders = b.ReadInt16();
                    if (variantSettingsKOTHSettingsHillMovementOrders >= 256)
                    {
                        variantSettings.KOTHSettings.HillMovementOrder = HillMovementOrder.Sequence;
                        variantSettingsKOTHSettingsHillMovementOrders -= 256; // Make variable become a valid HillMovement value to reduce operations
                    }
                    else
                    {
                        variantSettings.KOTHSettings.HillMovementOrder = HillMovementOrder.Random;
                    }
                    variantSettings.KOTHSettings.HillMovement = (HillMovement)variantSettingsKOTHSettingsHillMovementOrders;

                    variantSettings.KOTHSettings.OnHillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.KOTHSettings.OffHillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.KOTHSettings.UncontestedControlPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.KOTHSettings.KillPoints = (ScoreOptionsPointsByte)b.ReadByte();

                    // On Hill Traits
                    ParseTraits(ref b, ref variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth, ref variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage, ref variantSettings.KOTHSettings.OnHillTraits.Movement, ref variantSettings.KOTHSettings.OnHillTraits.Appearance, ref variantSettings.KOTHSettings.OnHillTraits.Sensors);
                    break;
                case GameType.Forge:
                    break;
                case GameType.VIP:
                    b.ReadInt16(); // unk0
                    variantSettings.VIPSettings.ScoreToWin = (ScoreToWin)b.ReadInt16();
                    b.ReadInt16(); // unk1
                    short SingleVIPVariable = b.ReadInt16();
                    switch (SingleVIPVariable)
                    {
                        case 0:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Disabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Disabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Disabled;
                            break;
                        case 1:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Enabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Disabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Disabled;
                            break;
                        case 2:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Disabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Disabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Enabled;
                            break;
                        case 3:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Enabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Disabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Enabled;
                            break;
                        case 4:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Disabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Enabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Disabled;
                            break;
                        case 5:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Enabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Enabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Disabled;
                            break;
                        case 6:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Disabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Enabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Enabled;
                            break;
                        case 7:
                            variantSettings.VIPSettings.SingleVIP = SingleVIP.Enabled;
                            variantSettings.VIPSettings.VIPDeathEndsRound = VIPDeathEndsRound.Enabled;
                            variantSettings.VIPSettings.GoalAreas = GoalAreas.Enabled;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("SingleVIPVariable is " + (int)SingleVIPVariable + ".\nAborting Variant Loading...");
                            return false;
                    }

                    variantSettings.VIPSettings.KillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.VIPTakedownPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.KillAsVIPPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.VIPDeathPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.GoalArrivalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.SuicidePoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.BetrayalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.VIPBetrayalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.VIPSettings.NextVIP = (NextVIP)b.ReadByte();
                    variantSettings.VIPSettings.GoalMovement = (GoalMovement)b.ReadByte();
                    variantSettings.VIPSettings.GoalMovementOrder = (GoalZoneOrder)b.ReadByte();
                    b.ReadByte(); // unk2
                    variantSettings.VIPSettings.InfluenceRadius = (InfluenceRadius)b.ReadInt16();

                    // VIP Team Traits
                    ParseTraits(ref b, ref variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth, ref variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage, ref variantSettings.VIPSettings.VIPTeamTraits.Movement, ref variantSettings.VIPSettings.VIPTeamTraits.Appearance, ref variantSettings.VIPSettings.VIPTeamTraits.Sensors);

                    // VIP Proximity Traits
                    ParseTraits(ref b, ref variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth, ref variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage, ref variantSettings.VIPSettings.VIPProximityTraits.Movement, ref variantSettings.VIPSettings.VIPProximityTraits.Appearance, ref variantSettings.VIPSettings.VIPProximityTraits.Sensors);

                    // VIP Traits
                    ParseTraits(ref b, ref variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth, ref variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage, ref variantSettings.VIPSettings.VIPTraits.Movement, ref variantSettings.VIPSettings.VIPTraits.Appearance, ref variantSettings.VIPSettings.VIPTraits.Sensors);
                    break;
                case GameType.Juggernaut:
                    b.ReadInt16(); // unk0
                    variantSettings.JuggernautSettings.ScoreToWin = (ScoreToWin)b.ReadInt16();
                    b.ReadInt16(); // unk1
                    b.ReadInt16(); // unk2
                    variantSettings.JuggernautSettings.FirstJuggernaut = (FirstJuggernaut)b.ReadByte();
                    variantSettings.JuggernautSettings.NextJuggernaut = (NextJuggernaut)b.ReadByte();
                    byte AlliedAgainstJuggernautVariable = b.ReadByte();
                    switch (AlliedAgainstJuggernautVariable)
                    {
                        case 0:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.NotAllied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Disabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Disabled;
                            break;
                        case 1:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.Allied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Disabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Disabled;
                            break;
                        case 2:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.NotAllied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Disabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Enabled;
                            break;
                        case 3:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.Allied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Disabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Enabled;
                            break;
                        case 4:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.NotAllied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Enabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Disabled;
                            break;
                        case 5:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.Allied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Enabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Disabled;
                            break;
                        case 6:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.NotAllied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Enabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Enabled;
                            break;
                        case 7:
                            variantSettings.JuggernautSettings.AlliedAgainstJuggernaut = AlliedAgainstJuggernaut.Allied;
                            variantSettings.JuggernautSettings.GoalZones = GoalZones.Enabled;
                            variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut = RespawnOnLoneJuggernaut.Enabled;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("AlliedAgainstJuggernautVariable is " + (int)AlliedAgainstJuggernautVariable + ".\nAborting Variant Loading...");
                            return false;
                    }

                    variantSettings.JuggernautSettings.GoalZoneMovement = (GoalZoneMovement)b.ReadByte();
                    variantSettings.JuggernautSettings.GoalZoneOrder = (GoalZoneOrder)b.ReadByte();
                    variantSettings.JuggernautSettings.KillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.JuggernautSettings.TakedownPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.JuggernautSettings.KillAsJuggernautPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.JuggernautSettings.GoalArrivalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.JuggernautSettings.SuicidePoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.JuggernautSettings.BetrayalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.JuggernautSettings.NextJuggernautDelay = (NextJuggernautDelay)b.ReadByte();

                    // Juggernaut Traits
                    ParseTraits(ref b, ref variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth, ref variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage, ref variantSettings.JuggernautSettings.JuggernautTraits.Movement, ref variantSettings.JuggernautSettings.JuggernautTraits.Appearance, ref variantSettings.JuggernautSettings.JuggernautTraits.Sensors);
                    break;
                case GameType.Territories:
                    b.ReadInt16(); // unk0
                    short OneSidedTerritoriesVariable = b.ReadInt16();
                    switch (OneSidedTerritoriesVariable)
                    {
                        case 0:
                            variantSettings.TerritoriesSettings.OneSidedTerritories = OneSidedTerritories.Disabled;
                            variantSettings.TerritoriesSettings.LockAfterCapture = LockAfterCapture.Disabled;
                            break;
                        case 1:
                            variantSettings.TerritoriesSettings.OneSidedTerritories = OneSidedTerritories.Enabled;
                            variantSettings.TerritoriesSettings.LockAfterCapture = LockAfterCapture.Disabled;
                            break;
                        case 2:
                            variantSettings.TerritoriesSettings.OneSidedTerritories = OneSidedTerritories.Disabled;
                            variantSettings.TerritoriesSettings.LockAfterCapture = LockAfterCapture.Enabled;
                            break;
                        case 3:
                            variantSettings.TerritoriesSettings.OneSidedTerritories = OneSidedTerritories.Enabled;
                            variantSettings.TerritoriesSettings.LockAfterCapture = LockAfterCapture.Enabled;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("OneSidedTerritoriesVariable is " + (short)OneSidedTerritoriesVariable + ".\nAborting Variant Loading...");
                            return false;
                    }

                    variantSettings.TerritoriesSettings.RespawnOnCapture = (RespawnOnCapture)b.ReadInt16();
                    variantSettings.TerritoriesSettings.TerritoryCaptureTime = (TerritoryCaptureTime)b.ReadInt16();
                    variantSettings.TerritoriesSettings.SuddenDeath = (SuddenDeath)b.ReadInt16();

                    // Defender Traits
                    ParseTraits(ref b, ref variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth, ref variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage, ref variantSettings.TerritoriesSettings.DefenderTraits.Movement, ref variantSettings.TerritoriesSettings.DefenderTraits.Appearance, ref variantSettings.TerritoriesSettings.DefenderTraits.Sensors);

                    // Attacker Traits
                    ParseTraits(ref b, ref variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth, ref variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage, ref variantSettings.TerritoriesSettings.AttackerTraits.Movement, ref variantSettings.TerritoriesSettings.AttackerTraits.Appearance, ref variantSettings.TerritoriesSettings.AttackerTraits.Sensors);
                    break;
                case GameType.Assault:
                    b.ReadInt16(); // unk0
                    variantSettings.AssaultSettings.ResetOnDisarm = (Boolean)b.ReadInt32();
                    variantSettings.AssaultSettings.AssaultBombMode = (AssaultBombMode)b.ReadInt16();
                    b.ReadInt16(); // Enemy Bomb Waypoint? - 0 = UNK, 1 = Always?, 2 = UNK, 3 = Not In Single Bomb?
                    variantSettings.AssaultSettings.DetonationsToWin = (DetonationsToWin)b.ReadInt16();
                    b.ReadInt16(); // unk1
                    b.ReadInt32(); // unk2
                    b.ReadInt16(); // unk3
                    variantSettings.AssaultSettings.SuddenDeath = (SuddenDeath)b.ReadInt16();
                    variantSettings.AssaultSettings.BombResetTime = (BombResetTime)b.ReadInt16();
                    variantSettings.AssaultSettings.BombArmingTime = (BombArmingTime)b.ReadInt16();
                    variantSettings.AssaultSettings.BombDisarmingTime = (BombDisarmingTime)b.ReadInt16();
                    variantSettings.AssaultSettings.BombFuseTime = (BombArmingTime)b.ReadInt16();

                    // Bomb Carrier Traits
                    ParseTraits(ref b, ref variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth, ref variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage, ref variantSettings.AssaultSettings.BombCarrierTraits.Movement, ref variantSettings.AssaultSettings.BombCarrierTraits.Appearance, ref variantSettings.AssaultSettings.BombCarrierTraits.Sensors);

                    // Secondary Traits
                    ParseTraits(ref b, ref variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth, ref variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage, ref variantSettings.AssaultSettings.SecondaryTraits.Movement, ref variantSettings.AssaultSettings.SecondaryTraits.Appearance, ref variantSettings.AssaultSettings.SecondaryTraits.Sensors);
                    break;
                case GameType.Infection:
                    b.ReadInt16(); // unk0
                    variantSettings.InfectionSettings.RespawnOnHavenMove = (Boolean)b.ReadByte();
                    variantSettings.InfectionSettings.SafeHavenMovement = (SafeHavenMovement)b.ReadByte();
                    variantSettings.InfectionSettings.NextZombie = (NextZombie)b.ReadByte();
                    variantSettings.InfectionSettings.InitialZombieCount = (InitialZombieCount)b.ReadByte();
                    variantSettings.InfectionSettings.SafeHavenMovementTime = (SafeHavenMovementTime)b.ReadByte();
                    b.ReadByte(); // unk1
                    variantSettings.InfectionSettings.ZombieKillPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.InfectionSettings.InfectionPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.InfectionSettings.SafeHavenArrivalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.InfectionSettings.SuicidePoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.InfectionSettings.BetrayalPoints = (ScoreOptionsPointsByte)b.ReadByte();
                    variantSettings.InfectionSettings.LastManStandingBonus = (ScoreOptionsPointsByte)b.ReadByte();

                    // Zombie Traits
                    ParseTraits(ref b, ref variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth, ref variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage, ref variantSettings.InfectionSettings.ZombieTraits.Movement, ref variantSettings.InfectionSettings.ZombieTraits.Appearance, ref variantSettings.InfectionSettings.ZombieTraits.Sensors);

                    // Alpha Zombie Traits
                    ParseTraits(ref b, ref variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth, ref variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage, ref variantSettings.InfectionSettings.AlphaZombieTraits.Movement, ref variantSettings.InfectionSettings.AlphaZombieTraits.Appearance, ref variantSettings.InfectionSettings.AlphaZombieTraits.Sensors);

                    // Safe Haven Traits
                    ParseTraits(ref b, ref variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth, ref variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage, ref variantSettings.InfectionSettings.SafeHavenTraits.Movement, ref variantSettings.InfectionSettings.SafeHavenTraits.Appearance, ref variantSettings.InfectionSettings.SafeHavenTraits.Sensors);

                    // Last Man Traits
                    ParseTraits(ref b, ref variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth, ref variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage, ref variantSettings.InfectionSettings.LastManTraits.Movement, ref variantSettings.InfectionSettings.LastManTraits.Appearance, ref variantSettings.InfectionSettings.LastManTraits.Sensors);
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("gameTypeMenu is " + (int)gameTypeMenu + ".\nAborting Variant Loading...");
                    return false;
            }

            return true;
        }

        public bool Write(BinaryWriter b)
        {
            // Write BLF Header
            b.Write("_blf".ToCharArray());
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x30 }); // Size
            b.Write(new byte[] { 0x00, 0x01, 0x00, 0x02 });
            b.Write(new byte[] { 0xFF, 0xFE, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            // Write CHDR
            b.Write("chdr".ToCharArray());
            b.Write(new byte[] { 0x00, 0x00, 0x01, 0x08 }); // Size
            b.Write(new byte[] { 0x00, 0x09, 0x00, 0x03 }); // Content Version
            b.Write(new byte[] { 0x00, 0x00 }); // Build version major number
            b.Write(new byte[] { 0xFF, 0xFF }); // Build version minor number
            b.Write(new byte[] { 0x35, 0x1A, 0x94, 0x82 }); // These two lines make a checksum
            b.Write(new byte[] { 0xE3, 0xB5, 0xB9, 0x2A }); // It still needs to be researched to be implemented in this editor
            b.Write(StringNullEscape(variantNameMenu, 16).ToCharArray());
            b.Write(StringPadNulls(descriptionMenu, 128).ToCharArray());
            b.Write(StringPadNulls(currentAuthorName, 16).ToCharArray());
            b.Write((int)gameTypeMenuOrder); // Another game type but uses a different order
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Is User Valid
            b.Write(currentAuthorXUID);
            b.Write(fileSize); // Expose to UI? If exposed, detecting gametype/map variant will need to be done using the variable which comes later in the file
            b.Write(fileCreatedTimestamp);
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // unk
            b.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); // Campaign ID
            b.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); // Map ID
            b.Write((int)gameTypeMenu);
            b.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); // Campaign Difficulty
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Campaign Insertion Index
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Is Survival
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Game ID
            // Write MPVR
            b.Write("mpvr".ToCharArray());
            b.Write(new byte[] { 0x00, 0x00, 0x02, 0x70 }); // Size
            b.Write(new byte[] { 0x00, 0x03, 0x00, 0x01 }); // Content Version
            b.Write((int)gameTypeInit); // Game Type, this appears to ultimately control how the game type is read
            b.Write((int)gameCode);
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // padding
            b.Write(StringPadNulls(metadata, 40).ToCharArray()); // Begins lower case only text/name of specific gametype this is based off of - is not always set - we're going to hijack this block for allowing the author to store their own persistent metadata as there appears to be no issues doing this
            b.Write(StringNullEscape(variantNameGame, 16).ToCharArray());
            b.Write(StringPadNulls(descriptionGame, 128).ToCharArray());
            b.Write(StringPadNulls(originalAuthorName, 16).ToCharArray());
            b.Write((int)gameTypeGameOrder); // Need to expose gameTypeGameOrder to UI and use for this.
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Is User Valid
            b.Write(originalAuthorXUID);
            b.Write(originalFileSize); // Expose to UI? If exposed, detecting gametype/map variant may become difficult
            b.Write(originalFileCreatedTimestamp);
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // unk
            b.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); // Campaign ID
            b.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); // Map ID
            b.Write((int)gameTypeGame);
            b.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); // Campaign Difficulty
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Campaign Insertiton Index
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Is Survival
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // Game ID

            // Start game type variant settings
            byte RoundsResetVariable = Convert.ToByte((int)generalSettings.RoundsReset + (int)generalSettings.TeamMode + (int)generalSettings.Spectating);
            b.Write((byte)RoundsResetVariable);

            b.Write((byte)generalSettings.TimeLimit);
            b.Write((byte)generalSettings.NumberOfRounds);
            b.Write((byte)0x00);
            if (generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam == SynchronizeWithTeam.Disabled)
            {
                if (generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills == RespawnOnKills.Enabled)
                {
                    b.Write((byte)0x18);
                }
                else
                {
                    b.Write((byte)0x10);
                }
            }
            else
            {
                if (generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills == RespawnOnKills.Enabled)
                {
                    b.Write((byte)0x19);
                }
                else
                {
                    b.Write((byte)0x11);
                }
            }
            b.Write((byte)generalSettings.RespawnSettings.LivesPerRound);
            b.Write((byte)0x00);
            b.Write((byte)generalSettings.RespawnSettings.RespawnTimeServer);
            b.Write((byte)generalSettings.RespawnSettings.RespawnTimeMenu);
            b.Write((byte)generalSettings.RespawnSettings.SuicidePenalty);
            b.Write((byte)generalSettings.RespawnSettings.BetrayalPenalty);
            b.Write((byte)generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnTimeGrowth);
            b.Write((int)generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Duration);
            WriteTraits(ref b, generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth, generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage, generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement, generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance, generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Sensors);
            if (generalSettings.FriendlyFire == FriendlyFire.Disabled)
            {
                if (generalSettings.BetrayalBooting == BetrayalBooting.Disabled)
                {
                    b.Write((short)0);
                }
                else
                {
                    b.Write((short)2);
                }
            }
            else
            {
                if (generalSettings.BetrayalBooting == BetrayalBooting.Disabled)
                {
                    b.Write((short)1);
                }
                else
                {
                    b.Write((short)3);
                }
            }
            b.Write((short)generalSettings.TeamChanging);
            if (weaponsAndVehicles.GrenadesOnMap == GrenadesOnMap.None)
            {
                if (weaponsAndVehicles.IndestructibleVehicles == IndestructibleVehicles.Disabled)
                {
                    b.Write((int)0);
                }
                else
                {
                    b.Write((int)2);
                }
            }
            else
            {
                if (weaponsAndVehicles.IndestructibleVehicles == IndestructibleVehicles.Disabled)
                {
                    b.Write((int)1);
                }
                else
                {
                    b.Write((int)3);
                }
            }
            WriteTraits(ref b, generalSettings.BasePlayerTraits.ShieldsAndHealth, generalSettings.BasePlayerTraits.WeaponsAndDamage, generalSettings.BasePlayerTraits.Movement, generalSettings.BasePlayerTraits.Appearance, generalSettings.BasePlayerTraits.Sensors);
            b.Write((short)weaponsAndVehicles.WeaponsOnMap);
            b.Write((short)weaponsAndVehicles.VehicleSet);

            WriteTraits(ref b, weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth, weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage, weaponsAndVehicles.OvershieldPowerupTraits.Movement, weaponsAndVehicles.OvershieldPowerupTraits.Appearance, weaponsAndVehicles.OvershieldPowerupTraits.Sensors);
            WriteTraits(ref b, weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth, weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage, weaponsAndVehicles.ActiveCamoPowerupTraits.Movement, weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance, weaponsAndVehicles.ActiveCamoPowerupTraits.Sensors);
            WriteTraits(ref b, weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth, weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage, weaponsAndVehicles.CustomPowerupTraits.Movement, weaponsAndVehicles.CustomPowerupTraits.Appearance, weaponsAndVehicles.CustomPowerupTraits.Sensors);
            b.Write((byte)weaponsAndVehicles.OvershieldPowerupTraits.Duration);
            b.Write((byte)weaponsAndVehicles.ActiveCamoPowerupTraits.Duration);
            b.Write((byte)weaponsAndVehicles.CustomPowerupTraits.Duration);
            b.Write(new byte[] { 0x00, 0x00, 0x00 }); // unk, Is Dirty, unk

            switch (gameTypeMenu)
            {
                case GameType.UNK:
                    break;
                case GameType.CTF:
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((byte)variantSettings.CTFSettings.FlagAtHomeToScore);
                    b.Write((byte)variantSettings.CTFSettings.FlagWaypoint);
                    b.Write((byte)variantSettings.CTFSettings.NumberOfFlags);
                    b.Write((byte)variantSettings.CTFSettings.RespawnOnCapture);
                    b.Write((short)variantSettings.CTFSettings.FlagReturnTime);
                    b.Write((short)variantSettings.CTFSettings.SuddenDeath);
                    b.Write((byte)variantSettings.CTFSettings.CapturesToWin);
                    b.Write(new byte[] { 0x00, 0x00, 0x00 });
                    b.Write((short)variantSettings.CTFSettings.FlagResetTime);

                    // Flag Carrier Traits
                    WriteTraits(ref b, variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth, variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage, variantSettings.CTFSettings.FlagCarrierTraits.Movement, variantSettings.CTFSettings.FlagCarrierTraits.Appearance, variantSettings.CTFSettings.FlagCarrierTraits.Sensors);
                    break;
                case GameType.Slayer:
                    b.Write((short)variantSettings.SlayerSettings.TeamScoring);
                    b.Write((short)variantSettings.SlayerSettings.ScoreToWin);
                    b.Write(new byte[] { 0x16, 0x00 });
                    b.Write((short)variantSettings.SlayerSettings.KillPoints);
                    b.Write((byte)variantSettings.SlayerSettings.AssistPoints);
                    b.Write((byte)variantSettings.SlayerSettings.DeathPoints);
                    b.Write((byte)variantSettings.SlayerSettings.SuicidePoints);
                    b.Write((byte)variantSettings.SlayerSettings.BetrayalPoints);
                    b.Write((byte)variantSettings.SlayerSettings.LeaderKillBonus);
                    b.Write((byte)variantSettings.SlayerSettings.EliminationBonus);
                    b.Write((byte)variantSettings.SlayerSettings.AssassinationBonus);
                    b.Write((byte)variantSettings.SlayerSettings.HeadshotBonus);
                    b.Write((byte)variantSettings.SlayerSettings.BeatdownBonus);
                    b.Write((byte)variantSettings.SlayerSettings.StickyBonus);
                    b.Write((byte)variantSettings.SlayerSettings.SplatterBonus);
                    b.Write((byte)variantSettings.SlayerSettings.SpreeBonus);

                    // Leader Traits
                    WriteTraits(ref b, variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth, variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage, variantSettings.SlayerSettings.LeaderTraits.Movement, variantSettings.SlayerSettings.LeaderTraits.Appearance, variantSettings.SlayerSettings.LeaderTraits.Sensors);
                    break;
                case GameType.Oddball:
                    b.Write((short)variantSettings.OddballSettings.TeamScoring);
                    b.Write((int)variantSettings.OddballSettings.AutoPickup);
                    b.Write((short)variantSettings.OddballSettings.ScoreToWin);
                    b.Write(new byte[] { 0x2D, 0x00 }); // Appears to always be 0x2D 0x00 - Early Win Count?
                    b.Write((short)variantSettings.OddballSettings.CarryingPoints);
                    b.Write((byte)variantSettings.OddballSettings.KillPoints);
                    b.Write((byte)variantSettings.OddballSettings.BallKillPoints);
                    b.Write((byte)variantSettings.OddballSettings.BallCarrierKillPoints);
                    b.Write((byte)variantSettings.OddballSettings.BallCount);
                    b.Write((short)variantSettings.OddballSettings.InitialBallDelay);
                    b.Write((short)variantSettings.OddballSettings.BallRespawnDelay);

                    // Ball Carrier Traits
                    WriteTraits(ref b, variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth, variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage, variantSettings.OddballSettings.BallCarrierTraits.Movement, variantSettings.OddballSettings.BallCarrierTraits.Appearance, variantSettings.OddballSettings.BallCarrierTraits.Sensors);
                    break;
                case GameType.KOTH:
                    b.Write((short)variantSettings.KOTHSettings.TeamScoring);
                    b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                    b.Write((short)variantSettings.KOTHSettings.ScoreToWin);
                    b.Write(new byte[] { 0x5A, 0x00 }); // Appears to always be 0x5A 0x00 - Early Win Count?
                    b.Write((short)((short)variantSettings.KOTHSettings.HillMovement + (short)variantSettings.KOTHSettings.HillMovementOrder));
                    b.Write((byte)variantSettings.KOTHSettings.OnHillPoints);
                    b.Write((byte)variantSettings.KOTHSettings.OffHillPoints);
                    b.Write((byte)variantSettings.KOTHSettings.UncontestedControlPoints);
                    b.Write((byte)variantSettings.KOTHSettings.KillPoints);

                    // On Hill Traits
                    WriteTraits(ref b, variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth, variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage, variantSettings.KOTHSettings.OnHillTraits.Movement, variantSettings.KOTHSettings.OnHillTraits.Appearance, variantSettings.KOTHSettings.OnHillTraits.Sensors);
                    break;
                case GameType.Forge:
                    // This could be supported but there does not appear to currently be a way to load a pre-defined game type variant in Forge
                    // so there is no real need to support this
                    break;
                case GameType.VIP:
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((short)variantSettings.VIPSettings.ScoreToWin);
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((short)((short)variantSettings.VIPSettings.SingleVIP + (short)variantSettings.VIPSettings.VIPDeathEndsRound + (short)variantSettings.VIPSettings.GoalAreas));
                    b.Write((byte)variantSettings.VIPSettings.KillPoints);
                    b.Write((byte)variantSettings.VIPSettings.VIPTakedownPoints);
                    b.Write((byte)variantSettings.VIPSettings.KillAsVIPPoints);
                    b.Write((byte)variantSettings.VIPSettings.VIPDeathEndsRound);
                    b.Write((byte)variantSettings.VIPSettings.GoalArrivalPoints);
                    b.Write((byte)variantSettings.VIPSettings.SuicidePoints);
                    b.Write((byte)variantSettings.VIPSettings.BetrayalPoints);
                    b.Write((byte)variantSettings.VIPSettings.VIPBetrayalPoints);
                    b.Write((byte)variantSettings.VIPSettings.NextVIP);
                    b.Write((byte)variantSettings.VIPSettings.GoalMovement);
                    b.Write((byte)variantSettings.VIPSettings.GoalMovementOrder);
                    b.Write((byte)0x00);
                    b.Write((short)variantSettings.VIPSettings.InfluenceRadius);

                    // VIP Team Traits
                    WriteTraits(ref b, variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth, variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage, variantSettings.VIPSettings.VIPTeamTraits.Movement, variantSettings.VIPSettings.VIPTeamTraits.Appearance, variantSettings.VIPSettings.VIPTeamTraits.Sensors);

                    // VIP Proximity Traits
                    WriteTraits(ref b, variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth, variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage, variantSettings.VIPSettings.VIPProximityTraits.Movement, variantSettings.VIPSettings.VIPProximityTraits.Appearance, variantSettings.VIPSettings.VIPProximityTraits.Sensors);

                    // VIP Traits
                    WriteTraits(ref b, variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth, variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage, variantSettings.VIPSettings.VIPTraits.Movement, variantSettings.VIPSettings.VIPTraits.Appearance, variantSettings.VIPSettings.VIPTraits.Sensors);
                    break;
                case GameType.Juggernaut:
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((short)variantSettings.JuggernautSettings.ScoreToWin);
                    b.Write(new byte[] { 0x08, 0x00 }); // Don't know what this is
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((byte)variantSettings.JuggernautSettings.FirstJuggernaut);
                    b.Write((byte)variantSettings.JuggernautSettings.NextJuggernaut);
                    b.Write((byte)((byte)variantSettings.JuggernautSettings.AlliedAgainstJuggernaut + (byte)variantSettings.JuggernautSettings.GoalZones + (byte)variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut));
                    b.Write((byte)variantSettings.JuggernautSettings.GoalZoneMovement);
                    b.Write((byte)variantSettings.JuggernautSettings.GoalZoneOrder);
                    b.Write((byte)variantSettings.JuggernautSettings.KillPoints);
                    b.Write((byte)variantSettings.JuggernautSettings.TakedownPoints);
                    b.Write((byte)variantSettings.JuggernautSettings.KillAsJuggernautPoints);
                    b.Write((byte)variantSettings.JuggernautSettings.GoalArrivalPoints);
                    b.Write((byte)variantSettings.JuggernautSettings.SuicidePoints);
                    b.Write((byte)variantSettings.JuggernautSettings.BetrayalPoints);
                    b.Write((byte)variantSettings.JuggernautSettings.NextJuggernautDelay);

                    // Juggernaut Traits
                    WriteTraits(ref b, variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth, variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage, variantSettings.JuggernautSettings.JuggernautTraits.Movement, variantSettings.JuggernautSettings.JuggernautTraits.Appearance, variantSettings.JuggernautSettings.JuggernautTraits.Sensors);
                    break;
                case GameType.Territories:
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((short)((short)variantSettings.TerritoriesSettings.OneSidedTerritories + (short)variantSettings.TerritoriesSettings.LockAfterCapture));
                    b.Write((short)variantSettings.TerritoriesSettings.RespawnOnCapture);
                    b.Write((short)variantSettings.TerritoriesSettings.TerritoryCaptureTime);
                    b.Write((short)variantSettings.TerritoriesSettings.SuddenDeath);

                    // Defender Traits
                    WriteTraits(ref b, variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth, variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage, variantSettings.TerritoriesSettings.DefenderTraits.Movement, variantSettings.TerritoriesSettings.DefenderTraits.Appearance, variantSettings.TerritoriesSettings.DefenderTraits.Sensors);

                    // Attacker Traits
                    WriteTraits(ref b, variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth, variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage, variantSettings.TerritoriesSettings.AttackerTraits.Movement, variantSettings.TerritoriesSettings.AttackerTraits.Appearance, variantSettings.TerritoriesSettings.AttackerTraits.Sensors);
                    break;
                case GameType.Assault:
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((int)variantSettings.AssaultSettings.ResetOnDisarm);
                    b.Write((short)variantSettings.AssaultSettings.AssaultBombMode);
                    b.Write(new byte[] { 0x01, 0x00 });
                    b.Write((short)variantSettings.AssaultSettings.DetonationsToWin);
                    b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                    b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                    b.Write((short)variantSettings.AssaultSettings.SuddenDeath);
                    b.Write((short)variantSettings.AssaultSettings.BombResetTime);
                    b.Write((short)variantSettings.AssaultSettings.BombArmingTime);
                    b.Write((short)variantSettings.AssaultSettings.BombDisarmingTime);
                    b.Write((short)variantSettings.AssaultSettings.BombFuseTime);

                    // Bomb Carrier Traits
                    WriteTraits(ref b, variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth, variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage, variantSettings.AssaultSettings.BombCarrierTraits.Movement, variantSettings.AssaultSettings.BombCarrierTraits.Appearance, variantSettings.AssaultSettings.BombCarrierTraits.Sensors);

                    // Secondary Traits
                    WriteTraits(ref b, variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth, variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage, variantSettings.AssaultSettings.SecondaryTraits.Movement, variantSettings.AssaultSettings.SecondaryTraits.Appearance, variantSettings.AssaultSettings.SecondaryTraits.Sensors);
                    break;
                case GameType.Infection:
                    b.Write(new byte[] { 0x00, 0x00 });
                    b.Write((byte)variantSettings.InfectionSettings.RespawnOnHavenMove);
                    b.Write((byte)variantSettings.InfectionSettings.SafeHavenMovement);
                    b.Write((byte)variantSettings.InfectionSettings.NextZombie);
                    b.Write((byte)variantSettings.InfectionSettings.InitialZombieCount);
                    b.Write((byte)variantSettings.InfectionSettings.SafeHavenMovementTime);
                    b.Write((byte)0x00);
                    b.Write((byte)variantSettings.InfectionSettings.ZombieKillPoints);
                    b.Write((byte)variantSettings.InfectionSettings.InfectionPoints);
                    b.Write((byte)variantSettings.InfectionSettings.SafeHavenArrivalPoints);
                    b.Write((byte)variantSettings.InfectionSettings.SuicidePoints);
                    b.Write((byte)variantSettings.InfectionSettings.BetrayalPoints);
                    b.Write((byte)variantSettings.InfectionSettings.LastManStandingBonus);

                    // Zombie Traits
                    WriteTraits(ref b, variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth, variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage, variantSettings.InfectionSettings.ZombieTraits.Movement, variantSettings.InfectionSettings.ZombieTraits.Appearance, variantSettings.InfectionSettings.ZombieTraits.Sensors);

                    // Alpha Zombie Traits
                    WriteTraits(ref b, variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth, variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage, variantSettings.InfectionSettings.AlphaZombieTraits.Movement, variantSettings.InfectionSettings.AlphaZombieTraits.Appearance, variantSettings.InfectionSettings.AlphaZombieTraits.Sensors);

                    // Safe Haven Traits
                    WriteTraits(ref b, variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth, variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage, variantSettings.InfectionSettings.SafeHavenTraits.Movement, variantSettings.InfectionSettings.SafeHavenTraits.Appearance, variantSettings.InfectionSettings.SafeHavenTraits.Sensors);

                    // Last Man Traits
                    WriteTraits(ref b, variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth, variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage, variantSettings.InfectionSettings.LastManTraits.Movement, variantSettings.InfectionSettings.LastManTraits.Appearance, variantSettings.InfectionSettings.LastManTraits.Sensors);
                    break;
                default:
                    break;
            }

            // Write NULLs to EOF marker
            for (long i = b.BaseStream.Position; i < fileSize - 20; i++)
            {
                b.Write((byte)0x00);
            }

            // Write EOF Block
            b.Write("_eof".ToCharArray());

            // These are null or flagged and appear to be debug/checksum flags
            // Setting them to the values they appear to be most often
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x11 }); // Size
            b.Write(new byte[] { 0x00, 0x01, 0x00, 0x01 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            b.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            // Write NULLs to true EOF
            for (long i = b.BaseStream.Position; i < 4096; i++)
            {
                b.Write((byte)0x00);
            }
            return true;
        }

        /// <summary>
        /// Only write the few variables that were requested to be supported in an Update only mode.
        /// </summary>
        /// <param name="b">BinaryWriter object to write the updates to.</param>
        /// <param name="bIsMap">Is the file being written to a MapVariant or a GameTypeVariant.</param>
        /// <returns></returns>
        public bool WriteUpdate(BinaryWriter b, bool bIsMap)
        {
            // Try to write all the variables, but if for some unexpected reason we can't
            // return false to let the caller cleanly handle it
            try
            {
                b.Seek(72, SeekOrigin.Begin);
                b.Write(StringNullEscape(variantNameMenu, 15).ToCharArray());
                b.Seek(2, SeekOrigin.Current);
                b.Write(StringPadNulls(descriptionMenu, 126).ToCharArray());
                b.Seek(2, SeekOrigin.Current);
                b.Write(StringPadNulls(currentAuthorName, 16).ToCharArray());
                b.Seek(8, SeekOrigin.Current);
                b.Write(currentAuthorXUID);

                // Map Variants do not have a metadata block so it ends early
                if (bIsMap)
                    b.Seek(336, SeekOrigin.Begin);
                else
                    b.Seek(376, SeekOrigin.Begin);

                b.Write(StringNullEscape(variantNameGame, 15).ToCharArray());
                b.Seek(2, SeekOrigin.Current);
                b.Write(StringPadNulls(descriptionGame, 126).ToCharArray());
                b.Seek(2, SeekOrigin.Current);
                b.Write(StringPadNulls(originalAuthorName, 16).ToCharArray());
                b.Seek(8, SeekOrigin.Current);
                b.Write(originalAuthorXUID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Adds a null character after every character of the Input string.
        /// </summary>
        /// <param name="Input">String to have Nulls added to.</param>
        /// <param name="TargetLength">How many characters should the non-Nulled string contain</param>
        /// <returns>String with Nulls after each character.</returns>
        public string StringNullEscape(string Input, int TargetLength = -1)
        {
            string Output = Input;
            if (TargetLength != -1)
            {
                Output = StringPadNulls(Input, TargetLength);
            }
            Output = Output.Aggregate(string.Empty, (c, i) => c + i + '\0');
            return Output;
        }

        public string StringPadNulls(string Input, int TargetLength)
        {
            string Output = Input;
            if (Input.Length != TargetLength)
            {
                for (int i = Output.Length; i < TargetLength; i++)
                {
                    Output = Output + '\0';
                }
            }
            return Output;
        }

        public byte[] BitConverterStringToByteArray(string Input)
        {
            string Escaped = System.Text.RegularExpressions.Regex.Replace(Input, ".{2}", "$0-").TrimEnd('-');
            return Escaped.Split('-').Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
        }

        public GameTypeOrder ConvertGameTypeToGameTypeOrder(GameType gameType)
        {
            switch(gameType)
            {
                case GameType.CTF:
                    return GameTypeOrder.CTF;
                case GameType.Slayer:
                    return GameTypeOrder.Slayer;
                case GameType.Oddball:
                    return GameTypeOrder.Oddball;
                case GameType.KOTH:
                    return GameTypeOrder.KOTH;
                case GameType.Forge:
                    return GameTypeOrder.Forge;
                case GameType.VIP:
                    return GameTypeOrder.VIP;
                case GameType.Juggernaut:
                    return GameTypeOrder.Juggernaut;
                case GameType.Territories:
                    return GameTypeOrder.Territories;
                case GameType.Assault:
                    return GameTypeOrder.Assault;
                case GameType.Infection:
                    return GameTypeOrder.Infection;
                default:
                    return GameTypeOrder.UNK;
            }
        }

        private void ParseTraits(ref BinaryReader b, ref ShieldsAndHealth shieldsAndHealth, ref WeaponsAndDamage weaponsAndDamage, ref Movement movement, ref Appearance appearance, ref Sensors sensors)
        {
            shieldsAndHealth.DamageResistance = (DamageResistance)b.ReadByte();
            shieldsAndHealth.ShieldRechargeRate = (ShieldRechargeRate)b.ReadByte();
            shieldsAndHealth.ShieldVampirism = (ShieldVampirism)b.ReadByte();
            shieldsAndHealth.ImmuneToHeadshots = (ToggleBoolean)b.ReadByte();
            shieldsAndHealth.ShieldMultiplier = (ShieldMultiplier)b.ReadByte();

            b.ReadBytes(3); // unk0

            weaponsAndDamage.GrenadeCount = (GrenadeCount)b.ReadByte();
            b.ReadByte(); // unk1

            weaponsAndDamage.PrimaryWeapon = (Weapon)b.ReadByte();
            weaponsAndDamage.SecondaryWeapon = (Weapon)b.ReadByte();
            weaponsAndDamage.DamageModifier = (DamageModifier)b.ReadByte();
            weaponsAndDamage.GrenadeRegen = (ToggleBoolean)b.ReadByte();
            weaponsAndDamage.InfiniteAmmo = (InfiniteAmmo)b.ReadByte();
            weaponsAndDamage.WeaponPickup = (ToggleBoolean)b.ReadByte();

            movement.PlayerSpeed = (PlayerSpeed)b.ReadByte();
            movement.PlayerGravity = (PlayerGravity)b.ReadByte();
            movement.VehicleUse = (VehicleUse)b.ReadByte();

            b.ReadByte(); // unk2

            appearance.ActiveCamo = (ActiveCamo)b.ReadByte();
            appearance.Waypoint = (Waypoint)b.ReadByte();
            appearance.PlayerSize = (PlayerSize)b.ReadByte();
            if ((int)appearance.PlayerSize == 1)
                appearance.PlayerSize = 0;
            appearance.ForcedColor = (ForcedColor)b.ReadByte();

            sensors.MotionTrackerMode = (MotionTrackerMode)b.ReadInt16();
            sensors.MotionTrackerRange = (MotionTrackerRange)b.ReadInt16();

            return;
        }

        private void WriteTraits(ref BinaryWriter b, ShieldsAndHealth shieldsAndHealth, WeaponsAndDamage weaponsAndDamage, Movement movement, Appearance appearance, Sensors sensors)
        {
            b.Write((byte)shieldsAndHealth.DamageResistance);
            b.Write((byte)shieldsAndHealth.ShieldRechargeRate);
            b.Write((byte)shieldsAndHealth.ShieldVampirism);
            b.Write((byte)shieldsAndHealth.ImmuneToHeadshots);
            b.Write((byte)shieldsAndHealth.ShieldMultiplier);
            b.Write(new byte[] { 0x00, 0x00, 0x00 }); // unk0
            b.Write((byte)weaponsAndDamage.GrenadeCount);
            b.Write((byte)0x00); // unk1
            b.Write((byte)weaponsAndDamage.PrimaryWeapon);
            b.Write((byte)weaponsAndDamage.SecondaryWeapon);
            b.Write((byte)weaponsAndDamage.DamageModifier);
            b.Write((byte)weaponsAndDamage.GrenadeRegen);
            b.Write((byte)weaponsAndDamage.InfiniteAmmo);
            b.Write((byte)weaponsAndDamage.WeaponPickup);
            b.Write((byte)movement.PlayerSpeed);
            b.Write((byte)movement.PlayerGravity);
            b.Write((byte)movement.VehicleUse);
            b.Write((byte)0x00); // unk2
            b.Write((byte)appearance.ActiveCamo);
            b.Write((byte)appearance.Waypoint);
            b.Write((byte)appearance.PlayerSize);
            b.Write((byte)appearance.ForcedColor);
            b.Write((short)sensors.MotionTrackerMode);
            b.Write((short)sensors.MotionTrackerRange);

            return;
        }
    }
}