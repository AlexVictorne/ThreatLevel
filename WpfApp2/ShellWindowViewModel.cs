using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace WpfApp2
{
    class ShellWindowViewModel
    {
        public ObservableCollection<object> Docs { get; set; }


        public ShellWindowViewModel()
        {
            Docs = new ObservableCollection<object>();
        }


        private RelayCommand openProjectsView;
        public RelayCommand OpenProjectsView
        {
            get
            {
                return openProjectsView ??
                    (openProjectsView = new RelayCommand(
                        param => this.AddNewDocument(typeof(ProjectsView), "Просмотр проектов", (DockingManager)param)
                    ));
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
                        new ProjectWindow().ShowProject((DockingManager)obj);
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
                        LoadProjectFile((DockingManager)obj);
                    }));
            }
        }



        private RelayCommand openBarrierDirectory;
        public RelayCommand OpenBarrierDirectory
        {
            get
            {
                return openBarrierDirectory ??
                    (openBarrierDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(BarrierDirectory), "Справочник. Барьеры", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openIndustObjectDirectory;
        public RelayCommand OpenIndustObjectDirectory
        {
            get
            {
                return openIndustObjectDirectory ??
                    (openIndustObjectDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(IndustObjectDirectory), "Справочник. Индустриальные объекты", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openProtectMechDirectory;
        public RelayCommand OpenProtectMechDirectory
        {
            get
            {
                return openProtectMechDirectory ??
                    (openProtectMechDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(ProtectMechDirectory), "Справочник. Механизмы защиты", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openObjectDirectory;
        public RelayCommand OpenObjectDirectory
        {
            get
            {
                return openObjectDirectory ??
                    (openObjectDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(ObjectDirectory), "Справочник. Объекты", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openWarningTypeDirectory;
        public RelayCommand OpenWarningTypeDirectory
        {
            get
            {
                return openWarningTypeDirectory ??
                    (openWarningTypeDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(WarningTypeDirectory), "Справочник. Типы угроз", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openWarningsDirectory;
        public RelayCommand OpenWarningsDirectory
        {
            get
            {
                return openWarningsDirectory ??
                    (openWarningsDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(WarningsDirectory), "Справочник. Угрозы", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openWeaknessDirectory;
        public RelayCommand OpenWeaknessDirectory
        {
            get
            {
                return openWeaknessDirectory ??
                    (openWeaknessDirectory = new RelayCommand(
                        param => this.AddNewDocument(typeof(WeaknessDirectory), "Справочник. Уязвимости", (DockingManager)param)
                    ));
            }
        }

        private RelayCommand openGraphView;
        public RelayCommand OpenGraphView
        {
            get
            {
                return openGraphView ??
                    (openGraphView = new RelayCommand(
                        param => this.AddNewDocument(typeof(GraphView), "Просмотр графа безопасности", (DockingManager)param)

                    ));
            }
        }

        private RelayCommand openGraphViewWithDoc;
        public RelayCommand OpenGraphViewWithDoc
        {
            get
            {
                return openGraphView ??
                    (openGraphView = new RelayCommand(
                        param=> this.AddNewDocumentWithFile(typeof(GraphView), "Просмотр графа безопасности", (DockingManager)((List<object>)param)[0], (ProjectDoc)((List<object>)param)[1])

                    ));
            }
        }


        private void CommandWithAParameter(object state)
        {
            AddNewDocument(typeof(BarrierDirectory), "THISISSISI", (DockingManager)state);
        }

        private void AddNewDocument(Type type, string title, DockingManager dockingManager)
        {
            var documentPane = dockingManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (documentPane != null)
            {
                var newDocument = Activator.CreateInstance(type);
                var newDocumentLayout = new LayoutDocument
                {
                    Title = title,
                    Content = newDocument
                };
                documentPane.Children.Add(newDocumentLayout);
            }
        }

        private void AddNewDocumentWithFile(Type type, string title, DockingManager dockingManager, ProjectDoc projectDoc)
        {
            var documentPane = dockingManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (documentPane != null)
            {
                var newDocument = Activator.CreateInstance(type);
                var newDocumentLayout = new LayoutDocument
                {
                    Title = title,
                    Content = newDocument
                };
                ((GraphView)newDocumentLayout.Content).CreateGraph(projectDoc);
                documentPane.Children.Add(newDocumentLayout);
            }
        }

        private void LoadProjectFile(DockingManager dockingManager)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var earlyBoundSerializer = new DataContractSerializer(typeof(ProjectDoc));
                ProjectDoc pDoc;
                using (var file = new FileStream(openFileDialog1.FileName, FileMode.Open))
                {
                    pDoc = (ProjectDoc)earlyBoundSerializer.ReadObject(file);
                }
                new ProjectWindow().ShowProject(dockingManager,pDoc);
            }
        }

        
    }
}
