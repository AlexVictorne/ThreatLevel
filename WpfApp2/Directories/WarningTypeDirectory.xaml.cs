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
    /// Interaction logic for WarningTypeDirectory.xaml
    /// </summary>
    public partial class WarningTypeDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public WarningTypeDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource warning_typeViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("warning_typeViewSource")));
            _context.warning_type.Load();
            warning_typeViewSource.Source = _context.warning_type.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var warning_type in _context.warning_type.Local.ToList())
            {
                if (warning_type.name == null)
                {
                    _context.warning_type.Remove(warning_type);
                }
            }
            _context.SaveChanges();
            // Refresh the grids so the database generated values show up.
            this.warning_typeDataGrid.Items.Refresh();
        }
    }
}
