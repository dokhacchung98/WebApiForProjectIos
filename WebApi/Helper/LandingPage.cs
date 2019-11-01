using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Helper
{
    public class LandingPage<T>
    {
        public static IList<T> LandingList(IList<T> list, int size, int? page)
        {
            var myList = new List<T>();
            int startIndex = size * page.Value;


            return list;
        }
    }
}