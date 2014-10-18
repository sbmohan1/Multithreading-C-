using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _445project2
{
    public class OrderClass
    {

        private int hID;
        private int tID;
        private int creditCard;
        private int amount;
        private int rooms;

        public OrderClass(int h, int t, int cc, int amt, int rm)
        {
            hID = h;
            tID = t;
            creditCard = cc;
            amount = amt;
            rooms = rm;
        }
        public void sethID(int iD)
        {
            hID = iD;
        }

        public int gethID()
        {
            return hID;
        }
        public void settID(int iD)
        {
            tID = iD;
        }
        public int gettID()
        {
            return tID;
        }

        public void setCNum(int id)
        {
            creditCard = id;

        }

        public int getCNum()
        {
            return creditCard;
        }

        public void setamt(int dolla)
        {
            amount = dolla;
        }

        public int getamt()
        {
            return amount;
        }

        public void setroom(int iD)
        {
            rooms = iD;
        }

        public int getroom()
        {
            return rooms;
        }

        public string toString()
        {
            return hID + "." + tID + "." + creditCard + "." + amount + "." + rooms + ".";

        }
    }
}
