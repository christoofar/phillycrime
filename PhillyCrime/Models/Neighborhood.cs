using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace PhillyBlotter.Models
{

    public class Hood : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public int ID { get; set; }
    }

    public class Neighborhoods : IEnumerable<Hood>
    {

        private List<Hood> hoods = new List<Hood>();

        public Neighborhoods()
        {
            foreach (Neighborhood hood in Global.Neighborhoods)
            {
                hoods.Add(new Hood { ID = hood.ID, Name = hood.Name });
            }
        }

        public IEnumerator<Hood> GetEnumerator()
        {
            return hoods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return hoods.GetEnumerator();
        }
    }

}
