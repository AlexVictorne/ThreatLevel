using System;
using System.Collections.Generic;
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
namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for DocWindow.xaml
    /// </summary>
    public partial class DocWindow : Window
    {
        public DocWindow()
        {
            InitializeComponent();
            DataContext = new ShellWindowViewModel();
        }

        public DocWindow(bool isdoc)
        {
            InitializeComponent();
            DataContext = new ShellWindowViewModel();
            ((ShellWindowViewModel)DataContext).OpenProjectsView.Execute(DockingManager);

        }

        public DocWindow(ProjectDoc projectDoc)
        {
            InitializeComponent();
            DataContext = new ShellWindowViewModel();
            List<object> paramList = new List<object>();
            paramList.Add(DockingManager);
            paramList.Add(projectDoc);
            ((ShellWindowViewModel)DataContext).OpenGraphViewWithDoc.Execute(paramList);
        }

        public void OpenGraphForm(ProjectDoc projectDoc)
        {
            List<object> paramList = new List<object>();
            paramList.Add(DockingManager);
            paramList.Add(projectDoc);
            ((ShellWindowViewModel)DataContext).OpenGraphViewWithDoc.Execute(paramList);
        }
    }
}
