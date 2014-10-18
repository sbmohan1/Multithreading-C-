using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _445project2
{
    class Program
    {
        const int hsize = 3;
        const int tasize = 5;
        public static HotelSupplier[] Hotels = new HotelSupplier[hsize];
        public static MultiCellBuffer mb = new MultiCellBuffer();
        
        

        static void Main(string[] args)
        {
            //Create and put Suppliers and Agencies into corresponding arrays
            //Also create array of threads for hotels and ta's

           
            

            for (int i = 0; i < hsize; i++)
            {
                Hotels[i] = new HotelSupplier(i);
                Hotels[i].setMultiCellBuffer(mb);
            }
            mb.setHotel(Hotels);

            TravelAgency[] Agencies = new TravelAgency[tasize];
            Thread[] orderprocessing = new Thread[tasize];

            for (int i = 0; i < tasize; i++)
            {
                Agencies[i] = new TravelAgency(i, Hotels);
                HotelSupplier.priceCut += new HotelSupplier.priceCutEvent(Agencies[i].priceCutListener);
                
                            
            }


            Thread[] hthreads = new Thread[hsize];
            Thread[] tathreads = new Thread[tasize];

            for (int i = 0; i < hsize; i++)
            {
                hthreads[i] = new Thread(new ThreadStart(Hotels[i].runHotel));
                hthreads[i].Start();
            }
            for (int i = 0; i < tasize; i++)
            {
                tathreads[i] = new Thread(new ThreadStart(Agencies[i].runAgency));
                tathreads[i].Start();
            }

            for (int i = 0; i < tasize; i++)
            { tathreads[i].Join(); }

            for (int i = 0; i < hsize; i++)
            { hthreads[i].Join(); }
           Console.ReadKey();
        }
    }
}
