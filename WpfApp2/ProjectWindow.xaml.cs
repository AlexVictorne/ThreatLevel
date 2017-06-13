using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
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
using Xceed.Wpf.AvalonDock;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        Boolean fromShell = false;
        object sendForm = null;


        private ProjectBaseEntities _context = new ProjectBaseEntities();

        private ObservableCollection<int> SelectedItems;

        public void ShowProject(object senderForm = null, ProjectDoc projectDoc = null)
        {
            if ((senderForm == null) && (projectDoc == null))
            {
                ShowDialog();
                return;
            }
            if (projectDoc == null)
            {
                sendForm = senderForm;
                fromShell = true;
                ShowDialog();
                return;
            }
            else
            {
                sendForm = senderForm;
                fromShell = true;

                Type3.IsChecked = projectDoc.calc_type == 0 ? true : false;
                Type5.IsChecked = projectDoc.calc_type == 1 ? true : false;

                edtName.Text = projectDoc.name;
                edtDesc.Text = projectDoc.description;
                indust_objectComboBox.SelectedValue = projectDoc.object_type;

                foreach (var item in projectDoc.list_of_warnings)
                    warningsSelector.SelectedItems.Add(warningsSelector.Items.Cast<warnings>().Where(p => p.id == item.id).ToList<warnings>()[0]);
                foreach (var item in projectDoc.list_of_protect_mech)
                    protect_mechSelector.SelectedItems.Add(protect_mechSelector.Items.Cast<protect_mech>().Where(p => p.id == item.id).ToList<protect_mech>()[0]);
                foreach (var item in projectDoc.list_of_barrier)
                    barrierSelector.SelectedItems.Add(barrierSelector.Items.Cast<barrier>().Where(p => p.id == item.id).ToList<barrier>()[0]);
                foreach (var item in projectDoc.list_of_weakness)
                    weaknessSelector.SelectedItems.Add(weaknessSelector.Items.Cast<weakness>().Where(p => p.id == item.id).ToList<weakness>()[0]);
                foreach (var item in projectDoc.list_of_objects)
                    objectSelector.SelectedItems.Add(objectSelector.Items.Cast<@object>().Where(p => p.id == item.id).ToList<@object>()[0]);




                ShowDialog();

                return;
            }
        }

        public ProjectWindow()
        {

            try
            {

                InitializeComponent();
                _context.Configuration.ProxyCreationEnabled = false;

                System.Windows.Data.CollectionViewSource warningsViewSource =
                    ((System.Windows.Data.CollectionViewSource)(this.FindResource("warningsViewSource")));
                _context.warnings.Load();
                warningsViewSource.Source = _context.warnings.Local;
                // Загрузите данные, установив свойство CollectionViewSource.Source:
                // warningsViewSource.Source = [универсальный источник данных]
                System.Windows.Data.CollectionViewSource protect_mechViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("protect_mechViewSource")));
                _context.protect_mech.Load();
                protect_mechViewSource.Source = _context.protect_mech.Local;
                // Загрузите данные, установив свойство CollectionViewSource.Source:
                // protect_mechViewSource.Source = [универсальный источник данных]
                System.Windows.Data.CollectionViewSource barrierViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("barrierViewSource")));
                _context.barrier.Load();
                barrierViewSource.Source = _context.barrier.Local;
                // Загрузите данные, установив свойство CollectionViewSource.Source:
                // barrierViewSource.Source = [универсальный источник данных]
                System.Windows.Data.CollectionViewSource weaknessViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("weaknessViewSource")));
                _context.weakness.Load();
                weaknessViewSource.Source = _context.weakness.Local;
                // Загрузите данные, установив свойство CollectionViewSource.Source:
                // weaknessViewSource.Source = [универсальный источник данных]
                System.Windows.Data.CollectionViewSource objectViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("objectViewSource")));
                _context.@object.Load();
                objectViewSource.Source = _context.@object.Local;
                // Загрузите данные, установив свойство CollectionViewSource.Source:
                // objectViewSource.Source = [универсальный источник данных]

                System.Windows.Data.CollectionViewSource indust_objectViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("indust_objectViewSource")));
                _context.indust_object.Load();
                indust_objectViewSource.Source = _context.indust_object.Local;


                //System.Windows.Data.CollectionViewSource indust_objectViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("indust_objectViewSource")));
                // Загрузите данные, установив свойство CollectionViewSource.Source:
                // indust_objectViewSource.Source = [универсальный источник данных]

                SelectedItems = new ObservableCollection<int>();
            }
            catch (EntityException e)
            {
                MessageBox.Show(e.Message+e.InnerException);
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            _context.Configuration.ProxyCreationEnabled = false;
            var t = warningsSelector.SelectedValue;
            var projectDoc = new ProjectDoc
            {
                list_of_barrier = barrierSelector.SelectedItems.Cast<barrier>().ToList<barrier>(),
                list_of_protect_mech = protect_mechSelector.SelectedItems.Cast<protect_mech>().ToList<protect_mech>(),
                list_of_warnings = warningsSelector.SelectedItems.Cast<warnings>().ToList<warnings>(),
                list_of_objects = objectSelector.SelectedItems.Cast<@object>().ToList<@object>(),
                list_of_weakness = weaknessSelector.SelectedItems.Cast<weakness>().ToList<weakness>(),

                calc_type = Type3.IsChecked == false ? 1 : 0,
                name = edtName.Text,
                description = edtDesc.Text,
                object_type = (int)indust_objectComboBox.SelectedValue

            };
            //если есть связь с окружением - оставляем
            //иначе - удаляем
            var tem = projectDoc.list_of_protect_mech.Select(pm => pm.id).ToList();
            
            projectDoc.list_of_warnings = projectDoc.list_of_warnings.Where<warnings>(w => _context.war_to_prot.Where(wtp => wtp.warcode == w.id && tem.Contains(wtp.protcode)).Count()>0).ToList<warnings>();

            tem = projectDoc.list_of_warnings.Select(pm => pm.id).ToList();

            if (projectDoc.calc_type == 1)
            {
                var tem2 = projectDoc.list_of_barrier.Select(pm => pm.id).ToList();
                projectDoc.list_of_protect_mech = projectDoc.list_of_protect_mech.Where<protect_mech>(p => _context.war_to_prot.Where(wtp => wtp.protcode == p.id && tem.Contains(wtp.warcode)).Count() > 0
                                                                                                        || _context.prot_to_bar.Where(ptb => ptb.protcode == p.id && tem2.Contains(ptb.barcode)).Count() > 0
                                                                                                     ).ToList<protect_mech>();
                tem = projectDoc.list_of_protect_mech.Select(pm => pm.id).ToList();
                tem2 = projectDoc.list_of_weakness.Select(pm => pm.id).ToList();
                projectDoc.list_of_barrier = projectDoc.list_of_barrier.Where<barrier>(b => _context.prot_to_bar.Where(ptb => ptb.barcode == b.id && tem.Contains(ptb.protcode)).Count() > 0
                                                                                         || _context.bar_to_weak.Where(btw => btw.barcode == b.id && tem2.Contains(btw.weakcode)).Count() > 0
                                                                                      ).ToList<barrier>();
                tem = projectDoc.list_of_barrier.Select(pm => pm.id).ToList();
                tem2 = projectDoc.list_of_objects.Select(pm => pm.id).ToList();
                projectDoc.list_of_weakness = projectDoc.list_of_weakness.Where<weakness>(we => _context.bar_to_weak.Where(btw => btw.weakcode == we.id && tem.Contains(btw.barcode)).Count() > 0
                                                                                             || _context.weak_to_object.Where(wto => wto.weakcode == we.id && tem2.Contains(wto.objcode)).Count() > 0
                                                                                         ).ToList<weakness>();
                tem = projectDoc.list_of_weakness.Select(pm => pm.id).ToList();
                projectDoc.list_of_objects = projectDoc.list_of_objects.Where<@object>(o => _context.weak_to_object.Where(wto => wto.objcode == o.id && tem.Contains(wto.weakcode)).Count() > 0).ToList<@object>();
            }
            else
            {
                var tem2 = projectDoc.list_of_objects.Select(pm => pm.id).ToList();
                projectDoc.list_of_protect_mech = projectDoc.list_of_protect_mech.Where<protect_mech>(p => _context.war_to_prot.Where(wtp => wtp.protcode == p.id && tem.Contains(wtp.warcode)).Count() > 0
                                                                                                        || _context.prot_to_obj.Where(pto => pto.protcode == p.id && tem2.Contains(pto.objcode)).Count() > 0
                                                                                                     ).ToList<protect_mech>();
                tem = projectDoc.list_of_protect_mech.Select(pm => pm.id).ToList();
                projectDoc.list_of_objects = projectDoc.list_of_objects.Where<@object>(o => _context.prot_to_obj.Where(pto => pto.objcode == o.id && tem.Contains(pto.protcode)).Count() > 0).ToList<@object>();
            }

            this.Close();
            if (fromShell)
            {
                ((DocWindow)((Grid)((DockingManager)sendForm).Parent).Parent).OpenGraphForm(projectDoc);
            } else
            {
                var d = new DocWindow(projectDoc);
                d.ShowDialog();
            }
        }
    }
}
