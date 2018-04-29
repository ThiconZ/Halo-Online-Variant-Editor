using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Halo_Online_Variant_Editor
{
    /// <summary>
    /// Interaction logic for New_Variant.xaml
    /// </summary>
    public partial class New_Variant : Window
    {
        public New_Variant()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Variant_Editor owner = (Variant_Editor)this.Owner;

            owner.LoadOptions<GameType>(gameTypeCB, GameType.Slayer);

            owner.LoadOptions<NumberOfRounds>(generalSettings_NumberOfRounds, NumberOfRounds.R1);

            owner.LoadOptions<TimeLimit>(generalSettings_TimeLimit, TimeLimit.Min10);

            owner.LoadOptions<RoundsReset>(generalSettings_RoundsReset, RoundsReset.Everything);
        }

        private void CreateNewVariantBtn_Click(object sender, RoutedEventArgs e)
        {
            Variant_Editor owner = (Variant_Editor)this.Owner;
            owner.NewVariant(variantNameTB.Text, descriptionTB.Text, authorNameTB.Text,
                (GameType)((ComboBoxItem)gameTypeCB.SelectedItem).Tag,
                (NumberOfRounds)((ComboBoxItem)generalSettings_NumberOfRounds.SelectedItem).Tag,
                (TimeLimit)((ComboBoxItem)generalSettings_TimeLimit.SelectedItem).Tag,
                (RoundsReset)((ComboBoxItem)generalSettings_RoundsReset.SelectedItem).Tag);
            Close();
        }

        private void CancelNewVariantBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
