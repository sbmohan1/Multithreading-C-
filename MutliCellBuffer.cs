using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _445project2
{
    struct cell
    {
        public String sobj;
        public int hsid;
        public bool filled;
    };
    public class MultiCellBuffer
    {
        public delegate void callOrderProcessEvent(int cellNum, int hsid);

        public static event callOrderProcessEvent callOrder;

        public HotelSupplier[] hotels;
        cell c1;
        cell c2;
        cell c3;

        public MultiCellBuffer()
        {
            
            c1 = new cell();
            c1.filled = false;
            c2 = new cell();
            c2.filled = false;
            c3 = new cell();

        }

        public void setHotel(HotelSupplier[] h)
        {
            hotels = h;
        }

        const int cells = 3;
        int Elements = 0;
        //String[] buffer = new String[cells];
        //int[] sendto = new int[cells];

        public Semaphore write = new Semaphore(2, 2);
        public Semaphore read = new Semaphore(2, 2);


        public void addElement(String obj, int hid)
        {
            write.WaitOne();
            int cellNum = 0;
            Console.WriteLine("Thread : " + Thread.CurrentThread.Name + "Entered Write");
            bool stored = false;

            lock (this)
            {
                Console.WriteLine("locking");
                while (Elements == cells)
                {
                    
                    Monitor.Wait(this);
                }

                Console.WriteLine("{0}", cells);

                if (!stored && !c1.filled) //if hasnt stored anything and c1 is empty
                {
                    stored = true;
                    c1.filled = true;
                    c1.hsid = hid;
                    c1.sobj = obj;
                    Elements++;
                    cellNum = 1;
                }
                else
                {
                    if (!stored && !c2.filled)
                    {
                        stored = true;
                        c2.filled = true;
                        c2.hsid = hid;
                        c2.sobj = obj;
                        Elements++;
                        cellNum = 2;

                    }
                    else
                        if (!stored && !c3.filled)
                        {
                            stored = true;
                            c3.filled = true;
                            c3.hsid = hid;
                            c3.sobj = obj;
                            Elements++;
                            cellNum = 3;
                        }
                        else
                            Console.WriteLine("error");
                    
                }

                Console.WriteLine("hotel {0} wrote to {1}", obj, cellNum);
                Console.WriteLine("Thread : " + Thread.CurrentThread.Name + "Leaving Write");
                callOrder(cellNum, hid);
                write.Release();
                Monitor.Pulse(this);
                Console.WriteLine("unlocking");
            }
        }





        //see if the buffer contains the id of the hotels that are trying to read


        public string getElement(int cellNum)
        {
            string sendback = "error"; //you dont want yomomma aka nothing gets written to it
            read.WaitOne();
            Console.WriteLine("{0}, is being read {1} elemnts", cellNum, Elements);

            if (cellNum == 1)
            {
                
                lock (this)
                {
                    Console.WriteLine("cellnum1");
                    while (Elements == 0)
                    {
                        Console.WriteLine("cellnumwhileloop");
                        Monitor.Wait(this);
                    }
                    //3 hotel threads
                    Console.WriteLine("cellnum1after while");
                    sendback = c1.sobj;
                    c1.filled = false;
                    c1.sobj = "error";//this hopefully won't ever be read
                    c1.hsid = 999;
                    Elements--;
                    Monitor.Pulse(this);
                    read.Release();

                    Console.WriteLine("cellnum1end");
                
                }
            }
            if (cellNum == 2)
            {
                lock (this)
                {
                    while (Elements == 0)
                    {
                        Monitor.Wait(this);
                    }
                    //3 hotel threads

                    sendback = c2.sobj;
                    c2.filled = false;
                    c2.sobj = "error";//this hopefully won't ever be read
                    c2.hsid = 999;
                    Elements--;
                    Monitor.Pulse(this);
                    read.Release();

                }
            }
            if (cellNum == 3)
            {
                lock (this)
                {
                    while (Elements == 0)
                    {
                        Monitor.Wait(this);
                    }
                    //3 hotel threads

                    sendback = c3.sobj;
                    c3.filled = false;
                    c3.sobj = "error";//this hopefully won't ever be read
                    c3.hsid = 999;
                    Elements--;
                    Monitor.Pulse(this);
                    read.Release();

                }
            }

            Console.WriteLine("{0} read from {1} leaving read",sendback,cellNum );
            return sendback;
        }

    }//end buffer class

}//end namespace
