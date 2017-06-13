using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WarningsDirectory.xaml
    /// </summary>
    public partial class WarningsDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public WarningsDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource warningsViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("warningsViewSource")));
            System.Windows.Data.CollectionViewSource protect_mechViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("protect_mechViewSource")));
            _context.warnings.Load();
            _context.protect_mech.Load();
            warningsViewSource.Source = _context.warnings.Local;
            protect_mechViewSource.Source = _context.protect_mech.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var warnings in _context.warnings.Local.ToList())
            {
                if (warnings.name == null)
                {
                    _context.warnings.Remove(warnings);
                }
            }
            foreach (var wtp in _context.war_to_prot.Local.ToList())
            {
                if (wtp.value > 1) wtp.value = 1;
                if ((wtp.warnings == null) || (wtp.protect_mech == null))
                {
                    _context.war_to_prot.Remove(wtp);
                }
            }
            _context.SaveChanges();
            // Refresh the grids so the database generated values show up.
            this.warningsDataGrid.Items.Refresh();
        }
    }
}
