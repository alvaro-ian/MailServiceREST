using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MailServiceREST
{
    public class Mensagem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Remetente { get; set; }
        public string Destinatario { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }

        public override string ToString()
        {
            return String.Format("De: {0}, Para: {1}\nAssunto: {2}\nCorpo:\n{3}", Remetente, Destinatario, Assunto, Corpo);
        }
    }
}
