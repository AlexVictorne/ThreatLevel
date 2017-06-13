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
    /// Interaction logic for BarrierDirectory.xaml
    /// </summary>
    public partial class BarrierDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public BarrierDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource barrierViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("barrierViewSource")));
            System.Windows.Data.CollectionViewSource weaknessViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("weaknessViewSource")));
            System.Windows.Data.CollectionViewSource protect_mechViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("protect_mechViewSource")));
            _context.barrier.Load();
            _context.weakness.Load();
            _context.protect_mech.Load();
            barrierViewSource.Source = _context.barrier.Local;
            weaknessViewSource.Source = _context.weakness.Local;
            protect_mechViewSource.Source = _context.protect_mech.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var barrier in _context.barrier.Local.ToList())
            {
                if (barrier.name == null)
                {
                    _context.barrier.Remove(barrier);
                }
            }
            foreach (var btw in _context.bar_to_weak.Local.ToList())
            {
                if (btw.value > 1) btw.value = 1;
                if ((btw.barrier == null) || (btw.weakness == null))
                {
                    _context.bar_to_weak.Remove(btw);
                }
            }
            foreach (var ptb in _context.prot_to_bar.Local.ToList())
            {
                if (ptb.value > 1) ptb.value = 1;
                if ((ptb.barrier == null) || (ptb.protect_mech == null))
                {
                    _context.prot_to_bar.Remove(ptb);
                }
            }

            _context.SaveChanges();
            // Refresh the grids so the database generated values show up.
            this.barrierDataGrid.Items.Refresh();
        }
    }
}
