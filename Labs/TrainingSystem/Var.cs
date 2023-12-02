using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingSystem
{
    internal class Var
    {
        public static string Login;
        public static string User;

        public class ListBoxItem
        {
            public string ID { get; set; }
            public string Item { get; set; }
            public override string ToString() => Item;
        }
    }
}
