using Microsoft.EntityFrameworkCore;
using Void.API.Models;

namespace Void.API.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<PacienteEntity> Pacientes { get; set; }
        public DbSet<FisioterapeutaEntity> Fisioterapeutas { get; set; }
        public DbSet<ProtocoloEspacialEntity> Protocolos { get; set; }
        public DbSet<SensorWearableEntity> Sensores { get; set; }
        public DbSet<SessaoReabilitacaoEntity> Sessoes { get; set; }
        public DbSet<LeituraFadigaEntity> LeiturasFadiga { get; set; }
        public DbSet<AlertaCriticoEntity> AlertasCriticos { get; set; }
        public DbSet<TelemetriaRawJsonEntity> TelemetriaLogs { get; set; }
        public DbSet<LogAuditoriaSessaoEntity> AuditoriaSessoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Herança TPT
            modelBuilder.Entity<UsuarioEntity>()
                .ToTable("TB_VOID_USUARIO");

            modelBuilder.Entity<UsuarioEntity>()
                .Property(u => u.Id)
                .HasColumnName("ID");

            modelBuilder.Entity<PacienteEntity>()
                .ToTable("TB_VOID_PACIENTE",
                    tb => tb.Property(p => p.Id)
                    .HasColumnName("ID_USUARIO"));

            modelBuilder.Entity<FisioterapeutaEntity>()
                .ToTable("TB_VOID_FISIOTERAPEUTA",
                    tb => tb.Property(f => f.Id)
                    .HasColumnName("ID_USUARIO"));

            // Chaves compostas
            modelBuilder.Entity<SessaoReabilitacaoEntity>()
                .HasKey(s => new { s.PacienteId, s.DataSessao });

            modelBuilder.Entity<LeituraFadigaEntity>()
                .HasKey(l => new { l.PacienteId, l.DataSessao, l.SegundoLeitura });

            // Sessão -> Paciente
            modelBuilder.Entity<SessaoReabilitacaoEntity>()
                .HasOne(s => s.Paciente)
                .WithMany(p => p.Sessoes)
                .HasForeignKey(s => s.PacienteId);

            // Sessão -> Fisioterapeuta
            modelBuilder.Entity<SessaoReabilitacaoEntity>()
                .HasOne(s => s.Fisioterapeuta)
                .WithMany()
                .HasForeignKey(s => s.IdFisio);

            // Sessão -> Protocolo
            modelBuilder.Entity<SessaoReabilitacaoEntity>()
                .HasOne(s => s.Protocolo)
                .WithMany()
                .HasForeignKey(s => s.IdProtocolo);

            // Leitura -> Sessão
            modelBuilder.Entity<LeituraFadigaEntity>()
                .HasOne(l => l.Sessao)
                .WithMany(s => s.Leituras)
                .HasForeignKey(l => new { l.PacienteId, l.DataSessao });

            // Leitura -> Sensor
            modelBuilder.Entity<LeituraFadigaEntity>()
                .HasOne(l => l.Sensor)
                .WithMany()
                .HasForeignKey(l => l.IdSensor);

            // Alerta -> Sessão
            modelBuilder.Entity<AlertaCriticoEntity>()
                .HasOne<SessaoReabilitacaoEntity>()
                .WithMany()
                .HasForeignKey(a => new { a.PacienteId, a.DataSessao });
        }
    }
}