using AppTarefa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTarefa.Banco
{
    public class TarefaDB
    {
        private Context Banco { get; set; }

        public TarefaDB()
        {
            Banco = new Context();
        }

        public async Task<List<Tarefa>> PesquisarAsync(DateTime data)
        {
            return await Banco.Tarefas.Where(a =>
                    a.Data.HasValue &&
                    a.Data.Value.Year == data.Year &&
                    a.Data.Value.Month == data.Month &&
                    a.Data.Value.Day == data.Day
                ).ToListAsync();
        }

        public async Task<bool> CadastrarAsync(Tarefa tarefa)
        {
            Banco.Tarefas.Add(tarefa);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        public async Task<bool> AtualizarAsync(Tarefa tarefa)
        {
            Banco.Tarefas.Update(tarefa);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        public async Task<bool> ExcluirAsync(Guid    id)
        {
            Tarefa tarefa = await ConsultarAsync(id);
            Banco.Tarefas.Remove(tarefa);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        public async Task<Tarefa> ConsultarAsync(Guid id)
        {
            return await Banco.Tarefas.FindAsync(id);
        }
    }
}
