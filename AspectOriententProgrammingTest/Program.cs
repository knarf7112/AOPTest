using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspectOriententProgrammingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Order order = new Order();
            IOrder order = factory.GetOrderInstance();//輸入false測試不啟用Log
            order.Update("91", "knock");
            order.Delete("92");

            IOrder order2 = factory.GetOrderInstance();//輸入true測試啟用Log
            order2.Update("91", "knock");
            order2.Delete("92");
            Console.ReadKey();
        }
    }
    #region 未要求加log(記錄或功能)的時候
    // 1.首先在建立一個 Order 的類別,有 Update 與 Delete 的方法,在 Main() 裡面呼叫 Order 的 Update()  與 Delete() 
    //public class Order
    //{
    //    public int Update(string id, string name)
    //    {
    //        Console.WriteLine("Update order, id={0}, name={1}", id, name);
    //        return 1;
    //    }
    //    public void Delete(string id)
    //    {
    //        Console.WriteLine("Delete order, id={0}", id);
    //    }
    //}
    #endregion


    #region 2. 需求異動－希望在 Update() 與 Delete() 之後加上 Log
    #region 2-1 所以加一個IOrder介面來界接方法
    public interface IOrder
    {
        int Update(string id, string name);
        void Delete(string id);
    }
    #endregion
    public class Order : IOrder
    {

        public int Update(string id, string name)
        {
            Console.WriteLine("Update order, id={0}, name={1}", id, name);
            return 1;
        }

        public void Delete(string id)
        {
            Console.WriteLine("Delete order, id={0}", id);
        }
    }
    #region 3.並建立一個Log物件並實作IOrder且將原物件參考丟入並實作其他方
    //(應該算是在原來的實作功能再包一層並附上要記錄的方式或要另外操作的功能)
    public class LogOrder : IOrder
    {
        private IOrder _order;
        public LogOrder(IOrder order)
        {
            this._order = order;//將原來的物件丟入到此參考
        }
        //執行原物件參考方法並且再加入自己要異動的功能(因實作同樣的介面所以方法一樣)
        public int Update(string id, string name)
        {
            Console.WriteLine("加料前的Update====看要記錄還是要做啥==");
            var result = this._order.Update(id, name);
            Console.WriteLine("加料後的Update====看要再做啥動作======");
            return result;
        }

        public void Delete(string id)
        {
            Console.WriteLine("加料前的Delete====看要記錄還是要做啥==");
            this._order.Delete(id);
            Console.WriteLine("加料後的Delete====看要再做啥動作======");
        }
    }
    #endregion
    
    #region 4.動態決定要不要加上Log
    //將IOrder的生成方式封裝到簡單工廠
    public class factory
    {
        public static IOrder GetOrderInstance()
        {
            Console.WriteLine("請輸入true或false，決定是否啟用Log");
            var isLogEnabled = Boolean.Parse(Console.ReadLine());
            if (isLogEnabled)
            {
                return new LogOrder(new Order());//輸入為true的話,回傳記錄Log的物件
            }
            else
            {
                return new Order();//輸入為false的話,回傳不記錄Log的物件
            }
        }
    }
    #endregion
    //程式碼說明：
    //1.新增一個 IOrder 的 interface, 讓 context 相依於 interface。並讓 Order 實作 IOrder 。
    //2.新增一個 LogOrder 的 class 來裝飾 Order ，並實作 IOrder ，於 Update() 與 Delete() 中，記錄 log 。
    //3.Context 端，也就是 Main() 中，原本直接 new Order() 的部分，改為初始化 LogOrder ，並傳入 Order 的 instance, 代表用 Log 裝飾這個 Order 物件。

    #endregion
}
