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
    /// Interaction logic for IndustObjectDirectory.xaml
    /// </summary>
    public partial class IndustObjectDirectory : UserControl
    {
        private ProjectBaseEntities _context = new ProjectBaseEntities();

        public IndustObjectDirectory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource indust_objectViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("indust_objectViewSource")));
            _context.indust_object.Load();
            indust_objectViewSource.Source = _context.indust_object.Local;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var indust_object in _context.indust_object.Local.ToList())
            {
                if (indust_object.name == null)
                {
                    _context.indust_object.Remove(indust_object);
                }
            }
            _context.SaveChanges();
            // Refresh the grids so the database generated values show up.
            this.indust_objectDataGrid.Items.Refresh();
        }
    }
}
