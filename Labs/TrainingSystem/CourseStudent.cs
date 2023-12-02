using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingSystem
{
    public partial class CourseStudent : Form
    {
        public CourseStudent(string id)
        {
            InitializeComponent();
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getCourseInfo",
                    id = id
                };
                Socket.ws.Send(JsonConvert.SerializeObject(data));

                var str = Socket.Data().Result;
                if (str != "")
                {
                    var course = str.Split('\n');
                    this.label1.Text = course[0];
                    this.textBox3.Text = id;
                    this.textBox2.Text = course[1];
                    if (course.Length == 3 && course[2] != "")
                    {
                        foreach (string item in course[2].Split('&'))
                        {
                            var tmp = item.Split('=');
                            this.listBox2.Items.Add(new Var.ListBoxItem() { ID = tmp[0], Item = $"{tmp[1]}" });
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            var id = this.textBox3.Text;
            var taskId = ((Var.ListBoxItem)listBox2.SelectedItem).ID;
            TaskStudent form = new TaskStudent(id, taskId);
            form.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
