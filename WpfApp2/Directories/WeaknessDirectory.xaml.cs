using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    /// Interaction logic for WeaknessDirectory.xaml
    /// </summary>
    public partial class WeaknessDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public WeaknessDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource weaknessViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("weaknessViewSource")));
            System.Windows.Data.CollectionViewSource barrierViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("barrierViewSource")));
            System.Windows.Data.CollectionViewSource objectViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("objectViewSource")));
            _context.weakness.Load();
            _context.barrier.Load();
            _context.@object.Load();
            weaknessViewSource.Source = _context.weakness.Local;
            barrierViewSource.Source = _context.barrier.Local;
            objectViewSource.Source = _context.@object.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var weakness in _context.weakness.Local.ToList())
            {
                if (weakness.name == null)
                {
                    _context.weakness.Remove(weakness);
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
            foreach (var wto in _context.weak_to_object.Local.ToList())
            {
                if (wto.value > 1) wto.value = 1;
                if ((wto.weakness == null) || (wto.@object == null))
                {
                    _context.weak_to_object.Remove(wto);
                }
            }
            _context.SaveChanges();
            // Refresh the grids so the database generated values show up.
            this.weaknessDataGrid.Items.Refresh();
        }
    }
}
