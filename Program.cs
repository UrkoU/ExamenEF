using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static System.Console;


public class InstitutoContext : DbContext
{

    public DbSet<Alumno> Alumnos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public string connString { get; private set; }

    public InstitutoContext()
    {
        var database = "EF13Urko"; // "EF{XX}Nombre" => EF00Santi
        connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database={database};User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Matricula>().HasIndex(m => new
        {
            m.AlumnoID,
            m.ModuloID
        }).IsUnique();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connString);



}
public class Alumno
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AlumnoID { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public decimal Efectivo { get; set; }
    public string Pelo { get; set; }
    public override string ToString() => $"{AlumnoID} con nombre {Nombre}, de {Edad} años y de pelo {Pelo} tiene {Efectivo} dinero";

}
public class Modulo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ModuloID { get; set; }
    public string Titulo { get; set; }
    public int Creditos { get; set; }
    public int Curso { get; set; }

    public override string ToString() => $"{ModuloID} con título {Titulo} de {Creditos} créditos en {Curso} curso";
}
public class Matricula
{
    [Key]
    public int MatriculaID { get; set; }
    public int AlumnoID { get; set; }
    public int ModuloID { get; set; }
    public Alumno alumno { get; set; }
    public Modulo modulo { get; set; }

    public override string ToString() => $"{MatriculaID} del alumno {AlumnoID} y módulo {ModuloID}";
}

class Program
{
    static void GenerarDatos()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar todo
            // Borrar todo
            db.Alumnos.RemoveRange(db.Alumnos);
            db.Modulos.RemoveRange(db.Modulos);
            db.Matriculas.RemoveRange(db.Matriculas);

            db.SaveChanges();
            // Añadir Alumnos
            Console.WriteLine("Añadir alumnos");
            // Id de 1 a 7
            db.Alumnos.Add(new Alumno { AlumnoID = 1, Nombre = "Dani", Edad = 32, Efectivo = 5, Pelo = "Moreno" });
            db.Add(new Alumno { AlumnoID = 2, Nombre = "Jenni", Edad = 27, Efectivo = 1, Pelo = "Castaño" });
            db.Add(new Alumno { AlumnoID = 3, Nombre = "Yasminn", Edad = 25, Efectivo = 10, Pelo = "Moreno" });
            db.Add(new Alumno { AlumnoID = 4, Nombre = "Javi", Edad = 20, Efectivo = 12, Pelo = "Moreno" });
            db.Add(new Alumno { AlumnoID = 5, Nombre = "Unai", Edad = 20, Efectivo = 3, Pelo = "Castaño" });
            db.Add(new Alumno { AlumnoID = 6, Nombre = "Asier", Edad = 19, Efectivo = 8, Pelo = "Moreno" });
            db.Add(new Alumno { AlumnoID = 7, Nombre = "Jon", Edad = 32, Efectivo = 9, Pelo = "Moreno" });
            // Añadir Módulos
            // Id de 1 a 10
            db.Add(new Modulo { ModuloID = 1, Titulo = "Gimnasia", Creditos = 6, Curso = 1 });
            db.Add(new Modulo { ModuloID = 2, Titulo = "Euskera", Creditos = 12, Curso = 1 });
            db.Add(new Modulo { ModuloID = 3, Titulo = "Inglés", Creditos = 12, Curso = 2 });
            db.Add(new Modulo { ModuloID = 4, Titulo = "Latín", Creditos = 12, Curso = 2 });
            db.Add(new Modulo { ModuloID = 5, Titulo = "Informática", Creditos = 6, Curso = 1 });
            db.Add(new Modulo { ModuloID = 6, Titulo = "Castellano", Creditos = 9, Curso = 1 });
            db.Add(new Modulo { ModuloID = 7, Titulo = "Administración de empresas", Creditos = 6, Curso = 2 });
            db.Add(new Modulo { ModuloID = 8, Titulo = "Francés", Creditos = 6, Curso = 1 });
            db.Add(new Modulo { ModuloID = 9, Titulo = "Física", Creditos = 9, Curso = 2 });
            db.Add(new Modulo { ModuloID = 10, Titulo = "Química", Creditos = 9, Curso = 2 });
            // Matricular Alumnos en Módulos

            foreach (Modulo modulo in db.Modulos)
                foreach (Alumno alumno in db.Alumnos)
                    db.Add(new Matricula
                    {
                        AlumnoID = alumno.AlumnoID,
                        ModuloID = modulo.ModuloID
                    });
            db.SaveChanges();
        }
    }

    static void BorrarMatriculaciones()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar las matriculas de
            // AlumnoId multiplo de 3 y ModuloId Multiplo de 2;
            foreach (Matricula matricula in db.Matriculas)
            {
                if (matricula.AlumnoID % 3 == 0 && matricula.ModuloID % 2 == 0)
                {
                    db.Matriculas.Remove(matricula);
                    Console.WriteLine("Matrícula de alumno " + matricula.AlumnoID + " y módulo " + matricula.ModuloID + " borrada ");
                }
                if (matricula.AlumnoID % 2 == 0 && matricula.ModuloID % 5 == 0)
                {
                    db.Matriculas.Remove(matricula);
                    Console.WriteLine("Matrícula de alumno " + matricula.AlumnoID + " y módulo " + matricula.ModuloID + " borrada ");
                }
            }
            db.SaveChanges();
        }
    }
    static void RealizarQuery()
    {
        using (var db = new InstitutoContext())
        {
            // Las queries que se piden en el examen
            Console.WriteLine("Order by de alumnos que tengan mucho dinero");
            List<Alumno> AlumnosOrdenados = (db.Alumnos.OrderBy(o => o.Nombre).Where(o => o.Efectivo > 8)).ToList<Alumno>();
            foreach (Alumno alumno in AlumnosOrdenados)
            {
                Console.WriteLine(alumno);
            }

            Console.WriteLine("Tipo anónimo");
            var AlumnosAnonimos = db.Alumnos.Select(al => new
            {
                AlumnoID = al.AlumnoID,
                Nombre = al.Nombre
            }).OrderByDescending(al => al.AlumnoID);
            foreach (var alumno in AlumnosAnonimos)
            {
                Console.WriteLine("ID: " + alumno.AlumnoID + " Nombre: " + alumno.Nombre);
            }

            // Console.WriteLine("Mostrar matriculaciones del penúltimo alumno");
            // var al = db.Matriculas.GroupBy(
            //     matricula => matricula.AlumnoID).
            //     Select(g => new
            //     {
            //         AlumnoID = g.Key,
            //         Matriculas = g.Count()
            //     }).OrderByDescending(m => m.AlumnoID).Skip(1).First();
            // Console.WriteLine("El alumno: " + al.AlumnoID.ToString() + " está matriculado en " + al.Matriculas + " módulos.");


            // Console.WriteLine("Mostrar asignaturas de los alumnos");
            // var ClasesDeAlumnos = (db.Matriculas.Join(db.Alumnos,
            //     al => al.AlumnoID, matr => matr.AlumnoID, (matricula, alumno) => new
            //     {
            //         matricula.AlumnoID,
            //         alumno.Nombre,
            //         alumno.Edad
            //     })).ToArray();
            // foreach (var alumno in ClasesDeAlumnos)
            // {
            //     Console.WriteLine(alumno);
            // }
        }

    }

    static void Main(string[] args)
    {
        GenerarDatos();
        BorrarMatriculaciones();
        RealizarQuery();
    }

}