using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailServiceREST
{
    class MensagemController
    {
        private string dbPath = "Mail.db";

        public void Insert(Mensagem m)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<Mensagem>();
                conn.Insert(m);
            }
        }

        public void Update(Mensagem m)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<Mensagem>();
                conn.Update(m);
            }
        }

        public List<Mensagem> GetAll()
        {
            List<Mensagem> mailList;

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<Mensagem>();
                mailList = conn.Table<Mensagem>().ToList();
            }

            return mailList;
        }

        public Mensagem Get(int id)
        {
            Mensagem msg;

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<Mensagem>();
                msg = conn.Table<Mensagem>().Where(x => x.Id == id).First();
            }

            return msg;
        }

        public List<Mensagem> GetUser(string username)
        {
            List<Mensagem> mailList;

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<Mensagem>();
                mailList = conn.Table<Mensagem>().Where(x => x.Remetente == username || x.Destinatario == username).ToList();
            }

            return mailList;
        }

        public void Delete(Mensagem m)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<Mensagem>();
                conn.Delete(m);
            }
        }
    }
}
