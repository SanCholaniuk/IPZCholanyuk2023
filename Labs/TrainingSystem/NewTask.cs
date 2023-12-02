using Newtonsoft.Json;
using System;
using System.Collections;
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
    public partial class NewTask : Form
    {
        ListBox list2;
        string courseId;
        public NewTask(string id, ListBox list)
        {
            courseId = id;
            list2 = list;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = this.textBox1.Text;
            string task = this.textBox2.Text;
            if (name.Length == 0 || task.Length == 0)
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "createTask",
                        id = courseId,
                        name = name,
                        task = task

                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        var tmp = str.Split('=');
                        list2.Items.Add(new Var.ListBoxItem() { ID = tmp[0], Item = $"{tmp[1]}" });
                        MessageBox.Show("Завдання успішно створено!");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
        }
    }
}
