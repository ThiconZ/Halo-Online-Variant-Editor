using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Halo_Online_Variant_Editor
{
    /// <summary>
    /// Interaction logic for Variant_Editor.xaml
    /// </summary>
    public partial class Variant_Editor : Window
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        Variant CachedVariant = new Variant();
        bool MiniUpdater = false;
        bool QueueSaveAbort = false;
        bool Verbose = false;

        public Variant_Editor()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Application.Current.Properties["MiniUpdater"] != null)
            {
                MiniUpdater = true;
            }

            if (System.Windows.Application.Current.Properties["Verbose"] != null)
            {
                Verbose = true;
            }

            if (MiniUpdater)
            {
                SaveVariantBtn.Click -= SaveVariantBtn_Click;
                SaveVariantBtn.Click += new RoutedEventHandler(SaveVariant_WriteUpdate);
                SaveVariantBtn.Content = "Update Variant";
            }

            if (System.Windows.Application.Current.Properties["OpenFilePath"] != null)
            {
                FilePathTB.Text = System.Windows.Application.Current.Properties["OpenFilePath"].ToString();

                LoadVariantFile();
            }
            else
            {
                if (MiniUpdater == false && System.Windows.Application.Current.Properties["NeverNewVariant"] == null && System.Windows.Forms.MessageBox.Show("Do you want to create a new variant?", "Create New Variant", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    New_Variant NewVariantWindow = new New_Variant();
                    NewVariantWindow.Owner = this;
                    NewVariantWindow.ShowDialog();
                }
            }
        }

        public void LoadVariantFile()
        {
            Variant v = new Variant();
            try
            {
                using (BinaryReader b = new BinaryReader(File.Open(FilePathTB.Text, FileMode.Open)))
                {
                    if (v.Load(b) == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Error Loading Variant", "Error Loading Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("File is unable to be read and/or not a valid Game Variant." + Environment.NewLine + ex.GetType().ToString() + ": " + ex.Message, "Error Loading Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadVariant(v);
        }

        public void LoadVariant(Variant v)
        {
            variantNameMenuTB.Text = v.variantNameMenu;
            variantNameGameTB.Text = v.variantNameGame;
            descriptionMenuTB.Text = v.descriptionMenu;
            descriptionGameTB.Text = v.descriptionGame;
            currentAuthorNameTB.Text = v.currentAuthorName;
            currentAuthorUIDTB.Text = BitConverter.ToString(v.currentAuthorXUID).Replace("-", "");
            originalAuthorNameTB.Text = v.originalAuthorName;
            originalAuthorUIDTB.Text = BitConverter.ToString(v.originalAuthorXUID).Replace("-", "");
            fileCreatedTimestampDTP.Text = FromUnixTime(v.fileCreatedTimestamp).ToString("dddd MMMM d, yyyy HH:mm:ss");
            originalFileCreatedTimestampDTP.Text = FromUnixTime(v.originalFileCreatedTimestamp).ToString("dddd MMMM d, yyyy HH:mm:ss");
            MetadataTB.Text = v.metadata;

            // Updater mode doesn't write any other variables so no point in loading them - perhaps this can be toggled with a launch parameter for 'advanced' users though
            if (MiniUpdater)
            {
                CachedVariant = new Variant();
                CachedVariant = v;
                return;
            }

            // We're allowing basic support for Map Variants based on map flag instead of size
            if (v.variantType == "mapv")
                return;
            // Backup check for mapv based on file size
            if (v.fileSize == 57840)
                return;

            LoadOptions<GameType>(gameTypeMenuCB, v.gameTypeMenu);
            LoadOptions<GameType>(gameTypeGameCB, v.gameTypeGame);
            LoadOptions<NumberOfRounds>(generalSettings_NumberOfRounds, v.generalSettings.NumberOfRounds);
            LoadOptions<TimeLimit>(generalSettings_TimeLimit, v.generalSettings.TimeLimit);
            LoadOptions<RoundsReset>(generalSettings_RoundsReset, v.generalSettings.RoundsReset);

            // GameTypeGame variable will be used to pick what the true game type should be
            SwitchVariantTab(v.gameTypeGame);

            LoadOptions<TeamChanging>(generalSettings_TeamChanging, v.generalSettings.TeamChanging);
            LoadOptions<FriendlyFire>(generalSettings_FriendlyFire, v.generalSettings.FriendlyFire);
            LoadOptions<BetrayalBooting>(generalSettings_BetrayalBooting, v.generalSettings.BetrayalBooting);
            LoadOptions<Spectating>(generalSettings_Spectating, v.generalSettings.Spectating);
            LoadOptions<DamageResistance>(generalSettings_BasePlayerTraits_ShieldsAndHealth_DamageResistance, v.generalSettings.BasePlayerTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ShieldMultiplier, v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ShieldRechargeRate, v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ShieldVampirism, v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ImmuneToHeadshots, v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(generalSettings_BasePlayerTraits_WeaponsAndDamage_DamageModifier, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(generalSettings_BasePlayerTraits_WeaponsAndDamage_PrimaryWeapon, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(generalSettings_BasePlayerTraits_WeaponsAndDamage_SecondaryWeapon, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(generalSettings_BasePlayerTraits_WeaponsAndDamage_GrenadeCount, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(generalSettings_BasePlayerTraits_WeaponsAndDamage_GrenadeRegen, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(generalSettings_BasePlayerTraits_WeaponsAndDamage_InfiniteAmmo, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(generalSettings_BasePlayerTraits_WeaponsAndDamage_WeaponPickup, v.generalSettings.BasePlayerTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(generalSettings_BasePlayerTraits_Movement_PlayerSpeed, v.generalSettings.BasePlayerTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(generalSettings_BasePlayerTraits_Movement_PlayerGravity, v.generalSettings.BasePlayerTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(generalSettings_BasePlayerTraits_Movement_VehicleUse, v.generalSettings.BasePlayerTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(generalSettings_BasePlayerTraits_Sensors_MotionTrackerMode, v.generalSettings.BasePlayerTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(generalSettings_BasePlayerTraits_Sensors_MotionTrackerRange, v.generalSettings.BasePlayerTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(generalSettings_BasePlayerTraits_Appearance_ActiveCamo, v.generalSettings.BasePlayerTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(generalSettings_BasePlayerTraits_Appearance_Waypoint, v.generalSettings.BasePlayerTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(generalSettings_BasePlayerTraits_Appearance_PlayerSize, v.generalSettings.BasePlayerTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(generalSettings_BasePlayerTraits_Appearance_ForcedColor, v.generalSettings.BasePlayerTraits.Appearance.ForcedColor);
            LoadOptions<RespawnTime>(generalSettings_RespawnSettings_RespawnTimeServer, v.generalSettings.RespawnSettings.RespawnTimeServer);
            LoadOptions<RespawnTime>(generalSettings_RespawnSettings_RespawnTimeMenu, v.generalSettings.RespawnSettings.RespawnTimeMenu);
            LoadOptions<SuicidePenalty>(generalSettings_RespawnSettings_SuicidePenalty, v.generalSettings.RespawnSettings.SuicidePenalty);
            LoadOptions<BetrayalPenalty>(generalSettings_RespawnSettings_BetrayalPenalty, v.generalSettings.RespawnSettings.BetrayalPenalty);
            LoadOptions<LivesPerRound>(generalSettings_RespawnSettings_LivesPerRound, v.generalSettings.RespawnSettings.LivesPerRound);
            LoadOptions<SynchronizeWithTeam>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnModifiers_SynchronizeWithTeam, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam);
            LoadOptions<RespawnTimeGrowth>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnModifiers_RespawnTimeGrowth, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnTimeGrowth);
            LoadOptions<RespawnOnKills>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnModifiers_RespawnOnKills, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills);
            LoadOptions<Duration>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Duration, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Duration);
            LoadOptions<DamageResistance>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_DamageResistance, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ShieldMultiplier, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ShieldRechargeRate, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ShieldVampirism, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ImmuneToHeadshots, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_DamageModifier, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_GrenadeRegen, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_InfiniteAmmo, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_WeaponPickup, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Movement_PlayerSpeed, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Movement_PlayerGravity, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Movement_VehicleUse, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Sensors_MotionTrackerMode, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Sensors_MotionTrackerRange, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_ActiveCamo, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_Waypoint, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_PlayerSize, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_ForcedColor, v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.ForcedColor);
            LoadOptions<GrenadesOnMap>(weaponsAndVehicles_GrenadesOnMap, v.weaponsAndVehicles.GrenadesOnMap);
            LoadOptions<WeaponsOnMap>(weaponsAndVehicles_WeaponsOnMap, v.weaponsAndVehicles.WeaponsOnMap);
            LoadOptions<VehicleSet>(weaponsAndVehicles_VehicleSet, v.weaponsAndVehicles.VehicleSet);
            LoadOptions<IndestructibleVehicles>(weaponsAndVehicles_IndestructibleVehicles, v.weaponsAndVehicles.IndestructibleVehicles);

            // Custom Powerup Traits
            LoadOptions<Duration>(weaponsAndVehicles_CustomPowerupTraits_Duration, v.weaponsAndVehicles.CustomPowerupTraits.Duration);
            LoadOptions<DamageResistance>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_DamageResistance, v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ShieldMultiplier, v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ShieldRechargeRate, v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ShieldVampirism, v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ImmuneToHeadshots, v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_DamageModifier, v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_GrenadeRegen, v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_InfiniteAmmo, v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_WeaponPickup, v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(weaponsAndVehicles_CustomPowerupTraits_Movement_PlayerSpeed, v.weaponsAndVehicles.CustomPowerupTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(weaponsAndVehicles_CustomPowerupTraits_Movement_PlayerGravity, v.weaponsAndVehicles.CustomPowerupTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(weaponsAndVehicles_CustomPowerupTraits_Movement_VehicleUse, v.weaponsAndVehicles.CustomPowerupTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(weaponsAndVehicles_CustomPowerupTraits_Sensors_MotionTrackerMode, v.weaponsAndVehicles.CustomPowerupTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(weaponsAndVehicles_CustomPowerupTraits_Sensors_MotionTrackerRange, v.weaponsAndVehicles.CustomPowerupTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(weaponsAndVehicles_CustomPowerupTraits_Appearance_ActiveCamo, v.weaponsAndVehicles.CustomPowerupTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(weaponsAndVehicles_CustomPowerupTraits_Appearance_Waypoint, v.weaponsAndVehicles.CustomPowerupTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(weaponsAndVehicles_CustomPowerupTraits_Appearance_PlayerSize, v.weaponsAndVehicles.CustomPowerupTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(weaponsAndVehicles_CustomPowerupTraits_Appearance_ForcedColor, v.weaponsAndVehicles.CustomPowerupTraits.Appearance.ForcedColor);

            // Overshield Powerup Traits
            LoadOptions<Duration>(weaponsAndVehicles_OvershieldPowerupTraits_Duration, v.weaponsAndVehicles.OvershieldPowerupTraits.Duration);
            LoadOptions<DamageResistance>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_DamageResistance, v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ShieldMultiplier, v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ShieldRechargeRate, v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ShieldVampirism, v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ImmuneToHeadshots, v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_DamageModifier, v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_GrenadeRegen, v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_InfiniteAmmo, v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_WeaponPickup, v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(weaponsAndVehicles_OvershieldPowerupTraits_Movement_PlayerSpeed, v.weaponsAndVehicles.OvershieldPowerupTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(weaponsAndVehicles_OvershieldPowerupTraits_Movement_PlayerGravity, v.weaponsAndVehicles.OvershieldPowerupTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(weaponsAndVehicles_OvershieldPowerupTraits_Movement_VehicleUse, v.weaponsAndVehicles.OvershieldPowerupTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(weaponsAndVehicles_OvershieldPowerupTraits_Sensors_MotionTrackerMode, v.weaponsAndVehicles.OvershieldPowerupTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(weaponsAndVehicles_OvershieldPowerupTraits_Sensors_MotionTrackerRange, v.weaponsAndVehicles.OvershieldPowerupTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_ActiveCamo, v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_Waypoint, v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_PlayerSize, v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_ForcedColor, v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.ForcedColor);

            // Active Camo Powerup Traits
            LoadOptions<Duration>(weaponsAndVehicles_ActiveCamoPowerupTraits_Duration, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Duration);
            LoadOptions<DamageResistance>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_DamageResistance, v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ShieldMultiplier, v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ShieldRechargeRate, v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ShieldVampirism, v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ImmuneToHeadshots, v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_DamageModifier, v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_GrenadeRegen, v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_InfiniteAmmo, v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_WeaponPickup, v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(weaponsAndVehicles_ActiveCamoPowerupTraits_Movement_PlayerSpeed, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(weaponsAndVehicles_ActiveCamoPowerupTraits_Movement_PlayerGravity, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(weaponsAndVehicles_ActiveCamoPowerupTraits_Movement_VehicleUse, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(weaponsAndVehicles_ActiveCamoPowerupTraits_Sensors_MotionTrackerMode, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(weaponsAndVehicles_ActiveCamoPowerupTraits_Sensors_MotionTrackerRange, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_ActiveCamo, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_Waypoint, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_PlayerSize, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_ForcedColor, v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.ForcedColor);

            // Load in the settings for all supported modes
            // There is a bit of a performance impact from doing this instead of just the current mode
            // But this will give us full support for switching the current variant to any other mode without issues

            // -- CTF --
            LoadOptions<CapturesToWin>(variantSettings_CTFSettings_CapturesToWin, v.variantSettings.CTFSettings.CapturesToWin);
            LoadOptions<NumberOfFlags>(variantSettings_CTFSettings_NumberOfFlags, v.variantSettings.CTFSettings.NumberOfFlags);
            LoadOptions<SuddenDeath>(variantSettings_CTFSettings_SuddenDeath, v.variantSettings.CTFSettings.SuddenDeath);
            LoadOptions<Boolean>(variantSettings_CTFSettings_FlagAtHomeToScore, v.variantSettings.CTFSettings.FlagAtHomeToScore);
            LoadOptions<FlagReturnTime>(variantSettings_CTFSettings_FlagReturnTime, v.variantSettings.CTFSettings.FlagReturnTime);
            LoadOptions<FlagResetTime>(variantSettings_CTFSettings_FlagResetTime, v.variantSettings.CTFSettings.FlagResetTime);
            LoadOptions<FlagWaypoint>(variantSettings_CTFSettings_FlagWaypoint, v.variantSettings.CTFSettings.FlagWaypoint);
            LoadOptions<RespawnOnCaptureByte>(variantSettings_CTFSettings_RespawnOnCapture, v.variantSettings.CTFSettings.RespawnOnCapture);

            // Flag Carrier Traits
            LoadOptions<DamageResistance>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_CTFSettings_FlagCarrierTraits_Movement_PlayerSpeed, v.variantSettings.CTFSettings.FlagCarrierTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_CTFSettings_FlagCarrierTraits_Movement_PlayerGravity, v.variantSettings.CTFSettings.FlagCarrierTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_CTFSettings_FlagCarrierTraits_Movement_VehicleUse, v.variantSettings.CTFSettings.FlagCarrierTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_CTFSettings_FlagCarrierTraits_Sensors_MotionTrackerMode, v.variantSettings.CTFSettings.FlagCarrierTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_CTFSettings_FlagCarrierTraits_Sensors_MotionTrackerRange, v.variantSettings.CTFSettings.FlagCarrierTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_ActiveCamo, v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_Waypoint, v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_PlayerSize, v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_ForcedColor, v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.ForcedColor);

            // -- Slayer --
            LoadOptions<TeamMode>(generalSettings_TeamSlayer, v.generalSettings.TeamMode);

            LoadOptions<TeamScoring>(variantSettings_SlayerSettings_TeamScoring, v.variantSettings.SlayerSettings.TeamScoring);
            LoadOptions<ScoreToWin>(variantSettings_SlayerSettings_ScoreToWin, v.variantSettings.SlayerSettings.ScoreToWin);
            LoadOptions<ScoreOptionsPointsShort>(variantSettings_SlayerSettings_KillPoints, v.variantSettings.SlayerSettings.KillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_AssistPoints, v.variantSettings.SlayerSettings.AssistPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_DeathPoints, v.variantSettings.SlayerSettings.DeathPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_SuicidePoints, v.variantSettings.SlayerSettings.SuicidePoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_BetrayalPoints, v.variantSettings.SlayerSettings.BetrayalPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_LeaderKillBonus, v.variantSettings.SlayerSettings.LeaderKillBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_EliminationBonus, v.variantSettings.SlayerSettings.EliminationBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_AssassinationBonus, v.variantSettings.SlayerSettings.AssassinationBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_HeadshotBonus, v.variantSettings.SlayerSettings.HeadshotBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_BeatdownBonus, v.variantSettings.SlayerSettings.BeatdownBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_StickyBonus, v.variantSettings.SlayerSettings.StickyBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_SplatterBonus, v.variantSettings.SlayerSettings.SplatterBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_SpreeBonus, v.variantSettings.SlayerSettings.SpreeBonus);

            // Leader Traits
            LoadOptions<DamageResistance>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_SlayerSettings_LeaderTraits_Movement_PlayerSpeed, v.variantSettings.SlayerSettings.LeaderTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_SlayerSettings_LeaderTraits_Movement_PlayerGravity, v.variantSettings.SlayerSettings.LeaderTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_SlayerSettings_LeaderTraits_Movement_VehicleUse, v.variantSettings.SlayerSettings.LeaderTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_SlayerSettings_LeaderTraits_Sensors_MotionTrackerMode, v.variantSettings.SlayerSettings.LeaderTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_SlayerSettings_LeaderTraits_Sensors_MotionTrackerRange, v.variantSettings.SlayerSettings.LeaderTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_SlayerSettings_LeaderTraits_Appearance_ActiveCamo, v.variantSettings.SlayerSettings.LeaderTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_SlayerSettings_LeaderTraits_Appearance_Waypoint, v.variantSettings.SlayerSettings.LeaderTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_SlayerSettings_LeaderTraits_Appearance_PlayerSize, v.variantSettings.SlayerSettings.LeaderTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_SlayerSettings_LeaderTraits_Appearance_ForcedColor, v.variantSettings.SlayerSettings.LeaderTraits.Appearance.ForcedColor);

            // -- Oddball --
            LoadOptions<TeamMode>(generalSettings_TeamOddball, v.generalSettings.TeamMode);

            LoadOptions<ScoreToWin>(variantSettings_OddballSettings_ScoreToWin, v.variantSettings.OddballSettings.ScoreToWin);
            LoadOptions<TeamScoring>(variantSettings_OddballSettings_TeamScoring, v.variantSettings.OddballSettings.TeamScoring);
            LoadOptions<ScoreOptionsPointsShort>(variantSettings_OddballSettings_CarryingPoints, v.variantSettings.OddballSettings.CarryingPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_OddballSettings_KillPoints, v.variantSettings.OddballSettings.KillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_OddballSettings_BallKillPoints, v.variantSettings.OddballSettings.BallKillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_OddballSettings_BallCarrierKillPoints, v.variantSettings.OddballSettings.BallCarrierKillPoints);
            LoadOptions<BallCount>(variantSettings_OddballSettings_BallCount, v.variantSettings.OddballSettings.BallCount);
            LoadOptions<Boolean>(variantSettings_OddballSettings_AutoPickup, v.variantSettings.OddballSettings.AutoPickup);
            LoadOptions<InitialBallDelay>(variantSettings_OddballSettings_InitialBallDelay, v.variantSettings.OddballSettings.InitialBallDelay);
            LoadOptions<BallRespawnDelay>(variantSettings_OddballSettings_BallRespawnDelay, v.variantSettings.OddballSettings.BallRespawnDelay);

            // Ball Carrier Traits
            LoadOptions<DamageResistance>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_OddballSettings_BallCarrierTraits_Movement_PlayerSpeed, v.variantSettings.OddballSettings.BallCarrierTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_OddballSettings_BallCarrierTraits_Movement_PlayerGravity, v.variantSettings.OddballSettings.BallCarrierTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_OddballSettings_BallCarrierTraits_Movement_VehicleUse, v.variantSettings.OddballSettings.BallCarrierTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_OddballSettings_BallCarrierTraits_Sensors_MotionTrackerMode, v.variantSettings.OddballSettings.BallCarrierTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_OddballSettings_BallCarrierTraits_Sensors_MotionTrackerRange, v.variantSettings.OddballSettings.BallCarrierTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_ActiveCamo, v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_Waypoint, v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_PlayerSize, v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_ForcedColor, v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.ForcedColor);

            // -- King of the Hill --
            LoadOptions<TeamMode>(generalSettings_TeamKOTH, v.generalSettings.TeamMode);

            LoadOptions<ScoreToWin>(variantSettings_KOTHSettings_ScoreToWin, v.variantSettings.KOTHSettings.ScoreToWin);
            LoadOptions<TeamScoringKOTH>(variantSettings_KOTHSettings_TeamScoring, v.variantSettings.KOTHSettings.TeamScoring);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_OnHillPoints, v.variantSettings.KOTHSettings.OnHillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_UncontestedControlPoints, v.variantSettings.KOTHSettings.UncontestedControlPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_OffHillPoints, v.variantSettings.KOTHSettings.OffHillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_KillPoints, v.variantSettings.KOTHSettings.KillPoints);
            LoadOptions<HillMovement>(variantSettings_KOTHSettings_HillMovement, v.variantSettings.KOTHSettings.HillMovement);
            LoadOptions<HillMovementOrder>(variantSettings_KOTHSettings_HillMovementOrder, v.variantSettings.KOTHSettings.HillMovementOrder);

            // On Hill Traits
            LoadOptions<DamageResistance>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_KOTHSettings_OnHillTraits_Movement_PlayerSpeed, v.variantSettings.KOTHSettings.OnHillTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_KOTHSettings_OnHillTraits_Movement_PlayerGravity, v.variantSettings.KOTHSettings.OnHillTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_KOTHSettings_OnHillTraits_Movement_VehicleUse, v.variantSettings.KOTHSettings.OnHillTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_KOTHSettings_OnHillTraits_Sensors_MotionTrackerMode, v.variantSettings.KOTHSettings.OnHillTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_KOTHSettings_OnHillTraits_Sensors_MotionTrackerRange, v.variantSettings.KOTHSettings.OnHillTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_KOTHSettings_OnHillTraits_Appearance_ActiveCamo, v.variantSettings.KOTHSettings.OnHillTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_KOTHSettings_OnHillTraits_Appearance_Waypoint, v.variantSettings.KOTHSettings.OnHillTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_KOTHSettings_OnHillTraits_Appearance_PlayerSize, v.variantSettings.KOTHSettings.OnHillTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_KOTHSettings_OnHillTraits_Appearance_ForcedColor, v.variantSettings.KOTHSettings.OnHillTraits.Appearance.ForcedColor);

            // -- VIP --
            LoadOptions<ScoreToWin>(variantSettings_VIPSettings_ScoreToWin, v.variantSettings.VIPSettings.ScoreToWin);
            LoadOptions<SingleVIP>(variantSettings_VIPSettings_SingleVIP, v.variantSettings.VIPSettings.SingleVIP);
            LoadOptions<NextVIP>(variantSettings_VIPSettings_NextVIP, v.variantSettings.VIPSettings.NextVIP);
            LoadOptions<VIPDeathEndsRound>(variantSettings_VIPSettings_VIPDeathEndsRound, v.variantSettings.VIPSettings.VIPDeathEndsRound);
            LoadOptions<GoalAreas>(variantSettings_VIPSettings_GoalAreas, v.variantSettings.VIPSettings.GoalAreas);
            LoadOptions<GoalMovement>(variantSettings_VIPSettings_GoalMovement, v.variantSettings.VIPSettings.GoalMovement);
            LoadOptions<GoalZoneOrder>(variantSettings_VIPSettings_GoalMovementOrder, v.variantSettings.VIPSettings.GoalMovementOrder);
            LoadOptions<InfluenceRadius>(variantSettings_VIPSettings_InfluenceRadius, v.variantSettings.VIPSettings.InfluenceRadius);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_VIPTakedownPoints, v.variantSettings.VIPSettings.VIPTakedownPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_VIPDeathPoints, v.variantSettings.VIPSettings.VIPDeathPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_VIPBetrayalPoints, v.variantSettings.VIPSettings.VIPBetrayalPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_KillPoints, v.variantSettings.VIPSettings.KillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_KillAsVIPPoints, v.variantSettings.VIPSettings.KillAsVIPPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_GoalArrivalPoints, v.variantSettings.VIPSettings.GoalArrivalPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_SuicidePoints, v.variantSettings.VIPSettings.SuicidePoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_VIPSettings_BetrayalPoints, v.variantSettings.VIPSettings.BetrayalPoints);

            // VIP Traits
            LoadOptions<DamageResistance>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_VIPSettings_VIPTraits_Movement_PlayerSpeed, v.variantSettings.VIPSettings.VIPTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_VIPSettings_VIPTraits_Movement_PlayerGravity, v.variantSettings.VIPSettings.VIPTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_VIPSettings_VIPTraits_Movement_VehicleUse, v.variantSettings.VIPSettings.VIPTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_VIPSettings_VIPTraits_Sensors_MotionTrackerMode, v.variantSettings.VIPSettings.VIPTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_VIPSettings_VIPTraits_Sensors_MotionTrackerRange, v.variantSettings.VIPSettings.VIPTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_VIPSettings_VIPTraits_Appearance_ActiveCamo, v.variantSettings.VIPSettings.VIPTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_VIPSettings_VIPTraits_Appearance_Waypoint, v.variantSettings.VIPSettings.VIPTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_VIPSettings_VIPTraits_Appearance_PlayerSize, v.variantSettings.VIPSettings.VIPTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_VIPSettings_VIPTraits_Appearance_ForcedColor, v.variantSettings.VIPSettings.VIPTraits.Appearance.ForcedColor);

            // VIP Proximity Traits
            LoadOptions<DamageResistance>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_VIPSettings_VIPProximityTraits_Movement_PlayerSpeed, v.variantSettings.VIPSettings.VIPProximityTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_VIPSettings_VIPProximityTraits_Movement_PlayerGravity, v.variantSettings.VIPSettings.VIPProximityTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_VIPSettings_VIPProximityTraits_Movement_VehicleUse, v.variantSettings.VIPSettings.VIPProximityTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_VIPSettings_VIPProximityTraits_Sensors_MotionTrackerMode, v.variantSettings.VIPSettings.VIPProximityTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_VIPSettings_VIPProximityTraits_Sensors_MotionTrackerRange, v.variantSettings.VIPSettings.VIPProximityTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_ActiveCamo, v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_Waypoint, v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_PlayerSize, v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_ForcedColor, v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.ForcedColor);

            // VIP Team Traits
            LoadOptions<DamageResistance>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_VIPSettings_VIPTeamTraits_Movement_PlayerSpeed, v.variantSettings.VIPSettings.VIPTeamTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_VIPSettings_VIPTeamTraits_Movement_PlayerGravity, v.variantSettings.VIPSettings.VIPTeamTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_VIPSettings_VIPTeamTraits_Movement_VehicleUse, v.variantSettings.VIPSettings.VIPTeamTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_VIPSettings_VIPTeamTraits_Sensors_MotionTrackerMode, v.variantSettings.VIPSettings.VIPTeamTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_VIPSettings_VIPTeamTraits_Sensors_MotionTrackerRange, v.variantSettings.VIPSettings.VIPTeamTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_ActiveCamo, v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_Waypoint, v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_PlayerSize, v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_ForcedColor, v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.ForcedColor);

            // -- Juggernaut --
            LoadOptions<ScoreToWin>(variantSettings_JuggernautSettings_ScoreToWin, v.variantSettings.JuggernautSettings.ScoreToWin);
            LoadOptions<FirstJuggernaut>(variantSettings_JuggernautSettings_FirstJuggernaut, v.variantSettings.JuggernautSettings.FirstJuggernaut);
            LoadOptions<NextJuggernaut>(variantSettings_JuggernautSettings_NextJuggernaut, v.variantSettings.JuggernautSettings.NextJuggernaut);
            LoadOptions<NextJuggernautDelay>(variantSettings_JuggernautSettings_NextJuggernautDelay, v.variantSettings.JuggernautSettings.NextJuggernautDelay);
            LoadOptions<AlliedAgainstJuggernaut>(variantSettings_JuggernautSettings_AlliedAgainstJuggernaut, v.variantSettings.JuggernautSettings.AlliedAgainstJuggernaut);
            LoadOptions<GoalZones>(variantSettings_JuggernautSettings_GoalZones, v.variantSettings.JuggernautSettings.GoalZones);
            LoadOptions<GoalZoneMovement>(variantSettings_JuggernautSettings_GoalZoneMovement, v.variantSettings.JuggernautSettings.GoalZoneMovement);
            LoadOptions<GoalZoneOrder>(variantSettings_JuggernautSettings_GoalZoneOrder, v.variantSettings.JuggernautSettings.GoalZoneOrder);
            LoadOptions<RespawnOnLoneJuggernaut>(variantSettings_JuggernautSettings_RespawnOnLoneJuggernaut, v.variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_KillPoints, v.variantSettings.JuggernautSettings.KillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_TakedownPoints, v.variantSettings.JuggernautSettings.TakedownPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_KillAsJuggernautPoints, v.variantSettings.JuggernautSettings.KillAsJuggernautPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_GoalArrivalPoints, v.variantSettings.JuggernautSettings.GoalArrivalPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_SuicidePoints, v.variantSettings.JuggernautSettings.SuicidePoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_BetrayalPoints, v.variantSettings.JuggernautSettings.BetrayalPoints);

            // Juggernaut Traits
            LoadOptions<DamageResistance>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_JuggernautSettings_JuggernautTraits_Movement_PlayerSpeed, v.variantSettings.JuggernautSettings.JuggernautTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_JuggernautSettings_JuggernautTraits_Movement_PlayerGravity, v.variantSettings.JuggernautSettings.JuggernautTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_JuggernautSettings_JuggernautTraits_Movement_VehicleUse, v.variantSettings.JuggernautSettings.JuggernautTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_JuggernautSettings_JuggernautTraits_Sensors_MotionTrackerMode, v.variantSettings.JuggernautSettings.JuggernautTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_JuggernautSettings_JuggernautTraits_Sensors_MotionTrackerRange, v.variantSettings.JuggernautSettings.JuggernautTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_ActiveCamo, v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_Waypoint, v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_PlayerSize, v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_ForcedColor, v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.ForcedColor);

            // -- Territories --
            LoadOptions<TerritoryCaptureTime>(variantSettings_TerritoriesSettings_TerritoryCaptureTime, v.variantSettings.TerritoriesSettings.TerritoryCaptureTime);
            LoadOptions<OneSidedTerritories>(variantSettings_TerritoriesSettings_OneSidedTerritories, v.variantSettings.TerritoriesSettings.OneSidedTerritories);
            LoadOptions<LockAfterCapture>(variantSettings_TerritoriesSettings_LockAfterCapture, v.variantSettings.TerritoriesSettings.LockAfterCapture);
            LoadOptions<SuddenDeath>(variantSettings_TerritoriesSettings_SuddenDeath, v.variantSettings.TerritoriesSettings.SuddenDeath);
            LoadOptions<RespawnOnCapture>(variantSettings_TerritoriesSettings_RespawnOnCapture, v.variantSettings.TerritoriesSettings.RespawnOnCapture);

            // Defender Traits
            LoadOptions<DamageResistance>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_TerritoriesSettings_DefenderTraits_Movement_PlayerSpeed, v.variantSettings.TerritoriesSettings.DefenderTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_TerritoriesSettings_DefenderTraits_Movement_PlayerGravity, v.variantSettings.TerritoriesSettings.DefenderTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_TerritoriesSettings_DefenderTraits_Movement_VehicleUse, v.variantSettings.TerritoriesSettings.DefenderTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_TerritoriesSettings_DefenderTraits_Sensors_MotionTrackerMode, v.variantSettings.TerritoriesSettings.DefenderTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_TerritoriesSettings_DefenderTraits_Sensors_MotionTrackerRange, v.variantSettings.TerritoriesSettings.DefenderTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_ActiveCamo, v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_Waypoint, v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_PlayerSize, v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_ForcedColor, v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.ForcedColor);

            // Attacker Traits
            LoadOptions<DamageResistance>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_TerritoriesSettings_AttackerTraits_Movement_PlayerSpeed, v.variantSettings.TerritoriesSettings.AttackerTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_TerritoriesSettings_AttackerTraits_Movement_PlayerGravity, v.variantSettings.TerritoriesSettings.AttackerTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_TerritoriesSettings_AttackerTraits_Movement_VehicleUse, v.variantSettings.TerritoriesSettings.AttackerTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_TerritoriesSettings_AttackerTraits_Sensors_MotionTrackerMode, v.variantSettings.TerritoriesSettings.AttackerTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_TerritoriesSettings_AttackerTraits_Sensors_MotionTrackerRange, v.variantSettings.TerritoriesSettings.AttackerTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_ActiveCamo, v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_Waypoint, v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_PlayerSize, v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_ForcedColor, v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.ForcedColor);

            // -- Assault --
            LoadOptions<AssaultBombMode>(variantSettings_AssaultSettings_AssaultBombMode, v.variantSettings.AssaultSettings.AssaultBombMode);
            LoadOptions<DetonationsToWin>(variantSettings_AssaultSettings_DetonationsToWin, v.variantSettings.AssaultSettings.DetonationsToWin);
            LoadOptions<Boolean>(variantSettings_AssaultSettings_ResetOnDisarm, v.variantSettings.AssaultSettings.ResetOnDisarm);
            LoadOptions<BombArmingTime>(variantSettings_AssaultSettings_BombArmingTime, v.variantSettings.AssaultSettings.BombArmingTime);
            LoadOptions<BombDisarmingTime>(variantSettings_AssaultSettings_BombDisarmingTime, v.variantSettings.AssaultSettings.BombDisarmingTime);
            LoadOptions<BombArmingTime>(variantSettings_AssaultSettings_BombFuseTime, v.variantSettings.AssaultSettings.BombFuseTime);
            LoadOptions<BombResetTime>(variantSettings_AssaultSettings_BombResetTime, v.variantSettings.AssaultSettings.BombResetTime);
            LoadOptions<SuddenDeath>(variantSettings_AssaultSettings_SuddenDeath, v.variantSettings.AssaultSettings.SuddenDeath);

            // Bomb Carrier Traits
            LoadOptions<DamageResistance>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_AssaultSettings_BombCarrierTraits_Movement_PlayerSpeed, v.variantSettings.AssaultSettings.BombCarrierTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_AssaultSettings_BombCarrierTraits_Movement_PlayerGravity, v.variantSettings.AssaultSettings.BombCarrierTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_AssaultSettings_BombCarrierTraits_Movement_VehicleUse, v.variantSettings.AssaultSettings.BombCarrierTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_AssaultSettings_BombCarrierTraits_Sensors_MotionTrackerMode, v.variantSettings.AssaultSettings.BombCarrierTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_AssaultSettings_BombCarrierTraits_Sensors_MotionTrackerRange, v.variantSettings.AssaultSettings.BombCarrierTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_ActiveCamo, v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_Waypoint, v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_PlayerSize, v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_ForcedColor, v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.ForcedColor);

            // Secondary Traits
            LoadOptions<DamageResistance>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_AssaultSettings_SecondaryTraits_Movement_PlayerSpeed, v.variantSettings.AssaultSettings.SecondaryTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_AssaultSettings_SecondaryTraits_Movement_PlayerGravity, v.variantSettings.AssaultSettings.SecondaryTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_AssaultSettings_SecondaryTraits_Movement_VehicleUse, v.variantSettings.AssaultSettings.SecondaryTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_AssaultSettings_SecondaryTraits_Sensors_MotionTrackerMode, v.variantSettings.AssaultSettings.SecondaryTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_AssaultSettings_SecondaryTraits_Sensors_MotionTrackerRange, v.variantSettings.AssaultSettings.SecondaryTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_ActiveCamo, v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_Waypoint, v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_PlayerSize, v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_ForcedColor, v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.ForcedColor);

            // -- Infection --
            LoadOptions<Boolean>(variantSettings_InfectionSettings_RespawnOnHavenMove, v.variantSettings.InfectionSettings.RespawnOnHavenMove);
            LoadOptions<SafeHavenMovement>(variantSettings_InfectionSettings_SafeHavenMovement, v.variantSettings.InfectionSettings.SafeHavenMovement);
            LoadOptions<SafeHavenMovementTime>(variantSettings_InfectionSettings_SafeHavenMovementTime, v.variantSettings.InfectionSettings.SafeHavenMovementTime);
            LoadOptions<InitialZombieCount>(variantSettings_InfectionSettings_InitialZombieCount, v.variantSettings.InfectionSettings.InitialZombieCount);
            LoadOptions<NextZombie>(variantSettings_InfectionSettings_NextZombie, v.variantSettings.InfectionSettings.NextZombie);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_ZombieKillPoints, v.variantSettings.InfectionSettings.ZombieKillPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_InfectionPoints, v.variantSettings.InfectionSettings.InfectionPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_LastManStandingBonus, v.variantSettings.InfectionSettings.LastManStandingBonus);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_SuicidePoints, v.variantSettings.InfectionSettings.SuicidePoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_BetrayalPoints, v.variantSettings.InfectionSettings.BetrayalPoints);
            LoadOptions<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_SafeHavenArrivalPoints, v.variantSettings.InfectionSettings.SafeHavenArrivalPoints);

            // Zombie Traits
            LoadOptions<DamageResistance>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_InfectionSettings_ZombieTraits_Movement_PlayerSpeed, v.variantSettings.InfectionSettings.ZombieTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_InfectionSettings_ZombieTraits_Movement_PlayerGravity, v.variantSettings.InfectionSettings.ZombieTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_InfectionSettings_ZombieTraits_Movement_VehicleUse, v.variantSettings.InfectionSettings.ZombieTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_InfectionSettings_ZombieTraits_Sensors_MotionTrackerMode, v.variantSettings.InfectionSettings.ZombieTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_InfectionSettings_ZombieTraits_Sensors_MotionTrackerRange, v.variantSettings.InfectionSettings.ZombieTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_InfectionSettings_ZombieTraits_Appearance_ActiveCamo, v.variantSettings.InfectionSettings.ZombieTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_InfectionSettings_ZombieTraits_Appearance_Waypoint, v.variantSettings.InfectionSettings.ZombieTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_InfectionSettings_ZombieTraits_Appearance_PlayerSize, v.variantSettings.InfectionSettings.ZombieTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_InfectionSettings_ZombieTraits_Appearance_ForcedColor, v.variantSettings.InfectionSettings.ZombieTraits.Appearance.ForcedColor);

            // Alpha Zombie Traits
            LoadOptions<DamageResistance>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_InfectionSettings_AlphaZombieTraits_Movement_PlayerSpeed, v.variantSettings.InfectionSettings.AlphaZombieTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_InfectionSettings_AlphaZombieTraits_Movement_PlayerGravity, v.variantSettings.InfectionSettings.AlphaZombieTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_InfectionSettings_AlphaZombieTraits_Movement_VehicleUse, v.variantSettings.InfectionSettings.AlphaZombieTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_InfectionSettings_AlphaZombieTraits_Sensors_MotionTrackerMode, v.variantSettings.InfectionSettings.AlphaZombieTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_InfectionSettings_AlphaZombieTraits_Sensors_MotionTrackerRange, v.variantSettings.InfectionSettings.AlphaZombieTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_ActiveCamo, v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_Waypoint, v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_PlayerSize, v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_ForcedColor, v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.ForcedColor);

            // Safe Haven Traits
            LoadOptions<DamageResistance>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_InfectionSettings_SafeHavenTraits_Movement_PlayerSpeed, v.variantSettings.InfectionSettings.SafeHavenTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_InfectionSettings_SafeHavenTraits_Movement_PlayerGravity, v.variantSettings.InfectionSettings.SafeHavenTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_InfectionSettings_SafeHavenTraits_Movement_VehicleUse, v.variantSettings.InfectionSettings.SafeHavenTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_InfectionSettings_SafeHavenTraits_Sensors_MotionTrackerMode, v.variantSettings.InfectionSettings.SafeHavenTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_InfectionSettings_SafeHavenTraits_Sensors_MotionTrackerRange, v.variantSettings.InfectionSettings.SafeHavenTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_ActiveCamo, v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_Waypoint, v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_PlayerSize, v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_ForcedColor, v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.ForcedColor);

            // Last Man Traits
            LoadOptions<DamageResistance>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_DamageResistance, v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.DamageResistance);
            LoadOptions<ShieldMultiplier>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ShieldMultiplier, v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ShieldMultiplier);
            LoadOptions<ShieldRechargeRate>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ShieldRechargeRate, v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ShieldRechargeRate);
            LoadOptions<ShieldVampirism>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ShieldVampirism, v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ShieldVampirism);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ImmuneToHeadshots, v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ImmuneToHeadshots);
            LoadOptions<DamageModifier>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_DamageModifier, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.DamageModifier);
            LoadOptions<Weapon>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_PrimaryWeapon, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.PrimaryWeapon);
            LoadOptions<Weapon>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_SecondaryWeapon, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.SecondaryWeapon);
            LoadOptions<GrenadeCount>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_GrenadeCount, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.GrenadeCount);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_GrenadeRegen, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.GrenadeRegen);
            LoadOptions<InfiniteAmmo>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_InfiniteAmmo, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.InfiniteAmmo);
            LoadOptions<ToggleBoolean>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_WeaponPickup, v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.WeaponPickup);
            LoadOptions<PlayerSpeed>(variantSettings_InfectionSettings_LastManTraits_Movement_PlayerSpeed, v.variantSettings.InfectionSettings.LastManTraits.Movement.PlayerSpeed);
            LoadOptions<PlayerGravity>(variantSettings_InfectionSettings_LastManTraits_Movement_PlayerGravity, v.variantSettings.InfectionSettings.LastManTraits.Movement.PlayerGravity);
            LoadOptions<VehicleUse>(variantSettings_InfectionSettings_LastManTraits_Movement_VehicleUse, v.variantSettings.InfectionSettings.LastManTraits.Movement.VehicleUse);
            LoadOptions<MotionTrackerMode>(variantSettings_InfectionSettings_LastManTraits_Sensors_MotionTrackerMode, v.variantSettings.InfectionSettings.LastManTraits.Sensors.MotionTrackerMode);
            LoadOptions<MotionTrackerRange>(variantSettings_InfectionSettings_LastManTraits_Sensors_MotionTrackerRange, v.variantSettings.InfectionSettings.LastManTraits.Sensors.MotionTrackerRange);
            LoadOptions<ActiveCamo>(variantSettings_InfectionSettings_LastManTraits_Appearance_ActiveCamo, v.variantSettings.InfectionSettings.LastManTraits.Appearance.ActiveCamo);
            LoadOptions<Waypoint>(variantSettings_InfectionSettings_LastManTraits_Appearance_Waypoint, v.variantSettings.InfectionSettings.LastManTraits.Appearance.Waypoint);
            LoadOptions<PlayerSize>(variantSettings_InfectionSettings_LastManTraits_Appearance_PlayerSize, v.variantSettings.InfectionSettings.LastManTraits.Appearance.PlayerSize);
            LoadOptions<ForcedColor>(variantSettings_InfectionSettings_LastManTraits_Appearance_ForcedColor, v.variantSettings.InfectionSettings.LastManTraits.Appearance.ForcedColor);

            CachedVariant = new Variant();
            CachedVariant = v;

            //LoadOptions<>(, v.);
        }

        public void LoadOptions<TEnum>(System.Windows.Controls.ComboBox CB, object Value)
        {
            CB.Items.Clear();
            // May be worth defaulting to 0 index so users can switch modes without having to worry about setting anything to get it working
            int SelectedIndex = -1;
            for (int i = 0; i < Enum.GetValues(typeof(TEnum)).Length; i++)
            {
                ComboBoxItem CBI = new ComboBoxItem();
                CBI.Content = ((Enum)Enum.GetValues(typeof(TEnum)).GetValue(i)).Description();
                CBI.Tag = Convert.ChangeType(Enum.GetValues(typeof(TEnum)).GetValue(i), Enum.GetUnderlyingType(typeof(TEnum)));
                CB.Items.Add(CBI);
                if (Value != null)
                {
                    try
                    {
                        if (CBI.Content.ToString() == ((Enum)Value).Description())
                        {
                            SelectedIndex = i;
                        }
                    }
                    catch (Exception)
                    {
                        // Failed to set, most likely not for the current game type
                    }
                }
                    
            }
            CB.SelectedIndex = SelectedIndex;
        }

        public void AddDefaultTimeComboItems(System.Windows.Controls.ComboBox CB)
        {
            for (int i = 0; i < 24; i++)
            {
                CB.Items.Add(i.ToString("D2") + ":00:00");
            }
        }

        private void LoadVariantBtn_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = "";

            OpenFileDialog OFD = new OpenFileDialog();
            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = OFD.FileName;
            }
            else
            {
                if (Verbose)
                    System.Windows.Forms.MessageBox.Show("Load Operation Aborted!", "Load Operation Aborted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FilePathTB.Text = FilePath;

            LoadVariantFile();
        }

        private void SwitchVariantTab(GameType gameType)
        {
            VariantSpecificTab_CTF.IsEnabled = false;
            VariantSpecificTab_Slayer.IsEnabled = false;
            VariantSpecificTab_Oddball.IsEnabled = false;
            VariantSpecificTab_KOTH.IsEnabled = false;
            VariantSpecificTab_VIP.IsEnabled = false;
            VariantSpecificTab_Juggernaut.IsEnabled = false;
            VariantSpecificTab_Territories.IsEnabled = false;
            VariantSpecificTab_Assault.IsEnabled = false;
            VariantSpecificTab_Infection.IsEnabled = false;

            // This happens while switching variants, should be safe to ignore
            if ((int)gameType == -1)
                return;

            switch (gameType)
            {
                case GameType.CTF:
                    VariantSpecificTab_CTF.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 0;
                    break;
                case GameType.Slayer:
                    VariantSpecificTab_Slayer.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 1;
                    break;
                case GameType.Oddball:
                    VariantSpecificTab_Oddball.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 2;
                    break;
                case GameType.KOTH:
                    VariantSpecificTab_KOTH.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 3;
                    break;
                case GameType.VIP:
                    VariantSpecificTab_VIP.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 4;
                    break;
                case GameType.Juggernaut:
                    VariantSpecificTab_Juggernaut.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 5;
                    break;
                case GameType.Territories:
                    VariantSpecificTab_Territories.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 6;
                    break;
                case GameType.Assault:
                    VariantSpecificTab_Assault.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 7;
                    break;
                case GameType.Infection:
                    VariantSpecificTab_Infection.IsEnabled = true;
                    VariantSpecificTabControl.SelectedIndex = 8;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Unknown GameType Value " + gameType + "!", "Unknown GameType Value!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        public void NewVariant(string VariantName, string Description, string AuthorName, GameType GameTypeMode, NumberOfRounds Rounds, TimeLimit Time, RoundsReset Reset)
        {
            Variant v = new Variant();

            using (BinaryReader b = new BinaryReader(new MemoryStream(Properties.Resources.Template)))
            {
                if (v.Load(b) == false)
                {
                    System.Windows.Forms.MessageBox.Show("Error Loading Default Template Variant.\nThis should never occur.\nPlease report it right away.", "Error Loading Default Template Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            v.variantNameMenu = VariantName;
            v.variantNameGame = VariantName;

            v.descriptionMenu = Description;
            v.descriptionGame = Description;

            v.currentAuthorName = AuthorName;
            v.originalAuthorName = AuthorName;

            v.currentAuthorXUID = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            v.originalAuthorXUID = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            v.fileCreatedTimestamp = unixTimestamp;
            v.originalFileCreatedTimestamp = unixTimestamp;
            v.metadata = "";

            v.gameTypeMenu = GameTypeMode;
            v.gameTypeGame = GameTypeMode;

            v.generalSettings.NumberOfRounds = Rounds;
            v.generalSettings.TimeLimit = Time;
            v.generalSettings.RoundsReset = Reset;

            LoadVariant(v);
        }

        public void SaveOption<TEnum>(System.Windows.Controls.ComboBox CB, ref TEnum Value)
        {
            try
            {
                Value = (TEnum)((ComboBoxItem)CB.SelectedItem).Tag;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(CB.Name.Replace("_", " ") + " was not set to a value. Please set a value and try saving again.");
                QueueSaveAbort = true;
                return;
            }
            
            //System.Windows.Forms.MessageBox.Show(Value.ToString());
        }

        // Function for writing the variant as an 'Update.' Almost no one will ever need this.
        public void SaveVariant_WriteUpdate(object sender, RoutedEventArgs e)
        {
            CachedVariant.variantNameMenu = variantNameMenuTB.Text;
            CachedVariant.variantNameGame = variantNameGameTB.Text;
            CachedVariant.descriptionMenu = descriptionMenuTB.Text;
            CachedVariant.descriptionGame = descriptionGameTB.Text;
            CachedVariant.currentAuthorName = currentAuthorNameTB.Text;
            if (currentAuthorUIDTB.Text.Length == 0)
                currentAuthorUIDTB.Text = "0000000000000000";
            else if (currentAuthorUIDTB.Text.Length < 16)
            {
                System.Windows.Forms.MessageBox.Show("Current Author UID is less than the required 16 Alphanumeric characters." + Environment.NewLine + "Enter Player.PrintUID in Console to see your current UID (UID is printed in Big-Endian in 0.6+, convert it to Little-Endian to use here)." + Environment.NewLine + "Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CachedVariant.currentAuthorXUID = CachedVariant.BitConverterStringToByteArray(currentAuthorUIDTB.Text);
            CachedVariant.originalAuthorName = originalAuthorNameTB.Text;
            if (originalAuthorUIDTB.Text.Length == 0)
                originalAuthorUIDTB.Text = "0000000000000000";
            else if (originalAuthorUIDTB.Text.Length < 16)
            {
                System.Windows.Forms.MessageBox.Show("Original Author UID is less than the required 16 Alphanumeric characters." + Environment.NewLine + "Enter Player.PrintUID in Console to see your current UID (UID is printed in Big-Endian in 0.6+, convert it to Little-Endian to use here)." + Environment.NewLine + "Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CachedVariant.originalAuthorXUID = CachedVariant.BitConverterStringToByteArray(originalAuthorUIDTB.Text);

            FileStream fs = new FileStream(FilePathTB.Text, FileMode.Open);
            using (BinaryWriter b = new BinaryWriter(fs, Encoding.UTF8))
            {
                if (CachedVariant.WriteUpdate(b, (CachedVariant.fileSize == 57840 ? true : false)) == false)
                {
                    System.Windows.Forms.MessageBox.Show("Error Updating Variant", "Error Updating Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            System.Windows.Forms.MessageBox.Show("Variant Successfully Updated.", CachedVariant.variantNameMenu + " Updated", MessageBoxButtons.OK);
        }

        private void SaveVariantBtn_Click(object sender, RoutedEventArgs e)
        {
            Variant v = new Variant();
            // Load the Cached Variant to autofill settings that don't have a UI component
            v = CachedVariant;
            v.variantNameMenu = variantNameMenuTB.Text;
            v.variantNameGame = variantNameGameTB.Text;
            v.descriptionMenu = descriptionMenuTB.Text;
            v.descriptionGame = descriptionGameTB.Text;
            v.currentAuthorName = currentAuthorNameTB.Text;
            if (currentAuthorUIDTB.Text.Length == 0)
                currentAuthorUIDTB.Text = "0000000000000000";
            else if (currentAuthorUIDTB.Text.Length < 16)
            {
                System.Windows.Forms.MessageBox.Show("Current Author UID is less than the required 16 Alphanumeric characters." + Environment.NewLine + "Enter Player.PrintUID in Console to see your current UID (UID is printed in Big-Endian in 0.6+, convert it to Little-Endian to use here)." + Environment.NewLine + "Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            v.currentAuthorXUID = v.BitConverterStringToByteArray(currentAuthorUIDTB.Text);
            v.originalAuthorName = originalAuthorNameTB.Text;
            if (originalAuthorUIDTB.Text.Length == 0)
                originalAuthorUIDTB.Text = "0000000000000000";
            else if (originalAuthorUIDTB.Text.Length < 16)
            {
                System.Windows.Forms.MessageBox.Show("Original Author UID is less than the required 16 Alphanumeric characters." + Environment.NewLine + "Enter Player.PrintUID in Console to see your current UID (UID is printed in Big-Endian in 0.6+, convert it to Little-Endian to use here)." + Environment.NewLine + "Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            v.originalAuthorXUID = v.BitConverterStringToByteArray(originalAuthorUIDTB.Text);
            SaveOption<GameType>(gameTypeMenuCB, ref v.gameTypeMenu);
            v.gameTypeMenuOrder = v.ConvertGameTypeToGameTypeOrder(v.gameTypeMenu);
            v.gameTypeInit = v.gameTypeMenu;
            SaveOption<GameType>(gameTypeGameCB, ref v.gameTypeGame);
            v.gameTypeGameOrder = v.ConvertGameTypeToGameTypeOrder(v.gameTypeGame);
            v.fileSize = 956;
            v.originalFileSize = 956;
            v.metadata = MetadataTB.Text;

            // Apply timestamps for File Created (Modified Timestamp) and Original File Created (Original Creation Timestamp)
            if (fileCreatedTimestampNowCB.IsChecked == true)
            {
                // Force fileCreatedTimestamp to now
                TimeSpan fileCreatedTime = DateTime.UtcNow - new DateTime(1970, 1, 1);
                int fileCreatedSecondsSinceEpoch = (int)fileCreatedTime.TotalSeconds;
                v.fileCreatedTimestamp = fileCreatedSecondsSinceEpoch;
                fileCreatedTimestampDTP.Text = FromUnixTime(fileCreatedSecondsSinceEpoch).ToString("dddd MMMM d, yyyy HH:mm:ss");
            }
            else
            {
                // Use selected timestamp value from the UI
                DateTime fileTimestamp = DateTime.ParseExact(fileCreatedTimestampDTP.Text, "dddd MMMM d, yyyy HH:mm:ss", null);
                TimeSpan time = fileTimestamp - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)time.TotalSeconds;
                v.fileCreatedTimestamp = secondsSinceEpoch;
            }

            if (originalFileCreatedTimestampNowCB.IsChecked == true)
            {
                // Force originalFileCreatedTimestamp to now
                TimeSpan fileCreatedTime = DateTime.UtcNow - new DateTime(1970, 1, 1);
                int fileCreatedSecondsSinceEpoch = (int)fileCreatedTime.TotalSeconds;
                v.originalFileCreatedTimestamp = fileCreatedSecondsSinceEpoch;
                originalFileCreatedTimestampDTP.Text = FromUnixTime(fileCreatedSecondsSinceEpoch).ToString("dddd MMMM d, yyyy HH:mm:ss");
            }
            else
            {
                // Use selected timestamp value from the UI
                DateTime originalFileTimestamp = DateTime.ParseExact(originalFileCreatedTimestampDTP.Text, "dddd MMMM d, yyyy HH:mm:ss", null);
                TimeSpan time = originalFileTimestamp - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)time.TotalSeconds;
                v.originalFileCreatedTimestamp = secondsSinceEpoch;
            }

            // Apply General Settings
            SaveOption<NumberOfRounds>(generalSettings_NumberOfRounds, ref v.generalSettings.NumberOfRounds);
            SaveOption<TimeLimit>(generalSettings_TimeLimit, ref v.generalSettings.TimeLimit);
            SaveOption<RoundsReset>(generalSettings_RoundsReset, ref v.generalSettings.RoundsReset);

            SaveOption<TeamChanging>(generalSettings_TeamChanging, ref v.generalSettings.TeamChanging);
            SaveOption<FriendlyFire>(generalSettings_FriendlyFire, ref v.generalSettings.FriendlyFire);
            SaveOption<BetrayalBooting>(generalSettings_BetrayalBooting, ref v.generalSettings.BetrayalBooting);
            SaveOption<Spectating>(generalSettings_Spectating, ref v.generalSettings.Spectating);
            SaveOption<DamageResistance>(generalSettings_BasePlayerTraits_ShieldsAndHealth_DamageResistance, ref v.generalSettings.BasePlayerTraits.ShieldsAndHealth.DamageResistance);
            SaveOption<ShieldMultiplier>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ShieldMultiplier, ref v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ShieldMultiplier);
            SaveOption<ShieldRechargeRate>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ShieldRechargeRate);
            SaveOption<ShieldVampirism>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ShieldVampirism, ref v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ShieldVampirism);
            SaveOption<ToggleBoolean>(generalSettings_BasePlayerTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.generalSettings.BasePlayerTraits.ShieldsAndHealth.ImmuneToHeadshots);
            SaveOption<DamageModifier>(generalSettings_BasePlayerTraits_WeaponsAndDamage_DamageModifier, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.DamageModifier);
            SaveOption<Weapon>(generalSettings_BasePlayerTraits_WeaponsAndDamage_PrimaryWeapon, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.PrimaryWeapon);
            SaveOption<Weapon>(generalSettings_BasePlayerTraits_WeaponsAndDamage_SecondaryWeapon, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.SecondaryWeapon);
            SaveOption<GrenadeCount>(generalSettings_BasePlayerTraits_WeaponsAndDamage_GrenadeCount, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.GrenadeCount);
            SaveOption<ToggleBoolean>(generalSettings_BasePlayerTraits_WeaponsAndDamage_GrenadeRegen, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.GrenadeRegen);
            SaveOption<InfiniteAmmo>(generalSettings_BasePlayerTraits_WeaponsAndDamage_InfiniteAmmo, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.InfiniteAmmo);
            SaveOption<ToggleBoolean>(generalSettings_BasePlayerTraits_WeaponsAndDamage_WeaponPickup, ref v.generalSettings.BasePlayerTraits.WeaponsAndDamage.WeaponPickup);
            SaveOption<PlayerSpeed>(generalSettings_BasePlayerTraits_Movement_PlayerSpeed, ref v.generalSettings.BasePlayerTraits.Movement.PlayerSpeed);
            SaveOption<PlayerGravity>(generalSettings_BasePlayerTraits_Movement_PlayerGravity, ref v.generalSettings.BasePlayerTraits.Movement.PlayerGravity);
            SaveOption<VehicleUse>(generalSettings_BasePlayerTraits_Movement_VehicleUse, ref v.generalSettings.BasePlayerTraits.Movement.VehicleUse);
            SaveOption<MotionTrackerMode>(generalSettings_BasePlayerTraits_Sensors_MotionTrackerMode, ref v.generalSettings.BasePlayerTraits.Sensors.MotionTrackerMode);
            SaveOption<MotionTrackerRange>(generalSettings_BasePlayerTraits_Sensors_MotionTrackerRange, ref v.generalSettings.BasePlayerTraits.Sensors.MotionTrackerRange);
            SaveOption<ActiveCamo>(generalSettings_BasePlayerTraits_Appearance_ActiveCamo, ref v.generalSettings.BasePlayerTraits.Appearance.ActiveCamo);
            SaveOption<Waypoint>(generalSettings_BasePlayerTraits_Appearance_Waypoint, ref v.generalSettings.BasePlayerTraits.Appearance.Waypoint);
            SaveOption<PlayerSize>(generalSettings_BasePlayerTraits_Appearance_PlayerSize, ref v.generalSettings.BasePlayerTraits.Appearance.PlayerSize);
            SaveOption<ForcedColor>(generalSettings_BasePlayerTraits_Appearance_ForcedColor, ref v.generalSettings.BasePlayerTraits.Appearance.ForcedColor);
            SaveOption<RespawnTime>(generalSettings_RespawnSettings_RespawnTimeServer, ref v.generalSettings.RespawnSettings.RespawnTimeServer);
            SaveOption<RespawnTime>(generalSettings_RespawnSettings_RespawnTimeMenu, ref v.generalSettings.RespawnSettings.RespawnTimeMenu);
            SaveOption<SuicidePenalty>(generalSettings_RespawnSettings_SuicidePenalty, ref v.generalSettings.RespawnSettings.SuicidePenalty);
            SaveOption<BetrayalPenalty>(generalSettings_RespawnSettings_BetrayalPenalty, ref v.generalSettings.RespawnSettings.BetrayalPenalty);
            SaveOption<LivesPerRound>(generalSettings_RespawnSettings_LivesPerRound, ref v.generalSettings.RespawnSettings.LivesPerRound);
            SaveOption<SynchronizeWithTeam>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnModifiers_SynchronizeWithTeam, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.SynchronizeWithTeam);
            SaveOption<RespawnTimeGrowth>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnModifiers_RespawnTimeGrowth, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnTimeGrowth);
            SaveOption<RespawnOnKills>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnModifiers_RespawnOnKills, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnModifiers.RespawnOnKills);
            SaveOption<Duration>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Duration, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Duration);
            SaveOption<DamageResistance>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_DamageResistance, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.DamageResistance);
            SaveOption<ShieldMultiplier>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ShieldMultiplier, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ShieldMultiplier);
            SaveOption<ShieldRechargeRate>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ShieldRechargeRate);
            SaveOption<ShieldVampirism>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ShieldVampirism, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ShieldVampirism);
            SaveOption<ToggleBoolean>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.ShieldsAndHealth.ImmuneToHeadshots);
            SaveOption<DamageModifier>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_DamageModifier, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.DamageModifier);
            SaveOption<ToggleBoolean>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_GrenadeRegen, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.GrenadeRegen);
            SaveOption<InfiniteAmmo>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_InfiniteAmmo, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.InfiniteAmmo);
            SaveOption<ToggleBoolean>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_WeaponsAndDamage_WeaponPickup, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.WeaponsAndDamage.WeaponPickup);
            SaveOption<PlayerSpeed>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Movement_PlayerSpeed, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement.PlayerSpeed);
            SaveOption<PlayerGravity>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Movement_PlayerGravity, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement.PlayerGravity);
            SaveOption<VehicleUse>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Movement_VehicleUse, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Movement.VehicleUse);
            SaveOption<MotionTrackerMode>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Sensors_MotionTrackerMode, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Sensors.MotionTrackerMode);
            SaveOption<MotionTrackerRange>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Sensors_MotionTrackerRange, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Sensors.MotionTrackerRange);
            SaveOption<ActiveCamo>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_ActiveCamo, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.ActiveCamo);
            SaveOption<Waypoint>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_Waypoint, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.Waypoint);
            SaveOption<PlayerSize>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_PlayerSize, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.PlayerSize);
            SaveOption<ForcedColor>(generalSettings_RespawnSettings_AdvancedRespawnSettings_RespawnTraits_Appearance_ForcedColor, ref v.generalSettings.RespawnSettings.AdvancedRespawnSettings.RespawnTraits.Appearance.ForcedColor);
            SaveOption<GrenadesOnMap>(weaponsAndVehicles_GrenadesOnMap, ref v.weaponsAndVehicles.GrenadesOnMap);
            SaveOption<WeaponsOnMap>(weaponsAndVehicles_WeaponsOnMap, ref v.weaponsAndVehicles.WeaponsOnMap);
            SaveOption<VehicleSet>(weaponsAndVehicles_VehicleSet, ref v.weaponsAndVehicles.VehicleSet);
            SaveOption<IndestructibleVehicles>(weaponsAndVehicles_IndestructibleVehicles, ref v.weaponsAndVehicles.IndestructibleVehicles);

            // Custom Powerup Traits
            SaveOption<Duration>(weaponsAndVehicles_CustomPowerupTraits_Duration, ref v.weaponsAndVehicles.CustomPowerupTraits.Duration);
            SaveOption<DamageResistance>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_DamageResistance, ref v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.DamageResistance);
            SaveOption<ShieldMultiplier>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ShieldMultiplier, ref v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ShieldMultiplier);
            SaveOption<ShieldRechargeRate>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ShieldRechargeRate);
            SaveOption<ShieldVampirism>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ShieldVampirism, ref v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ShieldVampirism);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_CustomPowerupTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.weaponsAndVehicles.CustomPowerupTraits.ShieldsAndHealth.ImmuneToHeadshots);
            SaveOption<DamageModifier>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_DamageModifier, ref v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.DamageModifier);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_GrenadeRegen, ref v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.GrenadeRegen);
            SaveOption<InfiniteAmmo>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_InfiniteAmmo, ref v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.InfiniteAmmo);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_CustomPowerupTraits_WeaponsAndDamage_WeaponPickup, ref v.weaponsAndVehicles.CustomPowerupTraits.WeaponsAndDamage.WeaponPickup);
            SaveOption<PlayerSpeed>(weaponsAndVehicles_CustomPowerupTraits_Movement_PlayerSpeed, ref v.weaponsAndVehicles.CustomPowerupTraits.Movement.PlayerSpeed);
            SaveOption<PlayerGravity>(weaponsAndVehicles_CustomPowerupTraits_Movement_PlayerGravity, ref v.weaponsAndVehicles.CustomPowerupTraits.Movement.PlayerGravity);
            SaveOption<VehicleUse>(weaponsAndVehicles_CustomPowerupTraits_Movement_VehicleUse, ref v.weaponsAndVehicles.CustomPowerupTraits.Movement.VehicleUse);
            SaveOption<MotionTrackerMode>(weaponsAndVehicles_CustomPowerupTraits_Sensors_MotionTrackerMode, ref v.weaponsAndVehicles.CustomPowerupTraits.Sensors.MotionTrackerMode);
            SaveOption<MotionTrackerRange>(weaponsAndVehicles_CustomPowerupTraits_Sensors_MotionTrackerRange, ref v.weaponsAndVehicles.CustomPowerupTraits.Sensors.MotionTrackerRange);
            SaveOption<ActiveCamo>(weaponsAndVehicles_CustomPowerupTraits_Appearance_ActiveCamo, ref v.weaponsAndVehicles.CustomPowerupTraits.Appearance.ActiveCamo);
            SaveOption<Waypoint>(weaponsAndVehicles_CustomPowerupTraits_Appearance_Waypoint, ref v.weaponsAndVehicles.CustomPowerupTraits.Appearance.Waypoint);
            SaveOption<PlayerSize>(weaponsAndVehicles_CustomPowerupTraits_Appearance_PlayerSize, ref v.weaponsAndVehicles.CustomPowerupTraits.Appearance.PlayerSize);
            SaveOption<ForcedColor>(weaponsAndVehicles_CustomPowerupTraits_Appearance_ForcedColor, ref v.weaponsAndVehicles.CustomPowerupTraits.Appearance.ForcedColor);

            // Overshield Powerup Traits
            SaveOption<Duration>(weaponsAndVehicles_OvershieldPowerupTraits_Duration, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Duration);
            SaveOption<DamageResistance>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_DamageResistance, ref v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.DamageResistance);
            SaveOption<ShieldMultiplier>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ShieldMultiplier, ref v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ShieldMultiplier);
            SaveOption<ShieldRechargeRate>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ShieldRechargeRate);
            SaveOption<ShieldVampirism>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ShieldVampirism, ref v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ShieldVampirism);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_OvershieldPowerupTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.weaponsAndVehicles.OvershieldPowerupTraits.ShieldsAndHealth.ImmuneToHeadshots);
            SaveOption<DamageModifier>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_DamageModifier, ref v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.DamageModifier);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_GrenadeRegen, ref v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.GrenadeRegen);
            SaveOption<InfiniteAmmo>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_InfiniteAmmo, ref v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.InfiniteAmmo);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_OvershieldPowerupTraits_WeaponsAndDamage_WeaponPickup, ref v.weaponsAndVehicles.OvershieldPowerupTraits.WeaponsAndDamage.WeaponPickup);
            SaveOption<PlayerSpeed>(weaponsAndVehicles_OvershieldPowerupTraits_Movement_PlayerSpeed, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Movement.PlayerSpeed);
            SaveOption<PlayerGravity>(weaponsAndVehicles_OvershieldPowerupTraits_Movement_PlayerGravity, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Movement.PlayerGravity);
            SaveOption<VehicleUse>(weaponsAndVehicles_OvershieldPowerupTraits_Movement_VehicleUse, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Movement.VehicleUse);
            SaveOption<MotionTrackerMode>(weaponsAndVehicles_OvershieldPowerupTraits_Sensors_MotionTrackerMode, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Sensors.MotionTrackerMode);
            SaveOption<MotionTrackerRange>(weaponsAndVehicles_OvershieldPowerupTraits_Sensors_MotionTrackerRange, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Sensors.MotionTrackerRange);
            SaveOption<ActiveCamo>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_ActiveCamo, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.ActiveCamo);
            SaveOption<Waypoint>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_Waypoint, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.Waypoint);
            SaveOption<PlayerSize>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_PlayerSize, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.PlayerSize);
            SaveOption<ForcedColor>(weaponsAndVehicles_OvershieldPowerupTraits_Appearance_ForcedColor, ref v.weaponsAndVehicles.OvershieldPowerupTraits.Appearance.ForcedColor);

            // Active Camo Powerup Traits
            SaveOption<Duration>(weaponsAndVehicles_ActiveCamoPowerupTraits_Duration, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Duration);
            SaveOption<DamageResistance>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_DamageResistance, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.DamageResistance);
            SaveOption<ShieldMultiplier>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ShieldMultiplier, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ShieldMultiplier);
            SaveOption<ShieldRechargeRate>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ShieldRechargeRate);
            SaveOption<ShieldVampirism>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ShieldVampirism, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ShieldVampirism);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_ActiveCamoPowerupTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.ShieldsAndHealth.ImmuneToHeadshots);
            SaveOption<DamageModifier>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_DamageModifier, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.DamageModifier);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_GrenadeRegen, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.GrenadeRegen);
            SaveOption<InfiniteAmmo>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_InfiniteAmmo, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.InfiniteAmmo);
            SaveOption<ToggleBoolean>(weaponsAndVehicles_ActiveCamoPowerupTraits_WeaponsAndDamage_WeaponPickup, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.WeaponsAndDamage.WeaponPickup);
            SaveOption<PlayerSpeed>(weaponsAndVehicles_ActiveCamoPowerupTraits_Movement_PlayerSpeed, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Movement.PlayerSpeed);
            SaveOption<PlayerGravity>(weaponsAndVehicles_ActiveCamoPowerupTraits_Movement_PlayerGravity, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Movement.PlayerGravity);
            SaveOption<VehicleUse>(weaponsAndVehicles_ActiveCamoPowerupTraits_Movement_VehicleUse, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Movement.VehicleUse);
            SaveOption<MotionTrackerMode>(weaponsAndVehicles_ActiveCamoPowerupTraits_Sensors_MotionTrackerMode, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Sensors.MotionTrackerMode);
            SaveOption<MotionTrackerRange>(weaponsAndVehicles_ActiveCamoPowerupTraits_Sensors_MotionTrackerRange, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Sensors.MotionTrackerRange);
            SaveOption<ActiveCamo>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_ActiveCamo, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.ActiveCamo);
            SaveOption<Waypoint>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_Waypoint, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.Waypoint);
            SaveOption<PlayerSize>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_PlayerSize, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.PlayerSize);
            SaveOption<ForcedColor>(weaponsAndVehicles_ActiveCamoPowerupTraits_Appearance_ForcedColor, ref v.weaponsAndVehicles.ActiveCamoPowerupTraits.Appearance.ForcedColor);

            // Apply Variant Specific Settings

            switch (v.gameTypeGame)
            {
                case GameType.UNK:
                    break;
                case GameType.CTF:
                    v.gameCode = GameCode.CTF;

                    v.generalSettings.TeamMode = TeamMode.Enabled;

                    SaveOption<CapturesToWin>(variantSettings_CTFSettings_CapturesToWin, ref v.variantSettings.CTFSettings.CapturesToWin);
                    SaveOption<NumberOfFlags>(variantSettings_CTFSettings_NumberOfFlags, ref v.variantSettings.CTFSettings.NumberOfFlags);
                    SaveOption<SuddenDeath>(variantSettings_CTFSettings_SuddenDeath, ref v.variantSettings.CTFSettings.SuddenDeath);
                    SaveOption<Boolean>(variantSettings_CTFSettings_FlagAtHomeToScore, ref v.variantSettings.CTFSettings.FlagAtHomeToScore);
                    SaveOption<FlagReturnTime>(variantSettings_CTFSettings_FlagReturnTime, ref v.variantSettings.CTFSettings.FlagReturnTime);
                    SaveOption<FlagResetTime>(variantSettings_CTFSettings_FlagResetTime, ref v.variantSettings.CTFSettings.FlagResetTime);
                    SaveOption<FlagWaypoint>(variantSettings_CTFSettings_FlagWaypoint, ref v.variantSettings.CTFSettings.FlagWaypoint);
                    SaveOption<RespawnOnCaptureByte>(variantSettings_CTFSettings_RespawnOnCapture, ref v.variantSettings.CTFSettings.RespawnOnCapture);

                    // Flag Carrier Traits
                    SaveOption<DamageResistance>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_CTFSettings_FlagCarrierTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.CTFSettings.FlagCarrierTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_CTFSettings_FlagCarrierTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.CTFSettings.FlagCarrierTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_CTFSettings_FlagCarrierTraits_Movement_PlayerSpeed, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_CTFSettings_FlagCarrierTraits_Movement_PlayerGravity, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_CTFSettings_FlagCarrierTraits_Movement_VehicleUse, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_CTFSettings_FlagCarrierTraits_Sensors_MotionTrackerMode, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_CTFSettings_FlagCarrierTraits_Sensors_MotionTrackerRange, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_ActiveCamo, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_Waypoint, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_PlayerSize, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_CTFSettings_FlagCarrierTraits_Appearance_ForcedColor, ref v.variantSettings.CTFSettings.FlagCarrierTraits.Appearance.ForcedColor);
                    break;
                case GameType.Slayer:
                    v.gameCode = GameCode.Slayer;

                    SaveOption<TeamMode>(generalSettings_TeamSlayer, ref v.generalSettings.TeamMode);

                    SaveOption<TeamScoring>(variantSettings_SlayerSettings_TeamScoring, ref v.variantSettings.SlayerSettings.TeamScoring);
                    SaveOption<ScoreToWin>(variantSettings_SlayerSettings_ScoreToWin, ref v.variantSettings.SlayerSettings.ScoreToWin);
                    SaveOption<ScoreOptionsPointsShort>(variantSettings_SlayerSettings_KillPoints, ref v.variantSettings.SlayerSettings.KillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_AssistPoints, ref v.variantSettings.SlayerSettings.AssistPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_DeathPoints, ref v.variantSettings.SlayerSettings.DeathPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_SuicidePoints, ref v.variantSettings.SlayerSettings.SuicidePoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_BetrayalPoints, ref v.variantSettings.SlayerSettings.BetrayalPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_LeaderKillBonus, ref v.variantSettings.SlayerSettings.LeaderKillBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_EliminationBonus, ref v.variantSettings.SlayerSettings.EliminationBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_AssassinationBonus, ref v.variantSettings.SlayerSettings.AssassinationBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_HeadshotBonus, ref v.variantSettings.SlayerSettings.HeadshotBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_BeatdownBonus, ref v.variantSettings.SlayerSettings.BeatdownBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_StickyBonus, ref v.variantSettings.SlayerSettings.StickyBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_SplatterBonus, ref v.variantSettings.SlayerSettings.SplatterBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_SlayerSettings_SpreeBonus, ref v.variantSettings.SlayerSettings.SpreeBonus);

                    // Leader Traits
                    SaveOption<DamageResistance>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_SlayerSettings_LeaderTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.SlayerSettings.LeaderTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_SlayerSettings_LeaderTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.SlayerSettings.LeaderTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_SlayerSettings_LeaderTraits_Movement_PlayerSpeed, ref v.variantSettings.SlayerSettings.LeaderTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_SlayerSettings_LeaderTraits_Movement_PlayerGravity, ref v.variantSettings.SlayerSettings.LeaderTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_SlayerSettings_LeaderTraits_Movement_VehicleUse, ref v.variantSettings.SlayerSettings.LeaderTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_SlayerSettings_LeaderTraits_Sensors_MotionTrackerMode, ref v.variantSettings.SlayerSettings.LeaderTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_SlayerSettings_LeaderTraits_Sensors_MotionTrackerRange, ref v.variantSettings.SlayerSettings.LeaderTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_SlayerSettings_LeaderTraits_Appearance_ActiveCamo, ref v.variantSettings.SlayerSettings.LeaderTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_SlayerSettings_LeaderTraits_Appearance_Waypoint, ref v.variantSettings.SlayerSettings.LeaderTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_SlayerSettings_LeaderTraits_Appearance_PlayerSize, ref v.variantSettings.SlayerSettings.LeaderTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_SlayerSettings_LeaderTraits_Appearance_ForcedColor, ref v.variantSettings.SlayerSettings.LeaderTraits.Appearance.ForcedColor);
                    break;
                case GameType.Oddball:
                    v.gameCode = GameCode.Oddball;

                    SaveOption<TeamMode>(generalSettings_TeamOddball, ref v.generalSettings.TeamMode);

                    SaveOption<ScoreToWin>(variantSettings_OddballSettings_ScoreToWin, ref v.variantSettings.OddballSettings.ScoreToWin);
                    SaveOption<TeamScoring>(variantSettings_OddballSettings_TeamScoring, ref v.variantSettings.OddballSettings.TeamScoring);
                    SaveOption<ScoreOptionsPointsShort>(variantSettings_OddballSettings_CarryingPoints, ref v.variantSettings.OddballSettings.CarryingPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_OddballSettings_KillPoints, ref v.variantSettings.OddballSettings.KillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_OddballSettings_BallKillPoints, ref v.variantSettings.OddballSettings.BallKillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_OddballSettings_BallCarrierKillPoints, ref v.variantSettings.OddballSettings.BallCarrierKillPoints);
                    SaveOption<BallCount>(variantSettings_OddballSettings_BallCount, ref v.variantSettings.OddballSettings.BallCount);
                    SaveOption<Boolean>(variantSettings_OddballSettings_AutoPickup, ref v.variantSettings.OddballSettings.AutoPickup);
                    SaveOption<InitialBallDelay>(variantSettings_OddballSettings_InitialBallDelay, ref v.variantSettings.OddballSettings.InitialBallDelay);
                    SaveOption<BallRespawnDelay>(variantSettings_OddballSettings_BallRespawnDelay, ref v.variantSettings.OddballSettings.BallRespawnDelay);

                    // Ball Carrier Traits
                    SaveOption<DamageResistance>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_OddballSettings_BallCarrierTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.OddballSettings.BallCarrierTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_OddballSettings_BallCarrierTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.OddballSettings.BallCarrierTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_OddballSettings_BallCarrierTraits_Movement_PlayerSpeed, ref v.variantSettings.OddballSettings.BallCarrierTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_OddballSettings_BallCarrierTraits_Movement_PlayerGravity, ref v.variantSettings.OddballSettings.BallCarrierTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_OddballSettings_BallCarrierTraits_Movement_VehicleUse, ref v.variantSettings.OddballSettings.BallCarrierTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_OddballSettings_BallCarrierTraits_Sensors_MotionTrackerMode, ref v.variantSettings.OddballSettings.BallCarrierTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_OddballSettings_BallCarrierTraits_Sensors_MotionTrackerRange, ref v.variantSettings.OddballSettings.BallCarrierTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_ActiveCamo, ref v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_Waypoint, ref v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_PlayerSize, ref v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_OddballSettings_BallCarrierTraits_Appearance_ForcedColor, ref v.variantSettings.OddballSettings.BallCarrierTraits.Appearance.ForcedColor);
                    break;
                case GameType.KOTH:
                    v.gameCode = GameCode.KOTH;

                    SaveOption<TeamMode>(generalSettings_TeamKOTH, ref v.generalSettings.TeamMode);

                    SaveOption<ScoreToWin>(variantSettings_KOTHSettings_ScoreToWin, ref v.variantSettings.KOTHSettings.ScoreToWin);
                    SaveOption<TeamScoringKOTH>(variantSettings_KOTHSettings_TeamScoring, ref v.variantSettings.KOTHSettings.TeamScoring);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_OnHillPoints, ref v.variantSettings.KOTHSettings.OnHillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_UncontestedControlPoints, ref v.variantSettings.KOTHSettings.UncontestedControlPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_OffHillPoints, ref v.variantSettings.KOTHSettings.OffHillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_KOTHSettings_KillPoints, ref v.variantSettings.KOTHSettings.KillPoints);
                    SaveOption<HillMovement>(variantSettings_KOTHSettings_HillMovement, ref v.variantSettings.KOTHSettings.HillMovement);
                    SaveOption<HillMovementOrder>(variantSettings_KOTHSettings_HillMovementOrder, ref v.variantSettings.KOTHSettings.HillMovementOrder);

                    // On Hill Traits
                    SaveOption<DamageResistance>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_KOTHSettings_OnHillTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.KOTHSettings.OnHillTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_KOTHSettings_OnHillTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.KOTHSettings.OnHillTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_KOTHSettings_OnHillTraits_Movement_PlayerSpeed, ref v.variantSettings.KOTHSettings.OnHillTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_KOTHSettings_OnHillTraits_Movement_PlayerGravity, ref v.variantSettings.KOTHSettings.OnHillTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_KOTHSettings_OnHillTraits_Movement_VehicleUse, ref v.variantSettings.KOTHSettings.OnHillTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_KOTHSettings_OnHillTraits_Sensors_MotionTrackerMode, ref v.variantSettings.KOTHSettings.OnHillTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_KOTHSettings_OnHillTraits_Sensors_MotionTrackerRange, ref v.variantSettings.KOTHSettings.OnHillTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_KOTHSettings_OnHillTraits_Appearance_ActiveCamo, ref v.variantSettings.KOTHSettings.OnHillTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_KOTHSettings_OnHillTraits_Appearance_Waypoint, ref v.variantSettings.KOTHSettings.OnHillTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_KOTHSettings_OnHillTraits_Appearance_PlayerSize, ref v.variantSettings.KOTHSettings.OnHillTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_KOTHSettings_OnHillTraits_Appearance_ForcedColor, ref v.variantSettings.KOTHSettings.OnHillTraits.Appearance.ForcedColor);
                    break;
                case GameType.Forge:
                    break;
                case GameType.VIP:
                    v.gameCode = GameCode.VIP;

                    v.generalSettings.TeamMode = TeamMode.Enabled;

                    SaveOption<ScoreToWin>(variantSettings_VIPSettings_ScoreToWin, ref v.variantSettings.VIPSettings.ScoreToWin);
                    SaveOption<SingleVIP>(variantSettings_VIPSettings_SingleVIP, ref v.variantSettings.VIPSettings.SingleVIP);
                    SaveOption<NextVIP>(variantSettings_VIPSettings_NextVIP, ref v.variantSettings.VIPSettings.NextVIP);
                    SaveOption<VIPDeathEndsRound>(variantSettings_VIPSettings_VIPDeathEndsRound, ref v.variantSettings.VIPSettings.VIPDeathEndsRound);
                    SaveOption<GoalAreas>(variantSettings_VIPSettings_GoalAreas, ref v.variantSettings.VIPSettings.GoalAreas);
                    SaveOption<GoalMovement>(variantSettings_VIPSettings_GoalMovement, ref v.variantSettings.VIPSettings.GoalMovement);
                    SaveOption<GoalZoneOrder>(variantSettings_VIPSettings_GoalMovementOrder, ref v.variantSettings.VIPSettings.GoalMovementOrder);
                    SaveOption<InfluenceRadius>(variantSettings_VIPSettings_InfluenceRadius, ref v.variantSettings.VIPSettings.InfluenceRadius);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_VIPTakedownPoints, ref v.variantSettings.VIPSettings.VIPTakedownPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_VIPDeathPoints, ref v.variantSettings.VIPSettings.VIPDeathPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_VIPBetrayalPoints, ref v.variantSettings.VIPSettings.VIPBetrayalPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_KillPoints, ref v.variantSettings.VIPSettings.KillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_KillAsVIPPoints, ref v.variantSettings.VIPSettings.KillAsVIPPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_GoalArrivalPoints, ref v.variantSettings.VIPSettings.GoalArrivalPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_SuicidePoints, ref v.variantSettings.VIPSettings.SuicidePoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_VIPSettings_BetrayalPoints, ref v.variantSettings.VIPSettings.BetrayalPoints);

                    // VIP Traits
                    SaveOption<DamageResistance>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.VIPSettings.VIPTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.VIPSettings.VIPTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_VIPSettings_VIPTraits_Movement_PlayerSpeed, ref v.variantSettings.VIPSettings.VIPTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_VIPSettings_VIPTraits_Movement_PlayerGravity, ref v.variantSettings.VIPSettings.VIPTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_VIPSettings_VIPTraits_Movement_VehicleUse, ref v.variantSettings.VIPSettings.VIPTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_VIPSettings_VIPTraits_Sensors_MotionTrackerMode, ref v.variantSettings.VIPSettings.VIPTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_VIPSettings_VIPTraits_Sensors_MotionTrackerRange, ref v.variantSettings.VIPSettings.VIPTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_VIPSettings_VIPTraits_Appearance_ActiveCamo, ref v.variantSettings.VIPSettings.VIPTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_VIPSettings_VIPTraits_Appearance_Waypoint, ref v.variantSettings.VIPSettings.VIPTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_VIPSettings_VIPTraits_Appearance_PlayerSize, ref v.variantSettings.VIPSettings.VIPTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_VIPSettings_VIPTraits_Appearance_ForcedColor, ref v.variantSettings.VIPSettings.VIPTraits.Appearance.ForcedColor);

                    // VIP Proximity Traits
                    SaveOption<DamageResistance>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPProximityTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.VIPSettings.VIPProximityTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPProximityTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.VIPSettings.VIPProximityTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_VIPSettings_VIPProximityTraits_Movement_PlayerSpeed, ref v.variantSettings.VIPSettings.VIPProximityTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_VIPSettings_VIPProximityTraits_Movement_PlayerGravity, ref v.variantSettings.VIPSettings.VIPProximityTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_VIPSettings_VIPProximityTraits_Movement_VehicleUse, ref v.variantSettings.VIPSettings.VIPProximityTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_VIPSettings_VIPProximityTraits_Sensors_MotionTrackerMode, ref v.variantSettings.VIPSettings.VIPProximityTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_VIPSettings_VIPProximityTraits_Sensors_MotionTrackerRange, ref v.variantSettings.VIPSettings.VIPProximityTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_ActiveCamo, ref v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_Waypoint, ref v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_PlayerSize, ref v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_VIPSettings_VIPProximityTraits_Appearance_ForcedColor, ref v.variantSettings.VIPSettings.VIPProximityTraits.Appearance.ForcedColor);

                    // VIP Team Traits
                    SaveOption<DamageResistance>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPTeamTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.VIPSettings.VIPTeamTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_VIPSettings_VIPTeamTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.VIPSettings.VIPTeamTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_VIPSettings_VIPTeamTraits_Movement_PlayerSpeed, ref v.variantSettings.VIPSettings.VIPTeamTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_VIPSettings_VIPTeamTraits_Movement_PlayerGravity, ref v.variantSettings.VIPSettings.VIPTeamTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_VIPSettings_VIPTeamTraits_Movement_VehicleUse, ref v.variantSettings.VIPSettings.VIPTeamTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_VIPSettings_VIPTeamTraits_Sensors_MotionTrackerMode, ref v.variantSettings.VIPSettings.VIPTeamTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_VIPSettings_VIPTeamTraits_Sensors_MotionTrackerRange, ref v.variantSettings.VIPSettings.VIPTeamTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_ActiveCamo, ref v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_Waypoint, ref v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_PlayerSize, ref v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_VIPSettings_VIPTeamTraits_Appearance_ForcedColor, ref v.variantSettings.VIPSettings.VIPTeamTraits.Appearance.ForcedColor);
                    break;
                case GameType.Juggernaut:
                    v.gameCode = GameCode.Juggernaut;

                    v.generalSettings.TeamMode = TeamMode.Disabled;

                    SaveOption<ScoreToWin>(variantSettings_JuggernautSettings_ScoreToWin, ref v.variantSettings.JuggernautSettings.ScoreToWin);
                    SaveOption<FirstJuggernaut>(variantSettings_JuggernautSettings_FirstJuggernaut, ref v.variantSettings.JuggernautSettings.FirstJuggernaut);
                    SaveOption<NextJuggernaut>(variantSettings_JuggernautSettings_NextJuggernaut, ref v.variantSettings.JuggernautSettings.NextJuggernaut);
                    SaveOption<NextJuggernautDelay>(variantSettings_JuggernautSettings_NextJuggernautDelay, ref v.variantSettings.JuggernautSettings.NextJuggernautDelay);
                    SaveOption<AlliedAgainstJuggernaut>(variantSettings_JuggernautSettings_AlliedAgainstJuggernaut, ref v.variantSettings.JuggernautSettings.AlliedAgainstJuggernaut);
                    SaveOption<GoalZones>(variantSettings_JuggernautSettings_GoalZones, ref v.variantSettings.JuggernautSettings.GoalZones);
                    SaveOption<GoalZoneMovement>(variantSettings_JuggernautSettings_GoalZoneMovement, ref v.variantSettings.JuggernautSettings.GoalZoneMovement);
                    SaveOption<GoalZoneOrder>(variantSettings_JuggernautSettings_GoalZoneOrder, ref v.variantSettings.JuggernautSettings.GoalZoneOrder);
                    SaveOption<RespawnOnLoneJuggernaut>(variantSettings_JuggernautSettings_RespawnOnLoneJuggernaut, ref v.variantSettings.JuggernautSettings.RespawnOnLoneJuggernaut);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_KillPoints, ref v.variantSettings.JuggernautSettings.KillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_TakedownPoints, ref v.variantSettings.JuggernautSettings.TakedownPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_KillAsJuggernautPoints, ref v.variantSettings.JuggernautSettings.KillAsJuggernautPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_GoalArrivalPoints, ref v.variantSettings.JuggernautSettings.GoalArrivalPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_SuicidePoints, ref v.variantSettings.JuggernautSettings.SuicidePoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_JuggernautSettings_BetrayalPoints, ref v.variantSettings.JuggernautSettings.BetrayalPoints);

                    // Juggernaut Traits
                    SaveOption<DamageResistance>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_JuggernautSettings_JuggernautTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.JuggernautSettings.JuggernautTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_JuggernautSettings_JuggernautTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.JuggernautSettings.JuggernautTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_JuggernautSettings_JuggernautTraits_Movement_PlayerSpeed, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_JuggernautSettings_JuggernautTraits_Movement_PlayerGravity, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_JuggernautSettings_JuggernautTraits_Movement_VehicleUse, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_JuggernautSettings_JuggernautTraits_Sensors_MotionTrackerMode, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_JuggernautSettings_JuggernautTraits_Sensors_MotionTrackerRange, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_ActiveCamo, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_Waypoint, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_PlayerSize, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_JuggernautSettings_JuggernautTraits_Appearance_ForcedColor, ref v.variantSettings.JuggernautSettings.JuggernautTraits.Appearance.ForcedColor);
                    break;
                case GameType.Territories:
                    v.gameCode = GameCode.Territories;

                    v.generalSettings.TeamMode = TeamMode.Enabled;

                    SaveOption<TerritoryCaptureTime>(variantSettings_TerritoriesSettings_TerritoryCaptureTime, ref v.variantSettings.TerritoriesSettings.TerritoryCaptureTime);
                    SaveOption<OneSidedTerritories>(variantSettings_TerritoriesSettings_OneSidedTerritories, ref v.variantSettings.TerritoriesSettings.OneSidedTerritories);
                    SaveOption<LockAfterCapture>(variantSettings_TerritoriesSettings_LockAfterCapture, ref v.variantSettings.TerritoriesSettings.LockAfterCapture);
                    SaveOption<SuddenDeath>(variantSettings_TerritoriesSettings_SuddenDeath, ref v.variantSettings.TerritoriesSettings.SuddenDeath);
                    SaveOption<RespawnOnCapture>(variantSettings_TerritoriesSettings_RespawnOnCapture, ref v.variantSettings.TerritoriesSettings.RespawnOnCapture);

                    // Defender Traits
                    SaveOption<DamageResistance>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_TerritoriesSettings_DefenderTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.TerritoriesSettings.DefenderTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_TerritoriesSettings_DefenderTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.TerritoriesSettings.DefenderTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_TerritoriesSettings_DefenderTraits_Movement_PlayerSpeed, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_TerritoriesSettings_DefenderTraits_Movement_PlayerGravity, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_TerritoriesSettings_DefenderTraits_Movement_VehicleUse, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_TerritoriesSettings_DefenderTraits_Sensors_MotionTrackerMode, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_TerritoriesSettings_DefenderTraits_Sensors_MotionTrackerRange, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_ActiveCamo, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_Waypoint, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_PlayerSize, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_TerritoriesSettings_DefenderTraits_Appearance_ForcedColor, ref v.variantSettings.TerritoriesSettings.DefenderTraits.Appearance.ForcedColor);

                    // Attacker Traits
                    SaveOption<DamageResistance>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_TerritoriesSettings_AttackerTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.TerritoriesSettings.AttackerTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_TerritoriesSettings_AttackerTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.TerritoriesSettings.AttackerTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_TerritoriesSettings_AttackerTraits_Movement_PlayerSpeed, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_TerritoriesSettings_AttackerTraits_Movement_PlayerGravity, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_TerritoriesSettings_AttackerTraits_Movement_VehicleUse, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_TerritoriesSettings_AttackerTraits_Sensors_MotionTrackerMode, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_TerritoriesSettings_AttackerTraits_Sensors_MotionTrackerRange, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_ActiveCamo, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_Waypoint, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_PlayerSize, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_TerritoriesSettings_AttackerTraits_Appearance_ForcedColor, ref v.variantSettings.TerritoriesSettings.AttackerTraits.Appearance.ForcedColor);
                    break;
                case GameType.Assault:
                    v.gameCode = GameCode.Assault;

                    v.generalSettings.TeamMode = TeamMode.Enabled;

                    SaveOption<AssaultBombMode>(variantSettings_AssaultSettings_AssaultBombMode, ref v.variantSettings.AssaultSettings.AssaultBombMode);
                    SaveOption<DetonationsToWin>(variantSettings_AssaultSettings_DetonationsToWin, ref v.variantSettings.AssaultSettings.DetonationsToWin);
                    SaveOption<Boolean>(variantSettings_AssaultSettings_ResetOnDisarm, ref v.variantSettings.AssaultSettings.ResetOnDisarm);
                    SaveOption<BombArmingTime>(variantSettings_AssaultSettings_BombArmingTime, ref v.variantSettings.AssaultSettings.BombArmingTime);
                    SaveOption<BombDisarmingTime>(variantSettings_AssaultSettings_BombDisarmingTime, ref v.variantSettings.AssaultSettings.BombDisarmingTime);
                    SaveOption<BombArmingTime>(variantSettings_AssaultSettings_BombFuseTime, ref v.variantSettings.AssaultSettings.BombFuseTime);
                    SaveOption<BombResetTime>(variantSettings_AssaultSettings_BombResetTime, ref v.variantSettings.AssaultSettings.BombResetTime);
                    SaveOption<SuddenDeath>(variantSettings_AssaultSettings_SuddenDeath, ref v.variantSettings.AssaultSettings.SuddenDeath);

                    // Bomb Carrier Traits
                    SaveOption<DamageResistance>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_AssaultSettings_BombCarrierTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.AssaultSettings.BombCarrierTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_AssaultSettings_BombCarrierTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.AssaultSettings.BombCarrierTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_AssaultSettings_BombCarrierTraits_Movement_PlayerSpeed, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_AssaultSettings_BombCarrierTraits_Movement_PlayerGravity, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_AssaultSettings_BombCarrierTraits_Movement_VehicleUse, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_AssaultSettings_BombCarrierTraits_Sensors_MotionTrackerMode, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_AssaultSettings_BombCarrierTraits_Sensors_MotionTrackerRange, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_ActiveCamo, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_Waypoint, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_PlayerSize, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_AssaultSettings_BombCarrierTraits_Appearance_ForcedColor, ref v.variantSettings.AssaultSettings.BombCarrierTraits.Appearance.ForcedColor);

                    // Secondary Traits
                    SaveOption<DamageResistance>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_AssaultSettings_SecondaryTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.AssaultSettings.SecondaryTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_AssaultSettings_SecondaryTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.AssaultSettings.SecondaryTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_AssaultSettings_SecondaryTraits_Movement_PlayerSpeed, ref v.variantSettings.AssaultSettings.SecondaryTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_AssaultSettings_SecondaryTraits_Movement_PlayerGravity, ref v.variantSettings.AssaultSettings.SecondaryTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_AssaultSettings_SecondaryTraits_Movement_VehicleUse, ref v.variantSettings.AssaultSettings.SecondaryTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_AssaultSettings_SecondaryTraits_Sensors_MotionTrackerMode, ref v.variantSettings.AssaultSettings.SecondaryTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_AssaultSettings_SecondaryTraits_Sensors_MotionTrackerRange, ref v.variantSettings.AssaultSettings.SecondaryTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_ActiveCamo, ref v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_Waypoint, ref v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_PlayerSize, ref v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_AssaultSettings_SecondaryTraits_Appearance_ForcedColor, ref v.variantSettings.AssaultSettings.SecondaryTraits.Appearance.ForcedColor);
                    break;
                case GameType.Infection:
                    v.gameCode = GameCode.Infection;

                    v.generalSettings.TeamMode = TeamMode.Disabled;

                    SaveOption<Boolean>(variantSettings_InfectionSettings_RespawnOnHavenMove, ref v.variantSettings.InfectionSettings.RespawnOnHavenMove);
                    SaveOption<SafeHavenMovement>(variantSettings_InfectionSettings_SafeHavenMovement, ref v.variantSettings.InfectionSettings.SafeHavenMovement);
                    SaveOption<SafeHavenMovementTime>(variantSettings_InfectionSettings_SafeHavenMovementTime, ref v.variantSettings.InfectionSettings.SafeHavenMovementTime);
                    SaveOption<InitialZombieCount>(variantSettings_InfectionSettings_InitialZombieCount, ref v.variantSettings.InfectionSettings.InitialZombieCount);
                    SaveOption<NextZombie>(variantSettings_InfectionSettings_NextZombie, ref v.variantSettings.InfectionSettings.NextZombie);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_ZombieKillPoints, ref v.variantSettings.InfectionSettings.ZombieKillPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_InfectionPoints, ref v.variantSettings.InfectionSettings.InfectionPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_LastManStandingBonus, ref v.variantSettings.InfectionSettings.LastManStandingBonus);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_SuicidePoints, ref v.variantSettings.InfectionSettings.SuicidePoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_BetrayalPoints, ref v.variantSettings.InfectionSettings.BetrayalPoints);
                    SaveOption<ScoreOptionsPointsByte>(variantSettings_InfectionSettings_SafeHavenArrivalPoints, ref v.variantSettings.InfectionSettings.SafeHavenArrivalPoints);

                    // Zombie Traits
                    SaveOption<DamageResistance>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_ZombieTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.InfectionSettings.ZombieTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_ZombieTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.InfectionSettings.ZombieTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_InfectionSettings_ZombieTraits_Movement_PlayerSpeed, ref v.variantSettings.InfectionSettings.ZombieTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_InfectionSettings_ZombieTraits_Movement_PlayerGravity, ref v.variantSettings.InfectionSettings.ZombieTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_InfectionSettings_ZombieTraits_Movement_VehicleUse, ref v.variantSettings.InfectionSettings.ZombieTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_InfectionSettings_ZombieTraits_Sensors_MotionTrackerMode, ref v.variantSettings.InfectionSettings.ZombieTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_InfectionSettings_ZombieTraits_Sensors_MotionTrackerRange, ref v.variantSettings.InfectionSettings.ZombieTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_InfectionSettings_ZombieTraits_Appearance_ActiveCamo, ref v.variantSettings.InfectionSettings.ZombieTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_InfectionSettings_ZombieTraits_Appearance_Waypoint, ref v.variantSettings.InfectionSettings.ZombieTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_InfectionSettings_ZombieTraits_Appearance_PlayerSize, ref v.variantSettings.InfectionSettings.ZombieTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_InfectionSettings_ZombieTraits_Appearance_ForcedColor, ref v.variantSettings.InfectionSettings.ZombieTraits.Appearance.ForcedColor);

                    // Alpha Zombie Traits
                    SaveOption<DamageResistance>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_AlphaZombieTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_AlphaZombieTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_InfectionSettings_AlphaZombieTraits_Movement_PlayerSpeed, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_InfectionSettings_AlphaZombieTraits_Movement_PlayerGravity, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_InfectionSettings_AlphaZombieTraits_Movement_VehicleUse, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_InfectionSettings_AlphaZombieTraits_Sensors_MotionTrackerMode, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_InfectionSettings_AlphaZombieTraits_Sensors_MotionTrackerRange, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_ActiveCamo, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_Waypoint, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_PlayerSize, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_InfectionSettings_AlphaZombieTraits_Appearance_ForcedColor, ref v.variantSettings.InfectionSettings.AlphaZombieTraits.Appearance.ForcedColor);

                    // Safe Haven Traits
                    SaveOption<DamageResistance>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_SafeHavenTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.InfectionSettings.SafeHavenTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_SafeHavenTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.InfectionSettings.SafeHavenTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_InfectionSettings_SafeHavenTraits_Movement_PlayerSpeed, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_InfectionSettings_SafeHavenTraits_Movement_PlayerGravity, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_InfectionSettings_SafeHavenTraits_Movement_VehicleUse, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_InfectionSettings_SafeHavenTraits_Sensors_MotionTrackerMode, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_InfectionSettings_SafeHavenTraits_Sensors_MotionTrackerRange, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_ActiveCamo, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_Waypoint, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_PlayerSize, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_InfectionSettings_SafeHavenTraits_Appearance_ForcedColor, ref v.variantSettings.InfectionSettings.SafeHavenTraits.Appearance.ForcedColor);

                    // Last Man Traits
                    SaveOption<DamageResistance>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_DamageResistance, ref v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.DamageResistance);
                    SaveOption<ShieldMultiplier>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ShieldMultiplier, ref v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ShieldMultiplier);
                    SaveOption<ShieldRechargeRate>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ShieldRechargeRate, ref v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ShieldRechargeRate);
                    SaveOption<ShieldVampirism>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ShieldVampirism, ref v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ShieldVampirism);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_LastManTraits_ShieldsAndHealth_ImmuneToHeadshots, ref v.variantSettings.InfectionSettings.LastManTraits.ShieldsAndHealth.ImmuneToHeadshots);
                    SaveOption<DamageModifier>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_DamageModifier, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.DamageModifier);
                    SaveOption<Weapon>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_PrimaryWeapon, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.PrimaryWeapon);
                    SaveOption<Weapon>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_SecondaryWeapon, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.SecondaryWeapon);
                    SaveOption<GrenadeCount>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_GrenadeCount, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.GrenadeCount);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_GrenadeRegen, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.GrenadeRegen);
                    SaveOption<InfiniteAmmo>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_InfiniteAmmo, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.InfiniteAmmo);
                    SaveOption<ToggleBoolean>(variantSettings_InfectionSettings_LastManTraits_WeaponsAndDamage_WeaponPickup, ref v.variantSettings.InfectionSettings.LastManTraits.WeaponsAndDamage.WeaponPickup);
                    SaveOption<PlayerSpeed>(variantSettings_InfectionSettings_LastManTraits_Movement_PlayerSpeed, ref v.variantSettings.InfectionSettings.LastManTraits.Movement.PlayerSpeed);
                    SaveOption<PlayerGravity>(variantSettings_InfectionSettings_LastManTraits_Movement_PlayerGravity, ref v.variantSettings.InfectionSettings.LastManTraits.Movement.PlayerGravity);
                    SaveOption<VehicleUse>(variantSettings_InfectionSettings_LastManTraits_Movement_VehicleUse, ref v.variantSettings.InfectionSettings.LastManTraits.Movement.VehicleUse);
                    SaveOption<MotionTrackerMode>(variantSettings_InfectionSettings_LastManTraits_Sensors_MotionTrackerMode, ref v.variantSettings.InfectionSettings.LastManTraits.Sensors.MotionTrackerMode);
                    SaveOption<MotionTrackerRange>(variantSettings_InfectionSettings_LastManTraits_Sensors_MotionTrackerRange, ref v.variantSettings.InfectionSettings.LastManTraits.Sensors.MotionTrackerRange);
                    SaveOption<ActiveCamo>(variantSettings_InfectionSettings_LastManTraits_Appearance_ActiveCamo, ref v.variantSettings.InfectionSettings.LastManTraits.Appearance.ActiveCamo);
                    SaveOption<Waypoint>(variantSettings_InfectionSettings_LastManTraits_Appearance_Waypoint, ref v.variantSettings.InfectionSettings.LastManTraits.Appearance.Waypoint);
                    SaveOption<PlayerSize>(variantSettings_InfectionSettings_LastManTraits_Appearance_PlayerSize, ref v.variantSettings.InfectionSettings.LastManTraits.Appearance.PlayerSize);
                    SaveOption<ForcedColor>(variantSettings_InfectionSettings_LastManTraits_Appearance_ForcedColor, ref v.variantSettings.InfectionSettings.LastManTraits.Appearance.ForcedColor);
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Unable to find matching Game Code for gameTypeGame value " + v.gameTypeGame.ToString() + "." + Environment.NewLine + "Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // There was an error performing SaveOption, abort save process so user can fix error and try again
            if (QueueSaveAbort == true)
            {
                System.Windows.Forms.MessageBox.Show("Save Operation Aborted!", "Save Operation Aborted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Begin File Writing Process

            string FilePath = "";
            FileMode WritingFileMode = FileMode.Create;

            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = "variant." + (GameTypeExtension)v.gameTypeGame;
            string FilterString = "";
            for (int i = 1; i < Enum.GetValues(typeof(GameType)).Length; i++)
            {
                FilterString += (GameType)i + " (variant." + (GameTypeExtension)i + ")|variant." + (GameTypeExtension)i + (i < Enum.GetValues(typeof(GameType)).Length - 1 ? "|" : "");
            }
            SFD.Filter = FilterString;
            SFD.FilterIndex = (int)v.gameTypeGame;

            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = SFD.FileName;
                // Check if Variant Name (Menus) matches folder name
                // Game cannot load files that Variant Name (Menus) and folder name do not match
                if (System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(FilePath)) != v.variantNameMenu)
                {
                    System.Windows.Forms.DialogResult NameMismatch = System.Windows.Forms.MessageBox.Show("WARNING: The 'Variant Name (Menus)' field and selected folder name do not match." + Environment.NewLine + "The game is unable to load files that have a 'Variant Name (Menus)' value different from the folder name they are stored in." + Environment.NewLine + "Do you want to continue saving this variant with mismatched names?", "Variant Name and Folder Name Mismatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (NameMismatch == System.Windows.Forms.DialogResult.No)
                    {
                        if (Verbose)
                            System.Windows.Forms.MessageBox.Show("Save Operation Canceled!", "Save Operation Canceled!", MessageBoxButtons.OK);
                        return;
                    }
                }
                if (File.Exists(FilePath) == true)
                {
                    System.Windows.Forms.DialogResult Overwrite = System.Windows.Forms.MessageBox.Show("The selected file already exists. Do you want to overwrite this file?", "Selected File Already Exists!", MessageBoxButtons.YesNo);
                    switch (Overwrite)
                    {
                        case System.Windows.Forms.DialogResult.None:
                        case System.Windows.Forms.DialogResult.OK:
                        case System.Windows.Forms.DialogResult.Cancel:
                        case System.Windows.Forms.DialogResult.Abort:
                        case System.Windows.Forms.DialogResult.Retry:
                        case System.Windows.Forms.DialogResult.Ignore:                        
                            System.Windows.Forms.MessageBox.Show("Unknown Error Occurred. Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        case System.Windows.Forms.DialogResult.Yes:
                            WritingFileMode = FileMode.Create;
                            break;
                        case System.Windows.Forms.DialogResult.No:
                            if (Verbose)
                                System.Windows.Forms.MessageBox.Show("Save Operation Canceled!", "Save Operation Canceled!", MessageBoxButtons.OK);
                            return;
                        default:
                            System.Windows.Forms.MessageBox.Show("Unknown Error Occurred. Aborting Save Operation!", "Aborting Save Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }
            }
            else
            {
                if (Verbose)
                    System.Windows.Forms.MessageBox.Show("Save Operation Aborted!", "Save Operation Aborted!", MessageBoxButtons.OK);
                return;
            }

            FilePathTB.Text = FilePath;

            try
            {
                FileStream fs = new FileStream(FilePathTB.Text, WritingFileMode);
                using (BinaryWriter b = new BinaryWriter(fs, Encoding.UTF8))
                {
                    if (v.Write(b) == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Error Writing Variant", "Error Writing Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                System.Windows.Forms.MessageBox.Show("Variant Successfully Written.", v.variantNameMenu + " Written", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error Writing Variant." + Environment.NewLine + ex.GetType().ToString() + ": " + ex.Message, "Error Writing Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        private void gameTypeGameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SwitchVariantTab((GameType)gameTypeGameCB.SelectedIndex);
        }
    }
}
