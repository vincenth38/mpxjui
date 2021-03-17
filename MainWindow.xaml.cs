using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using net.sf.mpxj;
using java.util;
using net.sf.mpxj.mspdi;

//using net.sf.mpxj.reader;

//using primavera reader
using net.sf.mpxj.primavera;
//doesn t work : using net.sf.mpxj.primavera.PrimaveraXERFileReader;

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
            //UniversalProjectReader reader = new UniversalProjectReader();
            
                PrimaveraXERFileReader reader = new PrimaveraXERFileReader();
                ProjectFile project = reader.read(file);
                //ProjectFile projectFile = reader.read(file);
               
               // ProjectReader reader = ProjectReaderUtility.getProjectReader(file); 
               // Map activities = reader.ActivityFieldMap;

              // activities.put(TaskField.TEXT10, "Activity_Id");
              // activities.put(TaskField.TEXT10, "task_code");
               //activities.put(TaskField.TEXT10, task.getActivityID());
             //  activities.setText(11, task.getActivityID());       
               
               //
// Read an Activity column called an_example_field and store it in TEXT10
//
//activityFieldMap.put(TaskField.TEXT10, task_code);



Map activities fieldTypeMap = reader.getFieldTypeMap();
fieldTypeMap.put("task_code", XerFieldType.STRING);
Map activities activityFieldMap = reader.getActivityFieldMap();
activityFieldMap.put(TaskField.TEXT10, "task_code");



             // CustomFieldContainer customFields = reader.getCustomFields();
              //CustomField field = customFields.getCustomField(TaskField.TEXT10);
              //field.setAlias("My Custom Field");
               
                
                MSPDIWriter writer = new MSPDIWriter();
                writer.write(project, file + ".xml");
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
