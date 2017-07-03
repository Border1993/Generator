using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Component
    {
        public bool GenerateRandom(List<Product> products, List<Component> currentComponents, string nazwaPotrawy)
        {
            List<int> availableIDS = new List<int>(products.Count);
            for(int i = 0; i < products.Count; i++)
            {
                bool found = false;

                foreach(var component in currentComponents)
                {
                    if (component.skladnikID == i)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) availableIDS.Add(i);
            }

            if (availableIDS.Count == 0) return false;

            int index = Tools.random.Next(0, availableIDS.Count);
            skladnikID = availableIDS[index];
            ilosc = products[skladnikID].GetAmount();
            jednostka = products[skladnikID].jednostka;
            this.nazwaPotrawy = nazwaPotrawy;
            return true;
        }

        public override string ToString()
        {
            return nazwaPotrawy +
                "|" + skladnikID + 
                "|" + ilosc + 
                "|" + jednostka;
        }

        public string nazwaPotrawy;
        public int skladnikID;
        public int ilosc;
        public string jednostka;
    }
}
