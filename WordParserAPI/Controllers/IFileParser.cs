using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordParserAPI.Controllers
{
    interface IFileParser
    {
        IDictionary<string, int> WordCount(string path);
       
    }
}
