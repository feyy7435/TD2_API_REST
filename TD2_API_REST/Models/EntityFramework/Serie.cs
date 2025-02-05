using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TD2_API_REST.Models.EntityFramework;

[Table("serie")]
//table : permet de definir le nom de la table dans la bd
public partial class Serie
{
    [Key]
    //key : definie la clé primaire
    [Column("serieid")]
    //column : specifie le nom de la colonne 
    public int Serieid { get; set; }

    [Column("titre")]
    [StringLength(100)]
    //stringlenght : taille
    public string Titre { get; set; } = null!;

    [Column("resume")]
    public string? Resume { get; set; }

    [Column("nbsaisons")]
    public int? Nbsaisons { get; set; }

    [Column("nbepisodes")]
    public int? Nbepisodes { get; set; }

    [Column("anneecreation")]
    public int? Anneecreation { get; set; }

    [Column("network")]
    [StringLength(50)]
    public string? Network { get; set; }
}
