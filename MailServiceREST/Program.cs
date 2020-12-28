using System;
using System.Collections.Generic;

namespace MailServiceREST
{
    class Program
    {
        static void Main(string[] args)
        {
            MailService ms = new MailService();
            ms.ServiceStart();
        }
    }
}
