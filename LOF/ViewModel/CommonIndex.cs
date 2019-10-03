using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOF.ViewModel
{
    public class CommonIndex
    {
        public IEnumerable<LOF.Foundtbl> found { get; set; }


        public IEnumerable<LOF.Losttbl> lost { get; set; }
    }
}