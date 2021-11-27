using AppTarefa.Banco;
using AppTarefa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTarefa.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cadastrar : ContentPage
    {
        Context db = new Context();
        public Cadastrar()
        {
            InitializeComponent();
            lblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void FecharModal(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void AdicionarTarefa(object sender, EventArgs e)
        {
            Tarefa tarefa = new Tarefa()
            {
                Id = Guid.NewGuid(),
                Nome = Nome.Text,
                Data = Data.Date,
                HoraFim = HorarioFinal.Time,
                HoraInicio = HorarioInicial.Time,
                Nota = Nota.Text,
                Finalizada = false
            };

            if (await ValidaAsync(tarefa))
            {
                if (await new TarefaDB().CadastrarAsync(tarefa))
                {
                    MessagingCenter.Send<Listar, Tarefa>(new Listar(), "OnTarefaCadastrada", tarefa);

                    await Navigation.PopModalAsync();
                }
            }
        }

        private async Task<bool> ValidaAsync(Tarefa tarefa)
        {
            bool validacao = true;


            if (tarefa.HoraInicio > tarefa.HoraFim)
            {
                await DisplayAlert("Erro", "O horário final não pode ser menos que o horário incial", "OK");
                validacao = false;
            }

            if (tarefa.Nome == null)
            {
                await DisplayAlert("Erro", "Digite o nome da tarefa", "OK");
                validacao = false;
            }

            if (tarefa.Nome != null && tarefa.Nome.Length < 5)
            {
                await DisplayAlert("Erro", "O nome da tarefa precisa ter pelo menos 5 caractéres", "OK");
                validacao = false;
            }


            return validacao;
        }


        private void Data_Unfocused(object sender, FocusEventArgs e)
        {
            lblData.Text = ((DatePicker)sender).Date.ToString("dd/MM/yyyy");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Data.Focus();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            HorarioInicial.Focus();
        }

        private void HorarioInicial_Unfocused(object sender, FocusEventArgs e)
        {
            lblInicial.Text = ((TimePicker)sender).Time.ToString(@"hh\:mm");
            HorarioFinal.Focus();
        }

        private void HorarioFinal_Unfocused(object sender, FocusEventArgs e)
        {
            lblFinal.Text = ((TimePicker)sender).Time.ToString(@"hh\:mm");
        }
    }
}