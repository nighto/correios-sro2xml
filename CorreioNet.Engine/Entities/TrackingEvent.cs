using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorreioNet.Engine.Entities
{
    /// <summary>
    /// valores para RA078796278CN
    /// </summary>
    public class TrackingEvent
    {

        /// <summary>
        /// Tracking number of this event
        /// </summary>
        public String TrackingNumber { get; set; }

        /// <summary>
        /// PO = Postado, RO(internacional?),DO(nacional? sedex?) = Encaminhado, PAR = Conferido, OEC = Saiu para entrega, BDE = Resultado da entrega, IT = Passagem interna, LDI = Aguardando retirada, BDI = Resultado da retirada
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// aparentemente é a ordem dos eventos na tabela, de baixo para cima, começando com 00
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// no xml vem <data>dd/mm/yyyy</data> e <hora>hh:mm</hora>
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Postado, Encaminhado
        /// </summary>
        public string Description { get; set; } 

        /// <summary>
        /// CHINA (antes do -)
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// não vem no HTML
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// CHINA (depois do -)
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// se internacional, vazio
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// desconhecido, no XML veio "99999999"
        /// </summary>
        public int STO { get; set; }

        /// <summary>
        /// quando tem "Em trânsito", quando tem duas linhas na tabela do mesmo evento
        /// </summary>
        public TrackingEventDestiny Destiny { get; set; }
    }
}
