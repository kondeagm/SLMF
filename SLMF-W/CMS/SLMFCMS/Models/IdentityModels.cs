using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SLMFCMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<SLMFCMS.Models.Disciplina> Disciplina { get; set; }
        public DbSet<SLMFCMS.Models.Cuestionario> Cuestionario { get; set; }
        public DbSet<SLMFCMS.Models.Pregunta> Pregunta { get; set; }
        public DbSet<SLMFCMS.Models.Respuesta> Respuesta { get; set; }
        public DbSet<SLMFCMS.Models.RedSocial> RedSocial { get; set; }
        public DbSet<SLMFCMS.Models.ContenidoEstatico> ContenidoEstatico { get; set; }
        public DbSet<SLMFCMS.Models.Tempo> Tempo { get; set; }
        public DbSet<SLMFCMS.Models.Porcion> Porcion { get; set; }
        public DbSet<SLMFCMS.Models.Nutriente> Nutriente { get; set; }
        public DbSet<SLMFCMS.Models.Alimento> Alimento { get; set; }
        public DbSet<SLMFCMS.Models.Producto> Producto { get; set; }
        public DbSet<SLMFCMS.Models.Dieta> Dieta { get; set; }
        public DbSet<SLMFCMS.Models.DietaTempos> DietaTempos { get; set; }
        public DbSet<SLMFCMS.Models.DietaAlimentacion> DietaAlimentacion { get; set; }
        public DbSet<SLMFCMS.Models.GrupoMusculos> GrupoMusculos { get; set; }
        public DbSet<SLMFCMS.Models.Musculo> Musculo { get; set; }
        public DbSet<SLMFCMS.Models.Accesorio> Accesorio { get; set; }
        public DbSet<SLMFCMS.Models.Elemento> Elemento { get; set; }
        public DbSet<SLMFCMS.Models.Posicion> Posicion { get; set; }
        public DbSet<SLMFCMS.Models.UnidadEjercicio> UnidadEjercicio { get; set; }
        public DbSet<SLMFCMS.Models.Ejercicio> Ejercicio { get; set; }
        public DbSet<SLMFCMS.Models.Rutina> Rutina { get; set; }
        public DbSet<SLMFCMS.Models.ProTip> ProTip { get; set; }
        public DbSet<SLMFCMS.Models.Evento> Evento { get; set; }
        public DbSet<SLMFCMS.Models.Plan> Plan { get; set; }
        public DbSet<SLMFCMS.Models.PlanDias> PlanDias { get; set; }
        public DbSet<SLMFCMS.Models.PlanDiaEjercicios> PlanDiaEjercicios { get; set; }
        public DbSet<SLMFCMS.Models.PlanDiaEjercicioSeries> PlanDiaEjercicioSeries { get; set; }
        public DbSet<SLMFCMS.Models.PlanEtiquetas> PlanEtiquetas { get; set; }
        public DbSet<SLMFCMS.Models.Usuario> Usuario { get; set; }
        public DbSet<SLMFCMS.Models.Banner> Banner { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Disciplina>().HasMany(d => d.CuestionarioDeLaDisciplina)
                .WithRequired().HasForeignKey(cd => cd.DisciplinaID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Disciplina>().HasMany(d => d.EventosDeLaDisciplina)
                .WithRequired().HasForeignKey(ed => ed.DisciplinaID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Disciplina>().HasMany(d => d.PlanesDeLaDisciplina)
                .WithRequired().HasForeignKey(pd => pd.DisciplinaID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Cuestionario>().HasMany(c => c.PreguntasDelCuestionario)
                .WithRequired().HasForeignKey(pc => pc.CuestionarioID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Pregunta>().HasMany(p => p.RespuestasDeLaPregunta)
                .WithRequired().HasForeignKey(rp => rp.PreguntaID).WillCascadeOnDelete(false);

            modelBuilder.Entity<RedSocial>().HasMany(rs => rs.PostEnLaRedSocial)
                .WithRequired().HasForeignKey(ce => ce.RedSocialID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Nutriente>().HasMany(n => n.AlimentosConElNutriente)
                .WithRequired().HasForeignKey(an => an.NutrienteID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Nutriente>().HasMany(n => n.ProductosConElNutriente)
                .WithRequired().HasForeignKey(pn => pn.NutrienteID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Dieta>().HasMany(d => d.TemposDeLaDieta)
                .WithRequired().HasForeignKey(td => td.DietaID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Dieta>().HasMany(d => d.DiasConLaDieta)
                .WithRequired().HasForeignKey(dd => dd.DietaID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Tempo>().HasMany(t => t.DietasConElTempo)
                .WithRequired().HasForeignKey(td => td.TempoID).WillCascadeOnDelete(false);

            modelBuilder.Entity<DietaTempos>().HasMany(dt => dt.AlimentosDelTempo)
                .WithRequired().HasForeignKey(at => at.DietaTemposID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Porcion>().HasMany(p => p.DietasConLaPorcion)
                .WithOptional(pa => pa.PorcionDelAlimento).HasForeignKey(dp => dp.PorcionID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Alimento>().HasMany(a => a.DietasConElAlimento)
                .WithRequired().HasForeignKey(da => da.AlimentoID).WillCascadeOnDelete(false);

            modelBuilder.Entity<GrupoMusculos>().HasMany(gm => gm.MusculosDelGrupo)
                .WithRequired().HasForeignKey(m => m.GrupoMusculosID).WillCascadeOnDelete(false);
            modelBuilder.Entity<GrupoMusculos>().HasMany(gm => gm.RutinasDelGrupo)
                .WithRequired().HasForeignKey(r => r.GrupoMusculosID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Accesorio>().HasMany(a => a.EjerciciosConElAccesorio)
                .WithOptional(ea => ea.AccesorioDelEjercicio).HasForeignKey(ae => ae.AccesorioID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Elemento>().HasMany(e => e.EjerciciosConElElemento)
                .WithOptional(ee => ee.ElementoDelEjercicio).HasForeignKey(ee => ee.ElementoID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Posicion>().HasMany(p => p.EjerciciosEnLaPosicion)
                .WithOptional(ep => ep.PosicionDelEjercicio).HasForeignKey(pe => pe.PosicionID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Musculo>().HasMany(m => m.EjerciciosConElMusculo)
                .WithRequired().HasForeignKey(me => me.MusculoID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Plan>().HasMany(p => p.DiasDelPlan)
                .WithRequired().HasForeignKey(dp => dp.PlanID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Plan>().HasMany(p => p.EtiquetasDelPlan)
                .WithRequired().HasForeignKey(ep => ep.PlanID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Plan>().HasMany(p => p.UsuariosDelPlan)
                .WithOptional(up => up.PlanDelUsuario).HasForeignKey(pu => pu.PlanID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Rutina>().HasMany(r => r.DiasConLaRutina)
                .WithRequired().HasForeignKey(dr => dr.RutinaID).WillCascadeOnDelete(false);

            modelBuilder.Entity<ProTip>().HasMany(p => p.DiasConElProTip)
                .WithRequired().HasForeignKey(dp => dp.ProTipID).WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanDias>().HasMany(pd => pd.EjerciciosDelDia)
                .WithRequired().HasForeignKey(ed => ed.PlanDiasID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Ejercicio>().HasMany(e => e.DiasConElEjercicio)
                .WithRequired().HasForeignKey(de => de.EjercicioID).WillCascadeOnDelete(false);

            modelBuilder.Entity<UnidadEjercicio>().HasMany(ue => ue.DiasConLaUnidad)
                .WithRequired().HasForeignKey(du => du.UnidadEjercicioID).WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanDiaEjercicios>().HasMany(de => de.SeriesDelEjercicio)
                .WithRequired().HasForeignKey(se => se.PlanDiaEjerciciosID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Respuesta>().HasMany(r => r.PlanesDeLaEtiqueta)
                .WithRequired().HasForeignKey(pe => pe.RespuestaID).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}