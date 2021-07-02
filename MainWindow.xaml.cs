using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using net.sf.mpxj;
using java.util;
using net.sf.mpxj.reader;
using net.sf.mpxj.mspdi;
using net.sf.mpxj.primavera;

namespace MPXJUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private string fileName = "Please drop your .xer file";
        public string File
        {
            get
            {
                return fileName;
            }

            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                File = files[0];
                HandleFileOpen(files[0]);
            }
        }

        private int HandleFileOpen(string file)
        {
            try
            {
                PrimaveraXERFileReader reader = new PrimaveraXERFileReader();
                //ProjectReader reader = ProjectReaderUtility.getProjectReader(file); 
                Map activities = reader.ActivityFieldMap;

                activities.put(TaskField.TEXT10, "task_code");
                setFieldByAlias("task_code",TaskField.TEXT10);
                activities.put(TaskField.TEXT11, "act_work_qty");
                activities.put(TaskField.TEXT12, "remain_work_qty");
                activities.put(TaskField.TEXT13, "target_work_qty");
                activities.put(TaskField.TEXT14, "target_drtn_hr_cnt");
                activities.put(TaskField.TEXT15, "target_equip_qty");
                activities.put(TaskField.TEXT16, "act_equip_qty");
                activities.put(TaskField.TEXT17, "remain_equip_qt");
                activities.put(TaskField.TEXT18, "comments");
                activities.put(TaskField.TEXT19, "BCR#");
                
                Map customFields = reader.getCustomFields();
                field = customFields.getCustomField(TaskField.TEXT10);
                field.setAlias("task_code");
                customFields.getCustomField(TaskField.TEXT11).setAlias("task_code2");
                        
                 ProjectFile projectFile = reader.read(file);
 
               //  Map customFields = projectFile.getCustomFields;
                //customFields.getCustomField(TaskField.TEXT1).setAlias("Code");

      
     // CustomFieldContainer customFields = projectFile.getCustomFields();
    //  customFields.getCustomField(TaskField.TEXT1).setAlias("Code");

                
                
                MSPDIWriter writer = new MSPDIWriter();
                foreach (Task task in projectFile.Tasks)
                {
                    task.Name = task.ActivityID + "_" + task.Name;
                }
                
                writer.write(projectFile, file + ".xml");
                File = "Done";
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
