using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
namespace WordParserAPI.Controllers
{
    public class FileParser:IFileParser
    {
        private static readonly char[] separators = { ' ',',','.','/',';','!','\\','"'};

        public IDictionary<string, int> WordCount(string path)
        {
            var wordCnt = new Dictionary<string, int>();

            foreach (var line in File.ReadLines(path, Encoding.UTF8))
            {
                var wordsinFile = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in wordsinFile)
                {
                    int count;
                    wordCnt.TryGetValue(word, out count);
                    wordCnt[word] = count + 1;
                }
            }

            return wordCnt;
        }
    }
}