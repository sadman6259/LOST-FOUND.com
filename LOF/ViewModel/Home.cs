using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LOF;
using PagedList;

namespace LOF.ViewModel
{
    public class Home
    {
        public IEnumerable< LOF.Topfoundtbl> Topfound { get; set; }


        public IEnumerable<LOF.TopLosttbl> Toplost { get; set; }

    }
}