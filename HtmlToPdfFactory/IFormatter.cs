using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdfFactory
{
    public interface IFormatter // Need implement it, format correspond on get html string;
    {
        string Format(string value);
    }
}
