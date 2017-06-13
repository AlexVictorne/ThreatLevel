using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
    /// Логика взаимодействия для ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : UserControl
    {
        List<ProjectDoc> pd;

        public ProjectsView()
        {

            InitializeComponent();
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    var list_of_projects = new List<ProjectDoc>();

                    var earlyBoundSerializer = new DataContractSerializer(typeof(ProjectDoc));

                    foreach (var fileName in files)
                        using (var file = new FileStream(fileName, FileMode.Open))
                            list_of_projects.Add((ProjectDoc)earlyBoundSerializer.ReadObject(file));

                    pd = list_of_projects;

                    var dt = new DataTable();
                    dt.Columns.Add("Наименование");
                    dt.Columns.Add("Тип объекта");
                    dt.Columns.Add("Описание");
                    dt.Columns.Add("Тип расчета");
                    foreach (var item in list_of_projects)
                    {
                        var r = dt.NewRow();
                        dt.Rows.Add(r);
                        r[0] = item.name;
                        r[1] = item.object_type;
                        r[2] = item.description;
                        r[3] = item.calc_type == 0 ? "3 звена" : "5 звеньев";
                    }

                    dg.ItemsSource = list_of_projects;
                }
                else
                    return;
            }
            
            
        }

        private void dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ProjectWindow().ShowProject(this.Parent, (ProjectDoc)dg.CurrentItem);
        }
    }
}
