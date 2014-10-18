using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _445project2
{



    public class HotelSupplier
    {
        //variables

        private int lowestprice;
        private int price;
        private int newprice;
        private int hsid; //tells which hotel supplier
        
        private MultiCellBuffer gbuffer = new MultiCellBuffer();
        private int p = 10;//counter for pricecuts
        private String o;


        //objects
        //private OrderClass order;

        //event caller
        public delegate void priceCutEvent(int newprice, int oldprice, int hID);

        //event

        public static event priceCutEvent priceCut;

        public HotelSupplier(int id)//took out MultiBuffer buffer
        {
            this.hsid = id;
            //this.gbuffer = buffer;
            

        }

        public void setMultiCellBuffer(MultiCellBuffer buff)
        {
            gbuffer = buff;
        }
        public int getHSID()
        { return hsid; }

        public void runHotel()
        {
            //have it listen to buffer to make new order //maybe not here
            MultiCellBuffer.callOrder += new MultiCellBuffer.callOrderProcessEvent(this.tellHotel);

            // print out completion
            Console.WriteLine("Hotel {0} has been created", hsid);

            Random rnd = new Random();
            price = rnd.Next(200, 400);
            int var = rnd.Next(1, hsid + 3);
            price = price * (var + 5) / (var + 4);
            lowestprice = price;

            //price model
            int ld = 30;
            int ud = +15;

            while (p > 0)//only 10 pricecuts per hotel
            {
                while (price < ld)//if price becomes really small, this loop makes it easier for price cuts to happen
                {
                    ld = ld / 2;
                    ud = 1;
                }
                newprice = rnd.Next(price - ld, price + ud); //more likely to have a price cut 
                Console.WriteLine("Hotel {0} now has a price of {1}", hsid, newprice);
                if (newprice < lowestprice)//only does price cut if lowest price yet
                {
                    //Thread.Sleep(100);
                    Console.WriteLine("Hotel {0}'s price cut to {1}", hsid, newprice);
                    priceCut(newprice, lowestprice, hsid);
                    p--;

                    lowestprice = newprice;

                }

                price = newprice;
                //Thread.Sleep(20);


            }
            
            

            //done with price cuts
            
            

        }//done with run hotel thread


        public void tellHotel(int cellNum, int hotID)
        {
            Console.WriteLine("Order is being called at {0} to {1}",cellNum,hotID);
            string order;
            if(hotID == hsid)
            {
                order = gbuffer.getElement(cellNum);
                ProcessOrder(order);
            }
        }

        public void ProcessOrder(String order)
        {

            //get order, decode, start thread
            String o = order;
            Console.WriteLine("decoding");
            OrderClass deco = Decode(o);
            Console.WriteLine(deco.toString());

            
            //call order processing
            Thread orderprocessing = new Thread(new ParameterizedThreadStart(OrdProc));
             orderprocessing.Start(deco);
        }

        private static void OrdProc(object ord)
        {
            OrderProcessingThread op = new OrderProcessingThread((OrderClass)ord);
        }

        
        

        public OrderClass Decode(string o)
        {

            //takes string and decodes into an object
            char[] arrayChar = o.ToCharArray();

            StringBuilder o1 = new StringBuilder();
            StringBuilder o2 = new StringBuilder();
            StringBuilder o3 = new StringBuilder();
            StringBuilder o4 = new StringBuilder();
            StringBuilder o5 = new StringBuilder();

            int i = 0;
            int j = 0;
            Console.WriteLine(o);
            for (i = 0; i < arrayChar.Length; i++)
            {
                switch (j)
                {
                    case 0:
                        while (arrayChar[i] != '.')
                        {
                            o1.Append(arrayChar[i]);
                            i++;
                        }
                        break;
                    case 1:
                        while (arrayChar[i] != '.')
                        {
                            o2.Append(arrayChar[i]);
                            i++;
                        }
                        break;
                    case 2:
                        while (arrayChar[i] != '.')
                        {
                            o3.Append(arrayChar[i]);
                            i++;
                        }
                        break;
                    case 3:
                        while (arrayChar[i] != '.')
                        {
                            o4.Append(arrayChar[i]);
                            i++;
                        }
                        break;
                    case 4:
                        while (arrayChar[i] != '.')
                        {
                            o5.Append(arrayChar[i]);
                            i++;
                        }
                        break;
                }
                j++;
            }

            int hID = Convert.ToInt32(o1.ToString());
            int tID = Convert.ToInt32(o2.ToString());
            int creditCard = Convert.ToInt32(o3.ToString());
            int amount = Convert.ToInt32(o4.ToString());
            int rooms = Convert.ToInt32(o5.ToString());
            OrderClass order = new OrderClass(hID, tID, creditCard, amount, rooms);
            return order;

            /*order.sethID(hID);
            order.settID(tID);
            order.setCNum(creditCard);
            order.setamt(amount);
            order.setroom(rooms);
             */
        }

        public void encode(OrderClass order)
        {
            //converts object into string
            StringBuilder tmp = new StringBuilder();
            tmp.Append(order.gethID().ToString());
            tmp.Append(".");
            tmp.Append(order.gettID().ToString());
            tmp.Append(".");
            tmp.Append(order.getCNum().ToString());
            tmp.Append(".");
            tmp.Append(order.getamt().ToString());
            tmp.Append(".");
            tmp.Append(order.getroom().ToString());


            o = tmp.ToString();



        }
    }

}
