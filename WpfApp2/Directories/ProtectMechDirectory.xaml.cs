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
    /// Interaction logic for ProtectMechDirectory.xaml
    /// </summary>
    public partial class ProtectMechDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public ProtectMechDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource protect_mechViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("protect_mechViewSource")));
            System.Windows.Data.CollectionViewSource barrierViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("barrierViewSource")));
            System.Windows.Data.CollectionViewSource objectViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("objectViewSource")));
            System.Windows.Data.CollectionViewSource warningsViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("warningsViewSource")));
            _context.protect_mech.Load();
            _context.barrier.Load();
            _context.@object.Load();
            _context.warnings.Load();
            protect_mechViewSource.Source = _context.protect_mech.Local;
            barrierViewSource.Source = _context.barrier.Local;
            objectViewSource.Source = _context.@object.Local;
            warningsViewSource.Source = _context.warnings.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var protect_mech in _context.protect_mech.Local.ToList())
            {
                if (protect_mech.name == null)
                {
                    _context.protect_mech.Remove(protect_mech);
                }       
            }
            foreach (var ptb in _context.prot_to_bar.Local.ToList())
            {
                if (ptb.value > 1) ptb.value = 1;
                if ((ptb.protect_mech == null) || (ptb.barrier == null))
                {
                    _context.prot_to_bar.Remove(ptb);
                }
            }
            foreach (var pto in _context.prot_to_obj.Local.ToList())
            {
                if (pto.value > 1) pto.value = 1;
                if ((pto.@object == null) || (pto.protect_mech == null))
                {
                    _context.prot_to_obj.Remove(pto);
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
            this.protect_mechDataGrid.Items.Refresh();
        }
    }
}
