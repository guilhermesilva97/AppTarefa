using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppTarefa.Models
{
    public class Tarefa
    {
        public Guid Id { get; set; }

        public DateTime? Data { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFim { get; set; }

        public string Nome { get; set; }

        public string Nota { get; set; }

        public bool Finalizada { get; set; }
    }
}
