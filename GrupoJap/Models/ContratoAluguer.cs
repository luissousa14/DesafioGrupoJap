using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GrupoJap.Models
{
    public class ContratoAluguer
    {
        public Guid Id { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Campo cliente é obrigatoria.")]
        public Guid ClienteId { get; set; }
        [Display(Name = "Veiculo")]
        [Required(ErrorMessage = "Campo veiculo é obrigatoria.")]
        public Guid VeiculoId { get; set; }
        [Display(Name = "Quilometragem inicial")]
        [Required(ErrorMessage = "Campo quilometragem inicial é obrigatoria.")]
        public int QuilometragemInicial { get; set; }
        [Display(Name = "Data inicio")]
        [Required(ErrorMessage = "Campo data inicio aluguer é obrigatoria.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }
        [Display(Name = "Data fim")]
        [Required(ErrorMessage = "Campo data fim aluguer é obrigatoria.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFim { get; set; }

        [ValidateNever]
        [Display(Name = "Nome Completo/Email")]
        public Cliente Cliente { get; set; }
        [ValidateNever]
        [Display(Name = "Marca/Modelo - Matricula")]
        public Veiculo Veiculo { get; set; }
    }
}
