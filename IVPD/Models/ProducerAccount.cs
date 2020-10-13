using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class ProducerAccount
    {
        public string n_cartas_CartasA { get; set; }
        public string valor_CartasA { get; set; }
        public string n_cartas_CartasB { get; set; }
        public string valor_CartasB { get; set; }
        public string n_cartas_Contratos { get; set; }
        public string valor_Contratos { get; set; }
        public string de_transferencias_relizadas { get; set; }
        public string Pagamentos_realizados { get; set; }
        public string Pagamentos_devolvidos { get; set; }
        public string Pagamentos_retidos { get; set; }
        public string Pagamentos_pendentes { get; set; }
        public string Nao_assinados { get; set; }
        public string Conciliacoes_Realizadas { get; set; }
        public string Saldo_Apurado { get; set; }
        public string sem_ficheiros_RV { get; set; }
    }
}
