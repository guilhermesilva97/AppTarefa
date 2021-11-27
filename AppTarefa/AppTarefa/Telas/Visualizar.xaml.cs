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
    public partial class Visualizar : ContentPage
    {
        public Visualizar()
        {
            InitializeComponent();
        }

        public Visualizar(Tarefa tarefa)
        {
            InitializeComponent();
            BindingContext = tarefa;
        }

        private void btnVoltar(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}