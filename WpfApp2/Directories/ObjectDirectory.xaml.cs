using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
    /// Interaction logic for ObjectDirectory.xaml
    /// </summary>
    public partial class ObjectDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public ObjectDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource objectViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("objectViewSource")));
            System.Windows.Data.CollectionViewSource weaknessViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("weaknessViewSource")));
            System.Windows.Data.CollectionViewSource protect_mechViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("protect_mechViewSource")));
            _context.@object.Load();
            _context.weakness.Load();
            _context.protect_mech.Load();
            objectViewSource.Source = _context.@object.Local;
            weaknessViewSource.Source = _context.weakness.Local;
            protect_mechViewSource.Source = _context.protect_mech.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in _context.@object.Local.ToList())
            {
                if (obj.name == null)
                {
                    _context.@object.Remove(obj);
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
            foreach (var wto in _context.weak_to_object.Local.ToList())
            {
                if (wto.value > 1) wto.value = 1;
                if ((wto.@object == null) || (wto.weakness == null))
                {
                    _context.weak_to_object.Remove(wto);
                }
            }
            _context.SaveChanges();
            // Refresh the grids so the database generated values show up.
            this.objectDataGrid.Items.Refresh();
        }
    }
}
