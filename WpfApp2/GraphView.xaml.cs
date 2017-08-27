using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Forms;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для GraphView.xaml
    /// </summary>
    public partial class GraphView : System.Windows.Controls.UserControl
    {
        Graph currentGraph;
        ProjectDoc currentDoc;

        public GraphView()
        {
            InitializeComponent();
        }


        public void CreateGraph(ProjectDoc projectDoc)
        {
            currentDoc = projectDoc;
            currentGraph = new Graph(projectDoc.name);
            currentGraph.Directed = false;
            currentGraph.Attr.LayerDirection = LayerDirection.LR;




            //form matrix
            //search max connect value
            int maxConnectCount = 0; 
            foreach (var war in projectDoc.list_of_warnings)
            {
                var tem = projectDoc.list_of_protect_mech.Select(pm => pm.id).ToList();
                int currentConnectCount = _context.war_to_prot.Where(wtp => wtp.warcode == war.id &&
                    tem.Contains(wtp.protcode)).Count<war_to_prot>();
                if (maxConnectCount < currentConnectCount)
                    maxConnectCount = currentConnectCount;
            }
            var dt = new DataTable();
            for (var i = 0; i < maxConnectCount; i++)
                dt.Columns.Add(new DataColumn("c" + i, typeof(double)));
            for (var i = 0; i < projectDoc.list_of_warnings.Count; i++)
            {
                var r = dt.NewRow();
                dt.Rows.Add(r);
                for (var c = 0; c < maxConnectCount; c++)
                    r[c] = 0;
            }
            
            foreach (var war in projectDoc.list_of_warnings)
                foreach (var prot in projectDoc.list_of_protect_mech)
                {
                    if (_context.war_to_prot.Where(wtp => wtp.warcode == war.id && wtp.protcode == prot.id).Count<war_to_prot>() > 0)
                    {
                        currentGraph.AddEdge(war.name,
                            _context.war_to_prot.Where(wtp => wtp.warcode == war.id && wtp.protcode == prot.id).ToList<war_to_prot>()[0].value.Value.ToString(),
                            prot.name);
                        dt.Rows[projectDoc.list_of_warnings.IndexOf(war)][projectDoc.list_of_protect_mech.IndexOf(prot)] = _context.war_to_prot.Where(wtp => wtp.warcode == war.id && wtp.protcode == prot.id).ToList<war_to_prot>()[0].value.Value.ToString();
                    }
                }

            dg1.DataContext = dt;
            maxConnectCount = 0;

            var resultDataTable = new DataTable();

            if (projectDoc.calc_type == 1)
            {

                foreach (var prot in projectDoc.list_of_protect_mech)
                {
                    var tem = projectDoc.list_of_barrier.Select(pm => pm.id).ToList();
                    int currentConnectCount = _context.prot_to_bar.Where(ptb => ptb.protcode == prot.id &&
                        tem.Contains(ptb.barcode)).Count<prot_to_bar>();
                    if (maxConnectCount < currentConnectCount)
                        maxConnectCount = currentConnectCount;
                }
                var dt2 = new DataTable();
                for (var i = 0; i < maxConnectCount; i++)
                    dt2.Columns.Add(new DataColumn("c" + i, typeof(double)));
                for (var i = 0; i < projectDoc.list_of_protect_mech.Count; i++)
                {
                    var r = dt2.NewRow();
                    dt2.Rows.Add(r);
                    for (var c = 0; c < maxConnectCount; c++)
                        r[c] = 0;
                }

                foreach (var prot in projectDoc.list_of_protect_mech)
                    foreach (var bar in projectDoc.list_of_barrier)
                    {
                        if (_context.prot_to_bar.Where(ptb => ptb.protcode == prot.id && ptb.barcode == bar.id).Count<prot_to_bar>() > 0)
                        {
                            currentGraph.AddEdge(prot.name,
                                _context.prot_to_bar.Where(ptb => ptb.protcode == prot.id && ptb.barcode == bar.id).ToList<prot_to_bar>()[0].value.Value.ToString(),
                                bar.name);
                            dt2.Rows[projectDoc.list_of_protect_mech.IndexOf(prot)][projectDoc.list_of_barrier.IndexOf(bar)] = _context.prot_to_bar.Where(ptb => ptb.protcode == prot.id && ptb.barcode == bar.id).ToList<prot_to_bar>()[0].value.Value.ToString();
                        }
                    }

                dg2.DataContext = dt2;
                maxConnectCount = 0;

                var multy = Multiplication(To2DArray(dt), To2DArray(dt2));
                var dt5 = ToDataTable(multy);
                dg5.DataContext = dt5;

                foreach (var bar in projectDoc.list_of_barrier)
                {
                    var tem = projectDoc.list_of_weakness.Select(pm => pm.id).ToList();
                    int currentConnectCount = _context.bar_to_weak.Where(btw => btw.barcode == bar.id &&
                        tem.Contains(btw.weakcode)).Count<bar_to_weak>();
                    if (maxConnectCount < currentConnectCount)
                        maxConnectCount = currentConnectCount;
                }
                var dt3 = new DataTable();
                for (var i = 0; i < maxConnectCount; i++)
                    dt3.Columns.Add(new DataColumn("c" + i, typeof(double)));
                for (var i = 0; i < projectDoc.list_of_barrier.Count; i++)
                {
                    var r = dt3.NewRow();
                    dt3.Rows.Add(r);
                    for (var c = 0; c < maxConnectCount; c++)
                        r[c] = 0;
                }

                foreach (var bar in projectDoc.list_of_barrier)
                    foreach (var weak in projectDoc.list_of_weakness)
                    {
                        if (_context.bar_to_weak.Where(btw => btw.barcode == bar.id && btw.weakcode == weak.id).Count<bar_to_weak>() > 0)
                        {
                            currentGraph.AddEdge(bar.name,
                                _context.bar_to_weak.Where(btw => btw.barcode == bar.id && btw.weakcode == weak.id).ToList<bar_to_weak>()[0].value.Value.ToString(),
                                weak.name);
                            dt3.Rows[projectDoc.list_of_barrier.IndexOf(bar)][projectDoc.list_of_weakness.IndexOf(weak)] = _context.bar_to_weak.Where(btw => btw.barcode == bar.id && btw.weakcode == weak.id).ToList<bar_to_weak>()[0].value.Value.ToString();
                        }
                    }

                dg3.DataContext = dt3;
                maxConnectCount = 0;

                multy = Multiplication(To2DArray(dt5), To2DArray(dt3));
                var dt6 = ToDataTable(multy);
                dg6.DataContext = dt6;

                foreach (var weak in projectDoc.list_of_weakness)
                {
                    var tem = projectDoc.list_of_objects.Select(pm => pm.id).ToList();
                    int currentConnectCount = _context.weak_to_object.Where(wto => wto.weakcode == weak.id &&
                        tem.Contains(wto.objcode)).Count<weak_to_object>();
                    if (maxConnectCount < currentConnectCount)
                        maxConnectCount = currentConnectCount;
                }
                var dt4 = new DataTable();
                for (var i = 0; i < maxConnectCount; i++)
                    dt4.Columns.Add(new DataColumn("c" + i, typeof(double)));
                for (var i = 0; i < projectDoc.list_of_weakness.Count; i++)
                {
                    var r = dt4.NewRow();
                    dt4.Rows.Add(r);
                    for (var c = 0; c < maxConnectCount; c++)
                        r[c] = 0;
                }

                foreach (var weak in projectDoc.list_of_weakness)
                    foreach (var obje in projectDoc.list_of_objects)
                    {
                        if (_context.weak_to_object.Where(wto => wto.weakcode == weak.id && wto.objcode == obje.id).Count<weak_to_object>() > 0)
                        {
                            currentGraph.AddEdge(weak.name,
                                _context.weak_to_object.Where(wto => wto.weakcode == weak.id && wto.objcode == obje.id).ToList<weak_to_object>()[0].value.Value.ToString(),
                                obje.name);
                            dt4.Rows[projectDoc.list_of_weakness.IndexOf(weak)][projectDoc.list_of_objects.IndexOf(obje)] = _context.weak_to_object.Where(wto => wto.weakcode == weak.id && wto.objcode == obje.id).ToList<weak_to_object>()[0].value.Value.ToString();
                        }
                    }

                dg4.DataContext = dt4;
                maxConnectCount = 0;

                multy = Multiplication(To2DArray(dt6), To2DArray(dt4));
                var dt7 = ToDataTable(multy);
                dg7.DataContext = dt7;

                resultDataTable = dt7;
            }
            else
            {
                foreach (var prot in projectDoc.list_of_protect_mech)
                {
                    var tem = projectDoc.list_of_objects.Select(pm => pm.id).ToList();
                    int currentConnectCount = _context.prot_to_obj.Where(pto => pto.protcode == prot.id &&
                        tem.Contains(pto.objcode)).Count<prot_to_obj>();
                    if (maxConnectCount < currentConnectCount)
                        maxConnectCount = currentConnectCount;
                }
                var dt2 = new DataTable();
                for (var i = 0; i < maxConnectCount; i++)
                    dt2.Columns.Add(new DataColumn("c" + i, typeof(double)));
                for (var i = 0; i < projectDoc.list_of_protect_mech.Count; i++)
                {
                    var r = dt2.NewRow();
                    dt2.Rows.Add(r);
                    for (var c = 0; c < maxConnectCount; c++)
                        r[c] = 0;
                }

                foreach (var prot in projectDoc.list_of_protect_mech)
                    foreach (var obj in projectDoc.list_of_objects)
                    {
                        if (_context.prot_to_obj.Where(pto => pto.protcode == prot.id && pto.objcode == obj.id).Count<prot_to_obj>() > 0)
                        {
                            currentGraph.AddEdge(prot.name,
                                _context.prot_to_obj.Where(pto => pto.protcode == prot.id && pto.objcode == obj.id).ToList<prot_to_obj>()[0].value.Value.ToString(),
                                obj.name);
                            dt2.Rows[projectDoc.list_of_protect_mech.IndexOf(prot)][projectDoc.list_of_objects.IndexOf(obj)] = _context.prot_to_obj.Where(pto => pto.protcode == prot.id && pto.objcode == obj.id).ToList<prot_to_obj>()[0].value.Value.ToString();
                        }
                    }

                dg2.DataContext = dt2;
                maxConnectCount = 0;

                var multy = Multiplication(To2DArray(dt), To2DArray(dt2));
                var dt5 = ToDataTable(multy);
                dg5.DataContext = dt5;

                resultDataTable = dt5;
            }

            var list_of_max = new List<double>();
            // в каждой строке ищем максимальное 
            var damageTable = projectDoc.list_of_warnings;
            for (int i = 0; i < resultDataTable.Rows.Count; i++)
            {
                var max_in_row = 0.0;
                for (int j = 0; j < resultDataTable.Columns.Count; j++)
                {
                    if (max_in_row<(double)(resultDataTable.Rows[i][j]))
                    {
                        max_in_row = (double)(resultDataTable.Rows[i][j]);
                    }
                }
                list_of_max.Add(max_in_row);
                damageTable[i].damage_cost= damageTable[i].damage_cost * max_in_row;
                max_in_row = 0;
            }
            double result_mark = 0;
            for (int i = 0; i < projectDoc.list_of_warnings.Count; i++)
            {
                result_mark += (double)damageTable[i].damage_cost;//list_of_max[i] * (double)(projectDoc.list_of_warnings[i].damage_cost==null?0: projectDoc.list_of_warnings[i].damage_cost);
            }

            if (result_mark != 0)
            {
                result_mark = 1 / result_mark;
                result_label.Content = result_mark;
            }
            else
                result_label.Content = "ОШИБКА";

            dg11.DataContext = damageTable.Select(s => new { s.name, s.damage_cost });


        }

        private DataTable ToDataTable(double[,] array)
        {
            var dataTable = new DataTable();
            for (var i = 0; i < array.GetLength(1); i++)
                dataTable.Columns.Add(new DataColumn("c" + i, typeof(double)));
            for (var i = 0; i < array.GetLength(0); i++)
            {
                var r = dataTable.NewRow();
                dataTable.Rows.Add(r);
                for (var c = 0; c < array.GetLength(1); c++)
                    r[c] = array[i,c];
            }

            return dataTable;
        }

        private double[,] To2DArray(DataTable dt)
        {
            int rowCount = dt.Rows.Count;
            int columnCount = dt.Columns.Count;

            double[,] arr = new double[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    arr[i, j] = (double)dt.Rows[i][j];
                }
            }
            return arr;
        }

        private double[,] Multiplication(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        r[i, j] = (a[i, k] * b[k, j]) > r[i, j] ? (a[i, k] * b[k, j]) : r[i, j];
                    }

                }
            }
            return r;
        }

        private ProjectBaseEntities _context = new ProjectBaseEntities();



        private bool CheckConnection(int id1, int id2, string table1, string table2)
        {
            var result = true;
            
            return result;
        }

        private void WindowsFormsHost1_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.ggViewer.Graph = currentGraph;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*_context.Configuration.ProxyCreationEnabled = false;
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var ser = new XmlSerializer(typeof(ProjectDoc));
                var writer = new StreamWriter(saveFileDialog1.FileName);
                ser.Serialize(writer, currentDoc);
            }
            _context.Configuration.ProxyCreationEnabled = true;*/


            var earlyBoundSerializer = new DataContractSerializer(typeof(ProjectDoc));

            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    // Write the XML to disk.

                    earlyBoundSerializer.WriteObject(file, currentDoc);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ProjectWindow().ShowProject(this.Parent, currentDoc);
        }
    }
}
