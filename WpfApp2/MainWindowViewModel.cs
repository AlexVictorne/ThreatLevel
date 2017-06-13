using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class MainWindowViewModel
    {
        private RelayCommand informationControl;
        public RelayCommand InformationControl
        {
            get
            {
                return informationControl ??
                    (informationControl = new RelayCommand(obj =>
                    {
                        new DocWindow().ShowDialog();
                    }));
            }
        }
        private RelayCommand createNewProject;
        public RelayCommand CreateNewProject
        {
            get
            {
                return createNewProject ??
                    (createNewProject = new RelayCommand(obj =>
                     {
                         try
                         {
                             var t = new ProjectWindow();
                             t.ShowDialog();
                         }
                         catch (Exception e)
                         {
                             System.Windows.Forms.MessageBox.Show(e.Message);
                         }
                     }));
            }
        }
        private RelayCommand loadProject;
        public RelayCommand LoadProject
        {
            get
            {
                return loadProject ??
                    (loadProject = new RelayCommand(obj =>
                    {
                        OpenShellWithProjects();
                    }));
            }
        }

        private void OpenShellWithProjects()
        {
            try
            {
                var d = new DocWindow(true);
                d.ShowDialog();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }
    }
}
