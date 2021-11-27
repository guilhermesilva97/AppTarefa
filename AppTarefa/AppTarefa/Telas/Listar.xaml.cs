using AppTarefa.Banco;
using AppTarefa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTarefa.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listar : ContentPage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Tarefa> _lista;
        public ObservableCollection<Tarefa> Lista
        {
            get
            {
                return _lista;
            }
            set
            {
                _lista = value;
                NotifyPropertyChanged("Lista");
            }
        }

        Context db = new Context();
        public Listar()
        {
            InitializeComponent();
            AtualizarDataCalendario(DateTime.Now);

            MessagingCenter.Subscribe<Listar, Tarefa>(this, "OnTarefaCadastrada", (sender, tarefa) =>
            {
                if (Lista != null)
                {

                    Lista.Add(tarefa);
                }
            });
        }

        private void AtualizarDataCalendario(DateTime data)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Lista = new ObservableCollection<Tarefa>(
                        await new TarefaDB().PesquisarAsync(data)
                    );
                    CVListaDeTarefas.ItemsSource = Lista;
                    QuantidadeTarefas.Text = Lista.Count.ToString();
                });
            });

            Dia.Text = data.Day.ToString();
            Mes.Text = data.ToString("MMM", CultureInfo.CurrentCulture).ToUpper();
            DiaDaSemana.Text = char.ToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(data.DayOfWeek)[0]) + CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(data.DayOfWeek).Substring(1);
        }

        private void AbrirPopUpAdicionar(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Telas.Cadastrar());
        }

        private void AbrirEvento(object sender, EventArgs e)
        {
            var evento = (TappedEventArgs)e;
            var tarefa = (Tarefa)evento.Parameter;

            Navigation.PushModalAsync(new Telas.Visualizar(tarefa));
        }

        private async void SwipeExcluir(object sender, EventArgs e)
        {
            if (await DisplayAlert("Excluir", "Deseja realmente excluir essa tarefa?", "Sim", "Não"))
            {
                Tarefa tarefa = (Tarefa)((SwipeItem)sender).CommandParameter;

                if (await new TarefaDB().ExcluirAsync(tarefa.Id))
                {
                    Lista.Remove(tarefa);
                };
            };
        }

        private async void CheckTarefa(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var label = checkbox.Parent.FindByName<Label>("lblTarefaDetalhe");
            if (label != null)
            {
                var gesture = (TapGestureRecognizer)label.GestureRecognizers[0];
                if (gesture != null)
                {
                    var tarefa = (Tarefa)gesture.CommandParameter;

                    if (tarefa != null)
                    {
                        await new TarefaDB().AtualizarAsync(tarefa);
                        Tachado(label, tarefa.Finalizada);
                    }
                }
            }
        }

        private void Tachado(Label label, bool finalizado)
        {
            if (finalizado)
            {
                label.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                label.TextDecorations = TextDecorations.None;
            }
        }

        private void AbrirCalendario(object sender, EventArgs e)
        {
            DPCalendario.Focus();
        }

        private void DPCalendario_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarDataCalendario(e.NewDate);
        }
    }
}